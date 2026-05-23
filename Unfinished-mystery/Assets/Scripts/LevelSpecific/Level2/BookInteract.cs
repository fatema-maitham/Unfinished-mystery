using TMPro;
using UnityEngine;

// ═══════════════════════════════════════════════════════════════════════════════
// LEVEL 2 — BOOK INTERACTION
// Lets the player take/open the clue book near the bookshelf.
// Shows the prompt, opens the BookCanvas, freezes player movement,
// and restores movement when the book closes.
// Attach this script to: IP_BookshelfBook
// ═══════════════════════════════════════════════════════════════════════════════
public class BookInteract : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private MonoBehaviour playerMovement;

    [Header("Book UI")]
    [SerializeField] private BookCanvasController bookCanvasController;

    [Header("Prompt UI")]
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TMP_Text promptTextUI;

    [Header("Prompt Text")]
    [SerializeField] private string promptMessage = "PRESS E TO TAKE BOOK";

    private bool bookOpen = false;

    private void Start()
    {
        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    private void Update()
    {
        if (player == null)
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

    // ───────────────────────────────────────────────────────────────────────────
    // Opens the book and disables player movement while reading.
    // ───────────────────────────────────────────────────────────────────────────
    private void OpenBook()
    {
        bookOpen = true;

        if (promptPanel != null)
            promptPanel.SetActive(false);

        if (bookCanvasController != null)
            bookCanvasController.OpenBook();

        if (playerMovement != null)
            playerMovement.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // ───────────────────────────────────────────────────────────────────────────
    // Closes the book and restores player movement.
    // This can be called by X/Escape or by the CloseButton OnClick.
    // ───────────────────────────────────────────────────────────────────────────
    public void CloseBook()
    {
        bookOpen = false;

        if (bookCanvasController != null)
            bookCanvasController.CloseBook();

        if (playerMovement != null)
            playerMovement.enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}