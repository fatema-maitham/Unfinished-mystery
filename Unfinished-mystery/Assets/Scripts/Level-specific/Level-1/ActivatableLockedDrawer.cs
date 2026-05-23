using UnityEngine;
using TMPro;
using UnityEngine.UI;

// ═══════════════════════════════════════════════════════════════════════════════
// PUZZLE 3 — Locked Desk Drawer
// Blocked until bookshelf phase is complete.
// Player must enter code 433. On success, reveals USB + sticky note.
// Uses a simple 3-digit UI panel driven by ActivationDialogUI's image panel
// swapped for a custom code-entry panel.
// ═══════════════════════════════════════════════════════════════════════════════
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Puzzle 3 — Locked Desk Drawer
/// Two gate checks: desk phase AND bookshelf phase must both be complete.
/// Player enters 3-digit code 433 via a UI panel.
/// Code logic: first words on pages 12, 31, 53 of the book are WHEN (4), YOU (3), LIE (3).
/// On success: reveals USB drive + Lynnette's sticky note inside.
/// Re-interacting after unlock just shows the drawer contents.
/// Close the code panel with Escape without submitting.
/// </summary>
public class ActivatableLockedDrawer : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label            = "Examine Lock";
    [SerializeField] private string subLabel         = "Locked Drawer";
    [SerializeField] private float  activationRadius = 1.5f;

    [Header("Blocked Messages")]
    [SerializeField] private string blockedNoDeskMessage =
        "You can't open it yet. Examine the desk first.";
    [SerializeField] private string blockedNoBookshelfMessage =
        "A locked drawer. You need to figure out the combination first.";

    [Header("Code Entry UI")]
    [Tooltip("A panel under HUD_Canvas — needs InputField, Button (Submit), and TMP_Text (feedback)")]
    [SerializeField] private GameObject     codeEntryPanel;
    [SerializeField] private TMP_InputField codeInputField;
    [SerializeField] private Button         submitButton;
    [SerializeField] private TMP_Text       feedbackText;

    [Header("Correct Code")]
    [SerializeField] private string correctCode = "433";

    [Header("On Success")]
    [Tooltip("Optional image of the open drawer — USB and sticky note inside")]
    [SerializeField] private Sprite drawerContentsImage;
    [TextArea(2, 5)]
    [SerializeField] private string successText =
        "The drawer clicks open.\n\n" +
        "Inside: A USB drive labeled \"N.O. — Final Submission\"\n\n" +
        "A sticky note in Lynnette's handwriting:\n" +
        "\"It's all on the drive. Everything you told me to delete. I didn't.\"";

    // ── State ─────────────────────────────────────────────────────────────────
    private bool _unlocked = false;

    // ── IActivatable ──────────────────────────────────────────────────────────
    public string ActivationLabel  => _unlocked ? "Open" : label;
    public string ActivationHint   => subLabel;
    public bool   CanActivate      => true;
    public float  ActivationRadius => activationRadius;

    private void Start()
    {
        if (submitButton    != null) submitButton.onClick.AddListener(OnSubmitCode);
        if (codeEntryPanel  != null) codeEntryPanel.SetActive(false);
    }

    public void OnActivate(GameObject source)
    {
        if (_unlocked)
        {
            ShowDrawerContents();
            return;
        }

        if (!Level1PuzzleSystem.Instance.DeskPhaseComplete)
        {
            Level1PuzzleSystem.ShowBlocked(blockedNoDeskMessage);
            return;
        }

        if (!Level1PuzzleSystem.Instance.BookshelfPhaseComplete)
        {
            Level1PuzzleSystem.ShowBlocked(blockedNoBookshelfMessage);
            return;
        }

        OpenCodeEntry();
    }

    private void OpenCodeEntry()
    {
        if (codeEntryPanel != null)
        {
            codeEntryPanel.SetActive(true);
            Time.timeScale = 0f;
            if (feedbackText  != null) feedbackText.text  = "";
            if (codeInputField != null) codeInputField.text = "";
        }
        else
        {
            // Fallback if no panel assigned
            ActivationDialogUI.ShowText(
                "Enter the 3-digit combination.\n\nHint: Count the letters in WHEN, YOU, LIE.",
                "Locked Drawer");
        }
    }

    private void OnSubmitCode()
    {
        if (codeInputField == null) return;

        string entered = codeInputField.text.Trim();

        if (entered == correctCode)
        {
            CloseCodeEntry();
            _unlocked = true;
            Level1PuzzleSystem.Instance?.UnlockDrawer();
            ShowDrawerContents();
        }
        else
        {
            if (feedbackText != null)
                feedbackText.text = "Wrong combination. Try again.";
        }
    }

    private void CloseCodeEntry()
    {
        if (codeEntryPanel != null)
            codeEntryPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void ShowDrawerContents()
    {
        if (drawerContentsImage != null)
            ActivationDialogUI.ShowImage(drawerContentsImage);
        else
            ActivationDialogUI.ShowText(successText, "Desk Drawer");
    }

    private void Update()
    {
        if (codeEntryPanel != null && codeEntryPanel.activeSelf)
            if (Input.GetKeyDown(KeyCode.Escape))
                CloseCodeEntry();
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}