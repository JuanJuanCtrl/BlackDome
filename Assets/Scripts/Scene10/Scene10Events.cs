using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;

public class Scene10Event : MonoBehaviour
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
    [FormerlySerializedAs("charMidnightConfused")]
    public GameObject charHiromiNeutral;
    [FormerlySerializedAs("charMidnightAngry")]
    public GameObject charHiromiAngry;
    [FormerlySerializedAs("charMidnightDelighted")]
    public GameObject charHiromiDelighted;
    [FormerlySerializedAs("charMidnightEmbarrassed")]
    public GameObject charHiromiShocked;
    [FormerlySerializedAs("charMidnightHappy")]
    public GameObject charHiromiSmile;
    public GameObject charHiromiSmug;
    public GameObject charKodaEmbarrassed;
    public GameObject charKodaNeutral;
    public GameObject charKodaSurprised;
    public GameObject charKodaSmile;
    public GameObject charKodaSmirk;
    public GameObject KodaBase;

    public GameObject fadeScreenIn;
    public GameObject textBox;

    [SerializeField] private string speakerName = "Hiromi";
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

[You and Koda arrived at Kasato Downtown by travelling via train. Koda looks rather excited]

Koda

Okay bro, welcome to the real Kasato!

Isn't it amazing? It's better than the dull school hallways, there's a lot of us students who hang around these areas.

Let's go, we can't possibly waste any more time, let's hit Nabana Diner!

[Just when you two started walking, you felt like someone bumped into you. You went and looked back at who it was.]

Huh, what's up br-

Oh, look who it is! [he turned to the unknown girl]

Hiromi

KODA! YOU STOLE MY HOMEWORK YESTERDAY!!!

YOU KNOW I HAD TO TURN THAT IN, GIVE IT BACK!

[She looked at him angrily, then looked at you, calming down.]

Oh, aren't you that new guy? What's your name again?

Koda

[interrupting]

His name is Asa, and...

[Koda looks over at you]

She's Hiromi Abara, she's my best friend. She's the one I was teasing you about on your first day here!

Hiromi

[He looked over at Koda]

You what?! You know I don't wanna date anyone, Koda!

[She looks back at you]

Sorry, this doofus always has something to say. You're not the only one, but definitely his favorite. Ugh!

I'm in your same class, I'm the girl sitting next to Koda. He can be quite a handful, I know...

[He looked at Koda]

Where are you two going anyways, wait let me guess...

Nabana? [looks at Koda]

Koda

Of course we are, wanna come? You'll have to pay for your own food though, I'm already treating Asa here.

Hiromi

WHAT?! Don't you have a sense of chivalry, why not pay for your best friend? I literally bought you that shirt for your birthday last year!

Koda

[embarassed] Uhh, well... Let's just go to Nabana Diner!

Hiromi

[with a smug grin] Knew it, same old Koda. Bring up something like that and he'll stop being a headache.

Let's go, I'm Hiromi Abara by the way, I guess Koda already told you that.

[She looked at Koda, already walking away in a rush]

W-wait for me, I got small legs!

[You started walking with the two, feeling like you just made a new friend. Hiromi Abara]

[You completely pushed the thoughts about Midnight aside and enjoyed your time with them. What could possibly happen if you didn't?]";

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
            Debug.LogWarning("Scene10Event could not parse any dialogue lines.");
        }
    }

    bool IsSpeakerHeader(string line)
    {
        string normalizedLine = NormalizeSpeakerHeader(line);
        return normalizedLine == "You" || normalizedLine == "Hiromi" || normalizedLine == "Koda";
    }

    string NormalizeSpeakerHeader(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return string.Empty;
        }

        string trimmed = line.Trim();
        if (trimmed.StartsWith("(") && trimmed.EndsWith(")") && trimmed.Length > 2)
        {
            return trimmed.Substring(1, trimmed.Length - 2).Trim();
        }

        if (trimmed == "Brown-haired Girl")
        {
            return "Hiromi";
        }

        return trimmed;
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
        charHiromiNeutral.SetActive(false);
        charHiromiAngry.SetActive(false);
        charHiromiDelighted.SetActive(false);
        charHiromiShocked.SetActive(false);
        charHiromiSmile.SetActive(false);
        charHiromiSmug.SetActive(false);
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

    void ShowHiromiExpression(string expression)
    {
        HideAllCharacterExpressions();

        switch (expression)
        {
            case "neutral":
                charHiromiNeutral.SetActive(true);
                break;
            case "angry":
                charHiromiAngry.SetActive(true);
                break;
            case "delighted":
                charHiromiDelighted.SetActive(true);
                break;
            case "shocked":
                charHiromiShocked.SetActive(true);
                break;
            case "smile":
                charHiromiSmile.SetActive(true);
                break;
            case "smug":
                charHiromiSmug.SetActive(true);
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

    IEnumerator FadeOutHiromiExpressions(float duration = 1f)
    {
        GameObject[] expressions = {
            charHiromiNeutral,
            charHiromiAngry,
            charHiromiDelighted,
            charHiromiShocked,
            charHiromiSmile,
            charHiromiSmug
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
            charHiromiNeutral,
            charHiromiAngry,
            charHiromiDelighted,
            charHiromiShocked,
            charHiromiSmile,
            charHiromiSmug,
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
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                ShowKodaFocusedExpression("smile");
                break;
            case 7:
            case 8:
            case 9:
                ShowHiromiExpression("angry");
                break;
            case 10:
                ShowHiromiExpression("shocked");
                break;
            case 11:
                ShowKodaFocusedExpression("neutral");
                break;
            case 12:
            case 13:
            case 14:
                ShowKodaFocusedExpression("smile");
                break;
            case 15:
            case 16:
                ShowHiromiExpression("angry");
                break;
            case 17:
            case 19:
            case 20:
                ShowHiromiExpression("neutral");
                break;
            case 18:
                ShowHiromiExpression("smile");
                break;
            case 21:
            case 22:
                ShowHiromiExpression("shocked");
                break;
            case 23:
                ShowKodaFocusedExpression("smile");
                break;
            case 24:
                ShowHiromiExpression("angry");
                break;
            case 25:
                ShowKodaFocusedExpression("embarrassed");
                break;
            case 26:
                ShowHiromiExpression("smug");
                break;
            case 27:
                ShowHiromiExpression("smile");
                break;
            case 28:
                ShowHiromiExpression("neutral");
                break;
            case 29:
                ShowHiromiExpression("shocked");
                break;
            case 30:
                ShowHiromiExpression("smile");
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

        if (speakerName == "Hiromi" && textToSpeak.Contains("screen turned black before the app closed"))
        {
            StartCoroutine(FadeOutHiromiExpressions(1.5f));
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
        SceneManager.LoadScene(19);
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
