using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Drop this component on an empty GameObject (e.g. "FadeOverlay") and drag the RawImage
/// you are fading into the slot below.
///
/// The script does not fade anything itself. It only watches that RawImage's alpha and
/// disables this GameObject the moment the alpha reaches zero, so a faded-out overlay
/// stops drawing and stops being animated. When it is enabled again it starts watching
/// straight away, and if the image is still transparent it closes itself immediately
/// rather than waiting a frame.
/// </summary>
[DisallowMultipleComponent]
public class RawImageAlphaWatcher : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("The RawImage whose transparency is being watched. Drag it here from the hierarchy.")]
    [SerializeField] private RawImage targetImage;

    [Header("Threshold")]
    [Tooltip("The image counts as fully transparent once its alpha drops to this value or below. " +
             "A small non-zero value absorbs the float rounding that tweening leaves behind.")]
    [SerializeField, Range(0f, 0.1f)] private float transparentThreshold = 0.001f;

    private void OnEnable()
    {
        // Check on the way in too, so re-showing an already transparent overlay closes it at once.
        CheckTransparency();
    }

    private void Update()
    {
        CheckTransparency();
    }

    private void CheckTransparency()
    {
        if (targetImage == null)
        {
            return;
        }

        if (targetImage.color.a <= transparentThreshold)
        {
            // Disabling the GameObject stops Update, so this runs once and then the object rests.
            gameObject.SetActive(false);
        }
    }
}
