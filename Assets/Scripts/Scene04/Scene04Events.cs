using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Scene04Event : MonoBehaviour
{
    [Header("Character Expressions")]
    public GameObject charMidnightHappy;
    public GameObject charMidnightEmbarrassed;
    public GameObject charMidnightDelighted;
    public GameObject charMidnightConfused;
    public GameObject charMidnightAngry;

    public GameObject fadeScreenIn;
    public GameObject textBox;

    [SerializeField] private string speakerName = "Midnight";
    [SerializeField] string textToSpeak;
    [SerializeField] int currentTextLength;
    [SerializeField] int textLength;
    [SerializeField] GameObject mainTextObject;
    [SerializeField] GameObject nextButton;
    [SerializeField] int eventPos = 0;
    [SerializeField] GameObject charName;
    [SerializeField] GameObject fadeOut;

    int introIndex = 0;
    bool isFadingOut = false;

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
        charMidnightHappy.SetActive(false);
        charMidnightEmbarrassed.SetActive(false);
        charMidnightDelighted.SetActive(false);
        charMidnightConfused.SetActive(false);
        charMidnightAngry.SetActive(false);
    }

    void ShowExpression(string expression)
    {
        HideAllExpressions();

        switch (expression)
        {
            case "happy":
                charMidnightHappy.SetActive(true);
                break;
            case "embarrassed":
                charMidnightEmbarrassed.SetActive(true);
                break;
            case "delighted":
                charMidnightDelighted.SetActive(true);
                break;
            case "confused":
                charMidnightConfused.SetActive(true);
                break;
            case "angry":
                charMidnightAngry.SetActive(true);
                break;
        }
    }

    void SetExpressionAlpha(GameObject expressionObject, float alpha)
    {
        if (expressionObject == null)
        {
            return;
        }

        CanvasGroup[] canvasGroups = expressionObject.GetComponentsInChildren<CanvasGroup>(true);
        foreach (CanvasGroup canvasGroup in canvasGroups)
        {
            canvasGroup.alpha = alpha;
        }

        Graphic[] graphics = expressionObject.GetComponentsInChildren<Graphic>(true);
        foreach (Graphic graphic in graphics)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }

        SpriteRenderer[] spriteRenderers = expressionObject.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }

    IEnumerator FadeInExpression(string expression)
    {
        GameObject target = null;

        switch (expression)
        {
            case "happy":
                target = charMidnightHappy;
                break;
            case "embarrassed":
                target = charMidnightEmbarrassed;
                break;
            case "delighted":
                target = charMidnightDelighted;
                break;
            case "confused":
                target = charMidnightConfused;
                break;
            case "angry":
                target = charMidnightAngry;
                break;
        }

        if (target == null)
        {
            yield break;
        }

        HideAllExpressions();
        target.SetActive(true);
        SetExpressionAlpha(target, 0f);

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);
            SetExpressionAlpha(target, alpha);
            yield return null;
        }

        SetExpressionAlpha(target, 1f);
    }

    IEnumerator FadeOutAllExpressions(float duration = 1f)
    {
        GameObject[] expressions = {
            charMidnightHappy,
            charMidnightEmbarrassed,
            charMidnightDelighted,
            charMidnightConfused,
            charMidnightAngry
        };

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (elapsed / duration));

            for (int i = 0; i < expressions.Length; i++)
            {
                if (expressions[i] != null && expressions[i].activeSelf)
                {
                    SetExpressionAlpha(expressions[i], alpha);
                }
            }

            yield return null;
        }

        for (int i = 0; i < expressions.Length; i++)
        {
            if (expressions[i] != null)
            {
                SetExpressionAlpha(expressions[i], 0f);
                expressions[i].SetActive(false);
            }
        }
    }

    IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(2f);
        fadeScreenIn.SetActive(false);
        HideAllExpressions();
        yield return new WaitForSeconds(2f);

        mainTextObject.SetActive(true);
        textBox.SetActive(true);

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
                speakerName = "You";
                textToSpeak = "[You woke up in the middle of the night.]";
                break;
            case 1:
                speakerName = "You";
                textToSpeak = "[There is a heavy atmosphere in your bedroom.]";
                break;
            case 2:
                speakerName = "You";
                textToSpeak = "[Your phone's screen was on.]";
                break;
            case 3:
                speakerName = "You";
                textToSpeak = "[A mysterious app called Midnight was open, with a black cat icon.]";
                break;
            case 4:
                speakerName = "You";
                textToSpeak = "[You do not recall installing that app.]";
                break;
            case 5:
                speakerName = "You";
                textToSpeak = "[So you deleted it from your phone.]";
                break;
            case 6:
                speakerName = "You";
                textToSpeak = "[The app was not deleted, and it opened on its own.]";
                break;
            case 7:
                speakerName = "You";
                textToSpeak = "[There was a white feline face on your screen, but...]";
                break;
            case 8:
                speakerName = "Unknown";
                StartCoroutine(FadeInExpression("happy"));
                textToSpeak = "H-hello?";
                break;
            case 9:
                speakerName = "Unknown";
                ShowExpression("delighted");
                textToSpeak = "Thank God! I was able to find a connection!";
                break;
            case 10:
                speakerName = "Unknown";
                ShowExpression("delighted");
                textToSpeak = "You must be that person I was looking for...";
                break;
            case 11:
                speakerName = "Unknown";
                ShowExpression("confused");
                textToSpeak = "Asa, ain't it?";
                break;
            case 12:
                speakerName = "You";
                ShowExpression("confused");
                textToSpeak = "[You don't know whoever is talking to you, but the person on the other side seems to know you.]";
                break;
            case 13:
                speakerName = "Midnight";
                ShowExpression("happy");
                textToSpeak = "The name's Midnight, I live inside your phone. That's how I know your name!";
                break;
            case 14:
                speakerName = "Midnight";
                ShowExpression("happy");
                textToSpeak = "Make sure to keep it charged!";
                break;
            case 15:
                speakerName = "Midnight";
                ShowExpression("happy");
                textToSpeak = "Anyways, you wouldn't mind if I used your time for a bit?";
                break;
            case 16:
                speakerName = "You";
                ShowExpression("confused");
                textToSpeak = "[He seems to have given you a choice, but you cannot escape it. You feel like you have to agree no matter how he puts it.]";
                break;
            case 17:
                speakerName = "Midnight";
                ShowExpression("delighted");
                textToSpeak = "Great! Now, how do I say this...";
                break;
            case 18:
                speakerName = "Midnight";
                ShowExpression("delighted");
                textToSpeak = "You see, I've made a grand discovery that'll sure shake this world of yours.";
                break;
            case 19:
                speakerName = "Midnight";
                ShowExpression("confused");
                textToSpeak = "Haven't you noticed some of the people outside acting weird?";
                break;
            case 20:
                speakerName = "Midnight";
                ShowExpression("confused");
                textToSpeak = "They even sound weird, I can't make this up!";
                break;
            case 21:
                speakerName = "You";
                ShowExpression("confused");
                textToSpeak = "[You felt the desperate tone in his boyish, slightly raspy voice. You feel rather confused.]";
                break;
            case 22:
                speakerName = "Midnight";
                ShowExpression("delighted");
                textToSpeak = "I... can't prove it yet, but I'm willing to investigate further!";
                break;
            case 23:
                speakerName = "Midnight";
                ShowExpression("delighted");
                textToSpeak = "If you could take me on regular walks, I'll collect enough information to back my evidence up!";
                break;
            case 24:
                speakerName = "Midnight";
                ShowExpression("confused");
                textToSpeak = "Although I hope it isn't weird taking your phone on a walk, right?";
                break;
            case 25:
                speakerName = "Midnight";
                ShowExpression("embarrassed");
                textToSpeak = "Um, I also need help with something, but I guess I don't wanna keep you awake all night.";
                break;
            case 26:
                speakerName = "Midnight";
                ShowExpression("happy");
                textToSpeak = "Let's leave that for tomorrow. Nighty-night, Asa!";
                break;
            case 27:
                speakerName = "You";
                ShowExpression("happy");
                textToSpeak = "[With that, your phone's screen went black and the app closed, leaving you on the homescreen. You put your phone away and went back to sleep, the atmosphere felt lighter.]";
                StartCoroutine(FadeOutAllExpressions(1.5f));
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
        SceneManager.LoadScene(8);
    }

    public void NextButton()
    {
        if (isFadingOut)
        {
            return;
        }

        if (eventPos == 0)
        {
            introIndex++;
            if (introIndex <= 27)
            {
                StartCoroutine(PlayLine(introIndex));
            }
            else
            {
                eventPos = 4;
                isFadingOut = true;
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