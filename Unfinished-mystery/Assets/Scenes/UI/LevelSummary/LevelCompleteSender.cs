using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteSender : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string summarySceneName = "LevelSummary1";
    [SerializeField] private string nextSceneName = "LevelsBook";
    [SerializeField] private string replaySceneName = "Level1";

    [Header("Level Summary Info")]
    [SerializeField] private string levelName = "LEVEL 1";
    [SerializeField] private string characterName = "Kyryll Flins";
    [SerializeField] private string role = "Mathematics Professor";
    [SerializeField] private string resultMessage = "The truth has been uncovered.";
    [SerializeField] private Sprite portrait;

    [Header("Loop Source")]
    [SerializeField] private TMP_Text loopCounterText;

    public void CompleteLevel()
    {
        LevelResultData.title = "LEVEL COMPLETE";
        LevelResultData.levelName = levelName;
        LevelResultData.characterName = characterName;
        LevelResultData.role = role;
        LevelResultData.resultMessage = resultMessage;
        LevelResultData.portrait = portrait;

        LevelResultData.loopsUsed = GetLoopNumber();

        LevelResultData.nextSceneName = nextSceneName;
        LevelResultData.replaySceneName = replaySceneName;

        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene(summarySceneName);
    }

    private int GetLoopNumber()
    {
        if (loopCounterText == null)
            return 1;

        if (int.TryParse(loopCounterText.text, out int loopNumber))
            return Mathf.Max(1, loopNumber);

        return 1;
    }
}