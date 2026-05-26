using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Drives the full laptop screen UI.
/// Opened by ActivatableLaptop when the player presses E.
/// Shows a desktop with a file explorer window.
/// Clicking ForYourEyes.txt opens a password window.
/// Connects to Level1PuzzleSystem on success.
/// </summary>
public class LaptopScreenUI : MonoBehaviour
{
    public static LaptopScreenUI Instance { get; private set; }

    // ── Root ──────────────────────────────────────────────────────────────────
    [Header("Root")]
    [SerializeField] private CanvasGroup rootCanvas;

    // ── Desktop ───────────────────────────────────────────────────────────────
    [Header("Desktop")]
    [Tooltip("wallpaper bw sprite — the grey desktop background")]
    [SerializeField] private Image wallpaperImage;
    [Tooltip("task bar bw sprite — bottom bar")]
    [SerializeField] private Image taskbarImage;

    // ── File Explorer Window ──────────────────────────────────────────────────
    [Header("File Explorer Window")]
    [Tooltip("window_bw type 1 sprite as the window background")]
    [SerializeField] private GameObject fileExplorerWindow;
    [SerializeField] private Image      fileWindowBackground;  // window_bw type 1
    [SerializeField] private Image      fileWindowTopBar;      // min_window bw
    [SerializeField] private TMP_Text   fileWindowTitle;       // "Kyrell_Flins_Evidence"
    [SerializeField] private Button     closeFileWindowButton; // close bw type 1
    [SerializeField] private Image      closeFileWindowIcon;   // close bw type 1 sprite

    [Header("File List — inside the explorer window")]
    [Tooltip("Assign one FileItemUI prefab per file row — see hierarchy guide below")]
    [SerializeField] private Transform  fileListContainer;     // vertical layout group
    [SerializeField] private GameObject fileItemPrefab;        // prefab: icon + filename button

    [Header("File Icons")]
    [Tooltip("icon from icons bw — document icon for regular files")]
    [SerializeField] private Sprite documentIconSprite;
    [Tooltip("icon from icons bw — use a different icon for the locked file")]
    [SerializeField] private Sprite lockedIconSprite;

    // ── Password Window ───────────────────────────────────────────────────────
    [Header("Password Window")]
    [Tooltip("window_bw type 2 sprite as window background")]
    [SerializeField] private GameObject passwordWindow;
    [SerializeField] private Image      passwordWindowBackground; // window_bw type 2
    [SerializeField] private Image      passwordWindowTopBar;     // min_window bw
    [SerializeField] private TMP_Text   passwordWindowTitle;      // "ForYourEyes.txt"
    [SerializeField] private Button     closePasswordButton;      // close bw type 1
    [SerializeField] private Image      closePasswordIcon;        // close bw type 1 sprite
    [SerializeField] private TMP_Text   passwordPromptText;       // "Enter password:"
    [SerializeField] private Image      inputFieldBackground;     // field bw / input bw
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Button     submitButton;
    [SerializeField] private Image      submitButtonImage;        // button type 1 grey / white
    [SerializeField] private TMP_Text   submitButtonText;         // "Unlock"
    [SerializeField] private TMP_Text   feedbackText;             // "Incorrect." or empty

    // ── Decrypted File Window ─────────────────────────────────────────────────
    [Header("Decrypted File Window")]
    [Tooltip("Shown after correct password — window_bw type 3")]
    [SerializeField] private GameObject decryptedWindow;
    [SerializeField] private Image      decryptedWindowBackground;
    [SerializeField] private Image      decryptedWindowTopBar;
    [SerializeField] private TMP_Text   decryptedWindowTitle;     // "ForYourEyes.txt — Unlocked"
    [SerializeField] private Button     closeDecryptedButton;
    [SerializeField] private TMP_Text   decryptedBodyText;
    [TextArea(3, 6)]
    [SerializeField] private string decryptedContent =
        "I'll expose you on 8th March if you don't come clean yourself.\n\n— Nadia Orin";

    // ── Settings ──────────────────────────────────────────────────────────────
    [Header("Settings")]
    [SerializeField] private string correctPassword = "58";
    [SerializeField] private float  fadeSpeed       = 6f;
    [SerializeField] private KeyCode exitKey        = KeyCode.Escape;

    [Header("File List Contents")]
    [Tooltip("These appear as regular files in the explorer — not clickable for puzzle")]
    [SerializeField] private string[] decoyFileNames = new string[]
    {
        "Thesis_Draft_Final.pdf",
        "GradeSheet_Spring2003.xlsx",
        "StudentRecords_Backup.zip",
        "CorrespondenceLog.txt"
    };
    [Tooltip("The locked file that triggers the password window")]
    [SerializeField] private string lockedFileName = "ForYourEyes.txt";

    // ── State ─────────────────────────────────────────────────────────────────
    private bool _isOpen      = false;
    private bool _decrypted   = false;
    private Coroutine _fadeRoutine;

    // ── Unity ─────────────────────────────────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        rootCanvas.alpha          = 0f;
        rootCanvas.interactable   = false;
        rootCanvas.blocksRaycasts = false;
        gameObject.SetActive(false);
    }

    private void Start()
    {
        // Wire close buttons
        closeFileWindowButton?.onClick.AddListener(CloseFileExplorer);
        closePasswordButton?.onClick.AddListener(ClosePasswordWindow);
        closeDecryptedButton?.onClick.AddListener(CloseDecryptedWindow);
        submitButton?.onClick.AddListener(OnSubmitPassword);

        // Build the file list dynamically
        BuildFileList();

        // Start with only the file explorer open
        fileExplorerWindow?.SetActive(true);
        passwordWindow?.SetActive(false);
        decryptedWindow?.SetActive(false);

        feedbackText.text = "";
    }

    private void Update()
    {
        if (!_isOpen) return;
        if (Input.GetKeyDown(exitKey)) Close();
    }

    // ── Public API ────────────────────────────────────────────────────────────

    /// <summary>Called by ActivatableLaptop to open the screen.</summary>
    public void Open()
    {
        // If already decrypted, show the decrypted window directly
        if (Level1PuzzleSystem.Instance != null && Level1PuzzleSystem.Instance.FileDecrypted)
        {
            _decrypted = true;
            fileExplorerWindow?.SetActive(true);
            passwordWindow?.SetActive(false);
            decryptedWindow?.SetActive(true);
        }
        else
        {
            fileExplorerWindow?.SetActive(true);
            passwordWindow?.SetActive(false);
            decryptedWindow?.SetActive(false);
        }

        _isOpen = true;
        gameObject.SetActive(true);
        Fade(1f);
        Time.timeScale = 0f;
    }

    public void Close()
    {
        _isOpen = false;
        Time.timeScale = 1f;
        Fade(0f, () => gameObject.SetActive(false));
    }

    // ── File List ─────────────────────────────────────────────────────────────
    private void BuildFileList()
    {
        if (fileListContainer == null || fileItemPrefab == null) return;

        // Clear existing children
        foreach (Transform child in fileListContainer)
            Destroy(child.gameObject);

        // Spawn decoy files first
        foreach (string fileName in decoyFileNames)
            SpawnFileItem(fileName, false);

        // Spawn the locked file last — highlighted differently
        SpawnFileItem(lockedFileName, true);
    }

    private void SpawnFileItem(string fileName, bool isLocked)
    {
        GameObject item = Instantiate(fileItemPrefab, fileListContainer);

        // Icon
        var icon = item.transform.Find("Icon")?.GetComponent<Image>();
        if (icon != null)
            icon.sprite = isLocked ? lockedIconSprite : documentIconSprite;

        // Label
        var label = item.transform.Find("FileName")?.GetComponent<TMP_Text>();
        if (label != null)
        {
            label.text = fileName;
            if (isLocked) label.color = new Color(0.2f, 0.2f, 0.2f); // darker to stand out
        }

        // Click — only the locked file opens the password window
        var btn = item.GetComponent<Button>();
        if (btn != null)
        {
            if (isLocked)
                btn.onClick.AddListener(OpenPasswordWindow);
            else
                btn.onClick.AddListener(() => OnDecoyFileClicked(fileName));
        }
    }

    private void OnDecoyFileClicked(string fileName)
    {
        // Decoy files just show a small feedback — no puzzle relevance
        feedbackText.text = $"Cannot open {fileName}.";
    }

    // ── Windows ───────────────────────────────────────────────────────────────
    private void CloseFileExplorer()
    {
        fileExplorerWindow?.SetActive(false);
        // If no windows left open, close the whole screen
        if (passwordWindow != null && !passwordWindow.activeSelf &&
            decryptedWindow != null && !decryptedWindow.activeSelf)
            Close();
    }

    private void OpenPasswordWindow()
    {
        if (_decrypted)
        {
            decryptedWindow?.SetActive(true);
            return;
        }

        passwordWindow?.SetActive(true);
        if (passwordInputField != null) passwordInputField.text = "";
        if (feedbackText       != null) feedbackText.text       = "";
        if (passwordWindowTitle != null)
            passwordWindowTitle.text = lockedFileName;
    }

    private void ClosePasswordWindow()
    {
        passwordWindow?.SetActive(false);
    }

    private void CloseDecryptedWindow()
    {
        decryptedWindow?.SetActive(false);
    }

    // ── Password ──────────────────────────────────────────────────────────────
    private void OnSubmitPassword()
    {
        if (passwordInputField == null) return;

        string entered = passwordInputField.text.Trim();

        if (entered == correctPassword)
        {
            _decrypted = true;
            Level1PuzzleSystem.Instance?.DecryptFile();

            passwordWindow?.SetActive(false);

            if (decryptedBodyText != null)
                decryptedBodyText.text = decryptedContent;
            if (decryptedWindowTitle != null)
                decryptedWindowTitle.text = lockedFileName + " — Unlocked";

            decryptedWindow?.SetActive(true);
        }
        else
        {
            if (feedbackText != null)
                feedbackText.text = "Incorrect password.";
        }
    }

    // ── Fade ──────────────────────────────────────────────────────────────────
    private void Fade(float target, System.Action onComplete = null)
    {
        if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(FadeRoutine(target, onComplete));
    }

    private IEnumerator FadeRoutine(float target, System.Action onComplete)
    {
        rootCanvas.interactable   = target > 0f;
        rootCanvas.blocksRaycasts = target > 0f;

        while (!Mathf.Approximately(rootCanvas.alpha, target))
        {
            rootCanvas.alpha = Mathf.MoveTowards(
                rootCanvas.alpha, target, fadeSpeed * Time.unscaledDeltaTime);
            yield return null;
        }

        rootCanvas.alpha = target;
        onComplete?.Invoke();
    }
}