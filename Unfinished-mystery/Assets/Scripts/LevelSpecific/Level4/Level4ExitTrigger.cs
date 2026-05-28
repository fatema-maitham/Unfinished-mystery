using UnityEngine;
using UnityEngine.SceneManagement;

public class Level4ExitTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "LevelSummary4";

    [Header("Requirements")]
    [SerializeField] private GameObject requiredKeyObject;

    private bool playerInRange = false;

    private void Update()
    {
        if (!playerInRange)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (requiredKeyObject != null)
            {
                Debug.Log("Door locked. Find the key first.");
                return;
            }

            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;

        Debug.Log("Press E to Exit");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;
    }
}