using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Level3";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Level2PuzzleSystem.Instance != null)
        {
            Level2PuzzleSystem.Instance.CompleteLevel();
        }

        SceneManager.LoadScene(nextSceneName);
    }
}