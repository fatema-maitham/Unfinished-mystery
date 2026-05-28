using TMPro;
using UnityEngine;

// Handles player interaction with the hidden book
public class BookInteract : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player; // Player transform used for distance check
    [SerializeField] private float interactDistance = 2.5f; // Distance needed to read the book

    [Header("Disable While Book Is Open")]
    [SerializeField] private MonoBehaviour[] behavioursToDisable; // Player movement/camera scripts disabled while book is open

    [Header("Book UI")]
    [SerializeField] private BookCanvasController bookCanvasController; // Book UI controller

    [Header("Prompt UI")]
    [SerializeField] private ActivationPromptUI promptUI; // Prompt system that shows Read / Hidden Book

    [Header("Prompt")]
    [SerializeField] private string label = "Read"; // Main prompt text
    [SerializeField] private string subLabel = "Hidden Book"; // Small prompt text

    private bool bookOpen; // True when the book UI is open
    private bool playerNear; // True when player is close enough to the book

    private void Update()
    {
        // Stop if required references are missing
        if (player == null || bookCanvasController == null || promptUI == null)
            return;

        // Check if player is close enough to the book
        float distance = Vector3.Distance(player.position, transform.position);
        bool nearBook = distance <= interactDistance;

        // While book is open, allow closing with E, X, or Escape
        if (bookOpen)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
            {
                CloseBook();
            }

            return;
        }

        // Show prompt when player enters interaction range
        if (nearBook && !playerNear)
        {
            playerNear = true;
            promptUI.ShowPrompt(label, subLabel);
        }

        // Hide prompt when player leaves interaction range
        if (!nearBook && playerNear)
        {
            playerNear = false;
            promptUI.HidePrompt();
        }

        // Open book when player is near and presses E
        if (nearBook && Input.GetKeyDown(KeyCode.E))
        {
            OpenBook();
        }
    }

    private void OpenBook()
    {
        // Mark book as open
        bookOpen = true;

        // Hide prompt while reading
        promptUI.HidePrompt();

        // Open the book UI
        bookCanvasController.OpenBook();

        // Disable player controls while book is open
        SetPlayerControls(false);

        // Unlock cursor for UI interaction
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseBook()
    {
        // Mark book as closed
        bookOpen = false;

        // Close the book UI
        bookCanvasController.CloseBook();

        // Enable player controls again
        SetPlayerControls(true);

        // Lock and hide cursor again for gameplay
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Show prompt again if player is still near the book
        if (playerNear)
            promptUI.ShowPrompt(label, subLabel);
    }

    private void SetPlayerControls(bool enabled)
    {
        // Enable or disable each assigned player behaviour
        foreach (MonoBehaviour behaviour in behavioursToDisable)
        {
            if (behaviour != null)
                behaviour.enabled = enabled;
        }
    }
}