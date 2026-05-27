using System.Collections;
using UnityEngine;
using TMPro;

public class ActivatePhoneCall : MonoBehaviour
{
    [Header("Prompt UI")]
    [SerializeField] private ActivationPromptUI promptUI;

    [Header("Prompt Text")]
    [SerializeField] private string label = "Use";
    [SerializeField] private string subLabel = "Phone";

    [Header("Message UI")]
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TMP_Text messageText;

    [Header("Audio")]
    [SerializeField] private AudioSource ringAudioSource;
    [SerializeField] private AudioSource callAudioSource;

    [Header("SIM Visual")]
    [SerializeField] private GameObject insertedSimCardObject;

    [Header("Messages")]
    [TextArea(2, 4)]
    [SerializeField] private string noSimMessage = "The phone is dead. Something is missing.";

    [TextArea(2, 4)]
    [SerializeField] private string insertingMessage = "SIM card inserted...";

    [TextArea(3, 6)]
    [SerializeField] private string finalMessage = "You found what she hid. The truth was never gone.";

    [Header("Timing")]
    [SerializeField] private float insertingMessageDuration = 2f;
    [SerializeField] private float ringingDuration = 7f;
    [SerializeField] private float finalMessageDuration = 5f;

    private bool playerInside;
    private bool hasPlayed;
    private bool isPlaying;

    private void Start()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);

        if (insertedSimCardObject != null)
            insertedSimCardObject.SetActive(false);
    }

    private void Update()
    {
        if (!playerInside) return;
        if (hasPlayed || isPlaying) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PhoneSequence());
        }
    }

    private IEnumerator PhoneSequence()
    {
        isPlaying = true;

        if (promptUI != null)
            promptUI.HidePrompt();

        if (!SimCardPickup.HasSimCard)
        {
            ShowMessage(noSimMessage);
            yield return new WaitForSeconds(2.5f);
            HideMessage();

            isPlaying = false;

            if (playerInside && promptUI != null)
                promptUI.ShowPrompt(label, subLabel);

            yield break;
        }

        hasPlayed = true;

        if (insertedSimCardObject != null)
            insertedSimCardObject.SetActive(true);

        ShowMessage(insertingMessage);
        yield return new WaitForSeconds(insertingMessageDuration);
        HideMessage();

        if (ringAudioSource != null)
            ringAudioSource.Play();

        yield return new WaitForSeconds(ringingDuration);

        if (ringAudioSource != null)
            ringAudioSource.Stop();

        if (callAudioSource != null)
            callAudioSource.Play();

        ShowMessage(finalMessage);
        yield return new WaitForSeconds(finalMessageDuration);
        HideMessage();

        isPlaying = false;
    }

    private void ShowMessage(string text)
    {
        if (messagePanel != null)
            messagePanel.SetActive(true);

        if (messageText != null)
            messageText.text = text;
    }

    private void HideMessage()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (hasPlayed) return;

        playerInside = true;

        if (promptUI != null)
            promptUI.ShowPrompt(label, subLabel);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;

        if (promptUI != null)
            promptUI.HidePrompt();
    }
}