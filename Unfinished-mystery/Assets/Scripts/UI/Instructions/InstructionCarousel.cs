using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Manages the instruction carousel overlay.
/// Attach to a GameObject that also has a UIDocument component.
/// Assign your 5 InstructionData assets in the Inspector.
/// </summary>
[RequireComponent(typeof(UIDocument))]
public class InstructionCarousel : MonoBehaviour
{
    // ── Inspector ─────────────────────────────────────────────────────────────
    [Header("Slide Data (assign 5 InstructionData assets)")]
    [SerializeField] private InstructionData[] slides;

    [Header("Animation")]
    [SerializeField] private float slideDuration = 0.25f;   // seconds
    [SerializeField] private float overlayFadeDuration = 0.2f;

    // ── Private state ─────────────────────────────────────────────────────────
    private UIDocument    _doc;
    private VisualElement _overlay;
    private VisualElement _panel;
    private VisualElement _slideImage;
    private Label         _slideTitle;
    private Label         _slideDesc;
    private Label         _slideCounter;
    private Button        _btnPrev;
    private Button        _btnNext;
    private Button        _btnClose;
    private VisualElement _dotsContainer;

    private int  _current = 0;
    private bool _isAnimating = false;
    private bool _isVisible   = false;

    // ── Unity lifecycle ───────────────────────────────────────────────────────
    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _doc.rootVisualElement;

        _overlay       = root.Q<VisualElement>("carousel-overlay");
        _panel         = root.Q<VisualElement>("carousel-panel");
        _slideImage    = root.Q<VisualElement>("slide-image");
        _slideTitle    = root.Q<Label>("slide-title");
        _slideDesc     = root.Q<Label>("slide-desc");
        _slideCounter  = root.Q<Label>("slide-counter");
        _btnPrev       = root.Q<Button>("btn-prev");
        _btnNext       = root.Q<Button>("btn-next");
        _btnClose      = root.Q<Button>("btn-close");
        _dotsContainer = root.Q<VisualElement>("dots-container");

        _btnPrev.clicked  += OnPrev;
        _btnNext.clicked  += OnNext;
        _btnClose.clicked += Hide;

        // Close when clicking the dim backdrop (outside the panel)
        _overlay.RegisterCallback<ClickEvent>(OnOverlayClick);

        BuildDots();
        ShowSlide(_current, animate: false);

        // Start hidden
        _overlay.AddToClassList("hidden");
    }

    private void OnDisable()
    {
        if (_btnPrev  != null) _btnPrev.clicked  -= OnPrev;
        if (_btnNext  != null) _btnNext.clicked  -= OnNext;
        if (_btnClose != null) _btnClose.clicked -= Hide;
    }

    // ── Public API (called by HUDManager) ────────────────────────────────────
    public void Show()
    {
        if (_isVisible) return;
        _isVisible = true;
        _current   = 0;
        ShowSlide(0, animate: false);
        _overlay.RemoveFromClassList("hidden");
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

    private void OnOverlayClick(ClickEvent e)
    {
        // Only close if the click target is the overlay itself, not a child
        if (e.target == _overlay) Hide();
    }

    // ── Slide display ─────────────────────────────────────────────────────────
    private void ShowSlide(int index, bool animate = true)
    {
        if (slides == null || slides.Length == 0) return;
        index = Mathf.Clamp(index, 0, slides.Length - 1);

        var data = slides[index];

        _slideTitle.text = data.title;
        _slideDesc.text  = data.description;
        _slideCounter.text = $"{index + 1} / {slides.Length}";

        if (data.illustration != null)
            _slideImage.style.backgroundImage = new StyleBackground(data.illustration);

        UpdateDots(index);
        UpdateArrows(index);
    }

    private void UpdateArrows(int index)
    {
        _btnPrev.SetEnabled(index > 0);
        _btnNext.SetEnabled(index < slides.Length - 1);

        // Visual dim when disabled
        _btnPrev.EnableInClassList("arrow-disabled", index == 0);
        _btnNext.EnableInClassList("arrow-disabled", index == slides.Length - 1);
    }

    // ── Dot indicators ────────────────────────────────────────────────────────
    private void BuildDots()
    {
        _dotsContainer.Clear();
        for (int i = 0; i < slides.Length; i++)
        {
            var dot = new VisualElement();
            dot.AddToClassList("dot");
            if (i == 0) dot.AddToClassList("dot-active");
            _dotsContainer.Add(dot);
        }
    }

    private void UpdateDots(int active)
    {
        var dots = _dotsContainer.Children();
        int i = 0;
        foreach (var dot in dots)
        {
            dot.EnableInClassList("dot-active", i == active);
            i++;
        }
    }

    // ── Coroutine animations ──────────────────────────────────────────────────
    private IEnumerator AnimateSlide(int nextIndex, bool goingForward)
    {
        _isAnimating = true;

        // Slide out current
        float elapsed = 0f;
        float startX  = 0f;
        float endX    = goingForward ? -60f : 60f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / slideDuration);
            float x = Mathf.Lerp(startX, endX, t);
            _panel.style.translate = new StyleTranslate(new Translate(x, 0));
            _panel.style.opacity   = new StyleFloat(Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        // Swap content
        _current = nextIndex;
        ShowSlide(_current, animate: false);

        // Slide in new
        elapsed = 0f;
        float fromX = goingForward ? 60f : -60f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / slideDuration);
            float x = Mathf.Lerp(fromX, 0f, t);
            _panel.style.translate = new StyleTranslate(new Translate(x, 0));
            _panel.style.opacity   = new StyleFloat(Mathf.Lerp(0f, 1f, t));
            yield return null;
        }

        _panel.style.translate = new StyleTranslate(new Translate(0, 0));
        _panel.style.opacity   = new StyleFloat(1f);
        _isAnimating = false;
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < overlayFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            _overlay.style.opacity = Mathf.Lerp(0f, 1f, elapsed / overlayFadeDuration);
            yield return null;
        }
        _overlay.style.opacity = 1f;
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        while (elapsed < overlayFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            _overlay.style.opacity = Mathf.Lerp(1f, 0f, elapsed / overlayFadeDuration);
            yield return null;
        }
        _overlay.style.opacity = 0f;
        _overlay.AddToClassList("hidden");
    }
}
