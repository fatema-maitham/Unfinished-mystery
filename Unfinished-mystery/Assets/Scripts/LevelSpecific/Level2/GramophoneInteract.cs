using TMPro;
using UnityEngine;

// ═══════════════════════════════════════════════════════════════════════════════
// LEVEL 2 — GRAMOPHONE MESSAGE
// Before the bookshelf clue is complete, the gramophone shows no prompt.
// After the final book spread is reached, the gramophone becomes playable.
// Attach this script to: IP_Gramophone
// ═══════════════════════════════════════════════════════════════════════════════
public class GramophoneInteract : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private KeyCode playKey = KeyCode.P;

    [Header("Prompt UI")]
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TMP_Text promptTextUI;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gramophoneMessage;

    [Header("Prompt Messages")]
    [SerializeField] private string playPrompt = "PRESS P TO PLAY";

    private bool messagePlayed = false;

    private void Start()
    {
        HidePrompt();
    }

    private void Update()
    {
        if (player == null)
            return;

        bool nearGramophone = Vector3.Distance(player.position, transform.position) <= interactDistance;

        bool bookshelfDone = Level2PuzzleSystem.Instance != null &&
                             Level2PuzzleSystem.Instance.BookshelfClueFound;

        if (!nearGramophone || !bookshelfDone || messagePlayed)
        {
            HidePrompt();
            return;
        }

        ShowPrompt(playPrompt);

        if (Input.GetKeyDown(playKey))
            PlayMessage();
    }

    private void PlayMessage()
    {
        messagePlayed = true;
        HidePrompt();

        if (audioSource != null && gramophoneMessage != null)
            audioSource.PlayOneShot(gramophoneMessage);

        Level2PuzzleSystem.Instance?.PlayGramophone();
    }

    private void ShowPrompt(string message)
    {
        if (promptPanel == null || promptTextUI == null)
            return;

        promptPanel.SetActive(true);
        promptTextUI.text = message;
    }

    private void HidePrompt()
    {
        if (promptPanel != null)
            promptPanel.SetActive(false);
    }
}