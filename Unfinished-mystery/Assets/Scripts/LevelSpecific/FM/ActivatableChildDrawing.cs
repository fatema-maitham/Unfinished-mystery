using UnityEngine;

public class ActivatableChildDrawing : MonoBehaviour
{
    private bool playerInside;
    private bool activated;

    private void Update()
    {
        if (activated) return;

        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            activated = true;

            if (Level2PuzzleSystem.Instance != null)
                Level2PuzzleSystem.Instance.FindChildDrawing();

            Debug.Log("Child drawing inspected and checked.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}