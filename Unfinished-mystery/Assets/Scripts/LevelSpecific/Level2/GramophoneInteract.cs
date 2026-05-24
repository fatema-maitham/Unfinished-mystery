using TMPro;
using UnityEngine;

// After the final book spread is reached, the gramophone becomes playable.
public class GramophoneInteract : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private KeyCode playKey = KeyCode.P;

    [Header("Prompt UI")]
    [SerializeField] private ActivationPromptUI promptUI;

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

        bool nearGramophone = Vector3.Distance(player.position, transform.position) <= interactDistance;

        bool bookshelfDone = Level2PuzzleSystem.Instance != null &&
                             Level2PuzzleSystem.Instance.BookshelfClueFound;

        if (!nearGramophone || !bookshelfDone || messagePlayed)
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