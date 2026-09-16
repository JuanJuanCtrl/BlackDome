using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Scene05Event : MonoBehaviour
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
        "Anyways, the train's gonna get to school pretty soon...",
        "[He softly groans]",
        "There was a weird sound coming from outside my house, it may have just been the wind",
        "I looked outside the window, and everything was... oddly blue.",
        "I swear I haven't gone crazy, bro. I'll even snap some pictures to show you later!",
        "Oh, we're already here. You gonna let me copy off your homework, right?",
        "[Koda's situation was similar to yours, you couldn't help but feel a bit uneasy about the whole situation. You decided to keep it to yourself for now, not wanting to worry about it.]"
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
                textToSpeak = "Yo! Good morning bro, you good?";
                break;
            case 1:
                ShowExpression("smile");
                textToSpeak = "You were like, practically dragging me yesterday!";
                break;
            case 2:
                ShowExpression("embarrassed");
                textToSpeak = "I had to go running home, I hate walking at night...";
                break;
            case 3:
                ShowExpression("smirk");
                textToSpeak = "[Your mind went back to the events that occurred the previous night. What was that all about?]";
                break;
            case 4:
                ShowExpression("smirk");
                textToSpeak = "[You felt the urge to tell Koda about what happened.]";
                break;
            case 5:
                ShowExpression("smirk");
                textToSpeak = "[How should you approach this?]";
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
                    "[Koda looked at you, scanning your face for any signs of joking as you told him what happened last night.]",
                    "A talking cat inside of your phone? Dude, is the phone possessed or something?",
                    "I mean, it's not like I haven't heard of that kind of stuff before, but still...",
                    "Maybe you were just seeing things, I mean everyone sees stuff once in a while, right? It's probably just your imagination running wild.",
                    "That must be it [laughs nerviously]"
                };
                break;
            case 1:
                ShowExpression("embarrassed");
                choiceLines = new string[]
                {
                    "[Koda gave you a weird look as you told him what haoppened last night.]",
                    "A talking cat, inside of your phone? That sounds like a dream, sure you weren't dreaming bro?",
                    "Sounds like you were just seeing things, I mean everyone sees stuff once in a while, right? It's probably just your imagination running wild.",
                };
                break;
            case 2:
                ShowExpression("embarrassed");
                choiceLines = new string[]
                {
                    "Cat go meow meow, phone go ring ring, and you go scream scream?",
                    "[laughs nervously]",
                    "Did you get enough sleep yesterday? Sounds like a bunch of nonsense to me, bro. You probably just had a nightmare or something.",
                    "Yeah, just a nightmare..."
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
        SceneManager.LoadScene(11);
    }
}