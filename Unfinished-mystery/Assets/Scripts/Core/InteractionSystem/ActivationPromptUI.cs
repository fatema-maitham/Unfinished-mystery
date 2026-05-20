using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Drives the interaction prompt UI (the floating "E — Open Chest" panel).
/// Wire this up in the Inspector. Teammates never need to touch this.
/// Supports animated show/hide with a gold-glow Genshin-style aesthetic.
/// </summary>
public class ActivationPromptUI : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform promptPanel;
    [SerializeField] private TextMeshProUGUI labelText;       // e.g. "Moonlit Offering's Parting Light"
    [SerializeField] private TextMeshProUGUI subLabelText;    // e.g. second entry (optional)
    [SerializeField] private TextMeshProUGUI keyHintText;     // shows "E"

    [Header("Animation")]
    [SerializeField] private float fadeSpeed = 6f;
    [SerializeField] private float slideAmount = 12f; // pixels the panel slides from

    [Header("Key Hint")]
    [SerializeField] private string interactKeyLabel = "E";

    // ── State ────────────────────────────────────────────────────────────────
    private bool _isVisible;
    private Coroutine _animCoroutine;
    private Vector2 _basePosition;

    // ── Unity ────────────────────────────────────────────────────────────────
    private void Awake()
    {
        if (promptPanel != null)
            _basePosition = promptPanel.anchoredPosition;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (keyHintText != null)
            keyHintText.text = interactKeyLabel;
    }

    // ── Public API ───────────────────────────────────────────────────────────

    /// <summary>Shows the prompt with a main label and optional sub-label.</summary>
    public void ShowPrompt(string label, string subLabel = "")
    {
        if (labelText != null) labelText.text = label;

        if (subLabelText != null)
        {
            bool hasSub = !string.IsNullOrEmpty(subLabel);
            subLabelText.gameObject.SetActive(hasSub);
            if (hasSub) subLabelText.text = subLabel;
        }

        if (_isVisible) return;
        _isVisible = true;
        Animate(true);
    }

    /// <summary>Hides the prompt.</summary>
    public void HidePrompt()
    {
        if (!_isVisible) return;
        _isVisible = false;
        Animate(false);
    }

    // ── Animation ────────────────────────────────────────────────────────────
    private void Animate(bool show)
    {
        if (_animCoroutine != null) StopCoroutine(_animCoroutine);
        _animCoroutine = StartCoroutine(AnimateRoutine(show));
    }

    private IEnumerator AnimateRoutine(bool show)
    {
        float target = show ? 1f : 0f;
        Vector2 startPos = promptPanel != null ? promptPanel.anchoredPosition : Vector2.zero;
        Vector2 endPos = show ? _basePosition : _basePosition + Vector2.right * -slideAmount;

        if (show && promptPanel != null)
            promptPanel.anchoredPosition = _basePosition + Vector2.right * -slideAmount;

        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;

        while (!Mathf.Approximately(canvasGroup.alpha, target))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, fadeSpeed * Time.deltaTime);
            if (promptPanel != null)
                promptPanel.anchoredPosition = Vector2.Lerp(promptPanel.anchoredPosition, endPos, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        canvasGroup.alpha = target;
        if (promptPanel != null) promptPanel.anchoredPosition = endPos;
    }
}