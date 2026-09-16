using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene01Events : MonoBehaviour
{
    [Header("Character Expressions")]
    public GameObject charKodaNeutral;
    public GameObject charKodaSurprised;
    public GameObject charKodaSmirk;
    public GameObject charKodaSmile;
    public GameObject charKodaEmbarrassed;
    
    public GameObject fadeScreenIn;
    public GameObject textBox;
    [SerializeField] AudioSource girlSigh;
    [SerializeField] AudioSource girlGasp;
    [SerializeField] string textToSpeak;
    [SerializeField] int currentTextLength;
    [SerializeField] int textLength;
    [SerializeField] GameObject mainTextObject;
    [SerializeField] GameObject nextButton;
    [SerializeField] int eventPos = 0;
    [SerializeField] GameObject charName;
    [SerializeField] GameObject fadeOut;

    // index within the intro dialogue sequence
    int introIndex = 0;
    // index within the Koda dialogue sequence (Event 1)
    int kodaIndex = 0;

    void Update()
    {
        textLength = TextCreator.charCount;
    }

    void Start()
    {
        StartCoroutine(EventStarter());
    }

    void HideAllExpressions()
    {
        charKodaNeutral.SetActive(false);
        charKodaSurprised.SetActive(false);
        charKodaSmirk.SetActive(false);
        charKodaSmile.SetActive(false);
        charKodaEmbarrassed.SetActive(false);
    }

    void ShowExpression(string expression)
    {
        HideAllExpressions();
        switch (expression)
        {
            case "neutral":
                charKodaNeutral.SetActive(true);
                break;
            case "surprised":
                charKodaSurprised.SetActive(true);
                break;
            case "smirk":
                charKodaSmirk.SetActive(true);
                break;
            case "smile":
                charKodaSmile.SetActive(true);
                break;
            case "embarrassed":
                charKodaEmbarrassed.SetActive(true);
                break;
        }
    }

    IEnumerator EventStarter()
    {
        // initial fade & character appear (Koda neutral initially)
        yield return new WaitForSeconds(2f);
        fadeScreenIn.SetActive(false);
        ShowExpression("neutral");
        yield return new WaitForSeconds(2f);

        mainTextObject.SetActive(true);
        textBox.SetActive(true);
        charName.GetComponent<TMPro.TMP_Text>().text = "Koda";

        // start first intro line
        introIndex = 0;
        yield return StartCoroutine(PlayIntroLine(introIndex));

        eventPos = 0; // 0 = still in intro sequence
    }

    IEnumerator PlayIntroLine(int index)
    {
        nextButton.SetActive(false);

        // Set expression based on line
        switch (index)
        {
            case 0: // "Oh! My bad..."
                ShowExpression("surprised");
                textToSpeak = "Oh! My bad, this always happens on this bump.";
                break;
            case 1: // laugh
                ShowExpression("smile");
                textToSpeak = "[He laughs, brushing his hair back]";
                break;
            case 2: // excited about meeting
                ShowExpression("smile");
                textToSpeak = "Wait… You're the new transfer student, right? No way! I actually got to meet you first.";
                break;
            case 3: // confident
                ShowExpression("smirk");
                textToSpeak = "[He grins] Guess that means I've got a head start on being your first friend!";
                break;
        }

        charName.GetComponent<TMPro.TMP_Text>().text = "Koda";

        textBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreator.runTextPrint = true;

        if (index == 0 && girlSigh != null)
        {
            girlSigh.Play();
        }

        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => textLength >= currentTextLength);
        yield return new WaitForSeconds(0.5f);
        nextButton.SetActive(true);
    }

    IEnumerator PlayKodaLine(int index)
    {
        nextButton.SetActive(false);

        // Set expression and name based on line
        bool isNarration = (index == 0 || index == 6 || index == 7);
        charName.GetComponent<TMPro.TMP_Text>().text = isNarration ? "You" : "Koda";

        switch (index)
        {
            case 0:
                ShowExpression("surprised");
                textToSpeak = "[You feel a surge of energy coursing through you. He facepalms]";
                break;
            case 1:
                ShowExpression("smile");
                textToSpeak = "Oh, I almost forgot to introduce myself. My name is Koda Sable, but you can just call me Koda.";
                break;
            case 2:
                ShowExpression("neutral");
                textToSpeak = "Anyways, we're close to school. Bummer…";
                break;
            case 3:
                ShowExpression("smile");
                textToSpeak = "I hope we have classes together, we can sneak notes and stuff!";
                break;
            case 4:
                ShowExpression("embarrassed"); // Perfect for "lovers" slip-up!
                textToSpeak = "Wait no, that's what lovers do.";
                break;
            case 5:
                ShowExpression("smile");
                textToSpeak = "You know what, let's hang out after school!";
                break;
            case 6:
                ShowExpression("smile"); // Neutral for player narration
                textToSpeak = "[You felt determined to make the most out of this situation. You agreed to hanging out with him after school.]";
                break;
            case 7:
                ShowExpression("smile");
                textToSpeak = "[What could be the worst that could happen?]";
                break;
        }

        textBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreator.runTextPrint = true;

        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => textLength >= currentTextLength);
        yield return new WaitForSeconds(0.5f);
        nextButton.SetActive(true);
    }

    IEnumerator EventFour()
    {
        nextButton.SetActive(false);
        ShowExpression("smile"); // Final smile before fade
        textBox.SetActive(true);
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(3);
    }

    public void NextButton()
    {
        if (eventPos == 0)
        {
            introIndex++;
            if (introIndex <= 3)
            {
                StartCoroutine(PlayIntroLine(introIndex));
            }
            else
            {
                eventPos = 1;
                kodaIndex = 0;
                StartCoroutine(PlayKodaLine(kodaIndex));
            }
            return;
        }

        if (eventPos == 1)
        {
            kodaIndex++;
            if (kodaIndex <= 7)
            {
                StartCoroutine(PlayKodaLine(kodaIndex));
            }
            else
            {
                eventPos = 4;
                StartCoroutine(EventFour());
            }
            return;
        }

        if (eventPos == 4)
        {
            StartCoroutine(EventFour());
        }
    }
}