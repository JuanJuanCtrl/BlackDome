using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntoTheStreets02 : MonoBehaviour
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
                ShowExpression("happy");
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
                eventPos = 2;
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
        ShowExpression("happy");
        textBox.SetActive(true);
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(13);
    }
}
