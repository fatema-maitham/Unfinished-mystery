using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Drives the full laptop screen UI.
/// Opened by ActivatableLaptop when the player presses E.
/// Shows a desktop with a file explorer containing only ForYourEyes.txt.
/// Clicking it opens a password window. Correct password connects to Level1PuzzleSystem.
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
    [SerializeField] private GameObject fileExplorerWindow;
    [SerializeField] private Image      fileWindowBackground;
    [SerializeField] private Image      fileWindowTopBar;
    [SerializeField] private TMP_Text   fileWindowTitle;
    [SerializeField] private Button     closeFileWindowButton;
    [SerializeField] private Image      closeFileWindowIcon;

    [Header("File List")]
    [SerializeField] private Transform  fileListContainer;
    [SerializeField] private GameObject fileItemPrefab;

    [Header("File Icons")]
    [Tooltip("Icon shown next to ForYourEyes.txt — use lock or settings icon from icons bw")]
    [SerializeField] private Sprite lockedIconSprite;

    // ── Password Window ───────────────────────────────────────────────────────
    [Header("Password Window")]
    [SerializeField] private GameObject     passwordWindow;
    [SerializeField] private Image          passwordWindowBackground;
    [SerializeField] private Image          passwordWindowTopBar;
    [SerializeField] private TMP_Text       passwordWindowTitle;
    [SerializeField] private Button         closePasswordButton;
    [SerializeField] private Image          closePasswordIcon;
    [SerializeField] private TMP_Text       passwordPromptText;
    [SerializeField] private Image          inputFieldBackground;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Button         submitButton;
    [SerializeField] private Image          submitButtonImage;
    [SerializeField] private TMP_Text       submitButtonText;
    [SerializeField] private TMP_Text       feedbackText;

    // ── Decrypted File Window ─────────────────────────────────────────────────
    [Header("Decrypted File Window")]
    [SerializeField] private GameObject decryptedWindow;
    [SerializeField] private Image      decryptedWindowBackground;
    [SerializeField] private Image      decryptedWindowTopBar;
    [SerializeField] private TMP_Text   decryptedWindowTitle;
    [SerializeField] private Button     closeDecryptedButton;
    [SerializeField] private TMP_Text   decryptedBodyText;
    [TextArea(3, 6)]
    [SerializeField] private string decryptedContent =
        "I'll expose you on 8th March if you don't come clean yourself.\n\n— Nadia Orin";

    // ── Settings ──────────────────────────────────────────────────────────────
    [Header("Settings")]
    [SerializeField] private string  correctPassword = "58";
    [SerializeField] private float   fadeSpeed       = 6f;
    [SerializeField] private KeyCode exitKey         = KeyCode.Escape;

    [Header("File")]
    [Tooltip("The single file shown in the explorer — clicking it opens the password window")]
    [SerializeField] private string lockedFileName = "ForYourEyes.txt";

    // ── State ─────────────────────────────────────────────────────────────────
    private bool _isOpen    = false;
    private bool _decrypted = false;
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
        closeFileWindowButton?.onClick.AddListener(CloseFileExplorer);
        closePasswordButton?.onClick.AddListener(ClosePasswordWindow);
        closeDecryptedButton?.onClick.AddListener(CloseDecryptedWindow);
        submitButton?.onClick.AddListener(OnSubmitPassword);

        BuildFileList();

        fileExplorerWindow?.SetActive(true);
        passwordWindow?.SetActive(false);
        decryptedWindow?.SetActive(false);

        if (feedbackText != null) feedbackText.text = "";
    }

    private void Update()
    {
        if (!_isOpen) return;
        if (Input.GetKeyDown(exitKey)) Close();
    }

    // ── Public API ────────────────────────────────────────────────────────────
    public void Open()
    {
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

        foreach (Transform child in fileListContainer)
            Destroy(child.gameObject);

        // Spawn only ForYourEyes.txt
        GameObject item = Instantiate(fileItemPrefab, fileListContainer);

        var icon = item.transform.Find("Icon")?.GetComponent<Image>();
        if (icon != null) icon.sprite = lockedIconSprite;

        var label = item.transform.Find("FileName")?.GetComponent<TMP_Text>();
        if (label != null)
        {
            label.text  = lockedFileName;
            label.color = new Color(0.2f, 0.2f, 0.2f);
        }

        var btn = item.GetComponent<Button>();
        btn?.onClick.AddListener(OpenPasswordWindow);
    }

    // ── Windows ───────────────────────────────────────────────────────────────
    private void CloseFileExplorer()
    {
        fileExplorerWindow?.SetActive(false);
        if (passwordWindow  != null && !passwordWindow.activeSelf &&
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
        if (passwordInputField  != null) passwordInputField.text  = "";
        if (feedbackText        != null) feedbackText.text        = "";
        if (passwordWindowTitle != null) passwordWindowTitle.text = lockedFileName;
    }

    private void ClosePasswordWindow()  => passwordWindow?.SetActive(false);
    private void CloseDecryptedWindow() => decryptedWindow?.SetActive(false);

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

            if (decryptedBodyText    != null) decryptedBodyText.text    = decryptedContent;
            if (decryptedWindowTitle != null) decryptedWindowTitle.text = lockedFileName + " — Unlocked";

            decryptedWindow?.SetActive(true);
        }
        else
        {
            if (feedbackText != null) feedbackText.text = "Incorrect password.";
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