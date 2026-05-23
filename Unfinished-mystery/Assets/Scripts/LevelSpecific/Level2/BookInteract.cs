using TMPro;
using UnityEngine;

// ═══════════════════════════════════════════════════════════════════════════════
// LEVEL 2 — BOOK INTERACTION
// Opens the clue book, freezes player/camera controls while reading,
// then restores everything when closing with X or CloseButton.
// Attach to: IP_BookshelfBook
// ═══════════════════════════════════════════════════════════════════════════════
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
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TMP_Text promptTextUI;

    [Header("Prompt Text")]
    [SerializeField] private string promptMessage = "PRESS E TO TAKE BOOK";

    private bool bookOpen;

    private void Start()
    {
        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    private void Update()
    {
        if (player == null || bookCanvasController == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);
        bool nearBook = distance <= interactDistance;

        if (!bookOpen)
        {
            if (promptPanel != null)
                promptPanel.SetActive(nearBook);

            if (nearBook && promptTextUI != null)
                promptTextUI.text = promptMessage;
        }

        if (nearBook && !bookOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenBook();
        }

        if (bookOpen && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape)))
        {
            CloseBook();
        }
    }

    private void OpenBook()
    {
        bookOpen = true;

        if (promptPanel != null)
            promptPanel.SetActive(false);

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