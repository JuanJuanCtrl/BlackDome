using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Plays a click sound when any assigned choice button is pressed, without letting
/// rapid clicks stack or retrigger the sound. Several clips can be supplied so a
/// short sound used over and over (dialogue blips) does not fatigue the ear.
/// </summary>
[DisallowMultipleComponent]
public class ChoiceButtonClickSound : MonoBehaviour
{
    [Header("References")]
    [Tooltip("First choice button to listen to. Drag the object's Button component here.")]
    [SerializeField] private Button choiceButton1;

    [Tooltip("Second choice button to listen to. Leave empty when the dialogue offers fewer choices.")]
    [SerializeField] private Button choiceButton2;

    [Tooltip("Third choice button to listen to. Leave empty when the dialogue offers fewer choices.")]
    [SerializeField] private Button choiceButton3;

    [Tooltip("AudioSource that plays the click sound. Assign the clip on this source; use a dedicated source so other audio does not affect the anti-spam timing.")]
    [SerializeField] private AudioSource audioSource;

    [Header("Variation")]
    [Tooltip("Extra clips to choose from at random, in addition to the AudioSource's own clip. Add a few takes of the same click/dialogue sound. With more than one clip available, the same clip never plays twice in a row.")]
    [SerializeField] private AudioClip[] extraClips;

    [Tooltip("Randomise pitch slightly on every play. The cheapest way to stop a short sound sounding mechanical, and it works even with a single clip.")]
    [SerializeField] private bool randomizePitch = true;

    [Tooltip("Random pitch multipliers (X = min, Y = max). 1 is the clip's normal pitch; small ranges like 0.96-1.04 are subtle, wider ranges are more cartoonish.")]
    [SerializeField] private Vector2 pitchRange = new Vector2(0.96f, 1.04f);

    [Header("Anti-spam")]
    [Tooltip("Clicks this soon after the previous click are ignored (in seconds).")]
    [SerializeField] private float minimumInterval = 0.15f;

    [Tooltip("Also ignore clicks until the click clip has finished playing, so the sound can never overlap itself.")]
    [SerializeField] private bool ignoreClicksWhileSoundPlaying = true;

    [Header("Debug")]
    [Tooltip("Log setup messages and the reason a click was ignored. Turn off for release builds.")]
    [SerializeField] private bool logDiagnostics;

    private float lastClickTime = float.NegativeInfinity;
    private float lastPlayedLength;
    private int lastClipIndex = -1;

    /// <summary>Pitch set on the AudioSource, used as the centre of the random range.</summary>
    private float basePitch = 1f;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            basePitch = audioSource.pitch;
        }
    }

    private void OnEnable()
    {
        if (AssignedButtonCount() == 0)
        {
            LogWarning("is enabled but no choice button is assigned, so it cannot listen for clicks");
            return;
        }

        // RemoveListener first guarantees each handler is never added twice.
        Subscribe(choiceButton1);
        Subscribe(choiceButton2);
        Subscribe(choiceButton3);
        Log($"is now listening to {AssignedButtonNames()}");
    }

    private void OnDisable()
    {
        Unsubscribe(choiceButton1);
        Unsubscribe(choiceButton2);
        Unsubscribe(choiceButton3);
    }

    private void Start()
    {
        if (AssignedButtonCount() == 0)
        {
            Debug.LogError($"{nameof(NextButtonClickSound)}: no choice button is assigned on '{name}'.", this);
        }

        if (audioSource == null)
        {
            Debug.LogError($"{nameof(NextButtonClickSound)}: assign an AudioSource on '{name}'.", this);
        }
        else if (!HasAnyClip())
        {
            Debug.LogError(
                $"{nameof(NextButtonClickSound)}: assign a clip on the AudioSource or add Extra Clips on '{name}'.",
                this);
        }

        if (EventSystem.current == null)
        {
            Debug.LogWarning(
                $"{nameof(NextButtonClickSound)}: there is no active EventSystem in the scene, so UI buttons will not receive clicks.",
                this);
        }

        Log($"is ready (buttons: {AssignedButtonNames()}, " +
            $"source: {(audioSource != null ? audioSource.name : "none")}, clips: {CountClips()})");
    }

    /// <summary>Plays the click sound if this component is enabled and the anti-spam timing allows it.</summary>
    public void PlayClickSound()
    {
        // The click subscription only exists while this component is enabled.
        if (!isActiveAndEnabled)
        {
            LogWarning("ignored a click because the component or its GameObject is disabled");
            return;
        }

        if (AssignedButtonCount() == 0)
        {
            LogWarning("ignored a click because no choice button is assigned");
            return;
        }

        // Deliberately no isActiveAndEnabled / IsInteractable check here. UGUI only calls
        // onClick on an active, interactable button, and other listeners on the same click
        // may have disabled the button or hidden its panel by the time we run. Re-checking
        // state would reject clicks that were perfectly valid when pressed.

        if (audioSource == null)
        {
            LogWarning("ignored a click because no AudioSource is assigned");
            return;
        }

        if (!audioSource.isActiveAndEnabled)
        {
            LogWarning("ignored a click because the AudioSource's GameObject is inactive or the component is disabled");
            return;
        }

        // Unscaled time so cooldowns still work while Time.timeScale is 0 (paused UI).
        float cooldown = minimumInterval;
        if (ignoreClicksWhileSoundPlaying)
        {
            cooldown = Mathf.Max(cooldown, lastPlayedLength);
        }

        float sinceLastClick = Time.unscaledTime - lastClickTime;
        if (sinceLastClick < cooldown)
        {
            LogWarning($"ignored a click because only {sinceLastClick:0.00}s passed since the last one (cooldown is {cooldown:0.00}s)");
            return;
        }

        AudioClip clip = PickClip();
        if (clip == null)
        {
            LogWarning("ignored a click because there is no clip available to play");
            return;
        }

        float pitch = GetRandomPitch();
        audioSource.pitch = pitch;

        lastClickTime = Time.unscaledTime;
        // Pitch changes playback speed, so the effective length changes with it.
        lastPlayedLength = clip.length / Mathf.Max(0.01f, pitch);

        audioSource.PlayOneShot(clip);
        Log($"played '{clip.name}' on '{audioSource.name}' (pitch {pitch:0.00})");
    }

    private void OnChoiceButtonClicked()
    {
        Log("received a click from a choice button");
        PlayClickSound();
    }

    private void Subscribe(Button button)
    {
        if (button == null)
        {
            return;
        }

        // RemoveListener first guarantees the handler is never added twice, even when the
        // same button is dropped into more than one slot.
        button.onClick.RemoveListener(OnChoiceButtonClicked);
        button.onClick.AddListener(OnChoiceButtonClicked);
    }

    private void Unsubscribe(Button button)
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnChoiceButtonClicked);
        }
    }

    private int AssignedButtonCount()
    {
        int count = 0;
        if (choiceButton1 != null)
        {
            count++;
        }

        if (choiceButton2 != null)
        {
            count++;
        }

        if (choiceButton3 != null)
        {
            count++;
        }

        return count;
    }

    private string AssignedButtonNames()
    {
        var names = new List<string>(3);
        AddButtonName(names, choiceButton1);
        AddButtonName(names, choiceButton2);
        AddButtonName(names, choiceButton3);
        return names.Count > 0 ? string.Join(", ", names) : "none";
    }

    private static void AddButtonName(List<string> names, Button button)
    {
        if (button != null)
        {
            names.Add($"'{button.name}'");
        }
    }

    /// <summary>
    /// Picks the AudioSource's own clip or one of the extra clips at random,
    /// never returning the same clip twice in a row when more than one exists.
    /// </summary>
    private AudioClip PickClip()
    {
        bool hasPrimary = audioSource.clip != null;
        int primaryCount = hasPrimary ? 1 : 0;
        int total = primaryCount + CountExtraClips();

        if (total == 0)
        {
            return null;
        }

        int pick = Random.Range(0, total);
        if (total > 1 && pick == lastClipIndex)
        {
            // Move to a different clip, chosen at random from the remaining ones.
            pick = (pick + 1 + Random.Range(0, total - 1)) % total;
        }

        lastClipIndex = pick;

        if (pick < primaryCount)
        {
            return audioSource.clip;
        }

        int wanted = pick - primaryCount;
        int seen = 0;
        for (int i = 0; i < extraClips.Length; i++)
        {
            if (extraClips[i] == null)
            {
                continue;
            }

            if (seen == wanted)
            {
                return extraClips[i];
            }

            seen++;
        }

        return audioSource.clip;
    }

    private float GetRandomPitch()
    {
        if (!randomizePitch)
        {
            return basePitch;
        }

        float min = Mathf.Min(pitchRange.x, pitchRange.y);
        float max = Mathf.Max(pitchRange.x, pitchRange.y);
        return basePitch * Random.Range(min, max);
    }

    private bool HasAnyClip()
    {
        return (audioSource != null && audioSource.clip != null) || CountExtraClips() > 0;
    }

    private int CountClips()
    {
        return (audioSource != null && audioSource.clip != null ? 1 : 0) + CountExtraClips();
    }

    private int CountExtraClips()
    {
        if (extraClips == null)
        {
            return 0;
        }

        int count = 0;
        for (int i = 0; i < extraClips.Length; i++)
        {
            if (extraClips[i] != null)
            {
                count++;
            }
        }

        return count;
    }

    private void Log(string message)
    {
        if (logDiagnostics)
        {
            Debug.Log($"{nameof(NextButtonClickSound)}: {message}.", this);
        }
    }

    private void LogWarning(string message)
    {
        if (logDiagnostics)
        {
            Debug.LogWarning($"{nameof(NextButtonClickSound)}: {message}.", this);
        }
    }

    private void OnValidate()
    {
        minimumInterval = Mathf.Max(0f, minimumInterval);

        // AudioSource pitch must stay above zero, and extreme values are rarely wanted.
        pitchRange.x = Mathf.Clamp(pitchRange.x, 0.1f, 3f);
        pitchRange.y = Mathf.Clamp(pitchRange.y, 0.1f, 3f);
    }
}
