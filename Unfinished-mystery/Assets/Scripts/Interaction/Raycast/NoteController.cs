using TMPro;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private TMP_Text noteTextAreaUI;
    [SerializeField] private TMP_Text promptTextUI;

    [Header("Note Content")]
    [TextArea(3, 10)]
    [SerializeField] private string noteText =
        "I hid the truth in the things\nI could not throw away.";

    [Header("Prompt")]
    [SerializeField] private string promptMessage = "PRESS E TO READ";

    [Header("Player")]
    [SerializeField] private MonoBehaviour playerMovement;

    private bool playerInside;
    private bool noteOpen;

    private void Start()
    {
        if (noteCanvas != null)
            noteCanvas.SetActive(false);

        HidePrompt();
    }

    private void Update()
    {
        if (playerInside && !noteOpen && Input.GetKeyDown(KeyCode.E))
            OpenNote();

        if (noteOpen && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape)))
            CloseNote();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;

        if (!noteOpen)
            ShowPrompt();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;
        HidePrompt();
    }

    private void OpenNote()
    {
        noteTextAreaUI.text = noteText;
        noteCanvas.SetActive(true);
        HidePrompt();

        if (playerMovement != null)
            playerMovement.enabled = false;

        noteOpen = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void CloseNote()
    {
        noteCanvas.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        noteOpen = false;

        if (playerInside)
            ShowPrompt();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ShowPrompt()
    {
        if (promptTextUI == null)
            return;

        promptTextUI.text = promptMessage;
        promptTextUI.gameObject.SetActive(true);
    }

    private void HidePrompt()
    {
        if (promptTextUI == null)
            return;

        promptTextUI.text = "";
        promptTextUI.gameObject.SetActive(false);
    }
}