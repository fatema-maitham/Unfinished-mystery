using UnityEngine;

public class CameraClueInteraction : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    public float interactDistance = 3f;

    [Header("References")]
    public GameObject cameraClueUI;
    public ActivationPromptUI activationPrompt;

    private bool isOpen = false;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(
            player.position, transform.position);
        bool playerInRange = distance <= interactDistance;

        // Show/hide prompt
        if (playerInRange && !isOpen)
            activationPrompt.ShowPrompt("Camera Clue", "Examine");
        else
            activationPrompt.HidePrompt();

        // Toggle open/close
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
            cameraClueUI.SetActive(isOpen);
        }
    }
}