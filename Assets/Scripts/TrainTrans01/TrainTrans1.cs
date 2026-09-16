using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainSceneController : MonoBehaviour
{
    [SerializeField] private GameObject train;
    [SerializeField] private AudioSource trainTracks;

    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;

    private void Start()
    {
        StartCoroutine(TrainSequence());
    }

    private IEnumerator TrainSequence()
    {
        if (trainTracks != null)
        {
            trainTracks.volume = 0f;
            trainTracks.Play();
            yield return StartCoroutine(FadeAudio(trainTracks, 0f, 1f, fadeInDuration));
        }

        yield return new WaitForSeconds(1f);

        if (train != null)
        {
            train.SetActive(true);
        }

        yield return new WaitForSeconds(2f);

        if (trainTracks != null)
        {
            yield return StartCoroutine(FadeAudio(trainTracks, trainTracks.volume, 0f, fadeOutDuration));
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(18);
    }

    private IEnumerator FadeAudio(AudioSource source, float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        source.volume = to;
    }
}