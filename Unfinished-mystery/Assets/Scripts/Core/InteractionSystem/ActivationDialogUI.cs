using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the full-screen or mid-screen dialog panel shown after interaction.
/// Supports two modes:
///   - TEXT MODE: Character name + dialogue text (like Amber's line in Image 2)
///   - IMAGE MODE: Show a custom sprite (letter, book page, map, painting, etc.)
///
/// Teammates use the static Show() methods — they never instantiate this manually.
/// </summary>
public class ActivationDialogUI : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────────────────────────────────
    public static ActivationDialogUI Instance { get; private set; }

    [Header("Root")]
    [SerializeField] private CanvasGroup rootCanvas;

    [Header("Text Mode")]
    [SerializeField] private GameObject textPanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;  // e.g. "Amber"
    [SerializeField] private TextMeshProUGUI dialogBodyText;   // body text
    [SerializeField] private TextMeshProUGUI continueHintText; // "Press E to close"

    [Header("Image Mode")]
    [SerializeField] private GameObject imagePanel;
    [SerializeField] private Image displayImage;               // letter/book/painting etc.

    [Header("Shared")]
    [SerializeField] private float fadeSpeed = 5f;
    [SerializeField] private KeyCode dismissKey = KeyCode.E;
    [SerializeField] private string dismissLabel = "E  Close";

    // ── State ─────────────────────────────────────────────────────────────────
    private bool _isOpen;
    private Coroutine _fadeRoutine;

    // ── Unity ─────────────────────────────────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        rootCanvas.alpha = 0f;
        rootCanvas.interactable = false;
        rootCanvas.blocksRaycasts = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!_isOpen) return;
        if (Input.GetKeyDown(dismissKey)) Hide();
    }

    // ── Static Convenience API ────────────────────────────────────────────────

    /// <summary>Show a text dialogue. Speaker name is optional.</summary>
    public static void ShowText(string body, string speakerName = "")
        => Instance?.InternalShowText(body, speakerName);

    /// <summary>Show a custom image (letter, painting, map, book page, etc.).</summary>
    public static void ShowImage(Sprite image)
        => Instance?.InternalShowImage(image);

    // ── Internal ──────────────────────────────────────────────────────────────
    private void InternalShowText(string body, string speakerName)
    {
        textPanel.SetActive(true);
        imagePanel.SetActive(false);

        bool hasSpeaker = !string.IsNullOrEmpty(speakerName);
        speakerNameText.gameObject.SetActive(hasSpeaker);
        if (hasSpeaker) speakerNameText.text = speakerName;

        dialogBodyText.text = body;
        if (continueHintText != null) continueHintText.text = dismissLabel;

        Open();
    }

    private void InternalShowImage(Sprite image)
    {
        textPanel.SetActive(false);
        imagePanel.SetActive(true);
        displayImage.sprite = image;

        Open();
    }

    private void Open()
    {
        _isOpen = true;
        gameObject.SetActive(true);
        Fade(1f);

        // Optionally pause the game while reading
        Time.timeScale = 0f;
    }

    private void Hide()
    {
        _isOpen = false;
        Time.timeScale = 1f;
        Fade(0f, onComplete: () => gameObject.SetActive(false));
    }

    private void Fade(float target, System.Action onComplete = null)
    {
        if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(FadeRoutine(target, onComplete));
    }

    private IEnumerator FadeRoutine(float target, System.Action onComplete)
    {
        rootCanvas.interactable = target > 0f;
        rootCanvas.blocksRaycasts = target > 0f;

        while (!Mathf.Approximately(rootCanvas.alpha, target))
        {
            // Use unscaled time so it works when timeScale = 0
            rootCanvas.alpha = Mathf.MoveTowards(rootCanvas.alpha, target, fadeSpeed * Time.unscaledDeltaTime);
            yield return null;
        }

        rootCanvas.alpha = target;
        onComplete?.Invoke();
    }
}