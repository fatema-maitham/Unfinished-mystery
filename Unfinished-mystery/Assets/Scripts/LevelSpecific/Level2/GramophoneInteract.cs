using UnityEngine;

// ═══════════════════════════════════════════════════════════════════════════════
// LEVEL 2 — GRAMOPHONE INTERACTION
// The gramophone becomes available only after the hidden book clue is completed.
// It shows the prompt only when the player is close to the gramophone.
// It does not show the prompt while the book panel is open.
// Attach this script to: IP_Gramophone
// ═══════════════════════════════════════════════════════════════════════════════
public class GramophoneInteract : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private KeyCode playKey = KeyCode.E;

    [Header("Prompt UI")]
    [SerializeField] private ActivationPromptUI promptUI;

    [Header("Book Check")]
    [SerializeField] private GameObject bookPanel;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gramophoneMessage;

    [Header("Prompt")]
    [SerializeField] private string label = "Play";
    [SerializeField] private string subLabel = "Gramophone";

    private bool messagePlayed = false;

    private void Update()
    {
        if (player == null || promptUI == null)
            return;

        // Do not show the gramophone prompt while the book is open.
        if (bookPanel != null && bookPanel.activeInHierarchy)
        {
            promptUI.HidePrompt();
            return;
        }

        bool bookshelfDone = Level2PuzzleSystem.Instance != null &&
                             Level2PuzzleSystem.Instance.BookshelfClueFound;

        // Before the book clue is complete, do nothing.
        if (!bookshelfDone || messagePlayed)
            return;

        bool nearGramophone = Vector3.Distance(player.position, transform.position) <= interactDistance;

        if (!nearGramophone)
        {
            promptUI.HidePrompt();
            return;
        }

        promptUI.ShowPrompt(label, subLabel);

        if (Input.GetKeyDown(playKey))
            PlayMessage();
    }

    private void PlayMessage()
    {
        messagePlayed = true;
        promptUI.HidePrompt();

        if (audioSource != null && gramophoneMessage != null)
            audioSource.PlayOneShot(gramophoneMessage);

        Level2PuzzleSystem.Instance?.PlayGramophone();
    }
}