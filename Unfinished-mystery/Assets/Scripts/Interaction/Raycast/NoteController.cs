using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NoteController : MonoBehaviour, IInteractable
{
    public static bool IsAnyNoteOpen { get; private set; }

    [Header("Input")]
    [SerializeField] private KeyCode closeKey = KeyCode.X;

    [Header("UI")]
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private TMP_Text noteTextAreaUI;

    [Header("Note Content")]
    [TextArea(3, 10)]
    [SerializeField] private string noteText;

    [Header("Prompt")]
    [SerializeField] private string promptText = "Press E to read";

    [Header("Optional Events")]
    [SerializeField] private UnityEvent openEvent;
    [SerializeField] private UnityEvent closeEvent;

    [Header("Optional Disable While Reading")]
    [SerializeField] private MonoBehaviour[] behavioursToDisableWhenOpen;

    private bool isOpen;

    private void Start()
    {
        if (noteCanvas != null)
        {
            noteCanvas.SetActive(false);
        }

        isOpen = false;
        IsAnyNoteOpen = false;
    }

    private void Update()
    {
        if (!isOpen)
            return;

        if (Input.GetKeyDown(closeKey) || Input.GetKeyDown(KeyCode.Escape))
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
        ShowNote();
    }

    public void ShowNote()
    {
        if (noteCanvas == null)
        {
            Debug.LogWarning("[NoteController] Note Canvas is not assigned.");
            return;
        }

        if (noteTextAreaUI == null)
        {
            Debug.LogWarning("[NoteController] Note Text Area UI is not assigned.");
            return;
        }

        noteTextAreaUI.text = noteText;
        noteCanvas.SetActive(true);

        SetPlayerControl(false);

        isOpen = true;
        IsAnyNoteOpen = true;

        openEvent?.Invoke();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseNote()
    {
        if (noteCanvas != null)
        {
            noteCanvas.SetActive(false);
        }

        SetPlayerControl(true);

        isOpen = false;
        IsAnyNoteOpen = false;

        closeEvent?.Invoke();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetPlayerControl(bool enabled)
    {
        if (behavioursToDisableWhenOpen == null)
            return;

        foreach (MonoBehaviour behaviour in behavioursToDisableWhenOpen)
        {
            if (behaviour != null)
            {
                behaviour.enabled = enabled;
            }
        }
    }
}