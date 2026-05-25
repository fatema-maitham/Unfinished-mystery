using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the Level 2 clue book UI.
/// Shows book pages, handles next/back buttons, page sounds, and reports
/// the bookshelf clue only after the player closes the book on the final spread.
/// </summary>
public class BookCanvasController : MonoBehaviour
{
    [System.Serializable]
    public class BookSpread
    {
        [Header("Left Page Image")]
        public Sprite leftImage;

        [Header("Left Page Text")]
        [TextArea(3, 10)]
        public string leftText;

        [Header("Right Page Text")]
        [TextArea(3, 10)]
        public string rightText;

        [Header("Style")]
        public bool bigText;
    }

    [Header("Main UI")]
    [SerializeField] private GameObject bookCanvas;
    [SerializeField] private RectTransform bookPanel;

    [Header("Left Page")]
    [SerializeField] private Image leftImage;
    [SerializeField] private TMP_Text leftText;

    [Header("Right Page")]
    [SerializeField] private TMP_Text rightText;

    [Header("Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pageTurnSound;

    [Header("Book Pages")]
    [SerializeField] private BookSpread[] spreads;

    [Header("Animation Settings")]
    [SerializeField] private float openAnimationTime = 0.2f;
    [SerializeField] private float pageBounceTime = 0.18f;

    private int currentSpreadIndex;
    private bool isAnimating;
    private bool reachedFinalSpread;
    private bool bookshelfClueReported;

    private void Start()
    {
        if (bookPanel != null)
            bookPanel.gameObject.SetActive(false);

        if (nextButton != null)
            nextButton.onClick.AddListener(NextSpread);

        if (backButton != null)
            backButton.onClick.AddListener(PreviousSpread);
    }

    public void OpenBook()
    {
        Debug.Log("[BookCanvasController] OpenBook called");

        currentSpreadIndex = 0;
        reachedFinalSpread = false;

        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = true;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999;
        }

        CanvasGroup group = GetComponent<CanvasGroup>();
        if (group != null)
        {
            group.alpha = 1f;
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        if (bookPanel != null)
        {
            bookPanel.gameObject.SetActive(true);
            bookPanel.SetAsLastSibling();
            bookPanel.anchoredPosition = new Vector2(0f, 80f);
            bookPanel.sizeDelta = new Vector2(1055f, 623f);
            bookPanel.localScale = Vector3.one;

            for (int i = 0; i < bookPanel.childCount; i++)
                bookPanel.GetChild(i).gameObject.SetActive(true);
        }

        UpdatePages();
        StartCoroutine(OpenAnimation());
    }

    public void CloseBook()
    {
        if (bookPanel != null)
            bookPanel.gameObject.SetActive(false);

        if (reachedFinalSpread && !bookshelfClueReported)
        {
            bookshelfClueReported = true;
            Level2PuzzleSystem.Instance?.FindBookshelfClue();
        }
    }

    public void NextSpread()
    {
        if (isAnimating)
            return;

        if (spreads == null || currentSpreadIndex >= spreads.Length - 1)
            return;

        currentSpreadIndex++;
        StartCoroutine(PageChangeAnimation());
    }

    public void PreviousSpread()
    {
        if (isAnimating)
            return;

        if (currentSpreadIndex <= 0)
            return;

        currentSpreadIndex--;
        StartCoroutine(PageChangeAnimation());
    }

    private void UpdatePages()
    {
        if (spreads == null || spreads.Length == 0)
            return;

        BookSpread spread = spreads[currentSpreadIndex];

        if (leftImage != null)
        {
            if (spread.leftImage != null)
            {
                leftImage.gameObject.SetActive(true);
                leftImage.sprite = spread.leftImage;
            }
            else
            {
                leftImage.gameObject.SetActive(false);
            }
        }

        if (leftText != null)
        {
            leftText.text = spread.leftText;
            leftText.fontSize = spread.bigText ? 46 : 28;
        }

        if (rightText != null)
        {
            rightText.text = spread.rightText;
            rightText.fontSize = spread.bigText ? 46 : 30;
        }

        if (nextButton != null)
            nextButton.gameObject.SetActive(currentSpreadIndex < spreads.Length - 1);

        if (backButton != null)
            backButton.gameObject.SetActive(currentSpreadIndex > 0);

        if (currentSpreadIndex == spreads.Length - 1)
            reachedFinalSpread = true;
    }

    private IEnumerator OpenAnimation()
    {
        isAnimating = true;

        if (bookPanel != null)
            bookPanel.localScale = new Vector3(0.85f, 0.85f, 1f);

        float timer = 0f;

        while (timer < openAnimationTime)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / openAnimationTime;

            if (bookPanel != null)
                bookPanel.localScale = Vector3.Lerp(new Vector3(0.85f, 0.85f, 1f), Vector3.one, t);

            yield return null;
        }

        if (bookPanel != null)
            bookPanel.localScale = Vector3.one;

        isAnimating = false;
    }

    private IEnumerator PageChangeAnimation()
    {
        isAnimating = true;

        if (audioSource != null && pageTurnSound != null)
            audioSource.PlayOneShot(pageTurnSound);

        if (bookPanel != null)
            bookPanel.localScale = new Vector3(0.96f, 0.96f, 1f);

        yield return new WaitForSecondsRealtime(pageBounceTime);

        UpdatePages();

        if (bookPanel != null)
            bookPanel.localScale = Vector3.one;

        isAnimating = false;
    }
}