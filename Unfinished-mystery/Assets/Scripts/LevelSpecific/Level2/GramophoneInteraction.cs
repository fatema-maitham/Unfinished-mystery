using UnityEngine;

public class GramophoneInteraction : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource gramophoneAudio;

    [Header("Optional UI")]
    [SerializeField] private L2NoteUI noteUI;

    [TextArea(3, 6)]
    [SerializeField] private string message =
        "If you found his number, then you are closer to the truth than I ever wanted you to be.\n\nThe next truth only appears where the light is strongest.";

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Behavior")]
    [SerializeField] private bool playOnlyOnce = true;
    [SerializeField] private bool showTextMessage = true;
    [SerializeField] private bool stopWhenPlayerLeaves = false;

    private bool playerInside;
    private bool hasPlayed;

    private void Update()
    {
        if (!playerInside)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            PlayGramophone();
        }
    }

    private void PlayGramophone()
    {
        if (playOnlyOnce && hasPlayed)
            return;

        hasPlayed = true;

        if (gramophoneAudio != null)
        {
            gramophoneAudio.Stop();
            gramophoneAudio.time = 0f;
            gramophoneAudio.Play();
        }
        else
        {
            Debug.LogWarning("[GramophoneInteraction] AudioSource is not assigned.");
        }

        if (showTextMessage && noteUI != null)
        {
            noteUI.ShowNote(message);
        }

        Debug.Log("[Level2] Gramophone message played.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        if (stopWhenPlayerLeaves && gramophoneAudio != null)
            gramophoneAudio.Stop();
    }
}