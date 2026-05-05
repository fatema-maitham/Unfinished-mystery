using UnityEngine;
using TMPro;
using System.Collections;

public class TVInteraction : MonoBehaviour
{
    [Header("Message")]
    [TextArea(2, 5)]
    [SerializeField] private string message = "She drew what I refused to write.";

    [Header("Objects")]
    [SerializeField] private GameObject tvStatic;
    [SerializeField] private GameObject tvMessage;
    [SerializeField] private TMP_Text messageText;

    [Header("Audio")]
    [SerializeField] private AudioSource staticSound;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private bool requireKeyPress = true;

    [Header("Timing")]
    [SerializeField] private float staticDuration = 1.5f;

    [Header("Behavior")]
    [SerializeField] private bool playOnlyOnce = true;
    [SerializeField] private bool stopWhenPlayerLeaves = true;

    private bool playerInside;
    private bool hasPlayed;
    private bool isPlaying;
    private Coroutine currentSequence;

    private void Start()
    {
        ResetTV();
    }

    private void Update()
    {
        if (!playerInside)
            return;

        if (!requireKeyPress)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            TryPlayTV();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;

        if (!requireKeyPress)
        {
            TryPlayTV();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        if (stopWhenPlayerLeaves)
        {
            StopTVSequence();
            ResetTV();
        }
    }

    private void TryPlayTV()
    {
        if (isPlaying)
            return;

        if (playOnlyOnce && hasPlayed)
            return;

        currentSequence = StartCoroutine(TVSequence());
    }

    private IEnumerator TVSequence()
    {
        isPlaying = true;
        hasPlayed = true;

        // Clean old/random text first.
        if (tvMessage != null)
            tvMessage.SetActive(false);

        if (messageText != null)
            messageText.text = "";

        // Static starts.
        if (tvStatic != null)
            tvStatic.SetActive(true);

        if (staticSound != null)
        {
            staticSound.Stop();
            staticSound.time = 0f;
            staticSound.Play();
        }

        yield return new WaitForSeconds(staticDuration);

        // Static stops.
        if (tvStatic != null)
            tvStatic.SetActive(false);

        if (staticSound != null)
            staticSound.Stop();

        // Real message appears and stays until player leaves.
        if (tvMessage != null)
            tvMessage.SetActive(true);

        if (messageText != null)
            messageText.text = message;

        currentSequence = null;
    }

    private void StopTVSequence()
    {
        if (currentSequence != null)
        {
            StopCoroutine(currentSequence);
            currentSequence = null;
        }

        isPlaying = false;

        if (staticSound != null)
            staticSound.Stop();
    }

    private void ResetTV()
    {
        if (tvStatic != null)
            tvStatic.SetActive(false);

        if (tvMessage != null)
            tvMessage.SetActive(false);

        if (messageText != null)
            messageText.text = "";
    }

    public void ResetForNewLoop()
    {
        hasPlayed = false;
        isPlaying = false;
        StopTVSequence();
        ResetTV();
    }
}