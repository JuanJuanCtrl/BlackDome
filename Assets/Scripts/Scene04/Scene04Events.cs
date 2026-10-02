using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene04Event : MonoBehaviour
{
    public GameObject fadeScreenIn;
    public GameObject textBox;

    [Header("Pictures")]
    public GameObject Picture1;
    public GameObject Picture2;
    public GameObject Picture3;
    public GameObject Picture4;

    [SerializeField] private string speakerName = "";
    [SerializeField] string textToSpeak;
    [SerializeField] int currentTextLength;
    [SerializeField] int textLength;
    [SerializeField] GameObject mainTextObject;
    [SerializeField] GameObject nextButton;
    [SerializeField] int eventPos = 0;
    [SerializeField] GameObject charName;
    [SerializeField] GameObject fadeOut;

    int introIndex = 0;
    bool isFadingOut = false;

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
        yield return new WaitForSeconds(2f);
        fadeScreenIn.SetActive(false);
        yield return new WaitForSeconds(2f);

        mainTextObject.SetActive(true);
        textBox.SetActive(true);

        introIndex = 0;
        yield return StartCoroutine(PlayLine(introIndex));

        eventPos = 0;
    }

    IEnumerator PlayLine(int index)
    {
        nextButton.SetActive(false);

        switch (index)
        {
            case 0:
                speakerName = "You";
                textToSpeak = "He exited the place, which turned out to be a sort of clinic as he expected.";
                break;
            case 1:
                speakerName = "You";
                textToSpeak = "To his surprise, the place is enormous.";
                break;
            case 2:
                speakerName = "You";
                textToSpeak = "An actual city, but...";
                break;
            case 3:
                speakerName = "You";
                textToSpeak = "He looked up and around, he could see the shape of the curved roof.";
                break;
            case 4:
                speakerName = "You";
                textToSpeak = "The air felt different, he wasn't outdoors.";
                break;
            case 5:
                speakerName = "You";
                textToSpeak = "He was somewhere inside, and unfamiliar of his surroundings...";
                break;
            case 6:
                speakerName = "You";
                textToSpeak = ". . .";
                break;
            case 7:
                speakerName = "You";
                textToSpeak = "He walked down the civilian pavement, but he felt as if something was missing...";
                break;
            case 8:
                speakerName = "You";
                textToSpeak = "...wasn't there something in-between these pavements?";
                break;
            case 9:
                speakerName = "You";
                textToSpeak = "He tried to recall the word, but his mind was clouded with thoughts...";
                break;
            case 10:
                speakerName = "You";
                textToSpeak = "...so he dismissed it and continued ahead.";
                break;
            case 11:
                speakerName = "You";
                textToSpeak = "He noticed something moving up on the corner of his eyes, and as he looked up...";
                break;
            case 12:
                speakerName = "You";
                textToSpeak = "He saw it:";
                break;
            case 13:
                speakerName = "You";
                textToSpeak = "White, spherical-shaped vehicles were floating all over the city.";
                break;
            case 14:
                speakerName = "You";
                textToSpeak = "Some were cautiously driving while others maneuvered expertly across the city.";
                break;
            case 15:
                speakerName = "You";
                textToSpeak = "While he walked, he noticed one of the spheres was slowly descending towards him.";
                break;
            case 16:
                speakerName = "You";
                textToSpeak = "He got out of the way, but he saw the driver was being wary of where he was standing.";
                break;
            case 17:
                speakerName = "You";
                textToSpeak = "The sphere landed safely, but it was still a few inches off the ground...";
                break;
            case 18:
                speakerName = "You";
                textToSpeak = "...still floating";
                break;
            case 19:
                speakerName = "You";
                textToSpeak = "A figure stepped out of the vehicle, and revealed itself...";
                break;
        }

        speakerName = "";

        charName.GetComponent<TMPro.TMP_Text>().text = speakerName;
        textBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;

        currentTextLength = textToSpeak.Length;
        TextCreator.runTextPrint = true;

        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => textLength >= currentTextLength);
        yield return new WaitForSeconds(0.5f);

        if (index == 5)
        {
            yield return StartCoroutine(FadeAndDisable(Picture1));
        }

        if (index == 10)
        {
            yield return StartCoroutine(FadeAndDisable(Picture2));
        }

        if (index == 16)
        {
            yield return StartCoroutine(FadeAndDisable(Picture3));
        }

        if (index == 18)
        {
            yield return StartCoroutine(FadeAndDisable(Picture4));
        }

        nextButton.SetActive(true);
    }

    IEnumerator FadeAndDisable(GameObject picture)
    {
        if (picture == null)
        {
            yield break;
        }

        CanvasGroup canvasGroup = picture.GetComponent<CanvasGroup>();
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
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, timer / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        picture.SetActive(false);
    }

    IEnumerator EventFour()
    {
        nextButton.SetActive(false);
        textBox.SetActive(true);
        fadeOut.SetActive(true);

        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(8);
    }

    public void NextButton()
    {
        if (isFadingOut)
        {
            return;
        }

        if (eventPos == 0)
        {
            introIndex++;
            if (introIndex <= 19)
            {
                StartCoroutine(PlayLine(introIndex));
            }
            else
            {
                eventPos = 4;
                isFadingOut = true;
                StartCoroutine(EventFour());
            }
            return;
        }

        if (eventPos == 4)
        {
            StartCoroutine(EventFour());
        }
    }
}
