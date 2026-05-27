using UnityEngine;

public class BookClueInteraction : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    public float interactDistance = 3f;

    [Header("References")]
    public GameObject bookClueUI;
    public ActivationPromptUI activationPrompt;

    private bool isOpen = false;
    private bool wasInRange = false;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(
            player.position, transform.position);
        bool playerInRange = distance <= interactDistance;

        if (playerInRange && !wasInRange && !isOpen)
            activationPrompt.ShowPrompt("Book Clue", "Examine");
        else if (!playerInRange && wasInRange)
            activationPrompt.HidePrompt();

        wasInRange = playerInRange;

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
            bookClueUI.SetActive(isOpen);
            if (isOpen) activationPrompt.HidePrompt();
        }
    }
}