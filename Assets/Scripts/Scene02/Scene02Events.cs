using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene02Events : MonoBehaviour
{
    [Header("UI")]
    public GameObject fadeScreenIn;
    public GameObject textBox;
    public GameObject mainTextObject;
    public GameObject nextButton;
    public GameObject charName;
    public GameObject fadeOut;

    [Header("Choice Buttons")]
    public GameObject ChoiceButton1;
    public GameObject ChoiceButton2;

    [Header("Pictures")]
    public GameObject Picture1;
    public GameObject Picture2;
    public GameObject Picture3;
    public GameObject Picture4;

    [Header("Audio")]
    [SerializeField] AudioSource workAlarm;

    [Header("Dialogue")]
    [SerializeField] string textToSpeak;
    [SerializeField] int currentTextLength;
    [SerializeField] int textLength;

    // Current position in the event
    [SerializeField] int eventPos = 0;

    // Index within the intro dialogue sequence
    int introIndex = 0;

    // Prevents the player from pressing buttons while dialogue is running
    bool choiceDialoguePlaying = false;

    // Used to make choice dialogue wait for the player to click Next
    bool waitingForChoiceNext = false;


    void Update()
    {
        textLength = TextCreator.charCount;
    }


    void Start()
    {
        // Hide choice buttons at the beginning
        if (ChoiceButton1 != null)
            ChoiceButton1.SetActive(false);

        if (ChoiceButton2 != null)
            ChoiceButton2.SetActive(false);

        StartCoroutine(EventStarter());
    }


    IEnumerator EventStarter()
    {
        // Initial fade
        yield return new WaitForSeconds(2f);

        fadeScreenIn.SetActive(false);

        yield return new WaitForSeconds(2f);

        mainTextObject.SetActive(true);
        textBox.SetActive(true);

        // Start first intro line
        introIndex = 0;
        eventPos = 0;

        yield return StartCoroutine(PlayIntroLine(introIndex));
    }


    IEnumerator PlayIntroLine(int index)
    {
        nextButton.SetActive(false);

        // Make sure choices are hidden while normal dialogue is playing
        if (ChoiceButton1 != null)
            ChoiceButton1.SetActive(false);

        if (ChoiceButton2 != null)
            ChoiceButton2.SetActive(false);

        switch (index)
        {
            case 0:
                textToSpeak = "Bay slowly opened his eyes, his world slowly unblurring as everything started to become clear.";
                break;

            case 1:
                textToSpeak = "He was in a clinical environment, a dark windowless room that carried the scent of disinfectants and bleach...";
                break;

            case 2:
                textToSpeak = "...yet dusty at the same time.";
                break;

            case 3:
                textToSpeak = "There was no one in the room, nor did it look like anyone was currently inside the building.";
                break;

            case 4:
                textToSpeak = "He still couldn’t recall anything that had happened to him before arriving to this place.";
                break;

            case 5:
                textToSpeak = "He moved to sit on the bed, then hopped off and unhurriedly made his way to the white door.";
                break;

            case 6:
                textToSpeak = "He turned around and scanned the room one last time. Will you do something before exiting the room?";
                break;

            default:
                yield break;
        }


        // Clear character name during intro
        charName.GetComponent<TMPro.TMP_Text>().text = "";

        // Set dialogue text
        textBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;

        currentTextLength = textToSpeak.Length;

        // Reset text counter
        TextCreator.charCount = 0;

        // Start typewriter effect
        TextCreator.runTextPrint = true;

        // Play alarm if needed
        if (index == 9 && workAlarm != null)
        {
            workAlarm.Play();
        }

        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1f);

        // Wait until all characters have printed
        yield return new WaitUntil(() => textLength >= currentTextLength);

        yield return new WaitForSeconds(0.5f);


        // -------------------------------------------------
        // PICTURE 1
        // -------------------------------------------------

        if (index == 4)
        {
            yield return StartCoroutine(FadeAndDisable(Picture1));
        }


        // -------------------------------------------------
        // PICTURE 2
        // -------------------------------------------------

        if (index == 5)
        {
            yield return StartCoroutine(FadeAndDisable(Picture2));
        }


        // -------------------------------------------------
        // CHOICE POINT
        // -------------------------------------------------

        if (index == 6)
        {
            // Hide the normal next button
            nextButton.SetActive(false);

            // Show both choices
            if (ChoiceButton1 != null)
                ChoiceButton1.SetActive(true);

            if (ChoiceButton2 != null)
                ChoiceButton2.SetActive(true);

            yield break;
        }


        // Allow player to continue normally
        nextButton.SetActive(true);
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

        // Hide both choice buttons
        if (ChoiceButton1 != null)
            ChoiceButton1.SetActive(false);

        if (ChoiceButton2 != null)
            ChoiceButton2.SetActive(false);

        // Hide normal next button
        nextButton.SetActive(false);


        // -------------------------------------------------
        // CHOICE 1:
        // Fade Picture 4 OUT before the dialogue begins
        // -------------------------------------------------

        if (Picture4 != null)
        {
            yield return StartCoroutine(FadeOutPicture(Picture4));
        }


        // -------------------------------------------------
        // CHOICE 1 DIALOGUE
        // -------------------------------------------------

        string[] choice1Dialogue =
        {
            "Bay decided to turn around and see if there was anything that could help him put an answer to his doubts.",
            "As he searched, he only found unimportant documents that didn’t help him much.",
            "After a few minutes, Bay found nothing so he decided to leave the room."
        };


        // Play each line one at a time.
        // PlayChoiceLine will now WAIT for the player
        // to click Next before returning.
        for (int i = 0; i < choice1Dialogue.Length; i++)
        {
            yield return StartCoroutine(PlayChoiceLine(choice1Dialogue[i]));
        }


        // -------------------------------------------------
        // CHOICE 1 FINISHED
        // Fade Picture 3 OUT
        // -------------------------------------------------

        if (Picture3 != null)
        {
            yield return StartCoroutine(FadeAndDisable(Picture3));
        }


        // -------------------------------------------------
        // CHOICE 1 FINISHED
        // Fade Picture 4 BACK IN
        // -------------------------------------------------

        if (Picture4 != null)
        {
            yield return StartCoroutine(FadeInPicture(Picture4));
        }


        choiceDialoguePlaying = false;

        // Continue to next event
        eventPos = 4;

        StartCoroutine(EventFour());
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

        // Hide both choice buttons
        if (ChoiceButton1 != null)
            ChoiceButton1.SetActive(false);

        if (ChoiceButton2 != null)
            ChoiceButton2.SetActive(false);

        // Hide normal next button
        nextButton.SetActive(false);


        // -------------------------------------------------
        // CHOICE 2 DIALOGUE
        // -------------------------------------------------

        string[] choice2Dialogue =
        {
            "Bay decided to turn back around and leave the room.",
            "Nothing relevant lay inside.",
            "Bay slowly made his way to the door and walked out."
        };


        // Play each line one at a time.
        // Each line waits for the player to click Next.
        for (int i = 0; i < choice2Dialogue.Length; i++)
        {
            yield return StartCoroutine(PlayChoiceLine(choice2Dialogue[i]));
        }


        // -------------------------------------------------
        // CHOICE 2 FINISHED
        // Fade Picture 3 OUT
        // -------------------------------------------------

        if (Picture3 != null)
        {
            yield return StartCoroutine(FadeAndDisable(Picture3));
        }


        choiceDialoguePlaying = false;

        // Continue to next event
        eventPos = 4;

        StartCoroutine(EventFour());
    }


    // =====================================================
    // PLAYS ONE CHOICE DIALOGUE LINE
    // =====================================================

    IEnumerator PlayChoiceLine(string dialogueLine)
    {
        // Hide Next while the new line is typing
        nextButton.SetActive(false);

        // Make sure the waiting state is reset
        waitingForChoiceNext = false;


        // Clear character name
        charName.GetComponent<TMPro.TMP_Text>().text = "";


        // Set dialogue
        textToSpeak = dialogueLine;

        textBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;

        currentTextLength = textToSpeak.Length;


        // Reset typewriter counter
        TextCreator.charCount = 0;


        // Start typewriter
        TextCreator.runTextPrint = true;


        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1f);


        // Wait until the entire line has finished typing
        yield return new WaitUntil(() => textLength >= currentTextLength);


        // The line is completely typed.
        // Show Next so the player can manually continue.
        nextButton.SetActive(true);


        // Tell NextButton() that we are waiting for a click.
        waitingForChoiceNext = true;


        // IMPORTANT:
        // Do NOT continue automatically.
        // Wait here until the player presses Next.
        yield return new WaitUntil(() => waitingForChoiceNext == false);


        // The player clicked Next.
        // The coroutine can now return, allowing the
        // for-loop to start the next dialogue line.
    }


    // =====================================================
    // FADE PICTURE OUT
    // =====================================================

    IEnumerator FadeOutPicture(GameObject picture)
    {
        if (picture == null)
        {
            yield break;
        }


        CanvasGroup canvasGroup = picture.GetComponent<CanvasGroup>();


        // Add CanvasGroup if needed
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

            canvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                0f,
                timer / fadeDuration
            );

            yield return null;
        }


        canvasGroup.alpha = 0f;
    }


    // =====================================================
    // FADE PICTURE IN
    // =====================================================

    IEnumerator FadeInPicture(GameObject picture)
    {
        if (picture == null)
        {
            yield break;
        }


        // Make sure picture is active
        picture.SetActive(true);


        CanvasGroup canvasGroup = picture.GetComponent<CanvasGroup>();


        // Add CanvasGroup if needed
        if (canvasGroup == null)
        {
            canvasGroup = picture.AddComponent<CanvasGroup>();
        }


        canvasGroup.alpha = 0f;


        float fadeDuration = 1f;
        float timer = 0f;


        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                0f,
                1f,
                timer / fadeDuration
            );

            yield return null;
        }


        canvasGroup.alpha = 1f;
    }


    // =====================================================
    // FADE AND DISABLE PICTURE
    // =====================================================

    IEnumerator FadeAndDisable(GameObject picture)
    {
        if (picture == null)
        {
            yield break;
        }


        CanvasGroup canvasGroup = picture.GetComponent<CanvasGroup>();


        // If the picture does not already have a CanvasGroup,
        // add one automatically.
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

            canvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                0f,
                timer / fadeDuration
            );

            yield return null;
        }


        canvasGroup.alpha = 0f;


        // Disable the picture after the fade finishes
        picture.SetActive(false);
    }


    // =====================================================
    // EVENT FOUR
    // =====================================================

    IEnumerator EventFour()
    {
        nextButton.SetActive(false);

        textBox.SetActive(true);

        fadeOut.SetActive(true);

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(3);
    }


    // =====================================================
    // NORMAL NEXT BUTTON
    // =====================================================

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

        if (eventPos == 0)
        {
            introIndex++;

            // The choice happens at introIndex 6,
            // so PlayIntroLine(6) will display the buttons.
            if (introIndex <= 6)
            {
                StartCoroutine(PlayIntroLine(introIndex));
            }
            else
            {
                // This should normally not be reached because
                // the choice buttons take over at line 6.
                eventPos = 4;

                StartCoroutine(EventFour());
            }

            return;
        }


        // -------------------------------------------------
        // END EVENT
        // -------------------------------------------------

        if (eventPos == 4)
        {
            StartCoroutine(EventFour());
        }
    }
}