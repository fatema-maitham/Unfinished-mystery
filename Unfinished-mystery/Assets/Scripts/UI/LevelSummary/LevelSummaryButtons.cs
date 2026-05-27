using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelSummaryButtons : MonoBehaviour
{
    [SerializeField] private string continueSceneName = "LevelsBook";
    [SerializeField] private string replaySceneName   = "Level1";

    // Called by ContinueButton onClick
    public void ContinueGame()
    {
        EnsureCursor();
        Time.timeScale = 1f;

        if (!string.IsNullOrWhiteSpace(continueSceneName))
            SceneManager.LoadScene(continueSceneName);
        else
            Debug.LogWarning("LevelSummaryButtons: continueSceneName is empty!");
    }

    // Called by ReplayButton onClick
    public void ReplayLevel()
    {
        EnsureCursor();
        Time.timeScale = 1f;

        if (!string.IsNullOrWhiteSpace(replaySceneName))
            SceneManager.LoadScene(replaySceneName);
        else
            Debug.LogWarning("LevelSummaryButtons: replaySceneName is empty!");
    }

    // Keep cursor visible/free right up until the scene loads
    private void EnsureCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
    }
}