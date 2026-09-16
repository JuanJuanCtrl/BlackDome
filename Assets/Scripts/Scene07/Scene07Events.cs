using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Scene07Event : MonoBehaviour
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
        "Anyways bro, you said we should wait till midnight for him to appear? I guess that makes sense, since his name is Midnight and all, but still...",
        "[He softly groans]",
        "It's 8:00 PM right now, so we have a few hours to kill.",
        "We don't really have anywhere to go, and I doubt they'd let some high schoolers roam the streets at night.",
        "They even caught me once trying to sneak into the arcade, that was like a year ago...",
        "I don't even know why I'm telling you this, but you don't seem to judge. I guess that's why we're friends...",
        "[You and Koda kept talking for a while, eventually going inside to wait for midnight to come. What could Midnight say tonight...]"
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
                textToSpeak = "Okay, we're here. I'm intrigued to see this cat you called Midnight.";
                break;
            case 1:
                ShowExpression("embarrassed");
                textToSpeak = "Is it like a cute little kitten, or a big scary cat? Either way I wanna see it!";
                break;
            case 2:
                ShowExpression("neutral");
                textToSpeak = "I hope it answers my question about what that weird blue fog is...";
                break;
            case 3:
                ShowExpression("neutral");
                textToSpeak = "[You felt as if your minds were interconnected with the same thoughts and questions.]";
                break;
            case 4:
                ShowExpression("neutral");
                textToSpeak = "[What would Midnight say tonight, and what could he be hiding from you?]";
                break;
            case 5:
                ShowExpression("neutral");
                textToSpeak = "[You thought about this for awhile, Koda turned to you, waiting for your input.]";
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
                ShowExpression("embarrassed");
                choiceLines = new string[]
                {
                    "[Koda looked at you with a puzzled expression.] Not a real cat?",
                    "Like, not a physical talking cat? ",
                    "A figure that only appears on your phone?",
                    "Dude, I can't tell if you're joking or not. By the looks of it, joking is the last thing on your mind"
                };
                break;
            case 1:
                ShowExpression("smirk");
                choiceLines = new string[]
                {
                    "[he supresses a chuckle] I see what you did there, quite a joker Asa!",
                    "What? You said he's a cat? Inside of your phone? Not a physical talking cat?",
                    "Well... that sounds, interesting. It makes me want to meet it even more.",
                };
                break;
            case 2:
                ShowExpression("embarrassed");
                choiceLines = new string[]
                {
                    "Beyond scary?!",
                    "Dude, you said he lives inside your phone, right? How is that even possible? How could he even be scary, now that I realize.",
                    "You're messing with me, he's probably not that scary, right?",
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

        ShowExpression("neutral");
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
        SceneManager.LoadScene(13);
    }
}