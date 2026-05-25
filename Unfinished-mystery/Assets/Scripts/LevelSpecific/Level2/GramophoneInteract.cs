using UnityEngine;

public class GramophoneInteract : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 1.2f;
    [SerializeField] private KeyCode playKey = KeyCode.E;

    [Header("Prompt UI")]
    [SerializeField] private ActivationPromptUI promptUI;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gramophoneMessage;

    [Header("Prompt Text")]
    [SerializeField] private string label = "Play";
    [SerializeField] private string subLabel = "Gramophone";

    private bool messagePlayed;
    private bool showingMyPrompt;

    private void Update()
    {
        if (player == null || promptUI == null)
            return;

        bool bookshelfDone = Level2PuzzleSystem.Instance != null &&
                             Level2PuzzleSystem.Instance.BookshelfClueFound;

        bool nearGramophone = Vector3.Distance(player.position, transform.position) <= interactDistance;

        if (!bookshelfDone || messagePlayed || !nearGramophone)
        {
            if (showingMyPrompt)
            {
                promptUI.HidePrompt();
                showingMyPrompt = false;
            }

            return;
        }

        promptUI.ShowPrompt(label, subLabel);
        showingMyPrompt = true;

        if (Input.GetKeyDown(playKey))
            PlayMessage();
    }

    private void PlayMessage()
    {
        messagePlayed = true;

        if (showingMyPrompt)
        {
            promptUI.HidePrompt();
            showingMyPrompt = false;
        }

        if (audioSource != null && gramophoneMessage != null)
            audioSource.PlayOneShot(gramophoneMessage);

        Level2PuzzleSystem.Instance?.PlayGramophone();
    }
}