using UnityEngine;
using UnityEngine.SceneManagement;

// ═══════════════════════════════════════════════════════════════════════════════
// LEVEL EXIT TRIGGER
// When the player enters the exit trigger, this script sends the completed
// level data to LevelResultData, then opens the shared LevelSummary scene.
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

    [Header("Performance")]
    [SerializeField] private int loopsUsed = 3;

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
}