using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene02Events : MonoBehaviour
{
    [Header("UI")]
    public GameObject fadeScreenIn;
    public GameObject textBox;
    public GameObject mainTextObject;
    public GameObject nextButton;
    public GameObject charName;
    public GameObject fadeOut;

    [Header("Pictures")]
    public GameObject Picture1;
    public GameObject Picture2;
    public GameObject Picture3;

    [Header("Audio")]
    [SerializeField] AudioSource workAlarm;

    [Header("Dialogue")]
    [SerializeField] string textToSpeak;
    [SerializeField] int currentTextLength;
    [SerializeField] int textLength;

    // Current position in the event
    [SerializeField] int eventPos = 0;

    // Index within the intro dialogue sequence
    int introIndex = 0;

    void Update()
    {
        textLength = TextCreator.charCount;
    }

    void Start()
    {
        StartCoroutine(EventStarter());
    }

    IEnumerator EventStarter()
    {
        // Initial fade
        yield return new WaitForSeconds(2f);

        fadeScreenIn.SetActive(false);

        yield return new WaitForSeconds(2f);

        mainTextObject.SetActive(true);
        textBox.SetActive(true);

        // Start first intro line
        introIndex = 0;
        eventPos = 0;

        yield return StartCoroutine(PlayIntroLine(introIndex));
    }

    IEnumerator PlayIntroLine(int index)
    {
        nextButton.SetActive(false);

        switch (index)
        {
            case 0:
                textToSpeak = "Bay slowly opened his eyes, his world slowly unblurring as everything started to become clear.";
                break;

            case 1:
                textToSpeak = "He was in a clinical environment, a dark windowless room that carried the scent of disinfectants and bleach...";
                break;

            case 2:
                textToSpeak = "...yet dusty at the same time.";
                break;

            case 3:
                textToSpeak = "There was no one in the room, nor did it look like anyone was currently inside the building.";
                break;

            case 4:
                textToSpeak = "He still couldn’t recall anything that had happened to him before arriving to this place.";
                break;

            case 5:
                textToSpeak = "He moved to sit on the bed, then hopped off and unhurriedly made his way to the white door.";
                break;

            case 6:
                textToSpeak = "He turned around and scanned the room one last time. Will you do something before exiting the room?";
                break;

            default:
                yield break;
        }

        // Clear character name during intro
        charName.GetComponent<TMPro.TMP_Text>().text = "";

        // Set dialogue text
        textBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;

        currentTextLength = textToSpeak.Length;

        // Reset text counter
        TextCreator.charCount = 0;

        // Start typewriter effect
        TextCreator.runTextPrint = true;

        // Play sigh on first intro line
        if (index == 9 && workAlarm != null)
        {
            workAlarm.Play();
        }

        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1f);

        // Wait until all characters have printed
        yield return new WaitUntil(() => textLength >= currentTextLength);

        yield return new WaitForSeconds(0.5f);

        // -------------------------------------------------
        // PICTURE 1
        // "Who is this?"
        // Fade Picture1 out, then disable it.
        // -------------------------------------------------
        if (index == 4)
        {
            yield return StartCoroutine(FadeAndDisable(Picture1));
        }

        // -------------------------------------------------
        // PICTURE 2
        // "Anyone who stood around him..."
        // Fade Picture2 out, then disable it.
        // -------------------------------------------------
        if (index == 5)
        {
            yield return StartCoroutine(FadeAndDisable(Picture2));
        }

        // -------------------------------------------------
        // PICTURE 3
        // "He's alive. Subject A10..."
        // Fade Picture3 out, then disable it.
        // -------------------------------------------------
        if (index == 14)
        {
            yield return StartCoroutine(FadeAndDisable(Picture3));
        }

        // Allow player to continue
        nextButton.SetActive(true);
    }

    IEnumerator FadeAndDisable(GameObject picture)
    {
        if (picture == null)
        {
            yield break;
        }

        CanvasGroup canvasGroup = picture.GetComponent<CanvasGroup>();

        // If the picture does not already have a CanvasGroup,
        // add one automatically.
        if (canvasGroup == null)
        {
            canvasGroup = picture.AddComponent<CanvasGroup>();
        }

        float startAlpha = canvasGroup.alpha;
        float fadeDuration = 1f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                0f,
                timer / fadeDuration
            );

            yield return null;
        }

        canvasGroup.alpha = 0f;

        // Disable the picture after the fade finishes
        picture.SetActive(false);
    }

    IEnumerator EventFour()
    {
        nextButton.SetActive(false);

        textBox.SetActive(true);

        fadeOut.SetActive(true);

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(3);
    }

    public void NextButton()
    {
        // INTRO SEQUENCE
        if (eventPos == 0)
        {
            introIndex++;

            // There are 20 intro lines: 0 through 19
            if (introIndex <= 19)
            {
                StartCoroutine(PlayIntroLine(introIndex));
            }
            else
            {
                // Intro finished
                eventPos = 4;

                StartCoroutine(EventFour());
            }

            return;
        }

        // END EVENT
        if (eventPos == 4)
        {
            StartCoroutine(EventFour());
        }
    }
}