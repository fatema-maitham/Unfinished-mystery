using UnityEngine;

// Handles gramophone interaction after the bookshelf clue is found
public class GramophoneInteract : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player; // Player transform used for distance check
    [SerializeField] private float interactDistance = 1.2f; // Distance needed to interact
    [SerializeField] private KeyCode playKey = KeyCode.E; // Key used to play gramophone

    [Header("Prompt UI")]
    [SerializeField] private ActivationPromptUI promptUI; // Shared prompt UI system

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource; // Audio source that plays the message
    [SerializeField] private AudioClip gramophoneMessage; // Gramophone message audio clip

    [Header("Prompt Text")]
    [SerializeField] private string label = "Play"; // Main prompt text
    [SerializeField] private string subLabel = "Gramophone"; // Small prompt text

    private bool messagePlayed; // Prevents the gramophone message from playing more than once
    private bool showingMyPrompt; // Tracks if this script is currently showing its prompt

    private void Update()
    {
        // Stop if required references are missing
        if (player == null || promptUI == null)
            return;

        // Gramophone becomes usable only after bookshelf clue is found
        bool bookshelfDone = Level2PuzzleSystem.Instance != null &&
                             Level2PuzzleSystem.Instance.BookshelfClueFound;

        // Check if player is close enough to gramophone
        bool nearGramophone = Vector3.Distance(player.position, transform.position) <= interactDistance;

        // Hide prompt if gramophone is locked, already played, or player is not near
        if (!bookshelfDone || messagePlayed || !nearGramophone)
        {
            if (showingMyPrompt)
            {
                promptUI.HidePrompt();
                showingMyPrompt = false;
            }

            return;
        }

        // Show gramophone prompt
        promptUI.ShowPrompt(label, subLabel);
        showingMyPrompt = true;

        // Play message when player presses E
        if (Input.GetKeyDown(playKey))
            PlayMessage();
    }

    private void PlayMessage()
    {
        // Mark message as played so it cannot repeat
        messagePlayed = true;

        // Hide prompt after playing
        if (showingMyPrompt)
        {
            promptUI.HidePrompt();
            showingMyPrompt = false;
        }

        // Play the gramophone audio message
        if (audioSource != null && gramophoneMessage != null)
            audioSource.PlayOneShot(gramophoneMessage);

        // Report progress to Level 2 puzzle system
        Level2PuzzleSystem.Instance?.PlayGramophone();
    }
}