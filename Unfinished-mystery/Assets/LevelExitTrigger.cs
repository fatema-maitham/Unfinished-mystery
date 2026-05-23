using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// ═══════════════════════════════════════════════════════════════════════════════
// LEVEL EXIT TRIGGER
// Sends the completed level data to LevelResultData,
// then opens the shared LevelSummary scene.
// Attach this script to: ExitTrigger object in each level.
// ═══════════════════════════════════════════════════════════════════════════════
public class LevelExitTrigger : MonoBehaviour
{
    [Header("Summary Scene")]
    [SerializeField] private string summarySceneName = "LevelSummary";

    [Header("Level Summary Data")]
    [SerializeField] private string title = "LEVEL COMPLETE";
    [SerializeField] private string levelName = "LEVEL 2";
    [SerializeField] private string characterName = "Detective Lana Cole";
    [SerializeField] private string role = "Homicide Detective, Missing Persons Unit";

    [Header("Loop Counter")]
    [SerializeField] private TMP_Text loopCounterText;
    [SerializeField] private int maxLoops = 5;
    [SerializeField] private bool textShowsLoopsLeft = false;
    [SerializeField] private int fallbackLoopsUsed = 1;

    [Header("Result Message")]
    [TextArea(2, 4)]
    [SerializeField] private string resultMessage =
        "Lana finally faced the truth she was too afraid to reveal.";

    [Header("Portrait")]
    [SerializeField] private Sprite portrait;

    [Header("Button Scenes")]
    [SerializeField] private string nextSceneName = "Level3CharacterPage";
    [SerializeField] private string replaySceneName = "Level2";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        int loopsUsed = GetLoopsUsed();

        LevelResultData.title = title;
        LevelResultData.levelName = levelName;
        LevelResultData.characterName = characterName;
        LevelResultData.role = role;
        LevelResultData.loopsUsed = loopsUsed;
        LevelResultData.resultMessage = resultMessage;
        LevelResultData.portrait = portrait;

        LevelResultData.nextSceneName = nextSceneName;
        LevelResultData.replaySceneName = replaySceneName;

        SceneManager.LoadScene(summarySceneName);
    }

    // ═══════════════════════════════════════════════════════════════════════════════
    // LOOP COUNT READER
    // Reads the number from the loop counter text.
    // If the text shows loops used, it uses the number directly.
    // If the text shows loops left, it converts loops left into loops used.
    // ═══════════════════════════════════════════════════════════════════════════════
    private int GetLoopsUsed()
    {
        if (loopCounterText == null)
            return Mathf.Clamp(fallbackLoopsUsed, 1, maxLoops);

        int numberFromText = ExtractFirstNumber(loopCounterText.text);

        if (numberFromText < 0)
            return Mathf.Clamp(fallbackLoopsUsed, 1, maxLoops);

        if (textShowsLoopsLeft)
        {
            int loopsLeft = Mathf.Clamp(numberFromText, 0, maxLoops);
            int loopsUsed = maxLoops - loopsLeft + 1;
            return Mathf.Clamp(loopsUsed, 1, maxLoops);
        }

        return Mathf.Clamp(numberFromText, 1, maxLoops);
    }

    // ═══════════════════════════════════════════════════════════════════════════════
    // NUMBER EXTRACTOR
    // Finds the first number inside a text string.
    // Works with text like: "3", "Loop: 3 / 5", or "Loops Left: 2".
    // ═══════════════════════════════════════════════════════════════════════════════
    private int ExtractFirstNumber(string text)
    {
        string digits = "";

        foreach (char c in text)
        {
            if (char.IsDigit(c))
            {
                digits += c;
            }
            else if (digits.Length > 0)
            {
                break;
            }
        }

        if (int.TryParse(digits, out int number))
            return number;

        return -1;
    }
}