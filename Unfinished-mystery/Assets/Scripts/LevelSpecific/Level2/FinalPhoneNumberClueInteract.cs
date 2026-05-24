using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ═══════════════════════════════════════════════════════════════════════════════
// LEVEL 2 — FINAL PHONE NUMBER CLUE
// Attach this script to: painting_3
// Flow:
// Near painting_3 → prompt appears
// Press E → phone number image appears
// Press E → black dialog text appears
// Press E → close
// ═══════════════════════════════════════════════════════════════════════════════
public class FinalPhoneNumberClueInteract : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Prompt UI")]
    [SerializeField] private ActivationPromptUI promptUI;
    [SerializeField] private string promptLabel = "Inspect";
    [SerializeField] private string promptSubLabel = "Wall Frame";

    [Header("DialogRoot UI")]
    [SerializeField] private GameObject dialogRoot;
    [SerializeField] private GameObject imagePanel;
    [SerializeField] private GameObject textPanel;

    [Header("Image UI")]
    [SerializeField] private Image displayImage;
    [SerializeField] private Sprite phoneNumberPaperImage;

    [Header("Text UI")]
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogBodyText;
    [SerializeField] private TMP_Text continueHintText;

    [Header("Dialog Text")]
    [SerializeField] private string dialogTitle = "Torn paper - BACK";

    [TextArea(4, 10)]
    [SerializeField] private string dialogBody =
        "On the back, written in tiny pencil:\n\n" +
        "916-2847\n\n" +
        "\"Isla's number. Lana kept it hidden for years.\"";

    [Header("Input Safety")]
    [SerializeField] private float inputDelay = 0.35f;

    private enum ClueState
    {
        Closed,
        ShowingImage,
        ShowingText
    }

    private ClueState state = ClueState.Closed;
    private float nextInputTime = 0f;
    private bool clueFound = false;

    private void Start()
    {
        CloseEverything();
    }

    private void Update()
    {
        if (player == null || promptUI == null)
            return;

        if (state == ClueState.ShowingImage)
        {
            if (Time.unscaledTime >= nextInputTime && Input.GetKeyDown(interactKey))
                ShowTextPanel();

            return;
        }

        if (state == ClueState.ShowingText)
        {
            if (Time.unscaledTime >= nextInputTime && Input.GetKeyDown(interactKey))
                CloseEverything();

            return;
        }

        float distance = Vector3.Distance(player.position, transform.position);
        bool nearFrame = distance <= interactDistance;

        if (!nearFrame)
        {
            promptUI.HidePrompt();
            return;
        }

        promptUI.ShowPrompt(promptLabel, promptSubLabel);

        if (Input.GetKeyDown(interactKey))
            ShowImagePanel();
    }

    private void ShowImagePanel()
    {
        state = ClueState.ShowingImage;
        nextInputTime = Time.unscaledTime + inputDelay;

        promptUI.HidePrompt();

        if (dialogRoot != null)
        {
            dialogRoot.SetActive(true);
            dialogRoot.transform.SetAsLastSibling();
        }

        if (textPanel != null)
            textPanel.SetActive(false);

        if (imagePanel != null)
        {
            imagePanel.SetActive(true);
            imagePanel.transform.SetAsLastSibling();
        }

        if (displayImage != null)
        {
            displayImage.gameObject.SetActive(true);
            displayImage.enabled = true;
            displayImage.color = Color.white;
            displayImage.sprite = phoneNumberPaperImage;
        }

        Debug.Log("[FinalPhoneNumberClue] Image shown.");
    }

    private void ShowTextPanel()
    {
        state = ClueState.ShowingText;
        nextInputTime = Time.unscaledTime + inputDelay;

        if (imagePanel != null)
            imagePanel.SetActive(false);

        if (dialogRoot != null)
        {
            dialogRoot.SetActive(true);
            dialogRoot.transform.SetAsLastSibling();
        }

        if (textPanel != null)
        {
            textPanel.SetActive(true);
            textPanel.transform.SetAsLastSibling();
        }

        if (speakerNameText != null)
            speakerNameText.text = dialogTitle;

        if (dialogBodyText != null)
            dialogBodyText.text = dialogBody;

        if (continueHintText != null)
            continueHintText.text = "E Close";

        if (!clueFound)
        {
            clueFound = true;
            Level2PuzzleSystem.Instance?.FindPhoneNumber();
        }

        Debug.Log("[FinalPhoneNumberClue] Text shown.");
    }

    private void CloseEverything()
    {
        state = ClueState.Closed;

        if (dialogRoot != null)
            dialogRoot.SetActive(false);

        if (imagePanel != null)
            imagePanel.SetActive(false);

        if (textPanel != null)
            textPanel.SetActive(false);

        Debug.Log("[FinalPhoneNumberClue] Closed.");
    }
}