using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;

public class Scene11Event : MonoBehaviour
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

    private const string DefaultSceneDialogue = @"[You all finished eating, Koda looks quite stuffed.]

Koda

Dude, I ate waaay too much of that steak...

Hiromi

You ordered three steaks, for yourself!

You didn't even share a piece with me, ugh!

At least Asa was chivalrous enough to share a french fry with me, you should learn from him Koda!

Koda

[looking at you] W-what? I did share a piece of my steak to y-

Oh wait, I did end up eating it. Whoopsies.

Hiromi

[looks at her phone] Wow, it's already evening. Dang, we spent a long time eating, no wonder Koda is so stuffed.

Although, how could it get late so quickly... That's weird.

It's 8PM, it's quite late.

Koda

[looks frantically at Hiromi, trying to search for any signs of joking. He looks quite disappointed.]

Dude, a whole day spent in Nabana. That's surprisingly fun!

That's a new record, we gotta celebrate with more eating [he teased]

Hiromi

[looked at Koda] You're gonna burst with all that eating.

A-anyways, we should call it a day. It's already pretty late, and I have studying to do for the upcoming test.

Koda

[looks at her, then at you] Ugh, I completely forgot about that. Guess I was having too much fun.

I really gotta study too, or else it'll kill my grade!

Hey, maybe we can schedule a study session! We can all study together over at my place!

How's that sound y'all?

Hiromi

Hey, that doesn't sound too bad. Surprisingly brilliant for a hollowhead like you!

Well, it's settled! Let's meet up tomorrow at Koda's house.

Maybe after three? I'm okay with any time as long as it's not too late.

Koda

Alright, 4PM it is! I'll organize my room for the perfect study session!

Hiromi

[looks at Koda, slightly confused] Um, how about the living room instead?

Koda

[he looks disappointed, but masks it with his signature smirk]

O-oh, of course! Living room it is. Let's get out of here, we don't wanna waste any more time.

You

[You heard a voice echoing from the kitchen, getting closer to you and your friends.] Hey kids, we're closing. Get outta here, scram!

Everyone

Yes sir!

You

[With that, you and your friends left the restaurant. The sun had set and only a few souls walked the streets.]

[Hiromi went her own way, you and Koda took the train back to your apartments respectively.]

[Midnight didn't call or interrupt you today. What could he be up to?]";

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
        if (string.IsNullOrWhiteSpace(lineText))
        {
            ShowHiromiExpression("neutral");
            return;
        }

        if (lineText.Contains("ordered three steaks")
            || lineText.Contains("didn't even share")
            || lineText.Contains("should learn from him Koda"))
        {
            ShowHiromiExpression("angry");
            return;
        }

        if (lineText.Contains("already evening")
            || lineText.Contains("how could it get late so quickly")
            || lineText.Contains("It’s 8PM")
            || lineText.Contains("It's 8PM")
            || lineText.Contains("Maybe after three")
            || lineText.Contains("slightly confused"))
        {
            ShowHiromiExpression("shocked");
            return;
        }

        if (lineText.Contains("settled")
            || lineText.Contains("Hiromi Abara")
            || lineText.Contains("not too late"))
        {
            ShowHiromiExpression("smile");
            return;
        }

        if (lineText.Contains("phone"))
        {
            ShowHiromiExpression("neutral");
            return;
        }

        ShowHiromiExpression("neutral");
    }

    void ApplyKodaExpressionForLine(string lineText)
    {
        if (string.IsNullOrWhiteSpace(lineText))
        {
            ShowKodaFocusedExpression("neutral");
            return;
        }

        if (lineText.Contains("ate waaay too much")
            || lineText.Contains("share a piece")
            || lineText.Contains("forgot about that")
            || lineText.Contains("kill my grade"))
        {
            ShowKodaFocusedExpression("embarrassed");
            return;
        }

        if (lineText.Contains("looks frantically")
            || lineText.Contains("closing. Get outta here")
            || lineText.Contains("scram"))
        {
            ShowKodaFocusedExpression("surprised");
            return;
        }

        if (lineText.Contains("surprisingly fun")
            || lineText.Contains("study session")
            || lineText.Contains("How's that sound")
            || lineText.Contains("4PM"))
        {
            ShowKodaFocusedExpression("smile");
            return;
        }

        if (lineText.Contains("celebrate with more eating")
            || lineText.Contains("signature smirk"))
        {
            ShowKodaFocusedExpression("smirk");
            return;
        }

        if (lineText.Contains("disappointed"))
        {
            ShowKodaFocusedExpression("embarrassed");
            return;
        }

        ShowKodaFocusedExpression("neutral");
    }

    void ApplyHiromiExpressionFromText(string lineText)
    {
        if (string.IsNullOrWhiteSpace(lineText))
        {
            ShowHiromiExpression("neutral");
            return;
        }

        if (lineText.Contains("ordered three steaks")
            || lineText.Contains("didn't even share")
            || lineText.Contains("should learn from him Koda"))
        {
            ShowHiromiExpression("angry");
            return;
        }

        if (lineText.Contains("hollowhead"))
        {
            ShowHiromiExpression("smug");
            return;
        }

        if (lineText.Contains("already evening")
            || lineText.Contains("how could it get late so quickly")
            || lineText.Contains("It's 8PM")
            || lineText.Contains("8PM")
            || lineText.Contains("Maybe after three")
            || lineText.Contains("slightly confused"))
        {
            ShowHiromiExpression("shocked");
            return;
        }

        if (lineText.Contains("doesn't sound too bad")
            || lineText.Contains("settled")
            || lineText.Contains("Hiromi Abara")
            || lineText.Contains("not too late"))
        {
            ShowHiromiExpression("smile");
            return;
        }

        if (lineText.Contains("phone"))
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

        if (lineText.Contains("ate waaay too much")
            || lineText.Contains("share a piece")
            || lineText.Contains("forgot about that")
            || lineText.Contains("kill my grade"))
        {
            ShowKodaFocusedExpression("embarrassed");
            return;
        }

        if (lineText.Contains("looks frantically")
            || lineText.Contains("closing. Get outta here")
            || lineText.Contains("scram"))
        {
            ShowKodaFocusedExpression("surprised");
            return;
        }

        if (lineText.Contains("surprisingly fun")
            || lineText.Contains("study session")
            || lineText.Contains("How's that sound")
            || lineText.Contains("4PM"))
        {
            ShowKodaFocusedExpression("smile");
            return;
        }

        if (lineText.Contains("celebrate with more eating")
            || lineText.Contains("signature smirk"))
        {
            ShowKodaFocusedExpression("smirk");
            return;
        }

        if (lineText.Contains("disappointed"))
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
        SceneManager.LoadScene(20);
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
