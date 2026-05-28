using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Drives the full laptop screen UI.
/// Opened by ActivatableLaptop when the player presses E.
/// </summary>
public class LaptopScreenUI : MonoBehaviour
{
    public static LaptopScreenUI Instance { get; private set; }

    [Header("Root")]
    [SerializeField] private CanvasGroup rootCanvas;

    [Header("Desktop")]
    [SerializeField] private Image wallpaperImage;
    [SerializeField] private Image taskbarImage;

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
    [SerializeField] private Sprite lockedIconSprite;

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

    [Header("Settings")]
    [SerializeField] private string  correctPassword = "58";
    [SerializeField] private float   fadeSpeed       = 6f;
    [SerializeField] private KeyCode exitKey         = KeyCode.Escape;

    [Header("File")]
    [SerializeField] private string lockedFileName = "ForYourEyes.txt";

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
        passwordWindow?.SetActive(true);   // always visible
        decryptedWindow?.SetActive(false);

        if (feedbackText != null) feedbackText.text = "";
    }

    private void Update()
    {
        if (!_isOpen) return;

        if (Input.GetKeyDown(exitKey))
            Close();

        if (passwordWindow != null && passwordWindow.activeSelf)
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                OnSubmitPassword();

        // Continuously re-assert cursor while laptop is open.
        // This counteracts anything else trying to lock it.
        EnsureCursor();
    }

    // ── Public API ────────────────────────────────────────────────────────────

    public void Open()
    {
        if (Level1PuzzleSystem.Instance != null && Level1PuzzleSystem.Instance.FileDecrypted)
        {
            _decrypted = true;
            fileExplorerWindow?.SetActive(true);
            passwordWindow?.SetActive(true);   // always visible
            decryptedWindow?.SetActive(true);
        }
        else
        {
            fileExplorerWindow?.SetActive(true);
            passwordWindow?.SetActive(true);   // always visible
            decryptedWindow?.SetActive(false);
        }

        _isOpen = true;
        gameObject.SetActive(true);
        Fade(1f);

        UIStateManager.Instance?.OpenNotebook();
        EnsureCursor();
        Time.timeScale = 0f;
        if (!_decrypted) StartCoroutine(FocusInputNextFrame());
    }

    public void Close()
    {
        _isOpen = false;
        UIStateManager.Instance?.CloseNotebook();
        Time.timeScale = 1f;
        Fade(0f, () => gameObject.SetActive(false));
    }

    // ── Cursor ────────────────────────────────────────────────────────────────

    /// Explicitly re-asserts cursor every time a window opens or in Update.
    /// Needed because ActivateInputField and Button clicks can steal
    /// Unity's EventSystem focus and hide the cursor under timeScale = 0.
    private void EnsureCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
    }

    // ── File List ─────────────────────────────────────────────────────────────

    private void BuildFileList()
    {
        if (fileListContainer == null || fileItemPrefab == null) return;

        foreach (Transform child in fileListContainer)
            Destroy(child.gameObject);

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
            EnsureCursor();
            return;
        }

        passwordWindow?.SetActive(true);
        EnsureCursor();

        if (passwordInputField != null) passwordInputField.text = "";
        if (feedbackText       != null) feedbackText.text       = "";
        if (passwordWindowTitle != null) passwordWindowTitle.text = lockedFileName;

        // Delay focus by one unscaled frame so the EventSystem doesn't
        // swallow the cursor when timeScale is 0
        StartCoroutine(FocusInputNextFrame());
    }

    private IEnumerator FocusInputNextFrame()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        EnsureCursor(); // re-assert AFTER the yield, before activating field
        if (passwordInputField != null)
        {
            EventSystem.current?.SetSelectedGameObject(passwordInputField.gameObject);
            passwordInputField.ActivateInputField();
        }
    }

    private void ClosePasswordWindow()
    {
        passwordWindow?.SetActive(true);   // always visible
        EnsureCursor();
    }

    private void CloseDecryptedWindow()
    {
        decryptedWindow?.SetActive(false);
        EnsureCursor();
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

            passwordWindow?.SetActive(true);   // always visible

            if (decryptedBodyText    != null) decryptedBodyText.text    = decryptedContent;
            if (decryptedWindowTitle != null) decryptedWindowTitle.text = lockedFileName + " — Unlocked";

            decryptedWindow?.SetActive(true);
            EnsureCursor();
        }
        else
        {
            if (feedbackText != null) feedbackText.text = "Incorrect password.";

            if (passwordInputField != null)
            {
                passwordInputField.text = "";
                StartCoroutine(FocusInputNextFrame());
            }
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