using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Drop this component on an empty GameObject (e.g. "MainMenu") and drag your four
/// menu buttons into the slots below. It gives each of them two things and nothing else:
///
///  1. A subtle hover animation: an ease-out scale pop plus a gentle brightness lift on
///     pointer enter, smoothly wound back on pointer exit.
///  2. One shared hover sound, played through an AudioSource it adds to itself.
///
/// Click handling is left entirely to your own Button.onClick wiring.
/// </summary>
[DisallowMultipleComponent]
public class MainMenuController : MonoBehaviour
{
    [Header("Menu Buttons")]
    [Tooltip("The four menu buttons. Any slot can be left empty if you do not use that button.")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [Header("Hover Animation")]
    [Tooltip("Scale the button grows to while hovered. Keep it close to 1 for a subtle feel.")]
    [SerializeField, Range(1f, 1.25f)] private float hoverScale = 1.05f;
    [Tooltip("How much brighter the button gets while hovered (1 = no change).")]
    [SerializeField, Range(1f, 2f)] private float hoverBrightness = 1.18f;
    [Tooltip("Seconds the button takes to grow when hovered.")]
    [SerializeField, Range(0.01f, 0.5f)] private float hoverEnterTime = 0.12f;
    [Tooltip("Seconds the button takes to settle back when the pointer leaves.")]
    [SerializeField, Range(0.01f, 0.5f)] private float hoverExitTime = 0.09f;

    [Header("Hover Sound")]
    [Tooltip("Single clip shared by all four buttons.")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField, Range(0f, 1f)] private float hoverVolume = 0.6f;
    [Tooltip("Minimum seconds between two hover sounds, so sweeping the mouse across the menu does not stack them.")]
    [SerializeField, Range(0f, 0.5f)] private float hoverSoundCooldown = 0.05f;

    /// <summary>Per-button visual state, cached at start so animations never drift.</summary>
    private sealed class ButtonVisual
    {
        public Button button;
        public Graphic graphic;
        public Color baseColor;
        public Vector3 baseScale;
        public float hoverAmount;   // 0 = idle, 1 = fully hovered
        public Coroutine animation;
    }

    private readonly List<ButtonVisual> _visuals = new List<ButtonVisual>();
    private AudioSource _audio;
    private float _lastHoverSoundTime = float.NegativeInfinity;

    private void Awake()
    {
        // A 2D audio source is all a UI hover sound needs.
        _audio = GetComponent<AudioSource>();
        if (_audio == null)
        {
            _audio = gameObject.AddComponent<AudioSource>();
        }

        _audio.playOnAwake = false;
        _audio.loop = false;
        _audio.spatialBlend = 0f;

        CacheButton(startButton);
        CacheButton(loadButton);
        CacheButton(creditsButton);
        CacheButton(quitButton);
    }

    private void OnDisable()
    {
        // Coroutines stop with the component, so put the buttons back to their idle look.
        ResetVisuals();
    }

    // ---------------------------------------------------------------- setup

    private void CacheButton(Button button)
    {
        if (button == null || _visuals.Exists(v => v.button == button))
        {
            return;
        }

        Graphic target = button.targetGraphic;

        var visual = new ButtonVisual
        {
            button = button,
            graphic = target,
            baseColor = target != null ? target.color : Color.white,
            baseScale = button.transform.localScale,
            hoverAmount = 0f,
        };

        _visuals.Add(visual);
        RegisterHoverEvents(visual);
        ApplyVisual(visual);
    }

    /// <summary>
    /// Hover is read through an EventTrigger so the buttons keep their own
    /// click handling and inspector wiring untouched.
    /// </summary>
    private void RegisterHoverEvents(ButtonVisual visual)
    {
        EventTrigger trigger = visual.button.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = visual.button.gameObject.AddComponent<EventTrigger>();
        }

        AddEntry(trigger, EventTriggerType.PointerEnter, () => HandleHover(visual, true));
        AddEntry(trigger, EventTriggerType.PointerExit, () => HandleHover(visual, false));
    }

    private static void AddEntry(EventTrigger trigger, EventTriggerType type, UnityAction callback)
    {
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(_ => callback());
        trigger.triggers.Add(entry);
    }

    // ---------------------------------------------------------------- hover

    private void HandleHover(ButtonVisual visual, bool isHovered)
    {
        if (visual == null)
        {
            return;
        }

        if (isHovered)
        {
            if (!visual.button.IsInteractable())
            {
                return;
            }

            PlayHoverSound();
        }

        if (visual.animation != null)
        {
            StopCoroutine(visual.animation);
        }

        float target = isHovered ? 1f : 0f;
        float duration = isHovered ? hoverEnterTime : hoverExitTime;
        visual.animation = StartCoroutine(AnimateHover(visual, target, duration));
    }

    private IEnumerator AnimateHover(ButtonVisual visual, float target, float duration)
    {
        duration = Mathf.Max(duration, 0.0001f);

        float start = visual.hoverAmount;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Unscaled time, so the menu still animates if the game pauses with timeScale = 0.
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            float eased = 1f - Mathf.Pow(1f - t, 3f); // ease-out, feels snappy but smooth
            visual.hoverAmount = Mathf.LerpUnclamped(start, target, eased);

            ApplyVisual(visual);
            yield return null;
        }

        visual.hoverAmount = target;
        visual.animation = null;
        ApplyVisual(visual);
    }

    private void ApplyVisual(ButtonVisual visual)
    {
        float t = Mathf.Clamp01(visual.hoverAmount);

        if (visual.button != null)
        {
            // Multiplicative so buttons with a custom base scale keep their proportions.
            visual.button.transform.localScale =
                Vector3.LerpUnclamped(visual.baseScale, visual.baseScale * hoverScale, t);
        }

        if (visual.graphic != null)
        {
            // Brighten by scaling the colour, and keep the original alpha.
            Color hovered = visual.baseColor * hoverBrightness;
            hovered.a = visual.baseColor.a;

            visual.graphic.color = Color.LerpUnclamped(visual.baseColor, hovered, t);
        }
    }

    private void ResetVisuals()
    {
        for (int i = 0; i < _visuals.Count; i++)
        {
            ButtonVisual visual = _visuals[i];

            if (visual.animation != null)
            {
                StopCoroutine(visual.animation);
                visual.animation = null;
            }

            visual.hoverAmount = 0f;
            ApplyVisual(visual);
        }
    }

    private void PlayHoverSound()
    {
        if (hoverSound == null || _audio == null)
        {
            return;
        }

        if (Time.unscaledTime - _lastHoverSoundTime < hoverSoundCooldown)
        {
            return;
        }

        _lastHoverSoundTime = Time.unscaledTime;
        _audio.PlayOneShot(hoverSound, hoverVolume);
    }
}
