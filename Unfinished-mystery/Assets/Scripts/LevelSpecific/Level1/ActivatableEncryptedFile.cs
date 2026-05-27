using UnityEngine;
using TMPro;
using UnityEngine.UI;

// ═══════════════════════════════════════════════════════════════════════════════
// PUZZLE 6 — Encrypted File (ForYourEyes.txt)
// Blocked until laptop is booted.
// Player enters password 58. On success, reveals Nadia's message.
// ═══════════════════════════════════════════════════════════════════════════════
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Puzzle 6 — Encrypted File (ForYourEyes.txt)
/// Blocked until laptop is booted.
/// Player enters password 58 (sum of first 7 primes: 2+3+5+7+11+13+17).
/// The 7 comes from the book dedication: "For the 7 students who believed in me."
/// Wrong attempts show a hint without giving the answer away.
/// Can be placed on the laptop GameObject itself or as a separate trigger.
/// </summary>
public class ActivatableEncryptedFile : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label            = "Unlock File";
    [SerializeField] private string subLabel         = "ForYourEyes.txt";
    [SerializeField] private float  activationRadius = 1.5f;

    [Header("Password UI")]
    [Tooltip("Simple panel under HUD_Canvas — needs InputField, Button, and feedback Text")]
    [SerializeField] private GameObject     passwordPanel;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Button         submitButton;
    [SerializeField] private TMP_Text       feedbackText;

    [Header("Correct Password")]
    [SerializeField] private string correctPassword = "58";

    [Header("Content — shown after correct password")]
    [Tooltip("Optional image of Nadia's message")]
    [SerializeField] private Sprite nadiaMessageImage;
    [TextArea(3, 6)]
    [SerializeField] private string nadiaText =
        "The file opens. A single line from Nadia:\n\n" +
        "\"I'll expose you on 8th March if you don't come clean yourself.\"\n\n" +
        "She knew. She documented everything. And Lynnette helped her.";

    [Header("Already Decrypted — shown on repeat visits")]
    [TextArea(2, 3)]
    [SerializeField] private string alreadyDecryptedText =
        "The file is already open. Nadia's message is on screen.";

    [Header("Blocked Message")]
    [SerializeField] private string blockedMessage =
        "There's nothing to decrypt yet. Find what boots the laptop first.";

    // ── State ─────────────────────────────────────────────────────────────────
    private bool _decrypted = false;

    // ── IActivatable ──────────────────────────────────────────────────────────
    public string ActivationLabel  => _decrypted ? "Read File" : label;
    public string ActivationHint   => subLabel;
    public bool   CanActivate      => true;
    public float  ActivationRadius => activationRadius;

    private void Start()
    {
        if (submitButton != null)
            submitButton.onClick.AddListener(OnSubmitPassword);
        if (passwordPanel != null)
            passwordPanel.SetActive(false);
    }

    public void OnActivate(GameObject source)
    {
        if (!Level1PuzzleSystem.Instance.LaptopBooted)
        {
            Level1PuzzleSystem.ShowBlocked(blockedMessage);
            return;
        }

        if (_decrypted)
        {
            ShowReveal();
            return;
        }

        OpenPasswordEntry();
    }

    private void OpenPasswordEntry()
    {
        if (passwordPanel != null)
        {
            passwordPanel.SetActive(true);
            Time.timeScale = 0f;
            if (feedbackText      != null) feedbackText.text      = "";
            if (passwordInputField != null) passwordInputField.text = "";
        }
        else
        {
            // Fallback if no panel assigned
            ActivationDialogUI.ShowText(
                "Enter the password.\n\n" +
                "Hint: Sum of first n primes. n = the year he stopped being honest.",
                "Encrypted File");
        }
    }

    private void OnSubmitPassword()
    {
        if (passwordInputField == null) return;

        string entered = passwordInputField.text.Trim();

        if (entered == correctPassword)
        {
            ClosePasswordEntry();
            _decrypted = true;
            Level1PuzzleSystem.Instance?.DecryptFile();
            ShowReveal();
        }
        else
        {
            if (feedbackText != null)
                feedbackText.text = "Incorrect. Think about what number he'd call honest.";
        }
    }

    private void ClosePasswordEntry()
    {
        if (passwordPanel != null)
            passwordPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void ShowReveal()
    {
        if (nadiaMessageImage != null)
            ActivationDialogUI.ShowImage(nadiaMessageImage);
        else
            ActivationDialogUI.ShowText(_decrypted ? nadiaText : alreadyDecryptedText, "ForYourEyes.txt");
    }

    private void Update()
    {
        if (passwordPanel != null && passwordPanel.activeSelf)
            if (Input.GetKeyDown(KeyCode.Escape))
                ClosePasswordEntry();
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}


// ═══════════════════════════════════════════════════════════════════════════════
// PUZZLE 7 — The Phone (Final Step)
// Only lights up / becomes interactable after file is decrypted.
// Player sends evidence to the university board — level complete.
// ═══════════════════════════════════════════════════════════════════════════════
// public class ActivatablePhone : MonoBehaviour, IActivatable
// {
//     [Header("Prompt")]
//     [SerializeField] private string label         = "Send Evidence";
//     [SerializeField] private string subLabel      = "Phone";
//     [SerializeField] private float  activationRadius = 1.5f;
//
//     [Header("Content")]
//     [SerializeField] private Sprite phoneScreenImage; // "Send Lynnette evidence to the university board"
//     [TextArea(2, 4)]
//     [SerializeField] private string completionText =
//         "You forward the plagiarism evidence to the university board.\n\nLoop broken.\n\nLevel complete.";
//
//     [Header("Blocked Message")]
//     [SerializeField] private string blockedMessage =
//         "The phone is locked. You don't have what you need yet.";
//
//     [Header("Optional: phone glow object to enable on unlock")]
//     [SerializeField] private GameObject phoneGlowVFX;
//
//     private bool _sent = false;
//
//     public string ActivationLabel  => label;
//     public string ActivationHint   => subLabel;
//     // Phone is only interactable after the file is decrypted
//     public bool   CanActivate      => Level1PuzzleSystem.Instance != null &&
//                                       Level1PuzzleSystem.Instance.FileDecrypted;
//     public float  ActivationRadius => activationRadius;
//
//     private void Update()
//     {
//         // Enable glow VFX when phone becomes active
//         if (phoneGlowVFX != null)
//             phoneGlowVFX.SetActive(
//                 Level1PuzzleSystem.Instance != null &&
//                 Level1PuzzleSystem.Instance.FileDecrypted);
//     }
//
//     public void OnActivate(GameObject source)
//     {
//         if (!Level1PuzzleSystem.Instance.FileDecrypted)
//         {
//             Level1PuzzleSystem.ShowBlocked(blockedMessage);
//             return;
//         }
//
//         if (_sent)
//         {
//             ActivationDialogUI.ShowText("The evidence has already been sent.", "Phone");
//             return;
//         }
//
//         _sent = true;
//         Level1PuzzleSystem.Instance?.CompleteLevel();
//
//         if (phoneScreenImage != null)
//             ActivationDialogUI.ShowImage(phoneScreenImage);
//         else
//             ActivationDialogUI.ShowText(completionText, "Phone");
//     }
//
//     public void OnActivatableFocus() { }
//     public void OnActivatableBlur()  { }
// }