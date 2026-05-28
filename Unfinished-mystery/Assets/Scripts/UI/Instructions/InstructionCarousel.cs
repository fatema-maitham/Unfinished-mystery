using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Instruction carousel — uGUI / Canvas version.
/// Attach this to the root CanvasGroup GameObject of the prefab.
/// Wire all [SerializeField] references in the Inspector.
/// </summary>
public class InstructionCarousel : MonoBehaviour
{
    // ── Slide data ────────────────────────────────────────────────────────────
    [Header("Slide Data (assign 5 InstructionData assets)")]
    [SerializeField] private InstructionData[] slides;

    // ── UI References (wired in prefab Inspector) ─────────────────────────────
    [Header("UI References")]
    [SerializeField] private CanvasGroup    rootCanvasGroup;   // root — drives fade in/out
    [SerializeField] private RectTransform  panel;             // the white card — drives slide anim
    [SerializeField] private Image          slideImage;        // illustration
    [SerializeField] private TextMeshProUGUI slideTitle;       // Cinzel title
    [SerializeField] private TextMeshProUGUI slideDesc;        // HY Wenhei body
    [SerializeField] private TextMeshProUGUI slideCounter;     // "1 / 5"
    [SerializeField] private Button         btnPrev;
    [SerializeField] private Button         btnNext;
    [SerializeField] private Button         btnClose;
    [SerializeField] private Button         backdropButton;    // invisible full-screen button behind panel
    [SerializeField] private Transform      dotsContainer;     // parent holding dot images
    [SerializeField] private Image          dotPrefab;         // simple dot image, dragged from prefab

    // ── Animation ─────────────────────────────────────────────────────────────
    [Header("Animation")]
    [SerializeField] private float slideDuration       = 0.22f;
    [SerializeField] private float overlayFadeDuration = 0.18f;
    [SerializeField] private float slideOffsetPx       = 80f;

    // ── Runtime state ─────────────────────────────────────────────────────────
    private int    _current     = 0;
    private bool   _isAnimating = false;
    private bool   _isVisible   = false;
    private Image[] _dots;

    // Parchment colours (match your USS palette)
    private static readonly Color DotActive   = new Color(80/255f,  65/255f,  50/255f,  1f);
    private static readonly Color DotInactive = new Color(180/255f, 168/255f, 150/255f, 1f);

    // ── Unity lifecycle ───────────────────────────────────────────────────────
    private void Awake()
    {
        // Start hidden
        rootCanvasGroup.alpha          = 0f;
        rootCanvasGroup.interactable   = false;
        rootCanvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        btnPrev.onClick.AddListener(OnPrev);
        btnNext.onClick.AddListener(OnNext);
        btnClose.onClick.AddListener(Hide);
        if (backdropButton != null)
            backdropButton.onClick.AddListener(Hide);

        BuildDots();
        ShowSlide(0, animate: false);
    }

    private void OnDisable()
    {
        btnPrev.onClick.RemoveListener(OnPrev);
        btnNext.onClick.RemoveListener(OnNext);
        btnClose.onClick.RemoveListener(Hide);
        if (backdropButton != null)
            backdropButton.onClick.RemoveListener(Hide);
    }

    // ── Public API ────────────────────────────────────────────────────────────
    public void Show()
    {
        if (_isVisible) return;
        _isVisible = true;
        _current   = 0;
        gameObject.SetActive(true);
        ShowSlide(0, animate: false);
        StartCoroutine(FadeIn());
    }

    public void Hide()
    {
        if (!_isVisible) return;
        _isVisible = false;
        StartCoroutine(FadeOut());
    }

    public void Toggle() { if (_isVisible) Hide(); else Show(); }

    // ── Navigation ────────────────────────────────────────────────────────────
    private void OnPrev()
    {
        if (_isAnimating || _current == 0) return;
        StartCoroutine(AnimateSlide(_current - 1, goingForward: false));
    }

    private void OnNext()
    {
        if (_isAnimating || _current == slides.Length - 1) return;
        StartCoroutine(AnimateSlide(_current + 1, goingForward: true));
    }

    // ── Slide display ─────────────────────────────────────────────────────────
    private void ShowSlide(int index, bool animate = true)
    {
        if (slides == null || slides.Length == 0) return;
        index = Mathf.Clamp(index, 0, slides.Length - 1);

        var data = slides[index];
        slideTitle.text   = data.title;
        slideDesc.text    = data.description;
        slideCounter.text = $"{index + 1} / {slides.Length}";

        slideImage.sprite  = data.illustration;
        slideImage.enabled = data.illustration != null;

        UpdateDots(index);
        UpdateArrows(index);
    }

    private void UpdateArrows(int index)
    {
        btnPrev.interactable = index > 0;
        btnNext.interactable = index < slides.Length - 1;

        SetButtonAlpha(btnPrev, index == 0          ? 0.25f : 1f);
        SetButtonAlpha(btnNext, index == slides.Length - 1 ? 0.25f : 1f);
    }

    private static void SetButtonAlpha(Button btn, float alpha)
    {
        var cg = btn.GetComponent<CanvasGroup>();
        if (cg == null) cg = btn.gameObject.AddComponent<CanvasGroup>();
        cg.alpha = alpha;
    }

    // ── Dots ──────────────────────────────────────────────────────────────────
    private void BuildDots()
    {
        // Clear any existing dots
        foreach (Transform child in dotsContainer)
            Destroy(child.gameObject);

        _dots = new Image[slides.Length];
        for (int i = 0; i < slides.Length; i++)
        {
            var dot = Instantiate(dotPrefab, dotsContainer);
            dot.color = i == 0 ? DotActive : DotInactive;
            _dots[i]  = dot;
        }
    }

    private void UpdateDots(int active)
    {
        if (_dots == null) return;
        for (int i = 0; i < _dots.Length; i++)
        {
            _dots[i].color = i == active ? DotActive : DotInactive;

            // Pill shape for active dot
            var rt = _dots[i].rectTransform;
            rt.sizeDelta = i == active
                ? new Vector2(20f, 8f)
                : new Vector2(8f,  8f);
        }
    }

    // ── Coroutine animations ──────────────────────────────────────────────────
    private IEnumerator AnimateSlide(int nextIndex, bool goingForward)
    {
        _isAnimating = true;

        // Slide OUT
        float elapsed = 0f;
        Vector2 startPos = Vector2.zero;
        Vector2 exitPos  = new Vector2(goingForward ? -slideOffsetPx : slideOffsetPx, 0f);
        CanvasGroup panelCG = panel.GetComponent<CanvasGroup>();
        if (panelCG == null) panelCG = panel.gameObject.AddComponent<CanvasGroup>();

        while (elapsed < slideDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / slideDuration);
            panel.anchoredPosition = Vector2.Lerp(startPos, exitPos, t);
            panelCG.alpha          = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        // Swap content
        _current = nextIndex;
        ShowSlide(_current, animate: false);

        // Slide IN
        elapsed = 0f;
        Vector2 enterPos = new Vector2(goingForward ? slideOffsetPx : -slideOffsetPx, 0f);
        panel.anchoredPosition = enterPos;

        while (elapsed < slideDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / slideDuration);
            panel.anchoredPosition = Vector2.Lerp(enterPos, Vector2.zero, t);
            panelCG.alpha          = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        panel.anchoredPosition = Vector2.zero;
        panelCG.alpha          = 1f;
        _isAnimating = false;
    }

    private IEnumerator FadeIn()
    {
        rootCanvasGroup.interactable   = false;
        rootCanvasGroup.blocksRaycasts = true;
        float elapsed = 0f;
        while (elapsed < overlayFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            rootCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / overlayFadeDuration);
            yield return null;
        }
        rootCanvasGroup.alpha        = 1f;
        rootCanvasGroup.interactable = true;
    }

    private IEnumerator FadeOut()
    {
        rootCanvasGroup.interactable = false;
        float elapsed = 0f;
        while (elapsed < overlayFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            rootCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / overlayFadeDuration);
            yield return null;
        }
        rootCanvasGroup.alpha          = 0f;
        rootCanvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
    }
}
