using TMPro;
using UnityEngine;

public class NoteController : MonoBehaviour, IInteractable
{
    public static bool IsAnyNoteOpen { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private TMP_Text noteTextAreaUI;

    [Header("Note Content")]
    [TextArea(3, 10)]
    [SerializeField] private string noteText =
        "I hid the truth in the things I could not throw away.";

    [Header("Prompt")]
    [SerializeField] private string promptText = "PRESS E TO READ";

    [Header("Disable While Reading")]
    [SerializeField] private MonoBehaviour playerMovement;

    private bool isOpen;

    private void Start()
    {
        if (noteCanvas != null)
            noteCanvas.SetActive(false);

        isOpen = false;
        IsAnyNoteOpen = false;
    }

    private void Update()
    {
        if (isOpen && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape)))
        {
            CloseNote();
        }
    }

    public string GetPromptText()
    {
        return promptText;
    }

    public void Interact()
    {
        OpenNote();
    }

    private void OpenNote()
    {
        if (noteCanvas == null || noteTextAreaUI == null)
        {
            Debug.LogWarning("NoteController: Canvas or text is missing.");
            return;
        }

        noteTextAreaUI.text = noteText;
        noteCanvas.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = false;

        isOpen = true;
        IsAnyNoteOpen = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void CloseNote()
    {
        if (noteCanvas != null)
            noteCanvas.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        isOpen = false;
        IsAnyNoteOpen = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}