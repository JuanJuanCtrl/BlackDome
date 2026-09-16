using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene03Event : MonoBehaviour
{
    public GameObject fadeScreenIn;
    public GameObject textBox;
    [SerializeField] private string speakerName = "You";

    [SerializeField] string textToSpeak;
    [SerializeField] int currentTextLength;
    [SerializeField] int textLength;
    [SerializeField] GameObject mainTextObject;
    [SerializeField] GameObject nextButton;
    [SerializeField] int eventPos = 0;
    [SerializeField] GameObject charName;
    [SerializeField] GameObject fadeOut;

    int introIndex = 0;

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
        yield return new WaitForSeconds(2f);
        fadeScreenIn.SetActive(false);
        yield return new WaitForSeconds(2f);

        mainTextObject.SetActive(true);
        textBox.SetActive(true);
        charName.GetComponent<TMPro.TMP_Text>().text = speakerName;

        introIndex = 0;
        yield return StartCoroutine(PlayLine(introIndex));

        eventPos = 0;
    }

    IEnumerator PlayLine(int index)
    {
        nextButton.SetActive(false);

        switch (index)
        {
            case 0:
                textToSpeak = "[You arrived to your apartment after hanging out with Koda]";
                break;
            case 1:
                textToSpeak = "[He quickly scurried back home, as it had gotten pretty late]";
                break;
            case 2:
                textToSpeak = "[Your mind drifted back to that mysterious call…]";
                break;
            case 3:
                textToSpeak = "[Who was that person who called you? How did they find your number?]";
                break;
            case 4:
                textToSpeak = "[Your mind was clouded with unanswered questions]";
                break;
            case 5:
                textToSpeak = "[You decided to shrug it off for the time being.]";
                break;
            case 6:
                textToSpeak = "[What an odd first day of school…]";
                break;
        }

        charName.GetComponent<TMPro.TMP_Text>().text = speakerName;
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
        textBox.SetActive(true);
        fadeOut.SetActive(true);

        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(10);
    }

    public void NextButton()
    {
        if (eventPos == 0)
        {
            introIndex++;
            if (introIndex <= 6)
            {
                StartCoroutine(PlayLine(introIndex));
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