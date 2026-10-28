using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroSequenceEvents : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject textBox;
    [SerializeField] GameObject mainTextObject;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject charName;
    [SerializeField] GameObject fadeOut;

    [Header("Text System")]
    [SerializeField] string textToSpeak;
    [SerializeField] float charDelay = 0.1f;   // typing speed (seconds per character)

    int introIndex = 0;
    bool isPlayingLine = false;
    bool isSkipping = false;

    TMP_Text tmp;

    void Start()
    {
        tmp = textBox.GetComponent<TMP_Text>();
        StartCoroutine(EventStarter());
    }

    void Update()
    {
        // No typing here anymore — typing is handled by a coroutine.
    }

    IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(7f);

        mainTextObject.SetActive(true);
        textBox.SetActive(true);
        charName.GetComponent<TMP_Text>().text = "";

        introIndex = 0;
        yield return StartCoroutine(PlayIntroLine(introIndex));
    }

    IEnumerator PlayIntroLine(int index)
    {
        isPlayingLine = true;
        isSkipping = false;
        nextButton.SetActive(false);

        switch (index)
        {
            case 0:
                textToSpeak = "Light hides within the darkness of the people...";
                break;
            case 1:
                textToSpeak = "...rooted from the darkness of their own world.";
                break;
            case 2:
                textToSpeak = "To find light, you must first overcome the darkness.";
                break;
            case 3:
                textToSpeak = "Only then...";
                break;
            case 4:
                textToSpeak = "...the world will shine bright.";
                break;
            case 5:
                textToSpeak = "You know how I know? Perhaps not...";
                break;
            case 6:
                textToSpeak = "...but it is a fact that already many take for granted.";
                break;
            case 7:
                textToSpeak = "Darkness can consume you in the scarcity of light.";
                break;
            case 8:
                textToSpeak = "For it's the latter reason that you and I can have a world as bright as...";
                break;
            case 9:
                textToSpeak = "But it is not the outside world.";
                break;
            case 10:
                textToSpeak = "It is your world.";
                break;
            case 11:
                textToSpeak = "The one you, and only you, can perceive.";
                break;
            case 12:
                textToSpeak = "Darkness prevails in the world of the people, but light prevails in your world.";
                break;
            case 13:
                textToSpeak = "As hard as it may seem at first, the light is always there—omnipresent...";
                break;
            case 14:
                textToSpeak = "...waiting.";
                break;
            case 15:
                textToSpeak = "Remember, never confuse light for darkness...";
                break;
            case 16:
                textToSpeak = "...for it's the greatest deception of all...";
                break;
            case 17:
                textToSpeak = "...the greatest error that a human being can commit.";
                break;
            case 18:
                textToSpeak = "And you, my friend, are a human being.";
                break;
            case 19:
                textToSpeak = "A human being, and a human being alone, can find the light.";
                break;
                
        }

        tmp.text = "";

        // Typewriter coroutine: slower, controlled speed
        for (int i = 0; i <= textToSpeak.Length; i++)
        {
            if (isSkipping)
            {
                // Instantly show full line if user clicked during typing
                tmp.text = textToSpeak;
                break;
            }

            tmp.text = textToSpeak.Substring(0, i);
            yield return new WaitForSeconds(charDelay);
        }

        // Small pause after full line is visible
        yield return new WaitForSeconds(0.3f);

        isPlayingLine = false;
        nextButton.SetActive(true);
    }

    public void NextButton()
    {
        // If still typing → skip to full line
        if (isPlayingLine)
        {
            isSkipping = true;
            return;
        }

        introIndex++;

        if (introIndex <= 18)
        {
            StartCoroutine(PlayIntroLine(introIndex));
        }
        else
        {
            StartCoroutine(FadeOutAndLoadNext());
        }
    }

    IEnumerator FadeOutAndLoadNext()
    {
        nextButton.SetActive(false);
        textBox.SetActive(false);
        mainTextObject.SetActive(false);

        if (fadeOut != null)
        {
            fadeOut.SetActive(true);
        }

        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene(4);
    }
}