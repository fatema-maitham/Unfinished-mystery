using UnityEngine;
using UnityEngine.SceneManagement;

public class L3ExitToSummary : MonoBehaviour
{
    [Header("Scene Names")]
    public string summarySceneName = "LevelSummary";
    public string replaySceneName = "Level3";
    public string nextSceneName = "Level4";

    [Header("Level 3 Summary Data")]
    public string levelTitle = "LEVEL COMPLETE";
    public string levelName = "LEVEL 3";
    public string characterName = "Detective Ethan Cole";
    public string role = "Cinema Detective";
    public int loopsUsed = 1;

    private bool exitUnlocked = false;

    [Header("Portrait")]
    public Sprite level3Portrait;



    [TextArea]
    public string resultMessage = "The final reel revealed Maya’s fate.";

    private bool triggered = false;

    public void UnlockExitSummary()
    {
        exitUnlocked = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!exitUnlocked)
            return;

        if (triggered)
            return;

        triggered = true;

        LevelResultData.title = levelTitle;
        LevelResultData.levelName = levelName;
        LevelResultData.characterName = characterName;
        LevelResultData.role = role;
        LevelResultData.loopsUsed = loopsUsed;
        LevelResultData.resultMessage = resultMessage;
        LevelResultData.replaySceneName = replaySceneName;
        LevelResultData.nextSceneName = nextSceneName;
        LevelResultData.portrait = level3Portrait;

        SceneManager.LoadScene(summarySceneName);
    }
}