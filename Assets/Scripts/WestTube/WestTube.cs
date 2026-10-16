using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WestTube : MonoBehaviour
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
        charName.GetComponent<TMP_Text>().text = "";

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
                textToSpeak = "[Agnes guided the Transpod towards a farther area from where they were.]";
                break;
            case 1:
                ShowExpression("smile");
                textToSpeak = "The tall buildings started to dissapear, the modern, dome-shaped houses now filling the space.";
                break;
            case 2:
                ShowExpression("smile");
                textToSpeak = "Agnes managed to land somewhere safely, although it looked like it wasn't her first time doing so.";
                break;
            case 3:
                ShowExpression("smile");
                textToSpeak = "She looked at Bay, who was now calmer, and smiled at him.";
                break;
            case 4:
                ShowExpression("embarrassed");
                textToSpeak = "Sorry for giving you a scare earlier.";
                break;
            case 5:
                ShowExpression("happy");
                textToSpeak = "As I was saying, those are officers from Dome Civil Protection.";
                break;
            case 6:
                ShowExpression("confused");
                textToSpeak = "Shortly, they scan the dome to check if anything bad is happening.";
                break;
            case 7:
                ShowExpression("happy");
                textToSpeak = "Otherwise, they're just minding their own business patrolling the streets.";
                break;
            case 8:
                ShowExpression("smile");
                textToSpeak = "[The Transpod's door opened, and they stepped out. The Transpod's door closed and the window opaqued to black.]";
                break;
            case 9:
                ShowExpression("smile");
                textToSpeak = "[Bay looked around, taking in the sights of the city.]";
                break;
            case 10:
                ShowExpression("smile");
                textToSpeak = "[Suddenly, Agnes pointed at something in the distance.]";
                break;
            case 11:
                ShowExpression("smile");
                textToSpeak = "You see that? We call that tunnel the West Tube.]";
                break;
            case 12:
                ShowExpression("smile");
                textToSpeak = "There are four of those around the dome. South Tube, East Tube, North Tube, and West Tube.";
                break;
            case 13:
                ShowExpression("smile");
                textToSpeak = "They only let people in only if you have a reason to go. Usually, people who work on another dome...";
                break;
            case 14:
                ShowExpression("smile");
                textToSpeak = "...but I use it for something else.";
                break;
            case 15:
                ShowExpression("smile");
                textToSpeak = "[She smiled mischievously, it was clear she had something in mind.]";
                break;
            case 16:
                ShowExpression("smile");
                textToSpeak = "[She had done this before.]";
                break;
            case 17:
                ShowExpression("embarrassed");
                textToSpeak = "You see, Tubes are normally guarded by a DCP officer...";
                break;
            case 18:
                ShowExpression("happy");
                textToSpeak = "...or at least under normal circumstances.";
                break;
            case 19:
                ShowExpression("happy");
                textToSpeak = "[Bay was confused, where was she getting at?]";
                break;
            case 20:
                ShowExpression("happy");
                textToSpeak = "My older brother, Ryder. He's working undercover as a DCP officer guarding the West Tube.";
                break;
            case 21:
                ShowExpression("happy");
                textToSpeak = "He's been doing it for a while now, I reckon about three months or so. Hasn't been caught since.";
                break;
            case 22:
                ShowExpression("smile");
                textToSpeak = "[Bay followed Agnes as she began walking. They climbed up a set of wide stairs that led to the West Tube.]";
                break;
            case 23:
                ShowExpression("smile");
                textToSpeak = "[Bay noticed there was almost no one here, other than a few passerby and the DCP officer, which he knows is Ryder.]";
                break;
            case 24:
                ShowExpression("smile");
                textToSpeak = "[The passerby wore their black full-suits, just like Agnes.]";
                break;
            case 25:
                ShowExpression("smile");
                textToSpeak = "[Finally, they reached the tunnel entrance. A man was standing right there.]";
                break;
            case 26:
                ShowExpression("happy");
                textToSpeak = "[He was wearing a black-armored suit and a black gas that made Bay shiver.]";
                break;
            case 27:
                ShowExpression("happy");
                textToSpeak = "Hey there, we're here for business!";
                break;
            case 28:
                ShowExpression("happy");
                textToSpeak = "[Agnes said with great joy, knowing well it was her brother underneath that intimidating mask.]";
                break;
            case 29:
                ShowExpression("happy");
                textToSpeak = "[Still, she had to put on an act to avoid suspicion, and she did it well.]";
                break;
            case 30:
                ShowExpression("happy");
                textToSpeak = "[Her brother looked at her, then at Bay — his eyes lingering on Bay for a moment before returning to Agnes.]";
                break;
            case 31:
                ShowExpression("happy");
                textToSpeak = "[He began speaking, his voice heavily distorted through the mask, like a robotic voice.]";
                break;
            case 32:
                ShowExpression("happy");
                textToSpeak = "You didn't tell me about your new boyfriend, Aggy.";
                break;
            case 32:
                ShowExpression("embarrassed");
                textToSpeak = "[The mask drained all human warmth from his voice, but he was probably smiling under it.]";
                break;
            case 33:
                ShowExpression("embarrassed");
                textToSpeak = "[Agnes blushed, taking a quick look at Bat before returning her gaze to her brother.]";
                break;
            case 34:
                ShowExpression("embarrassed");
                textToSpeak = "I'm not blushing! And he's not my boyfriend! We're just friends!";
                break;
            case 35:
                ShowExpression("embarrassed");
                textToSpeak = "[Agnes didn't know how to respond, but she knew she had to keep up the act.]";
                break;
            case 36:
                ShowExpression("embarrassed");
                textToSpeak = "[Ryder looked at them, an awkward silence filling the air. He then spoke again:]";
                break;
            case 37:
                ShowExpression("embarrassed");
                textToSpeak = "Whatever, carry on then...";
                break;
            case 38:
                ShowExpression("embarrassed");
                textToSpeak = "[But then, Ryder took a small step towards Bay, locking eyes with him.]";
                break;
            case 39:
                ShowExpression("embarrassed");
                textToSpeak = "Don't you dare lay a hand on my little sister, or else...";
                break;
            case 40:
                ShowExpression("embarrassed");
                textToSpeak = "[He had a threatening tone, but it was clear he was half-joking. What do you say?";
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
        SceneManager.LoadScene(8);
    }
}
