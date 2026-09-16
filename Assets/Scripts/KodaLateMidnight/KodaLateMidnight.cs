using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class KodaLateMidnight : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject textBox;
    [SerializeField] GameObject mainTextObject;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject charName;
    [SerializeField] GameObject fadeOut;

    [Header("Audio")]
    [SerializeField] AudioSource bgm;
    [SerializeField] float startingVolume = 0.094f;

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

        if (bgm != null)
        {
            bgm.volume = startingVolume;
        }

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
                textToSpeak = "You and Hiromi waited for Koda for a long time, you studied with her for some time but decided to stop after Koda started taking too long.";
                break;

            case 1:
                nameText.text = "Hiromi";
                textToSpeak = "Why's it taking him so long to buy some snacks? He's the type to pick those pretty easily!";
                break;

            case 2:
                nameText.text = "Hiromi";
                textToSpeak = "I hope he didn't get mugged or something, we'd have to schedule a rescue mission! [she tried to joke, but it came out more worried than anything.]";
                break;

            case 3:
                nameText.text = "You";
                textToSpeak = "[You felt like hiding the strange blue fog and Midnight was becoming too much to bear, so you went on and explained to her just that...]";
                break;

            case 4:
                nameText.text = "Hiromi";
                textToSpeak = "Oh no, it's 11:50PM! He HAS to get here soon! Or else... If what you're telling me is true...";
                break;

            case 5:
                nameText.text = "Hiromi";
                textToSpeak = "We... We have to go and look for him but... I don't want to go out there, it's too dangerous! I don't want to get hurt or worse, I just want to stay here and wait for him to come back...";
                break;

            case 6:
                nameText.text = "Hiromi";
                textToSpeak = "He's not dumb enough to forget that, I know him better than anyone else... [her voice seems to be wavering as she spoke, so you decided to stop her before she broke into tears.]";
                break;

            case 7:
                nameText.text = "You";
                textToSpeak = "[You tried to comfort her, but she was too scared to listen to you, so you just stayed quiet and waited for Koda to come back.]";
                break;

            case 8:
                nameText.text = "You";
                textToSpeak = "[Your mind went back to Midnight. He seems like the only hope for some kind of explanation of the whole thing...]";
                break;

            case 9:
                nameText.text = "You";
                textToSpeak = "[Time went too fast that you and Hiromi failed to realize...]";
                break;

            case 10:
                nameText.text = "";
                textToSpeak = "[The clock has striked twelve...]";

                // Instantly cut the music
                if (bgm != null)
                {
                    bgm.volume = 0f;
                }

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

        if (introIndex <= 10)
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
        SceneManager.LoadScene(24);
    }
}