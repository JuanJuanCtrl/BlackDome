using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DayTransition : MonoBehaviour
{
    [Header("Assign UI GameObjects")]
    public GameObject previousDayImage;
    public GameObject nextDayImage;

    [Header("Timing")]
    public float fadeDuration = 1.5f;

    private CanvasGroup nextCanvasGroup;

    private void Start()
    {
        if (previousDayImage != null)
            previousDayImage.SetActive(false);

        if (nextDayImage != null)
        {
            nextDayImage.SetActive(false);

            nextCanvasGroup = nextDayImage.GetComponent<CanvasGroup>();
            if (nextCanvasGroup == null)
                nextCanvasGroup = nextDayImage.AddComponent<CanvasGroup>();

            nextCanvasGroup.alpha = 1f;
        }

        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence()
    {
        yield return new WaitForSeconds(1f);

        if (previousDayImage != null)
            previousDayImage.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        if (previousDayImage != null)
            previousDayImage.SetActive(false);

        if (nextDayImage != null)
            nextDayImage.SetActive(true);

        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(FadeOutNextDay());

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(9);
    }

    private IEnumerator FadeOutNextDay()
    {
        if (nextCanvasGroup == null)
            yield break;

        float startAlpha = nextCanvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            nextCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / fadeDuration);
            yield return null;
        }

        nextCanvasGroup.alpha = 0f;
    }
}