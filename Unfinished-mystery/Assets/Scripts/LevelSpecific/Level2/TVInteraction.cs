using UnityEngine;
using TMPro;
using System.Collections;

public class TVInteraction : MonoBehaviour
{
    [Header("Prompt UI")]
    [SerializeField] private ActivationPromptUI promptUI;

    [Header("Prompt")]
    [SerializeField] private string label = "Examine";
    [SerializeField] private string subLabel = "Television";

    [Header("TV Objects")]
    [SerializeField] private GameObject tvStatic;
    [SerializeField] private GameObject tvMessage;
    [SerializeField] private TMP_Text messageText;

    [Header("Audio")]
    [SerializeField] private AudioSource staticSound;

    [Header("Message")]
    [TextArea(2, 5)]
    [SerializeField] private string message = "She drew what I refused to write.";

    [Header("Input")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Timing")]
    [SerializeField] private float staticDuration = 1.2f;
    [SerializeField] private float typingSpeed = 0.05f;

    private bool playerInside;
    private bool hasPlayed;
    private bool isPlaying;

    private void Start()
    {
        if (tvStatic != null) tvStatic.SetActive(false);
        if (tvMessage != null) tvMessage.SetActive(false);
        if (messageText != null) messageText.text = "";
    }

    private void Update()
    {
        if (!playerInside) return;
        if (isPlaying) return;

        if (!hasPlayed)
        {
            promptUI?.ShowPrompt(label, subLabel);
        }

        if (Input.GetKeyDown(interactKey) && !hasPlayed)
        {
            promptUI?.HidePrompt();
            StartCoroutine(PlayTVSequence());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!Level2PuzzleSystem.Instance.FirstNoteRead) return;

        playerInside = true;

        if (!hasPlayed && !isPlaying)
            promptUI?.ShowPrompt(label, subLabel);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        promptUI?.HidePrompt();
    }

    private IEnumerator PlayTVSequence()
    {
        isPlaying = true;
        hasPlayed = true;

        if (tvStatic != null) tvStatic.SetActive(true);
        if (tvMessage != null) tvMessage.SetActive(false);
        if (messageText != null) messageText.text = "";

        yield return new WaitForSeconds(staticDuration);

        if (tvMessage != null) tvMessage.SetActive(true);

        foreach (char c in message)
        {
            if (messageText != null) messageText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (staticSound != null) staticSound.Stop();
        if (tvStatic != null) tvStatic.SetActive(false);

        Level2PuzzleSystem.Instance?.SeeTVMessage();

        isPlaying = false;
    }
}