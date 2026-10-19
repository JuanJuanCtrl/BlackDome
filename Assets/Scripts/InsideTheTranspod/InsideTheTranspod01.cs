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

    [Header("UI")]
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
    public GameObject Picture1;

    bool[] characterWasActive;

    [Header("Choice Buttons")]
    public GameObject ChoiceButton1;
    public GameObject ChoiceButton2;
    public GameObject ChoiceButton3;

    int introIndex = 0;
    bool isTyping = false; // Prevents spamming Next

    // Prevents the player from pressing buttons while a choice dialogue is running
    bool choiceDialoguePlaying = false;

    // Used to make choice dialogue wait for the player to click Next
    bool waitingForChoiceNext = false;

    void Update()
    {
        textLength = TextCreator.charCount;
    }

    void Start()
    {
        // Hide the choice buttons at the beginning
        if (ChoiceButton1 != null)
            ChoiceButton1.SetActive(false);

        if (ChoiceButton2 != null)
            ChoiceButton2.SetActive(false);

        if (ChoiceButton3 != null)
            ChoiceButton3.SetActive(false);

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
        charName.GetComponent<TMP_Text>().text = "";

        introIndex = 0;
        yield return StartCoroutine(PlayIntroLine(introIndex));
        eventPos = 0;
    }

    IEnumerator PlayIntroLine(int index)
    {
        nextButton.SetActive(false);

        // Make sure the choices are hidden while normal dialogue is playing
        if (ChoiceButton1 != null)
            ChoiceButton1.SetActive(false);

        if (ChoiceButton2 != null)
            ChoiceButton2.SetActive(false);

        if (ChoiceButton3 != null)
            ChoiceButton3.SetActive(false);

        isTyping = true;
        switch (index)
        {
            case 0:
                ShowExpression("smile");
                textToSpeak = "[Once inside, the Transpod's door closed automatically when it detected Agnes' presence.]";
                break;
            case 1:
                ShowExpression("smile");
                textToSpeak = "[Bay looked around the Transpod, amazed.]";
                break;
            case 2:
                ShowExpression("smile");
                textToSpeak = "[Agnes noticed his expression and smiled.";
                break;
            case 3:
                ShowExpression("smile");
                textToSpeak = "[The girl opened her mouth to speak:]";
                break;
            case 4:
                ShowExpression("smile");
                textToSpeak = "Pretty cool, huh?";
                break;
            case 5:
                ShowExpression("smile");
                textToSpeak = "Transpods have this mechanism where it's able to detect its owner, and it opens and closes automatically when it does.";
                break;
            case 6:
                ShowExpression("smile");
                textToSpeak = "Transpods are blank when manufactured by default, and they are assigned to you once you're old enough.";
                break;
            case 7:
                ShowExpression("smile");
                textToSpeak = "[Agnes continued to talk about her Transpod passionately, but once it was over—her expression changed.]";
                break;
            case 8:
                ShowExpression("smile");
                textToSpeak = "[She fiddled with a few buttons and the Transpod rocketed off the ground and began floating.]";
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
                textToSpeak = "[He noticed Transpods like the one they were riding, but they were black and floated close to each others.]";
                break;
            case 22:
                ShowExpression("smile");
                textToSpeak = "[What do you say?]";
                break;

        }

        charName.GetComponent<TMP_Text>().text = textToSpeak.StartsWith("[") ? "" : "Agnes";
        textBox.GetComponent<TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreator.runTextPrint = true;

        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => textLength >= currentTextLength);
        yield return new WaitForSeconds(0.5f);

        // Fade Picture1 out when Bay looks out the window.
        if (index == 20)
        {
            yield return StartCoroutine(FadeOutPictureAndCharacters());
        }

        // -------------------------------------------------
        // CHOICE POINT (after Case 22)
        // -------------------------------------------------

        if (index == 22)
        {
            // Hide the normal next button
            nextButton.SetActive(false);

            // Show all three choices
            if (ChoiceButton1 != null)
                ChoiceButton1.SetActive(true);

            if (ChoiceButton2 != null)
                ChoiceButton2.SetActive(true);

            if (ChoiceButton3 != null)
                ChoiceButton3.SetActive(true);

            isTyping = false;
            yield break;
        }

        nextButton.SetActive(true);
        isTyping = false;
    }

    // NextButton to be called by Unity UI OnClick
    public void NextButton()
    {
        // -------------------------------------------------
        // CHOICE DIALOGUE
        // -------------------------------------------------

        // If a choice dialogue is currently playing,
        // Next should ONLY be used to advance the current
        // choice line.
        if (choiceDialoguePlaying)
        {
            if (waitingForChoiceNext)
            {
                // Hide the button immediately after clicking it
                nextButton.SetActive(false);

                // Allow PlayChoiceLine() to continue
                waitingForChoiceNext = false;
            }

            return;
        }

        // -------------------------------------------------
        // INTRO SEQUENCE
        // -------------------------------------------------

        if (isTyping) return; // Don't skip the typing animation

        introIndex++;
        if (introIndex <= 22)
        {
            StartCoroutine(PlayIntroLine(introIndex));
        }
        else
        {
            StartCoroutine(EventFour());
        }
    }

    // =====================================================
    // CHOICE 1
    // =====================================================

    public void ChoiceButton1Pressed()
    {
        if (choiceDialoguePlaying)
            return;

        StartCoroutine(PlayChoice1());
    }

    IEnumerator PlayChoice1()
    {
        choiceDialoguePlaying = true;
        HideChoiceButtons();

        string[] choice1Dialogue =
        {
            "[As you mentioned the odd colored Transpods, Agnes quickly took a peek out the window.]",
            "[Her eyes widening as she let out a soft gasp.]",
            "Uh oh, that's not good news.",
            "Thanks for noticing..."
        };

        yield return StartCoroutine(PlayDialogueLines(choice1Dialogue));
        yield return StartCoroutine(PlayDialogueLines(aftermathDialogue));

        FinishChoice();
    }

    // =====================================================
    // CHOICE 2
    // =====================================================

    public void ChoiceButton2Pressed()
    {
        if (choiceDialoguePlaying)
            return;

        StartCoroutine(PlayChoice2());
    }

    IEnumerator PlayChoice2()
    {
        choiceDialoguePlaying = true;
        HideChoiceButtons();

        string[] choice2Dialogue =
        {
            "[At your warning, Agnes quickly looked out thw window.]",
            "[Her eyes widening as she let out a soft gasp.]",
            "Uh oh, I thought they were not around here right now!",
            "Thanks for noticing!"
        };

        yield return StartCoroutine(PlayDialogueLines(choice2Dialogue));
        yield return StartCoroutine(PlayDialogueLines(aftermathDialogue));

        FinishChoice();
    }

    // =====================================================
    // CHOICE 3
    // =====================================================

    public void ChoiceButton3Pressed()
    {
        if (choiceDialoguePlaying)
            return;

        StartCoroutine(PlayChoice3());
    }

    IEnumerator PlayChoice3()
    {
        choiceDialoguePlaying = true;
        HideChoiceButtons();

        string[] choice3Dialogue =
        {
            "[Agnes smiled, thinking you were looking at other people's Transpods.]",
            "[She looked out the window.]",
            "Wait...",
            "Those are definitely not sick!",
            "Thanks for noticing though..."
        };

        yield return StartCoroutine(PlayDialogueLines(choice3Dialogue));
        yield return StartCoroutine(PlayDialogueLines(aftermathDialogue));

        FinishChoice();
    }

    // =====================================================
    // DIALOGUE THAT PLAYS AFTER ANY OF THE THREE CHOICES
    // =====================================================

    string[] aftermathDialogue =
    {
        "Those are officers from Dome Civil Protection, we call it the DCP.",
        "I call them the DCPigs.",
        "They mean business, they can scan the streets for any wrongdoing in the domes.",
        "The Dome Control Administration enforces that they do their job correctly.",
        "Everyone hates them, including me.",
        "We better get moving, they're on our tail.",
        "I think they can even scan inside Transpods too, such technology.",
        "[She pressed some buttons on the Transpod pad and it started floating to another location.]",
        "People hate them, we all hate them. Though, it is what it is.",
        "Do the right thing and they won't even acknowledgue you.",
        "Do the slightliest bit of bad, and they'll sniff you up till they find you.",
        "Good thing you're with me, let's get somewhere safer.",
        "[Bay took a deep gulp.]",
        "[Hearing that made him overly nervous, sweat beads forming on his forehead.]",
        "[DCP Officers are able to scan your every movement.]",
        "[And that felt... exposing.]",
        "[Agnes looked the same, but she managed to get the Transpod somewhere off their radars.]"
    };

    // =====================================================
    // HELPERS
    // =====================================================

    void HideChoiceButtons()
    {
        if (ChoiceButton1 != null)
            ChoiceButton1.SetActive(false);

        if (ChoiceButton2 != null)
            ChoiceButton2.SetActive(false);

        if (ChoiceButton3 != null)
            ChoiceButton3.SetActive(false);

        nextButton.SetActive(false);
    }

    // Plays a list of dialogue lines one at a time.
    // Each line waits for the player to click Next.
    IEnumerator PlayDialogueLines(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            yield return StartCoroutine(PlayChoiceLine(lines[i]));
        }
    }

    IEnumerator PlayChoiceLine(string dialogueLine)
    {
        // Hide Next while the new line is typing
        nextButton.SetActive(false);

        // Make sure the waiting state is reset
        waitingForChoiceNext = false;

        // Set dialogue
        textToSpeak = dialogueLine;
        charName.GetComponent<TMP_Text>().text = textToSpeak.StartsWith("[") ? "" : "Agnes";

        textBox.GetComponent<TMP_Text>().text = textToSpeak;

        currentTextLength = textToSpeak.Length;

        // Reset typewriter counter
        TextCreator.charCount = 0;

        // Start typewriter
        TextCreator.runTextPrint = true;

        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1f);

        // Wait until the entire line has finished typing
        yield return new WaitUntil(() => textLength >= currentTextLength);

        // Fade Picture1 back in when the DCP is revealed.
        if (dialogueLine == "Those are officers from Dome Civil Protection, we call it the DCP.")
        {
            yield return StartCoroutine(FadeInPictureAndCharacters());
        }

        // The line is completely typed.
        // Show Next so the player can manually continue.
        nextButton.SetActive(true);

        // Tell NextButton() that we are waiting for a click.
        waitingForChoiceNext = true;

        // IMPORTANT:
        // Do NOT continue automatically.
        // Wait here until the player presses Next.
        yield return new WaitUntil(() => waitingForChoiceNext == false);
    }

    GameObject[] GetCharacterImages()
    {
        return new GameObject[]
        {
            charNeutral,
            charSurprised,
            charHappy,
            charEmbarrassed,
            charConfused,
            charSad,
            charExcited
        };
    }

    IEnumerator FadeOutPictureAndCharacters()
    {
        GameObject[] characters = GetCharacterImages();
        characterWasActive = new bool[characters.Length];

        for (int i = 0; i < characters.Length; i++)
        {
            characterWasActive[i] = characters[i] != null && characters[i].activeSelf;
        }

        yield return StartCoroutine(FadeObjects(Picture1, characters, false));
    }

    IEnumerator FadeInPictureAndCharacters()
    {
        GameObject[] characters = GetCharacterImages();
        yield return StartCoroutine(FadeObjects(Picture1, characters, true));
    }

    IEnumerator FadeObjects(GameObject picture, GameObject[] characters, bool fadeIn)
    {
        int objectCount = characters.Length + 1;
        CanvasGroup[] canvasGroups = new CanvasGroup[objectCount];
        GameObject[] objects = new GameObject[objectCount];
        objects[0] = picture;

        for (int i = 0; i < characters.Length; i++)
        {
            objects[i + 1] = characters[i];
        }

        for (int i = 0; i < objectCount; i++)
        {
            if (objects[i] == null)
                continue;

            if (fadeIn && (i == 0 || characterWasActive == null || characterWasActive[i - 1]))
                objects[i].SetActive(true);

            canvasGroups[i] = objects[i].GetComponent<CanvasGroup>();
            if (canvasGroups[i] == null)
                canvasGroups[i] = objects[i].AddComponent<CanvasGroup>();
        }

        float[] startAlphas = new float[objectCount];
        for (int i = 0; i < objectCount; i++)
        {
            if (canvasGroups[i] == null)
                continue;

            startAlphas[i] = fadeIn ? 0f : canvasGroups[i].alpha;
            if (fadeIn)
                canvasGroups[i].alpha = 0f;
        }

        float fadeDuration = 1f;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < objectCount; i++)
            {
                if (canvasGroups[i] == null)
                    continue;

                float targetAlpha = fadeIn ? 1f : 0f;
                canvasGroups[i].alpha = Mathf.Lerp(startAlphas[i], targetAlpha, timer / fadeDuration);
            }

            yield return null;
        }

        for (int i = 0; i < objectCount; i++)
        {
            if (canvasGroups[i] == null)
                continue;

            canvasGroups[i].alpha = fadeIn ? 1f : 0f;
            if (!fadeIn)
                objects[i].SetActive(false);
        }
    }

    void FinishChoice()
    {
        choiceDialoguePlaying = false;

        // Continue to next event
        eventPos = 4;
        StartCoroutine(EventFour());
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
