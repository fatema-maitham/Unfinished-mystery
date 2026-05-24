using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ═══════════════════════════════════════════════════════════════════════════════
// LEVEL 2 — FINAL PHONE NUMBER CLUE
// Flow:
// 1) Player approaches painting_3.
// 2) Prompt appears.
// 3) Press E → small front paper image appears.
// 4) Press E → flips to back paper image.
// 5) Press E → black DialogRoot appears with the phone number text.
// 6) Press E → closes everything.
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

    [Header("Small Paper Image UI")]
    [SerializeField] private GameObject imagePanel;
    [SerializeField] private Image displayImage;
    [SerializeField] private Sprite frontPaperImage;
    [SerializeField] private Sprite backPaperImage;

    [Header("Dialog UI")]
    [SerializeField] private GameObject dialogRoot;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogBodyText;
    [SerializeField] private TMP_Text continueHintText;

    [Header("Dialog Text")]
    [SerializeField] private string dialogTitle = "TORN PAPER — BACK";

    [TextArea(4, 10)]
    [SerializeField] private string dialogBody =
        "On the back, written in tiny pencil:\n\n" +
        "916-2847\n\n" +
        "\"Isla's number. Lana kept it hidden for years.\"";

    private enum ClueState
    {
        Closed,
        ShowingFront,
        ShowingBack,
        ShowingDialog
    }

    private ClueState state = ClueState.Closed;
    private bool clueFound = false;

    private void Start()
    {
        if (imagePanel != null)
            imagePanel.SetActive(false);

        if (dialogRoot != null)
            dialogRoot.SetActive(false);
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
            OpenFrontImage();
    }

    private void OpenFrontImage()
    {
        state = ClueState.ShowingFront;
        promptUI.HidePrompt();

        if (imagePanel != null)
            imagePanel.SetActive(true);

        if (displayImage != null && frontPaperImage != null)
            displayImage.sprite = frontPaperImage;

        if (continueHintText != null)
            continueHintText.text = "E Flip Over";
    }

    private void ContinueClue()
    {
        if (state == ClueState.ShowingFront)
        {
            ShowBackImage();
            return;
        }

        if (state == ClueState.ShowingBack)
        {
            ShowDialog();
            return;
        }

        if (state == ClueState.ShowingDialog)
        {
            CloseClue();
        }
    }

    private void ShowBackImage()
    {
        state = ClueState.ShowingBack;

        if (displayImage != null && backPaperImage != null)
            displayImage.sprite = backPaperImage;

        if (continueHintText != null)
            continueHintText.text = "E Continue";
    }

    private void ShowDialog()
    {
        state = ClueState.ShowingDialog;

        if (imagePanel != null)
            imagePanel.SetActive(false);

        if (dialogRoot != null)
            dialogRoot.SetActive(true);

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
    }

    private void CloseClue()
    {
        state = ClueState.Closed;

        if (imagePanel != null)
            imagePanel.SetActive(false);

        if (dialogRoot != null)
            dialogRoot.SetActive(false);
    }
}