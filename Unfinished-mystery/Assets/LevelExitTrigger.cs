using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitTrigger : MonoBehaviour
{
    [Header("Scene To Load")]
    [SerializeField] private string nextSceneName = "LevelSummary";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}