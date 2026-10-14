using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InsideTheTranspod01 : MonoBehaviour
{
    [Header("Character Expressions")]
    public GameObject charNeutral;
    public GameObject charSurprised;
    public GameObject charHappy;
    public GameObject charEmbarrassed;
    public GameObject charConfused;
    public GameObject charSad;
    public GameObject charExcited;

    public GameObject fadeScreenIn;
    public GameObject textBox;
    [SerializeField] string textToSpeak;
    [SerializeField] int currentTextLength;
    [SerializeField] int textLength;
    [SerializeField] GameObject mainTextObject;
    [SerializeField] GameObject nextButton;
    [SerializeField] int eventPos = 0;
    [SerializeField] GameObject charName;
    [SerializeField] GameObject fadeOut;

    int introIndex = 0;
    bool isTyping = false; // Prevents spamming Next

    void Update()
    {
        textLength = TextCreator.charCount;
    }

    void Start()
    {
        StartCoroutine(EventStarter());
        nextButton.SetActive(false); // Ensure it's hidden at start
    }

    void HideAllExpressions()
    {
        charNeutral.SetActive(false);
        charSurprised.SetActive(false);
        charHappy.SetActive(false);
        charEmbarrassed.SetActive(false);
        charConfused.SetActive(false);
        charSad.SetActive(false);
        charExcited.SetActive(false);
    }

    void ShowExpression(string expression)
    {
        HideAllExpressions();
        switch (expression)
        {
            case "neutral":     charNeutral.SetActive(true); break;
            case "surprised":   charSurprised.SetActive(true); break;
            case "happy":       charHappy.SetActive(true); break;
            case "smile":       charHappy.SetActive(true); break;
            case "embarrassed": charEmbarrassed.SetActive(true); break;
            case "confused":    charConfused.SetActive(true); break;
            case "sad":         charSad.SetActive(true); break;
            case "excited":     charExcited.SetActive(true); break;
        }
    }

    IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(2f);
        fadeScreenIn.SetActive(false);
        ShowExpression("neutral");
        yield return new WaitForSeconds(2f);
        mainTextObject.SetActive(true);
        textBox.SetActive(true);
        charName.GetComponent<TMP_Text>().text = "Unknown Girl";

        introIndex = 0;
        yield return StartCoroutine(PlayIntroLine(introIndex));
        eventPos = 0;
    }

    IEnumerator PlayIntroLine(int index)
    {
        nextButton.SetActive(false);
        isTyping = true;
        switch (index)
        {
            case 0:
                ShowExpression("smile");
                textToSpeak = "Once inside, the Transpod's door closed automatically when it detected Agnes' presence.";
                break;
            case 1:
                ShowExpression("smile");
                textToSpeak = "Bay looked around the Transpod, amazed.";
                break;
            case 2:
                ShowExpression("smile");
                textToSpeak = "Agnes noticed his expression and smiled.";
                break;
            case 3:
                ShowExpression("smile");
                textToSpeak = "The girl opened her mouth to speak:";
                break;
            case 4:
                ShowExpression("smile");
                textToSpeak = "Pretty cool, huh?";
                break;
            case 5:
                ShowExpression("smile");
                textToSpeak = "Transpoew have this mechanism where it's able to detect its owner, and it opens and closes automatically when it does.";
                break;
            case 6:
                ShowExpression("smile");
                textToSpeak = "Transpods are blank when manufactured by default, and they are assigned to you once you're old enough.";
                break;
            case 7:
                ShowExpression("smile");
                textToSpeak = "[Agnes continued to talk about her Transpod passionately, but once it was over—her expressiion changed.]";
                break;
            case 8:
                ShowExpression("smile");
                textToSpeak = "[She fiddled with a few buttons and the Transpod was off the ground and floating.]";
                break;
            case 9:
                ShowExpression("smile");
                textToSpeak = "[Bay felt that despite the fast launch, he didn't get flung around.]";
                break;
            case 10:
                ShowExpression("smile");
                textToSpeak = "[But Agnes' expression expression was more... purposeful now.]";
                break;
            case 11:
                ShowExpression("smile");
                textToSpeak = "[Whatever came out of her mouth next, was something he needed to hear.]";
                break;
            case 12:
                ShowExpression("smile");
                textToSpeak = "So, now that we're alone—nobody can listen to us.";
                break;
            case 13:
                ShowExpression("smile");
                textToSpeak = "[She looked out the window, both ways before coming to face him again. Her expression was serious now.]";
                break;
            case 14:
                ShowExpression("smile");
                textToSpeak = "I know who you are, Bay.";
                break;
            case 15:
                ShowExpression("smile");
                textToSpeak = "You are from The Surface, you are what they call a Dustbreather.";
                break;
            case 16:
                ShowExpression("smile");
                textToSpeak = "They don't like people like you around here, you better be safe.";
                break;
            case 17:
                ShowExpression("smile");
                textToSpeak = "Right now, they are probably looking for you. I can help you disguise yourself, give you a place to live.";
                break;
            case 18:
                ShowExpression("smile");
                textToSpeak = "But, it may be difficult—there are those who already know what you look like.";
                break;
            case 19:
                ShowExpression("smile");
                textToSpeak = "They are the ones who we should look out for, in case they snitch...";
                break;
            case 20:
                ShowExpression("smile");
                textToSpeak = "[As she spat new information at him, he looked out the window out of curiosity.]";
                break;
            case 21:
                ShowExpression("smile");
                textToSpeak = "He noticed Transpods like the one they were riding, but they were black and floated close to each others.";
                break;
            case 22:
                ShowExpression("smile");
                textToSpeak = "What do you say?";
                break;

        }

        charName.GetComponent<TMP_Text>().text = index < 4 ? "" : (index == 18 ? "Agnes" : "Unknown Girl");
        textBox.GetComponent<TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreator.runTextPrint = true;

        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => textLength >= currentTextLength);
        yield return new WaitForSeconds(0.5f);
        nextButton.SetActive(true);
        isTyping = false;
    }

    // NextButton to be called by Unity UI OnClick
    public void NextButton()
    {
        if (isTyping) return; // Don't skip the typing animation

        introIndex++;
        if (introIndex <= 28)
        {
            StartCoroutine(PlayIntroLine(introIndex));
        }
        else
        {
            StartCoroutine(EventFour());
        }
    }

    IEnumerator EventFour()
    {
        nextButton.SetActive(false);
        ShowExpression("happy");
        textBox.SetActive(true);
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(13);
    }
}
