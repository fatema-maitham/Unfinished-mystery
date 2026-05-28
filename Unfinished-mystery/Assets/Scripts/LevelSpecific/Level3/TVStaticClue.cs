using System.Collections;
using TMPro;
using UnityEngine;

public class TVStaticClue : MonoBehaviour
{
    [Header("Prompt")]
    public InteractionPromptUI interactPrompt;
    public string promptText = "LISTEN";
    public string promptSubLabel = "Static";

    [Header("Sound")]
    public TVStaticSoundController staticSoundController;

    [Header("Message UI")]
    public RectTransform messagePanel;
    public CanvasGroup messageCanvasGroup;
    public TMP_Text messageText;

    [TextArea]
    public string clueMessage =
        "A broken voice whispers:\n“Behind the mirror…”";

    public float messageStayTime = 4f;

    private bool playerInRange = false;
    private bool messageShowing = false;

    private void Start()
    {
        if (messagePanel != null)
            messagePanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && !messageShowing && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ShowMessage());
        }
    }

   private IEnumerator ShowMessage()
{
    messageShowing = true;

    if (interactPrompt != null)
        interactPrompt.HidePrompt();

    if (staticSoundController != null &&
        staticSoundController.staticAudioSource != null)
    {
staticSoundController.staticAudioSource.PlayOneShot(
    staticSoundController.staticAudioSource.clip
);    }

    if (messageText != null)
        messageText.text = clueMessage;

    if (messagePanel != null)
        messagePanel.gameObject.SetActive(true);

    yield return new WaitForSeconds(messageStayTime);

    if (messagePanel != null)
        messagePanel.gameObject.SetActive(false);

    messageShowing = false;

    if (playerInRange && interactPrompt != null)
        interactPrompt.ShowPrompt(promptText, promptSubLabel);
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactPrompt != null)
                interactPrompt.ShowPrompt(promptText, promptSubLabel);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactPrompt != null)
                interactPrompt.HidePrompt();
        }
    }
}