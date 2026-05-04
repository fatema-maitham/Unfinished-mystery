using UnityEngine;

public class L2FirstNoteInteract : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private L2NoteUI noteUI;

    [Header("Note Message")]
    [TextArea(3, 8)]
    [SerializeField] private string noteMessage =
        "I hid the truth in the things I could not throw away.";

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string promptMessage = "Press E to read";

    private bool playerInside;
    private bool noteRead;

    private void Update()
    {
        if (!playerInside)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            ReadNote();
        }
    }

    private void ReadNote()
    {
        if (noteUI == null)
        {
            Debug.LogWarning("L2FirstNoteInteract: noteUI is not assigned.");
            return;
        }

        noteUI.ShowNote(noteMessage);
        noteRead = true;

        Debug.Log("First Note read.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;

        if (noteUI != null)
            noteUI.ShowPrompt(promptMessage);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        if (noteUI != null)
            noteUI.HidePrompt();
    }

    public bool IsNoteRead()
    {
        return noteRead;
    }
}