using System.Collections;
using TMPro;
using UnityEngine;

public class L3ExitInspect : MonoBehaviour
{
    [Header("Prompt UI")]
    public InteractionPromptUI interactPrompt;
    public string promptText = "INSPECT";
    public string promptSubLabel = "Exit Door";

    [Header("Inspect Message")]
    public RectTransform messagePanel;
    public CanvasGroup messageCanvasGroup;
    public TMP_Text messageText;

    [TextArea]
    public string inspectMessage =
        "The exit was locked from the inside.\nSomeone tried to force it open.";

    public float messageStayTime = 4f;

    private bool playerInRange = false;
    private bool messageShowing = false;
    private bool inspectDisabled = false;

    private void Start()
    {
        if (messagePanel != null)
            messagePanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (inspectDisabled)
            return;

        if (playerInRange && !messageShowing && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ShowInspectMessage());
        }
    }

    private IEnumerator ShowInspectMessage()
    {
        messageShowing = true;

        if (interactPrompt != null)
            interactPrompt.HidePrompt();

        if (messageText != null)
            messageText.text = inspectMessage;

        if (messagePanel != null)
        {
            messagePanel.gameObject.SetActive(true);

            Vector2 shownPos = messagePanel.anchoredPosition;
            Vector2 hiddenPos = shownPos + new Vector2(700f, 0f);

            messagePanel.anchoredPosition = hiddenPos;

            if (messageCanvasGroup != null)
                messageCanvasGroup.alpha = 0f;

            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * 3f;
                messagePanel.anchoredPosition = Vector2.Lerp(hiddenPos, shownPos, t);

                if (messageCanvasGroup != null)
                    messageCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

                yield return null;
            }

            yield return new WaitForSeconds(messageStayTime);

            t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * 3f;
                messagePanel.anchoredPosition = Vector2.Lerp(shownPos, hiddenPos, t);

                if (messageCanvasGroup != null)
                    messageCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

                yield return null;
            }

            messagePanel.anchoredPosition = shownPos;
            messagePanel.gameObject.SetActive(false);
        }

        messageShowing = false;

        if (!inspectDisabled && playerInRange && interactPrompt != null)
            interactPrompt.ShowPrompt(promptText, promptSubLabel);
    }

    public void DisableInspectPermanently()
{
    inspectDisabled = true;
    playerInRange = false;

    if (interactPrompt != null)
        interactPrompt.HidePrompt();

    Collider col = GetComponent<Collider>();
    if (col != null)
        col.enabled = false;
}

    private void OnTriggerEnter(Collider other)
    {
        if (inspectDisabled)
            return;

        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactPrompt != null && !messageShowing)
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



    public void PauseInspectBriefly(float seconds)
{
    if (inspectDisabled)
        return;

    if (interactPrompt != null)
        interactPrompt.HidePrompt();
}
}