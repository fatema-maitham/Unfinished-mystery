using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ═══════════════════════════════════════════════════════════════════════════════
// LEVEL 2 — FINAL PHONE NUMBER CLUE
// Flow:
// 1) Player approaches painting_3.
// 2) Prompt appears.
// 3) Press E → small torn paper image with the phone number appears.
// 4) Press E again → image hides and DialogRoot black text panel appears.
// 5) Press E again → closes everything.
// Attach this script to: painting_3
// ═══════════════════════════════════════════════════════════════════════════════
public class FinalPhoneNumberClueInteract : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Prompt UI")]
    [SerializeField] private ActivationPromptUI promptUI;
    [SerializeField] private string promptLabel = "Inspect";
    [SerializeField] private string promptSubLabel = "Wall Frame";

    [Header("DialogRoot UI")]
    [SerializeField] private GameObject dialogRoot;
    [SerializeField] private GameObject textPanel;
    [SerializeField] private GameObject imagePanel;

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

    private enum ClueState
    {
        Closed,
        ShowingImage,
        ShowingText
    }

    private ClueState state = ClueState.Closed;
    private bool clueFound = false;

    private void Start()
    {
        HideAll();
    }

    private void Update()
    {
        if (player == null || promptUI == null)
            return;

        if (state != ClueState.Closed)
        {
            if (Input.GetKeyDown(interactKey))
                ContinueClue();

            return;
        }

        bool nearFrame = Vector3.Distance(player.position, transform.position) <= interactDistance;

        if (!nearFrame)
        {
            promptUI.HidePrompt();
            return;
        }

        promptUI.ShowPrompt(promptLabel, promptSubLabel);

        if (Input.GetKeyDown(interactKey))
            ShowPhoneNumberImage();
    }

private void ShowPhoneNumberImage()
{
    state = ClueState.ShowingImage;
    promptUI.HidePrompt();

    if (dialogRoot != null)
    {
        dialogRoot.SetActive(true);
        dialogRoot.transform.SetAsLastSibling();

        CanvasGroup cg = dialogRoot.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 1f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
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

        if (phoneNumberPaperImage != null)
            displayImage.sprite = phoneNumberPaperImage;
    }

    if (continueHintText != null)
        continueHintText.text = "E Continue";

    Debug.Log("[FinalPhoneNumberClue] Showing phone number image.");
}

    private void ContinueClue()
    {
        if (state == ClueState.ShowingImage)
        {
            ShowBlackDialogText();
            return;
        }

        if (state == ClueState.ShowingText)
        {
            CloseClue();
        }
    }

private void ShowBlackDialogText()
{
    state = ClueState.ShowingText;

    if (dialogRoot != null)
    {
        dialogRoot.SetActive(true);
        dialogRoot.transform.SetAsLastSibling();
    }

    if (imagePanel != null)
        imagePanel.SetActive(false);

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

    Debug.Log("[FinalPhoneNumberClue] Showing dialog text.");
}

    private void CloseClue()
    {
        state = ClueState.Closed;
        HideAll();
    }

    private void HideAll()
    {
        if (dialogRoot != null)
            dialogRoot.SetActive(false);

        if (textPanel != null)
            textPanel.SetActive(false);

        if (imagePanel != null)
            imagePanel.SetActive(false);
    }
}