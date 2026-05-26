using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Level3";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Level2PuzzleSystem.Instance == null)
            return;

        if (!Level2PuzzleSystem.Instance.DoorUnlocked)
        {
            Level2PuzzleSystem.ShowBlocked("The exit is locked. Finish the clues first.");
            return;
        }

        Level2PuzzleSystem.Instance.CompleteLevel();
        SceneManager.LoadScene(nextSceneName);
    }
}