using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class KodaCall01 : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject textBox;
    [SerializeField] GameObject mainTextObject;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject charName;
    [SerializeField] GameObject fadeOut;

    [Header("Text System")]
    [SerializeField] string textToSpeak;
    [SerializeField] float charDelay = 0.04f;

    int introIndex = 0;
    bool isPlayingLine = false;
    bool isSkipping = false;

    TMP_Text tmp;
    TMP_Text nameText;

    void Start()
    {
        tmp = textBox.GetComponent<TMP_Text>();
        nameText = charName.GetComponent<TMP_Text>();
        StartCoroutine(EventStarter());
    }

    void Update()
    {
    }

    IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(2f);

        mainTextObject.SetActive(true);
        textBox.SetActive(true);

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
                nameText.text = "You";
                textToSpeak = "It's almost evening. You received a message from Koda.";
                break;
            case 1:
                nameText.text = "Koda";
                textToSpeak = "Yo bro! Sorry for the wait, come here to get the study session going!";
                break;
            case 2:
                nameText.text = "Koda";
                textToSpeak = "Oh I almost forgot, I live over at Main Street, so I hope you don't mind the walk.";
                break;
            case 3:
                nameText.text = "Koda";
                textToSpeak = "I already phoned Hiromi, she was totally pissed about the idea of studying so late.";
                break;
            case 4:
                nameText.text = "Koda";
                textToSpeak = "Don't worry, she calmed down. She's coming anyways. Well anyways, see you in a bit bro!";
                break;
            case 5:
                nameText.text = "You";
                textToSpeak = "You shoved your phone back in your pocket. You grabbed your keys and headed out the door. You made your way into Koda's house...";
                break;
        }

        tmp.text = "";

        for (int i = 0; i <= textToSpeak.Length; i++)
        {
            if (isSkipping)
            {
                tmp.text = textToSpeak;
                break;
            }

            tmp.text = textToSpeak.Substring(0, i);
            yield return new WaitForSeconds(charDelay);
        }

        yield return new WaitForSeconds(0.3f);

        isPlayingLine = false;
        nextButton.SetActive(true);
    }

    public void NextButton()
    {
        if (isPlayingLine)
        {
            isSkipping = true;
            return;
        }

        introIndex++;

        if (introIndex <= 5)
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

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(21);
    }
}