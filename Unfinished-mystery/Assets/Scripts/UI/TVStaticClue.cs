using System.Collections;
using TMPro;
using UnityEngine;

public class TVStaticClue : MonoBehaviour
{
    [Header("Prompt")]
    public InteractionPromptUI interactPrompt;
    public string promptText = "Listen";

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
        if (playerInRange &&
            !messageShowing &&
            Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ShowMessage());
        }
    }

    private IEnumerator ShowMessage()
    {
        messageShowing = true;

        if (interactPrompt != null)
            interactPrompt.HidePrompt();

        if (messageText != null)
            messageText.text = clueMessage;

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

                messagePanel.anchoredPosition =
                    Vector2.Lerp(hiddenPos, shownPos, t);

                if (messageCanvasGroup != null)
                    messageCanvasGroup.alpha =
                        Mathf.Lerp(0f, 1f, t);

                yield return null;
            }

            yield return new WaitForSeconds(messageStayTime);

            t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * 3f;

                messagePanel.anchoredPosition =
                    Vector2.Lerp(shownPos, hiddenPos, t);

                if (messageCanvasGroup != null)
                    messageCanvasGroup.alpha =
                        Mathf.Lerp(1f, 0f, t);

                yield return null;
            }

            messagePanel.anchoredPosition = shownPos;
            messagePanel.gameObject.SetActive(false);
        }

        messageShowing = false;

        if (playerInRange && interactPrompt != null)
            interactPrompt.ShowPrompt(promptText);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactPrompt != null)
                interactPrompt.ShowPrompt(promptText);
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