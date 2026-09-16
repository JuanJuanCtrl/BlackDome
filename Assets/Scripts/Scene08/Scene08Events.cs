using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Scene08Event : MonoBehaviour
{
    [System.Serializable]
    private class DialogueEntry
    {
        public string speaker;
        public string text;

        public DialogueEntry(string speaker, string text)
        {
            this.speaker = speaker;
            this.text = text;
        }
    }

    [Header("Dialogue Source")]
    [SerializeField] private TextAsset sceneDialogueFile;

    [Header("Character Expressions")]
    public GameObject charMidnightHappy;
    public GameObject charMidnightEmbarrassed;
    public GameObject charMidnightDelighted;
    public GameObject charMidnightConfused;
    public GameObject charMidnightAngry;
    public GameObject charKodaEmbarrassed;
    public GameObject charKodaNeutral;
    public GameObject charKodaSurprised;
    public GameObject charKodaSmile;
    public GameObject charKodaSmirk;
    public GameObject KodaBase;

    public GameObject fadeScreenIn;
    public GameObject textBox;

    [SerializeField] private string speakerName = "Midnight";
    [SerializeField] private string textToSpeak;
    [SerializeField] private int currentTextLength;
    [SerializeField] private int textLength;
    [SerializeField] private GameObject mainTextObject;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private int eventPos = 0;
    [SerializeField] private GameObject charName;
    [SerializeField] private GameObject fadeOut;

    private readonly List<DialogueEntry> dialogueLines = new List<DialogueEntry>();
    private int dialogueIndex = 0;
    private bool isFadingOut = false;

    private const string DefaultSceneDialogue = @"You

[You and Koda waited for a long time, before your phone's screen went white.]

[After a few seconds, the white feline from before appeared on your screen.]

Midnight

Oh, Asa! About yesterd-

[He stopped, the digital eyes surprised by the sight of another human next to you]

Asa, who's this? [He said, curiously.]

Is he a friend of yours? Why is he here, it's midnight!

[He looked at Koda, then back at you]

Judging by the stars outside and the set time on your phone, it's past your bedtime!

[Koda looked at you, puzzled. He leaned into your ear, whispering.]

Koda

So... This is Midnight, he acts as if he were your mother.

He's not your mother, right? Well, he doesn't look scary at all!

I knew it, he's a cute kitten!

[Midnight's digital eyes moved to Koda, giving him a warning glare.]

Midnight

I'm not a cute kitty! I'm a grown cat, I'm 2 years old. That's around
25 years old if converted to human years!

Which means... I'm older than you! [He said, almost offended]

[A digital sigh came off Midnight, as if he was really alive.]

Look, there's something bizarre I discovered!

It has to do with this weird blue fog, and I'm currently detecting its presence outside in town. The entire town of Kasato.

You two, don't go outside when the clock strikes twelve. [He said frantically.]

That's what I wanted to tell you yesterday, Asa... Everyone should know, I think.

[There was a long silence, like Midnight was doing something in the background.]

I... have to go, they're calling me.

[He said, not letting any more questions be answered for the day. The screen turned black before the app closed, leaving you on the homescreen once again.]

Koda

[with a disappointed look] Hmm... Well he did end up answering my question about the fog.

[he started recalling what it said] Don't go outside when twelve strikes... Don't go out at midnight. That weird blue fog envelops the entire town of Kasato...

Well, I suppose we can wait for tomorrow and see what he says.

[he looked at the clock in your bedroom]
Uh oh, well... That means I can't go home right now. I guess you won't care if I sleep here tonight?

And, do you have any extra pillows? I don't mind sleeping without a blanket.

You

[You began processing everything Midnight had told you about the blue fog, and that it only appears when the clock strikes twelve.]

[What is that blue fog? Where did it come from? What does it mean... Your mind was once again cluttered with unanswerable questions for the time being. You and Koda decided to sleep, and begin planning what to do tomorrow.]";

    void Awake()
    {
        LoadDialogueLines();
    }

    void Update()
    {
        textLength = TextCreator.charCount;
    }

    void Start()
    {
        StartCoroutine(EventStarter());
    }

    void LoadDialogueLines()
    {
        dialogueLines.Clear();

        string rawDialogue = sceneDialogueFile != null && !string.IsNullOrWhiteSpace(sceneDialogueFile.text)
            ? sceneDialogueFile.text
            : DefaultSceneDialogue;

        ParseDialogue(rawDialogue);
    }

    void ParseDialogue(string rawDialogue)
    {
        string currentSpeaker = "You";
        List<string> blockLines = new List<string>();
        string[] lines = rawDialogue.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string trimmed = lines[i].Trim();

            if (IsSpeakerHeader(trimmed))
            {
                FlushDialogueBlock(currentSpeaker, blockLines);
                currentSpeaker = trimmed;
                continue;
            }

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                FlushDialogueBlock(currentSpeaker, blockLines);
                continue;
            }

            blockLines.Add(trimmed);
        }

        FlushDialogueBlock(currentSpeaker, blockLines);

        if (dialogueLines.Count == 0)
        {
            Debug.LogWarning("Scene08Event could not parse any dialogue lines.");
        }
    }

    bool IsSpeakerHeader(string line)
    {
        return line == "You" || line == "Midnight" || line == "Koda";
    }

    void FlushDialogueBlock(string speaker, List<string> blockLines)
    {
        if (blockLines.Count == 0)
        {
            return;
        }

        string text = string.Join(" ", blockLines).Trim();
        dialogueLines.Add(new DialogueEntry(speaker, text));
        blockLines.Clear();
    }

    void HideAllExpressions()
    {
        charMidnightHappy.SetActive(false);
        charMidnightEmbarrassed.SetActive(false);
        charMidnightDelighted.SetActive(false);
        charMidnightConfused.SetActive(false);
        charMidnightAngry.SetActive(false);
    }

    void HideAllKodaExpressions()
    {
        charKodaEmbarrassed.SetActive(false);
        charKodaNeutral.SetActive(false);
        charKodaSurprised.SetActive(false);
        charKodaSmile.SetActive(false);
        charKodaSmirk.SetActive(false);
    }

    void HideAllCharacterExpressions()
    {
        HideAllExpressions();
        HideAllKodaExpressions();
        SetKodaBaseActive(false);
    }

    void SetKodaBaseActive(bool isActive)
    {
        if (KodaBase != null)
        {
            KodaBase.SetActive(isActive);
        }
    }

    void ShowExpression(string expression)
    {
        HideAllCharacterExpressions();

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

    void ShowKodaExpression(string expression)
    {
        HideAllCharacterExpressions();

        switch (expression)
        {
            case "embarrassed":
                charKodaEmbarrassed.SetActive(true);
                SetKodaBaseActive(true);
                break;
            case "neutral":
                charKodaNeutral.SetActive(true);
                SetKodaBaseActive(false);
                break;
            case "surprised":
                charKodaSurprised.SetActive(true);
                SetKodaBaseActive(true);
                break;
            case "smile":
                charKodaSmile.SetActive(true);
                SetKodaBaseActive(true);
                break;
            case "smirk":
                charKodaSmirk.SetActive(true);
                SetKodaBaseActive(true);
                break;
            }
    }

    void ShowMidnightExpression(string expression)
    {
        ShowExpression(expression);
    }

    void ShowKodaFocusedExpression(string expression)
    {
        ShowKodaExpression(expression);
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

    IEnumerator FadeOutMidnightExpressions(float duration = 1f)
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

    void HideAllCharacterExpressionsInstant()
    {
        GameObject[] expressions = {
            charMidnightHappy,
            charMidnightEmbarrassed,
            charMidnightDelighted,
            charMidnightConfused,
            charMidnightAngry,
            charKodaEmbarrassed,
            charKodaNeutral,
            charKodaSurprised,
            charKodaSmile,
            charKodaSmirk
        };

        for (int i = 0; i < expressions.Length; i++)
        {
            if (expressions[i] != null)
            {
                expressions[i].SetActive(false);
            }
        }
    }

    IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(2f);
        fadeScreenIn.SetActive(false);
        HideAllCharacterExpressions();
        ShowKodaExpression("neutral");
        yield return new WaitForSeconds(2f);

        mainTextObject.SetActive(true);
        textBox.SetActive(true);

        dialogueIndex = 0;
        yield return StartCoroutine(PlayLine(dialogueIndex));

        eventPos = 0;
    }

    void ApplyDialogueExpression(int index, string speaker)
    {
        if (speaker == "You")
        {
            if (index == dialogueLines.Count - 1)
            {
                HideAllCharacterExpressionsInstant();
            }

            return;
        }

        switch (index)
        {
            case 2:
                ShowMidnightExpression("happy");
                break;
            case 3:
            case 4:
            case 5:
                ShowMidnightExpression("confused");
                break;
            case 6:
            case 7:
                ShowMidnightExpression("angry");
                break;
            case 8:
                ShowKodaFocusedExpression("surprised");
                break;
            case 9:
                ShowKodaFocusedExpression("smile");
                break;
            case 10:
                ShowKodaFocusedExpression("smirk");
                break;
            case 11:
                ShowKodaFocusedExpression("embarrassed");
                break;
            case 12:
                ShowMidnightExpression("angry");
                break;
            case 13:
            case 14:
                ShowMidnightExpression("angry");
                break;
            case 15:
                ShowMidnightExpression("confused");
                break;
            case 16:
                ShowMidnightExpression("delighted");
                break;
            case 17:
                ShowMidnightExpression("confused");
                break;
            case 18:
                ShowMidnightExpression("angry");
                break;
            case 19:
                ShowMidnightExpression("confused");
                break;
            case 20:
                ShowMidnightExpression("confused");
                break;
            case 21:
                ShowMidnightExpression("angry");
                break;
            case 22:
                ShowMidnightExpression("happy");
                break;
            case 23:
                ShowKodaFocusedExpression("embarrassed");
                break;
            case 24:
                ShowKodaFocusedExpression("neutral");
                break;
            case 25:
            case 26:
                ShowKodaFocusedExpression("neutral");
                break;
            case 27:
            case 28:
                ShowKodaFocusedExpression("embarrassed");
                break;
            case 29:
                ShowKodaFocusedExpression("neutral");
                break;
        }
    }

    IEnumerator PlayLine(int index)
    {
        if (dialogueLines.Count == 0)
        {
            LoadDialogueLines();
        }

        nextButton.SetActive(false);

        if (index < 0 || index >= dialogueLines.Count)
        {
            yield break;
        }

        DialogueEntry entry = dialogueLines[index];
        speakerName = entry.speaker;
        textToSpeak = entry.text;

        ApplyDialogueExpression(index, speakerName);

        if (speakerName == "Midnight" && textToSpeak.Contains("screen turned black before the app closed"))
        {
            StartCoroutine(FadeOutMidnightExpressions(1.5f));
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
        SceneManager.LoadScene(15);
    }

    public void NextButton()
    {
        if (isFadingOut)
        {
            return;
        }

        if (eventPos == 0)
        {
            dialogueIndex++;
            if (dialogueIndex < dialogueLines.Count)
            {
                StartCoroutine(PlayLine(dialogueIndex));
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
