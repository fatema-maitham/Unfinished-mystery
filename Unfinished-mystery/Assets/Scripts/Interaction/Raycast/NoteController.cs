using TMPro;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private MonoBehaviour playerMovement;

    [Header("UI")]
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private TMP_Text noteTextAreaUI;
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TMP_Text promptTextUI;

    [Header("Note")]
    [TextArea(3, 10)]
    [SerializeField] private string noteText =
        "I hid the truth in the things\nI could not throw away.";

    private bool noteOpen;

    private void Start()
    {
        noteOpen = false;

        if (noteCanvas != null)
            noteCanvas.SetActive(false);

        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);
        bool nearNote = distance <= interactDistance;

        if (!noteOpen)
        {
            promptPanel.SetActive(nearNote);

            if (nearNote)
                promptTextUI.text = "PRESS E TO READ";
        }

        if (nearNote && !noteOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenNote();
        }

        if (noteOpen && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape)))
        {
            CloseNote();
        }
    }

    private void OpenNote()
    {
        noteOpen = true;

        noteTextAreaUI.text = noteText;
        noteCanvas.SetActive(true);
        promptPanel.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseNote()
    {
        noteOpen = false;

        noteCanvas.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}