using TMPro;
using UnityEngine;

public class BookInteract : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 2.5f;

    [Header("Disable While Book Is Open")]
    [SerializeField] private MonoBehaviour[] behavioursToDisable;

    [Header("Book UI")]
    [SerializeField] private BookCanvasController bookCanvasController;

    [Header("Prompt UI")]
    [SerializeField] private ActivationPromptUI promptUI;

    [Header("Prompt")]
    [SerializeField] private string label = "Read";
    [SerializeField] private string subLabel = "Hidden Book";

    private bool bookOpen;
    private bool playerNear;

    private void Update()
    {
        if (player == null || bookCanvasController == null || promptUI == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);
        bool nearBook = distance <= interactDistance;

        if (nearBook && !playerNear && !bookOpen)
        {
            playerNear = true;
            promptUI.ShowPrompt(label, subLabel);
        }

        if (!nearBook && playerNear)
        {
            playerNear = false;
            promptUI.HidePrompt();
        }

        if (nearBook && !bookOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenBook();
        }

        if (bookOpen && Input.GetKeyDown(KeyCode.E))
        {
            CloseBook();
        }
    }

    private void OpenBook()
    {
        bookOpen = true;
        promptUI.HidePrompt();

        bookCanvasController.OpenBook();
        SetPlayerControls(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseBook()
    {
        bookOpen = false;

        bookCanvasController.CloseBook();
        SetPlayerControls(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetPlayerControls(bool enabled)
    {
        foreach (MonoBehaviour behaviour in behavioursToDisable)
        {
            if (behaviour != null)
                behaviour.enabled = enabled;
        }
    }
}