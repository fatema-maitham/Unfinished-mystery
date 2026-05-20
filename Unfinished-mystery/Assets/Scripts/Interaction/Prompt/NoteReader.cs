using TMPro;
using UnityEngine;

public class NoteReader : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float readDistance = 1f;
    [SerializeField] private float playerHeightOffset = 1f;

    [Header("Prompt UI")]
    [SerializeField] private InteractionPromptUI interactPrompt;

    [Header("Note UI")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private TMP_Text noteText;

    [Header("Note Content")]
    [TextArea(3, 8)]
    [SerializeField] private string message;

    private bool playerInRange;
    private bool noteOpen;
    private Renderer noteRenderer;

    private void Start()
    {
        noteRenderer = GetComponent<Renderer>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                player = playerObject.transform;
        }

        if (notePanel != null)
            notePanel.SetActive(false);

        if (interactPrompt != null)
            interactPrompt.HidePrompt();
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 noteCenter = noteRenderer != null ? noteRenderer.bounds.center : transform.position;
        Vector3 playerPoint = player.position + Vector3.up * playerHeightOffset;

        float distance = Vector3.Distance(playerPoint, noteCenter);
        bool nearNote = distance <= readDistance;

        if (nearNote && !playerInRange)
        {
            playerInRange = true;
            if (!noteOpen && interactPrompt != null)
                interactPrompt.ShowPrompt("Read");
        }

        if (!nearNote && playerInRange)
        {
            playerInRange = false;
            CloseNote();
            if (interactPrompt != null)
                interactPrompt.HidePrompt();
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (noteOpen) CloseNote();
            else OpenNote();
        }
    }

    private void OpenNote()
    {
        noteOpen = true;

        if (notePanel != null)
            notePanel.SetActive(true);

        if (noteText != null)
            noteText.text = message;

        if (interactPrompt != null)
            interactPrompt.HidePrompt();
    }

    private void CloseNote()
    {
        noteOpen = false;

        if (notePanel != null)
            notePanel.SetActive(false);

        if (playerInRange && interactPrompt != null)
            interactPrompt.ShowPrompt("Read");
    }
}