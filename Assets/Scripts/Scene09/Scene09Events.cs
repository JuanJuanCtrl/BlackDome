using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Scene09Event : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource studentChatter;

    [Header("Character Expressions")]
    public GameObject charNeutral;
    public GameObject charSurprised;
    public GameObject charSmirk;
    public GameObject charSmile;
    public GameObject charEmbarrassed;

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

    [Header("Choice Buttons")]
    [SerializeField] Button choiceButton1;
    [SerializeField] Button choiceButton2;
    [SerializeField] Button choiceButton3;

    [Header("Choice Lines")]
    private string[] choiceLines;
    private int currentLine = 0;

    // --- Dialogue segment arrays and indices ---
    private string[] transitionLines =
    {
        "Anyways dude, let's hit town before it gets too late!",
        "We can go to that restaurant I told you about after I show you around a bit!",
        "We can even try to get some chicks phone numbers if you're like me [he lightly teased]",
        "Although, I suppose they'll all settle for Koda, the local legend around here [he smirked]",
        "Anyways , let's stop yapping and get going, we got a lot to do before the day ends!",
        "[As Koda finished his sentence, he processed to stretch and then turn to walk]",
        "[You felt like this was a great opportunity to hang out with Koda and get to know him better...]"
    };
    private int transitionLineIndex = 0;

    int introIndex = 0;
    bool isTyping = false; // Prevents spamming Next

    // State flag for end-of-scene transition
    private bool awaitingFinalContinue = false;

    void Update()
    {
        textLength = TextCreator.charCount;
    }

    void Start()
    {
        // --- Audio ----
        if (bgm != null)
        {
            bgm.volume = 0.7f;
            bgm.loop = true;
            if (!bgm.isPlaying) bgm.Play();
        }
        if (studentChatter != null)
        {
            studentChatter.volume = 0.1f;
            studentChatter.loop = true;
            if (!studentChatter.isPlaying) studentChatter.Play();
        }
        // --------------

        StartCoroutine(EventStarter());
        choiceButton1.onClick.AddListener(OnChoice1);
        choiceButton2.onClick.AddListener(OnChoice2);
        choiceButton3.onClick.AddListener(OnChoice3);
        nextButton.SetActive(false); // Ensure it's hidden at start
    }

    void HideAllExpressions()
    {
        charNeutral.SetActive(false);
        charSurprised.SetActive(false);
        charSmirk.SetActive(false);
        charSmile.SetActive(false);
        charEmbarrassed.SetActive(false);
    }

    void ShowExpression(string expression)
    {
        HideAllExpressions();
        switch (expression)
        {
            case "neutral":     charNeutral.SetActive(true); break;
            case "surprised":   charSurprised.SetActive(true); break;
            case "smirk":       charSmirk.SetActive(true); break;
            case "smile":       charSmile.SetActive(true); break;
            case "embarrassed": charEmbarrassed.SetActive(true); break;
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
        charName.GetComponent<TMP_Text>().text = "Koda";

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
                textToSpeak = "Good morning bro, we don't have school today since it's Saturday!";
                break;
            case 1:
                ShowExpression("smirk");
                textToSpeak = "We can finally enjoy the weekend, and... Oh! I can also show you around town, it's all on me!";
                break;
            case 2:
                ShowExpression("smirk");
            textToSpeak = "There's a great restaurant downtown that I think you'd like, it's called Nabana Diner. They got the best steaks, you gotta try Kasato's specialty!";
                break;
            case 3:
                ShowExpression("smirk");
                textToSpeak = "[You felt Koda's excitement, the previous tension from the day before replaced by the thrill of hanging out with you.]";
                break;
            case 4:
                ShowExpression("smirk");
                textToSpeak = "[You remembered what Midnight said last night about not going out during midnight. Koda should remember too, since he was there when you talked to him about it. You wonder if you should tell him again, just to be safe.]";
                break;
            case 5:
                ShowExpression("smirk");
                textToSpeak = "[Koda looks at you with his signature smirk, ready for your answer.]";
                break;
        }

        // Set charName depending on the line (introspective lines use "You")
        if (index == 3 || index == 4 || index == 5)
        {
            charName.GetComponent<TMP_Text>().text = "You";
        }
        else
        {
            charName.GetComponent<TMP_Text>().text = "Koda";
        }
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

    void ShowChoiceButtons()
    {
        choiceButton1.gameObject.SetActive(true);
        choiceButton2.gameObject.SetActive(true);
        choiceButton3.gameObject.SetActive(true);
        nextButton.SetActive(false);
        textBox.SetActive(false);
        mainTextObject.SetActive(false);
    }

    void HideChoiceButtons()
    {
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
        choiceButton3.gameObject.SetActive(false);
    }

    public void OnChoice1()
    {
        HideChoiceButtons();
        StartChoiceDialogue(0);
    }

    public void OnChoice2()
    {
        HideChoiceButtons();
        StartChoiceDialogue(1);
    }

    public void OnChoice3()
    {
        HideChoiceButtons();
        StartChoiceDialogue(2);
    }

    void StartChoiceDialogue(int choice)
    {
        textBox.SetActive(true);
        mainTextObject.SetActive(true);
        nextButton.SetActive(false);

        switch (choice)
        {
            case 0:
                ShowExpression("smile");
                choiceLines = new string[]
                {
                    "Oh, of course bro! I won't forget about that, don't worry.",
                    "Everything is under control...",
                    "...yeah, probably."
                };
                break;
            case 1:
                ShowExpression("smirk");
                choiceLines = new string[]
                {
                    "Yeah, right? I knew you'd be thrilled about hanging out! I'll take you on the ultimate Kasato tour!",
                    "You won't get this experience anywhere else.",
                    "[He has a confident smirk in his face, as if he knew every detail and secret about Kasato.]",
                };
                break;
            case 2:
                ShowExpression("smirk");
                choiceLines = new string[]
                {
                    "Know me? Of course they do, I'm like a local celebritiy around her!",
                    "Except people don't really approach me, nor ask for pictures or autographs, but they know who I am, and that's what matters!",
                    "You can even become pretty famous around here... well, I got my own reasons because of the arcade...",
                };
                break;
        }
        eventPos = 1; // Set event state to choice dialogue
        currentLine = 0;
        StartCoroutine(DisplayChoiceLine(currentLine));
    }

    IEnumerator DisplayChoiceLine(int lineIndex)
    {
        nextButton.SetActive(false);
        isTyping = true;
        if (lineIndex >= 0 && lineIndex < choiceLines.Length)
        {
            textToSpeak = choiceLines[lineIndex];
            charName.GetComponent<TMP_Text>().text = "Koda";
            textBox.GetComponent<TMP_Text>().text = textToSpeak;
            currentTextLength = textToSpeak.Length;
            TextCreator.runTextPrint = true;

            yield return new WaitForSeconds(0.05f);
            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() => textLength >= currentTextLength);
            yield return new WaitForSeconds(0.5f);
            nextButton.SetActive(true);
        }
        isTyping = false;
    }

    IEnumerator DisplayTransitionLine(int lineIndex)
    {
        nextButton.SetActive(false);
        isTyping = true;

        ShowExpression("smile");
        if (lineIndex >= 0 && lineIndex < transitionLines.Length)
        {
            textToSpeak = transitionLines[lineIndex];
            charName.GetComponent<TMP_Text>().text = "Koda";

            textBox.SetActive(true);
            mainTextObject.SetActive(true);
            textBox.GetComponent<TMP_Text>().text = textToSpeak;
            currentTextLength = textToSpeak.Length;
            TextCreator.runTextPrint = true;

            yield return new WaitForSeconds(0.05f);
            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() => textLength >= currentTextLength);
            yield return new WaitForSeconds(0.5f);

            bool isFinalTransitionLine = lineIndex == transitionLines.Length - 1;
            if (isFinalTransitionLine)
            {
                // Instead of starting EventFour here, prompt with the next button
                awaitingFinalContinue = true;   // set flag to wait for final Next press
                nextButton.SetActive(true);     // allow user to continue
                isTyping = false;
                yield break;
            }

            nextButton.SetActive(true);
        }
        isTyping = false;
    }

    // NextButton to be called by Unity UI OnClick
    public void NextButton()
    {
        if (isTyping) return; // Don't skip the typing animation

        // Awaiting user input after the last line before ending scene
        if (awaitingFinalContinue)
        {
            awaitingFinalContinue = false;
            nextButton.SetActive(false);
            StartCoroutine(EventFour());
            return;
        }

        if (eventPos == 0)
        {
            introIndex++;
            if (introIndex <= 5)
            {
                StartCoroutine(PlayIntroLine(introIndex));
            }
            else
            {
                eventPos = 1;
                ShowChoiceButtons();
            }
            return;
        }

        if (eventPos == 1)
        {
            if (currentLine < choiceLines.Length - 1)
            {
                currentLine++;
                StartCoroutine(DisplayChoiceLine(currentLine));
            }
            else
            {
                eventPos = 2;
                nextButton.SetActive(false);
                transitionLineIndex = 0;
                StartCoroutine(DisplayTransitionLine(transitionLineIndex));
            }
            return;
        }

        if (eventPos == 2)
        {
            transitionLineIndex++;
            if (transitionLineIndex < transitionLines.Length)
            {
                StartCoroutine(DisplayTransitionLine(transitionLineIndex));
            }
            // else will hang here, until awaitingFinalContinue above is hit
            return;
        }
    }

    IEnumerator EventFour()
    {
        nextButton.SetActive(false);
        ShowExpression("smile");
        textBox.SetActive(true);
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(17);
    }
}