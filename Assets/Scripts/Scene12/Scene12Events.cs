using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;

public class Scene12Event : MonoBehaviour
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
    public GameObject nightTownBG;

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

    private const string DefaultSceneDialogue = @"[You were here with Koda and Hiromi, although Koda totally forgot about the whole thing. It's 7PM]

Hiromi

Koda, I swear. I said I wasn't doing anything so late, yet here we are...

Anyways, we have to study this stuff if we all wanna pass. Let's make the best of it!

Koda

O-okay, sorry about the time. I was busy doing other things.

Hiromi

Your friends tell me those ""other things"" are you playing games on Stream, that one app you buy games from.

Koda

[looking embarrassed] Well, the point's that we're all here. I propose we begin with the hardest stuff so we get that out of the way first.

Hiromi

Alright, Mr. Einstein. Let's see...

""A gambler starts their journey with k dollars in their pocket and begins playing a game of chance.

In every round, they win 1 dollar with probability p or lose 1 dollar with probability q, where q is 1 minus p.

The game only ends when they reach a target of N dollars or lose everything and reach a total of 0 dollars.

Given that the game is inherently biased because p is not equal to q, you must calculate the gambler's odds.

Determine the exact probability that the gambler successfully reaches N dollars before they ever hit zero.""

Koda

...

Dude, what in the world is this? I'm never passing that test!

Losing just like the gambler himself...

Hiromi

You said we should tackle the hard questions first, this is just the tip of the iceberg.

Anyways, I don't think this question's too bad. I'm not the smartest person, but I could try...

[Hiromi went on to explain the process for her answer.]

The probability is 1 minus the quantity q divided by p raised to the power of k.
This entire value is then divided by 1 minus the quantity q divided by p raised to the power of N.

See Koda? It isn't that hard! I think I got it!

Koda

Easy for you to say, girls are way smarter than guys...

Hiromi

[with a smug grin] Hehe, you're surrendering that fast?

Koda

[stands up from his chair] This is gonna be a long session, so I'll go buy some stuff first. I won't take so long.

Hiromi

Don't be so late, I still have to get my eight hours of sleep!

[With that, Koda walked over to the entrance and opened the door to the cool air outside. He stepped out]

[You thought about him getting distracted, and if he could get back in time before midnight.]

[You realized Hiromi doesn't know about Midnight or what happens when the clock strikes twelve. You decided to keep this a secret for the time being.]";

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
            Debug.LogWarning("Scene11Event could not parse any dialogue lines.");
        }
    }

    bool IsSpeakerHeader(string line)
    {
        string normalizedLine = NormalizeSpeakerHeader(line);
        return normalizedLine == "You" || normalizedLine == "Hiromi" || normalizedLine == "Koda" || normalizedLine == "Everyone";
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

    bool ContainsAny(string text, params string[] phrases)
    {
        if (string.IsNullOrWhiteSpace(text) || phrases == null)
        {
            return false;
        }

        for (int i = 0; i < phrases.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(phrases[i]) && text.Contains(phrases[i]))
            {
                return true;
            }
        }

        return false;
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

    IEnumerator FadeInGameObject(GameObject targetObject, float duration = 1f)
    {
        if (targetObject == null)
        {
            yield break;
        }

        targetObject.SetActive(true);
        SetExpressionAlpha(targetObject, 0f);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);
            SetExpressionAlpha(targetObject, alpha);
            yield return null;
        }

        SetExpressionAlpha(targetObject, 1f);
    }

    IEnumerator FadeOutGameObject(GameObject targetObject, float duration = 1f)
    {
        if (targetObject == null)
        {
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (elapsed / duration));
            SetExpressionAlpha(targetObject, alpha);
            yield return null;
        }

        SetExpressionAlpha(targetObject, 0f);
        targetObject.SetActive(false);
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
        if (speaker == "Hiromi")
        {
            ApplyHiromiExpressionFromText(textToSpeak);
            return;
        }

        if (speaker == "Koda")
        {
            ApplyKodaExpressionFromText(textToSpeak);
            return;
        }

        if (speaker == "You")
        {
            if (!string.IsNullOrWhiteSpace(textToSpeak) && textToSpeak.Contains("Koda looks quite"))
            {
                ShowKodaFocusedExpression("embarrassed");
                return;
            }

            if (index == dialogueLines.Count - 1)
            {
                HideAllCharacterExpressionsInstant();
            }

            return;
        }

        if (speaker == "Everyone")
        {
            HideAllCharacterExpressionsInstant();
            return;
        }

        HideAllCharacterExpressionsInstant();
    }

    void ApplyHiromiExpressionForLine(string lineText)
    {
        ApplyHiromiExpressionFromText(lineText);
    }

    void ApplyKodaExpressionForLine(string lineText)
    {
        ApplyKodaExpressionFromText(lineText);
    }

    void ApplyHiromiExpressionFromText(string lineText)
    {
        if (string.IsNullOrWhiteSpace(lineText))
        {
            ShowHiromiExpression("neutral");
            return;
        }

        if (ContainsAny(lineText,
            "I wasn't doing anything so late",
            "yet here we are",
            "Don't be so late",
            "eight hours of sleep"))
        {
            ShowHiromiExpression("angry");
            return;
        }

        if (ContainsAny(lineText,
            "Let's make the best of it",
            "I don't think this question's too bad",
            "I could try",
            "not the smartest person"))
        {
            ShowHiromiExpression("smile");
            return;
        }

        if (ContainsAny(lineText,
            "Mr. Einstein",
            "tip of the iceberg",
            "smug grin",
            "surrendering that fast"))
        {
            ShowHiromiExpression("smug");
            return;
        }

        if (ContainsAny(lineText,
            "Let's see",
            "The probability is 1 minus",
            "Hiromi went on to explain",
            "The game only ends"))
        {
            ShowHiromiExpression("shocked");
            return;
        }

        if (ContainsAny(lineText,
            "question's too bad",
            "The probability is",
            "guess we begin with the hardest stuff"))
        {
            ShowHiromiExpression("smile");
            return;
        }

        if (ContainsAny(lineText,
            "phone",
            "playing games on Stream",
            "other things"))
        {
            ShowHiromiExpression("neutral");
            return;
        }

        ShowHiromiExpression("neutral");
    }

    void ApplyKodaExpressionFromText(string lineText)
    {
        if (string.IsNullOrWhiteSpace(lineText))
        {
            ShowKodaFocusedExpression("neutral");
            return;
        }

        if (ContainsAny(lineText,
            "sorry about the time",
            "busy doing other things",
            "looking embarrassed",
            "won't take so long",
            "Easy for you to say"))
        {
            ShowKodaFocusedExpression("embarrassed");
            return;
        }

        if (ContainsAny(lineText,
            "what in the world is this",
            "I'm never passing that test",
            "Losing just like the gambler himself"))
        {
            ShowKodaFocusedExpression("surprised");
            return;
        }

        if (ContainsAny(lineText,
            "we're all here",
            "begin with the hardest stuff",
            "buy some stuff first",
            "won't take so long"))
        {
            ShowKodaFocusedExpression("smile");
            return;
        }

        if (ContainsAny(lineText,
            "Hehe",
            "signature smirk",
            "stands up from his chair"))
        {
            ShowKodaFocusedExpression("smirk");
            return;
        }

        if (ContainsAny(lineText,
            "This is gonna be a long session"))
        {
            ShowKodaFocusedExpression("embarrassed");
            return;
        }

        ShowKodaFocusedExpression("neutral");
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

        if (textToSpeak.Contains("With that, you and your friends left the restaurant. The sun had set and only a few souls walked the streets."))
        {
            StartCoroutine(FadeInGameObject(nightTownBG, 1.5f));
            StartCoroutine(FadeOutGameObject(KodaBase, 1.5f));
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
        SceneManager.LoadScene(23);
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

