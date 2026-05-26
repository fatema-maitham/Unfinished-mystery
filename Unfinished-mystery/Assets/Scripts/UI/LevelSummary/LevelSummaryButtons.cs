using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSummaryButtons : MonoBehaviour
{
    [SerializeField] private string continueSceneName = "LevelsBook";
    [SerializeField] private string replaySceneName = "Level1";

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(continueSceneName);
    }

    public void ReplayLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(replaySceneName);
    }
}