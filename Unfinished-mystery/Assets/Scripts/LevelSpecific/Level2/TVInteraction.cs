using UnityEngine;
using TMPro;
using System.Collections;

// Handles the TV interaction after the first note is read
public class TVInteraction : MonoBehaviour
{
    [Header("Prompt UI")]
    [SerializeField] private ActivationPromptUI promptUI; // Shared prompt UI system

    [Header("Prompt")]
    [SerializeField] private string label = "Examine"; // Main prompt text
    [SerializeField] private string subLabel = "Television"; // Small prompt text

    [Header("TV Objects")]
    [SerializeField] private GameObject tvStatic; // Static effect shown first
    [SerializeField] private GameObject tvMessage; // Message object shown after static
    [SerializeField] private TMP_Text messageText; // Text component for typed TV message

    [Header("Audio")]
    [SerializeField] private AudioSource staticSound; // Static sound audio source

    [Header("Message")]
    [TextArea(2, 5)]
    [SerializeField] private string message = "She drew what I refused to write."; // TV message content

    [Header("Input")]
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Key used to examine TV

    [Header("Timing")]
    [SerializeField] private float staticDuration = 1.2f; // How long static stays before message
    [SerializeField] private float typingSpeed = 0.05f; // Delay between each typed character

    private bool playerInside; // True when player is inside TV trigger
    private bool hasPlayed; // Prevents TV sequence from playing more than once
    private bool isPlaying; // Prevents repeated input while TV sequence is running

    private void Start()
    {
        // Hide TV static at the start
        if (tvStatic != null) tvStatic.SetActive(false);

        // Hide TV message at the start
        if (tvMessage != null) tvMessage.SetActive(false);

        // Clear message text at the start
        if (messageText != null) messageText.text = "";
    }

    private void Update()
    {
        // Stop if player is not inside TV trigger
        if (!playerInside) return;

        // Stop if TV sequence is already playing
        if (isPlaying) return;

        // Show prompt if TV has not played yet
        if (!hasPlayed)
        {
            promptUI?.ShowPrompt(label, subLabel);
        }

        // Start TV sequence when player presses E
        if (Input.GetKeyDown(interactKey) && !hasPlayed)
        {
            promptUI?.HidePrompt();
            StartCoroutine(PlayTVSequence());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only player can trigger the TV
        if (!other.CompareTag("Player")) return;

        // TV is locked until the first note is read
        if (!Level2PuzzleSystem.Instance.FirstNoteRead) return;

        // Mark player as inside TV trigger
        playerInside = true;

        // Show prompt if TV has not played yet
        if (!hasPlayed && !isPlaying)
            promptUI?.ShowPrompt(label, subLabel);
    }

    private void OnTriggerExit(Collider other)
    {
        // Only react when player exits
        if (!other.CompareTag("Player")) return;

        // Mark player as outside TV trigger
        playerInside = false;

        // Hide prompt when leaving TV area
        promptUI?.HidePrompt();
    }

    private IEnumerator PlayTVSequence()
    {
        // Mark sequence as playing and prevent replay
        isPlaying = true;
        hasPlayed = true;

        // Show static first and hide message
        if (tvStatic != null) tvStatic.SetActive(true);
        if (tvMessage != null) tvMessage.SetActive(false);
        if (messageText != null) messageText.text = "";

        // Wait during static effect
        yield return new WaitForSeconds(staticDuration);

        // Show message object
        if (tvMessage != null) tvMessage.SetActive(true);

        // Type the message one character at a time
        foreach (char c in message)
        {
            if (messageText != null) messageText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Stop static sound and hide static effect
        if (staticSound != null) staticSound.Stop();
        if (tvStatic != null) tvStatic.SetActive(false);

        // Report TV message progress to Level 2 puzzle system
        Level2PuzzleSystem.Instance?.SeeTVMessage();

        // Allow sequence state to finish
        isPlaying = false;
    }
}