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
public class ActivatableLockedDrawer : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label         = "Examine";
    [SerializeField] private string subLabel      = "Locked Drawer";
    [SerializeField] private float  activationRadius = 1.5f;

    [Header("Blocked Messages")]
    [SerializeField] private string blockedEarlyMessage =
        "A locked drawer. You need to figure out the combination first.";
    [SerializeField] private string blockedNoDeskMessage =
        "You can't open it yet. Examine the desk first.";

    [Header("Code Entry UI")]
    [Tooltip("Assign the DrawerCodePanel prefab (separate Canvas panel, starts disabled)")]
    [SerializeField] private GameObject codeEntryPanel;
    [SerializeField] private TMP_InputField codeInputField;
    [SerializeField] private Button submitButton;
    [SerializeField] private TMP_Text feedbackText;

    [Header("On Success")]
    [SerializeField] private Sprite drawerContentsImage; // USB + sticky note image
    [TextArea(2, 5)]
    [SerializeField] private string successText =
        "The drawer clicks open.\n\nInside: A USB drive labeled \"N.O. — Final Submission\"\n" +
        "A sticky note in Lynnette's handwriting:\n\"It's all on the drive. Everything you told me to delete. I didn't.\"";

    [Header("Correct Code")]
    [SerializeField] private string correctCode = "433";

    private bool _unlocked = false;

    public string ActivationLabel  => _unlocked ? "Open" : "Examine Lock";
    public string ActivationHint   => subLabel;
    public bool   CanActivate      => true;
    public float  ActivationRadius => activationRadius;

    private void Start()
    {
        if (submitButton != null)
            submitButton.onClick.AddListener(OnSubmitCode);

        if (codeEntryPanel != null)
            codeEntryPanel.SetActive(false);
    }

    public void OnActivate(GameObject source)
    {
        // Already unlocked — just show contents
        if (_unlocked)
        {
            ShowDrawerContents();
            return;
        }

        // Gate: desk must be done first
        if (!Level1PuzzleSystem.Instance.DeskPhaseComplete)
        {
            Level1PuzzleSystem.ShowBlocked(blockedNoDeskMessage);
            return;
        }

        // Gate: bookshelf must be done to get the combination
        if (!Level1PuzzleSystem.Instance.BookshelfPhaseComplete)
        {
            Level1PuzzleSystem.ShowBlocked(blockedEarlyMessage);
            return;
        }

        // Open code entry panel
        OpenCodeEntry();
    }

    private void OpenCodeEntry()
    {
        if (codeEntryPanel != null)
        {
            codeEntryPanel.SetActive(true);
            Time.timeScale = 0f;
            if (feedbackText != null) feedbackText.text = "";
            if (codeInputField != null) codeInputField.text = "";
        }
        else
        {
            // Fallback if no UI panel assigned — prompt via dialog
            ActivationDialogUI.ShowText(
                "Enter the 3-digit combination.\nHint: Count the letters in WHEN, YOU, LIE.",
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

    // Allow closing panel with E key
    private void Update()
    {
        if (codeEntryPanel != null && codeEntryPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseCodeEntry();
            }
        }
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}