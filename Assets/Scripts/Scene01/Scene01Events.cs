using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene01Events : MonoBehaviour
{
    public GameObject fadeScreenIn;
    public GameObject textBox;

    [SerializeField] AudioSource girlSigh;
    [SerializeField] string textToSpeak;
    [SerializeField] int currentTextLength;
    [SerializeField] int textLength;
    [SerializeField] GameObject mainTextObject;
    [SerializeField] GameObject nextButton;
    [SerializeField] int eventPos = 0;
    [SerializeField] GameObject charName;
    [SerializeField] GameObject fadeOut;

    // Index within the intro dialogue sequence
    int introIndex = 0;

    // Index within the dialogue sequence
    int dialogueIndex = 0;

    void Update()
    {
        textLength = TextCreator.charCount;
    }

    void Start()
    {
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

        switch (index)
        {
            case 0:
                textToSpeak = "Bay could only hear the sounds of people around him, being concious enough to know that he was in a public place, but not concious enough to know where he was or what was going on.";
                break;

            case 1:
                textToSpeak = "Who is this?";
                break;

            case 2:
                textToSpeak = "Where’d he come from?";
                break;

            case 3:
                textToSpeak = "From up there, you silly!";
                break;

            case 4:
                textToSpeak = "Look at his hair, it’s blonde!";
                break;

            case 5:
                textToSpeak = "Is he okay? Is he dead?!";
                break;

            case 6:
                textToSpeak = "...";
                break;

            case 7:
                textToSpeak = "They’re definitely getting him…";
                break;
            case 8:
                textToSpeak = "The loud cacophony of mixed opinions seemed to last forever, until a resonant sound shut everyone up.";
                break;
            case 9:
                textToSpeak = "Three long beeps echoed and bounced off the surface of the dome, a signal for something perhaps?";
                break;
            case 10:
                textToSpeak = "Anyone who stood around him just a few seconds ago were now walking away, resuming their daily lives as if nothing had ever happened.";
                break;
            case 11:
                textToSpeak = "He tried to recall how he’d gotten into this situation, but nothing came.";
                break;
            case 12:
                textToSpeak = "Tiny fragments floated around his brain, but nothing he could use to understand where he was, or who he even was.";
                break;
             case 13:
                textToSpeak = "Then, he heard two voices. One male, one female:";
                break;
            case 14:
                textToSpeak = "He’s alive. Subject A10, name’s Bay Ausman.";
                break;
            case 15:
                textToSpeak = "Yeah, he’s the one — definitely.";
            case 16:
                textToSpeak = "Do you think it’s really possible, I mean—";
            case 17:
                textToSpeak = "Yeah, this is the one. He’ll soon learn that, but for now… let’s take him to the place.";
            case 18:
                textToSpeak = "Yeah, let’s go.";
            case 19:
                textToSpeak = "...";

            default:
                yield break;
        }

        // Clear character name during intro
        charName.GetComponent<TMPro.TMP_Text>().text = "";

        // Set dialogue text
        textBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;

        currentTextLength = textToSpeak.Length;

        // Reset text counter if TextCreator allows it
        TextCreator.charCount = 0;

        // Start typewriter effect
        TextCreator.runTextPrint = true;

        // Play sigh on first intro line
        if (index == 0 && girlSigh != null)
        {
            girlSigh.Play();
        }

        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1f);

        // Wait until all characters have printed
        yield return new WaitUntil(() => textLength >= currentTextLength);

        yield return new WaitForSeconds(0.5f);

        // Allow player to continue
        nextButton.SetActive(true);
    }

    IEnumerator PlayDialogueLine(int index)
    {
        nextButton.SetActive(false);

        bool isNarration = (index == 0 || index == 6 || index == 7);

        charName.GetComponent<TMPro.TMP_Text>().text =
            isNarration ? "You" : "";

        switch (index)
        {
            case 0:
                textToSpeak = "[You feel a surge of energy coursing through you. He facepalms]";
                break;

            case 1:
                textToSpeak = "Oh, I almost forgot to introduce myself. My name is Sable, but you can just call me Sable.";
                break;

            case 2:
                textToSpeak = "Anyways, we're close to school. Bummer…";
                break;

            case 3:
                textToSpeak = "I hope we have classes together, we can sneak notes and stuff!";
                break;

            case 4:
                textToSpeak = "Wait no, that's what lovers do.";
                break;

            case 5:
                textToSpeak = "You know what, let's hang out after school!";
                break;

            case 6:
                textToSpeak = "[You felt determined to make the most out of this situation. You agreed to hanging out with him after school.]";
                break;

            case 7:
                textToSpeak = "[What could be the worst that could happen?]";
                break;

            default:
                yield break;
        }

        // Set dialogue text
        textBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;

        currentTextLength = textToSpeak.Length;

        // Reset text counter
        TextCreator.charCount = 0;

        // Start typewriter effect
        TextCreator.runTextPrint = true;

        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1f);

        // Wait until all characters have printed
        yield return new WaitUntil(() => textLength >= currentTextLength);

        yield return new WaitForSeconds(0.5f);

        // Allow player to continue
        nextButton.SetActive(true);
    }

    IEnumerator EventFour()
    {
        nextButton.SetActive(false);

        textBox.SetActive(true);

        fadeOut.SetActive(true);

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(3);
    }

    public void NextButton()
    {
        // INTRO SEQUENCE
        if (eventPos == 0)
        {
            introIndex++;

            // There are 8 intro lines: 0 through 7
            if (introIndex <= 7)
            {
                StartCoroutine(PlayIntroLine(introIndex));
            }
            else
            {
                // Intro finished
                eventPos = 1;
                dialogueIndex = 0;

                StartCoroutine(PlayDialogueLine(dialogueIndex));
            }

            return;
        }

        // DIALOGUE SEQUENCE
        if (eventPos == 1)
        {
            dialogueIndex++;

            // There are 8 dialogue lines: 0 through 7
            if (dialogueIndex <= 7)
            {
                StartCoroutine(PlayDialogueLine(dialogueIndex));
            }
            else
            {
                // Dialogue finished
                eventPos = 4;

                StartCoroutine(EventFour());
            }

            return;
        }

        // END EVENT
        if (eventPos == 4)
        {
            StartCoroutine(EventFour());
        }
    }
}