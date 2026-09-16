using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class Scene13Events : MonoBehaviour
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

    [Header("Audio")]
    public AudioSource atmosphereSource;
    public AudioSource bgmSource;
    public AudioSource Knocking;

    [Header("Character Expressions")]
    public GameObject charMidnightHappy;
    public GameObject charMidnightEmbarrassed;
    public GameObject charMidnightDelighted;
    public GameObject charMidnightConfused;
    public GameObject charMidnightAngry;
    public GameObject charMidnightEyesWhite;
    [FormerlySerializedAs("charKodaSmile")] public GameObject charHiromiSmile;
    [FormerlySerializedAs("charKodaDelighted")] public GameObject charHiromiDelighted;
    [FormerlySerializedAs("charKodaSurprised")] public GameObject charHiromiShocked;
    [FormerlySerializedAs("charKodaNeutral")] public GameObject charHiromiNeutral;
    [FormerlySerializedAs("charKodaAngry")] public GameObject charHiromiAngry;
    [FormerlySerializedAs("charKodaSmirk")] public GameObject charHiromiSmug;
    [FormerlySerializedAs("KodaBase")] public GameObject HiromiBase;

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

    private const string DefaultSceneDialogue = @"[You and Hiromi were inside the apartment. The blue fog enveloped the entire town of Kasato]

[Your mind went back to Koda, and what happened to him. Did he get lost? Or did something worse happen…]

[Hiromi looked out the window, then at you, worried.]

Hiromi

Koda… Something bad definitely happened to him. He would’ve come back the first thirty minutes or so, not four or more hours later.

Will this Midnight, this digital cat you told me about, appear tonight?

[As if on cue, your phone’s screen went black and the recognizable feline face flashed on your screen.]

Midnight

Asa! This is bad. Is Koda wi-

[Midnight looked around, scanning the room desperately.]

Oh no… Then that presence I felt in the blue fog… It’s him.

He’s trapped in the blue fog, everyone who roams the street at midnight is…

This is bad, truly bad. I really did warn him about what would happen if he went outside at a time like this…


Hiromi

[Hiromi looked at Midnight, grabbing your phone and pulling it close to her.]

Stop yapping and just tell us! What will happen to Koda, my best friend! [she yelled]

Midnight

Sorry, we can’t do anything right now. We can’t go outside or we’ll be swallowed by the fog…

I… foresaw something like this could happen. But, why so soon…

H--e, n--o… They to--ld me i-t would ta--ke ti-me, n-ot n-o--w…

T-th--e… [Midnight’s voice started glitching, as if he was revealing too much information.]

Hiromi

[held your phone tightly on her hand]

Midnight? What’s happening, come back!

Midnight

[looked at Hiromi with what’s left of his glitchy expression. His unglitched expression seemed to return for a few seconds.]

The Blue Arika will start manifesting, the beginning of the end has started…! P-p-p-plee…ase, r---run!

[Midnight’s voice got cut, your phone’s screen went black before exiting into the homescreen ]
Hiromi

The Blue Arika? Start manifesting? [She looked outside frantically]

Does he mean the weird blue fog is connected to the Blue Fog, or something like that?

Oh no, he’s right…

We have to wait till morning and see what happens… School can wait, we have to look for our friend. Most of all, my best friend…

Koda… [she looked at you, possibly fighting the urge to tackle the door down and go out there herself.]

[While she was talking to you, three knocks on the entrance door startled the two of you]

Hiromi

W-what? Who, who is that… Or, what is that?

[The sounds of the night ambience were broken by the sound of that something out there. A familiar voice.]

???

Ah, finally out…

I knew that phone limited my presence, but here I am…

In the flesh…";

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
                currentSpeaker = NormalizeSpeakerName(trimmed);
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
            Debug.LogWarning("Scene13Events could not parse any dialogue lines.");
        }
    }

    bool IsSpeakerHeader(string line)
    {
        return line == "You" || line == "Midnight" || line == "Hiromi" || line == "Koda" || line == "???";
    }

    string NormalizeSpeakerName(string line)
    {
        return line == "Koda" ? "Hiromi" : line;
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
        charMidnightEyesWhite.SetActive(false);
    }

    void HideAllHiromiExpressions()
    {
        charHiromiSmile.SetActive(false);
        charHiromiDelighted.SetActive(false);
        charHiromiShocked.SetActive(false);
        charHiromiNeutral.SetActive(false);
        charHiromiAngry.SetActive(false);
        charHiromiSmug.SetActive(false);
    }

    void HideAllCharacterExpressions()
    {
        HideAllExpressions();
        HideAllHiromiExpressions();
        SetHiromiBaseActive(false);
    }

    void SetHiromiBaseActive(bool isActive)
    {
        if (HiromiBase != null)
        {
            HiromiBase.SetActive(isActive);
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

    void ShowHiromiExpression(string expression)
    {
        HideAllCharacterExpressions();

        switch (expression)
        {
            case "smile":
                charHiromiSmile.SetActive(true);
                SetHiromiBaseActive(true);
                break;
            case "delighted":
                charHiromiDelighted.SetActive(true);
                SetHiromiBaseActive(true);
                break;
            case "shocked":
                charHiromiShocked.SetActive(true);
                SetHiromiBaseActive(true);
                break;
            case "neutral":
                charHiromiNeutral.SetActive(true);
                SetHiromiBaseActive(false);
                break;
            case "angry":
                charHiromiAngry.SetActive(true);
                SetHiromiBaseActive(true);
                break;
            case "smug":
                charHiromiSmug.SetActive(true);
                SetHiromiBaseActive(true);
                break;
            }
    }

    void ShowMidnightExpression(string expression)
    {
        ShowExpression(expression);
    }

    void ShowHiromiFocusedExpression(string expression)
    {
        ShowHiromiExpression(expression);
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
            charMidnightAngry,
            charMidnightEyesWhite
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

    IEnumerator FadeOutAudioSource(AudioSource audioSource, float duration = 1f)
    {
        if (audioSource == null || !audioSource.isPlaying)
        {
            yield break;
        }

        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }

    void HideAllCharacterExpressionsInstant()
    {
        GameObject[] expressions = {
            charMidnightHappy,
            charMidnightEmbarrassed,
            charMidnightDelighted,
            charMidnightConfused,
            charMidnightAngry,
            charMidnightEyesWhite,
            charHiromiSmile,
            charHiromiDelighted,
            charHiromiShocked,
            charHiromiNeutral,
            charHiromiAngry,
            charHiromiSmug
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
        ShowHiromiExpression("neutral");
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

        if (speaker == "Hiromi")
        {
            switch (index)
            {
                case 3:
                    ShowHiromiExpression("neutral");
                    break;
                case 4:
                    ShowHiromiExpression("smile");
                    break;
                case 5:
                    ShowHiromiExpression("shocked");
                    break;
                case 11:
                    ShowHiromiExpression("neutral");
                    break;
                case 12:
                    ShowHiromiExpression("angry");
                    break;
                case 17:
                    ShowHiromiExpression("smug");
                    break;
                case 18:
                    ShowHiromiExpression("shocked");
                    break;
                case 22:
                    ShowHiromiExpression("shocked");
                    break;
                case 23:
                    ShowHiromiExpression("neutral");
                    break;
                case 24:
                    ShowHiromiExpression("angry");
                    break;
                case 25:
                    ShowHiromiExpression("delighted");
                    break;
                case 26:
                    ShowHiromiExpression("neutral");
                    break;
                case 27:
                case 28:
                case 29:
                    ShowHiromiExpression("shocked");
                    break;
            }

            return;
        }

        switch (index)
        {
            case 6:
                ShowMidnightExpression("confused");
                break;
            case 7:
                ShowMidnightExpression("confused");
                break;
            case 8:
            case 9:
                ShowMidnightExpression("angry");
                break;
            case 10:
                ShowMidnightExpression("angry");
                break;
            case 13:
                ShowMidnightExpression("angry");
                break;
            case 14:
                ShowMidnightExpression("confused");
                break;
            case 15:
                ShowMidnightExpression("confused");
                break;
            case 16:
                ShowMidnightExpression("confused");
                break;
            case 19:
                ShowMidnightExpression("confused");
                break;
            case 20:
                charMidnightEyesWhite.SetActive(true);
                break;
            case 21:
                ShowMidnightExpression("confused");
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

        if (speakerName == "Midnight" && textToSpeak.Contains("screen went black before exiting into the homescreen"))
        {
            StartCoroutine(FadeOutMidnightExpressions(1.5f));
        }

        if (speakerName == "Midnight" && textToSpeak.Contains("The Blue Arika will start manifesting, the beginning of the end has started"))
        {
            StartCoroutine(FadeOutAudioSource(bgmSource, 1.5f));
        }

        if (speakerName == "???")
        {
            StartCoroutine(FadeOutAudioSource(atmosphereSource, 1.5f));
        }

        if (textToSpeak.Contains("While she was talking to you, three knocks on the entrance door startled the two of you"))
        {
            if (Knocking != null)
            {
                Knocking.Stop();
                Knocking.Play();
            }
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
        SceneManager.LoadScene(0);
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
