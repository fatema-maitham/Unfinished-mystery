using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ═══════════════════════════════════════════════════════════════════════════════
// LEVEL 2 — CLUE BOOK CANVAS CONTROLLER
// Controls the visual book UI.
// Shows left/right page content, handles the real page-flip animation,
// plays page-turn audio, and closes the book canvas.
// When the player reaches the final spread, the bookshelf clue becomes complete.
// Attach this script to: BookCanvas
// ═══════════════════════════════════════════════════════════════════════════════
public class BookCanvasController : MonoBehaviour
{
    // ───────────────────────────────────────────────────────────────────────────
    // BOOK SPREAD DATA
    // One spread = left page + right page.
    // Example: Spread 1 contains Page 1 on the left and Page 2 on the right.
    // ───────────────────────────────────────────────────────────────────────────
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

    [Header("Turning Page Animation")]
    [SerializeField] private RectTransform turningPage;
    [SerializeField] private TMP_Text turningText;

    [Header("Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button closeButton;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pageTurnSound;

    [Header("Book Pages")]
    [SerializeField] private BookSpread[] spreads;

    [Header("Animation Settings")]
    [SerializeField] private float openAnimationTime = 0.2f;
    [SerializeField] private float pageTurnTime = 0.45f;

    private int currentSpreadIndex = 0;
    private bool isAnimating = false;
    private bool bookshelfClueReported = false;

    private void Start()
    {
        if (bookCanvas != null)
            bookCanvas.SetActive(false);

        if (turningPage != null)
            turningPage.gameObject.SetActive(false);

        if (nextButton != null)
            nextButton.onClick.AddListener(NextSpread);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseBook);
    }

    // ───────────────────────────────────────────────────────────────────────────
    // Opens the book from the first spread.
    // Called by BookInteract when the player presses E near the bookshelf book.
    // ───────────────────────────────────────────────────────────────────────────
    public void OpenBook()
    {
        currentSpreadIndex = 0;
        bookshelfClueReported = false;

        bookCanvas.SetActive(true);
        turningPage.gameObject.SetActive(false);

        UpdatePages();
        StartCoroutine(OpenAnimation());
    }

    // ───────────────────────────────────────────────────────────────────────────
    // Closes the book canvas only.
    // Player movement is restored from BookInteract.CloseBook().
    // ───────────────────────────────────────────────────────────────────────────
    public void CloseBook()
    {
        if (bookCanvas != null)
            bookCanvas.SetActive(false);

        if (turningPage != null)
            turningPage.gameObject.SetActive(false);
    }

    // ───────────────────────────────────────────────────────────────────────────
    // Moves to the next spread using the page flip animation.
    // If the player is already on the final spread, nothing happens.
    // ───────────────────────────────────────────────────────────────────────────
    public void NextSpread()
    {
        if (isAnimating)
            return;

        if (currentSpreadIndex >= spreads.Length - 1)
            return;

        StartCoroutine(PageFlipAnimation());
    }

    // ───────────────────────────────────────────────────────────────────────────
    // Updates the visible text and image for the current spread.
    // If no image is assigned, LeftImage is hidden automatically.
    // When the final spread is reached, it reports the bookshelf clue as found.
    // ───────────────────────────────────────────────────────────────────────────
    private void UpdatePages()
    {
        if (spreads == null || spreads.Length == 0)
            return;

        BookSpread spread = spreads[currentSpreadIndex];

        if (spread.leftImage != null)
        {
            leftImage.gameObject.SetActive(true);
            leftImage.sprite = spread.leftImage;
        }
        else
        {
            leftImage.gameObject.SetActive(false);
        }

        leftText.text = spread.leftText;
        rightText.text = spread.rightText;

        leftText.fontSize = spread.bigText ? 46 : 28;
        rightText.fontSize = spread.bigText ? 46 : 30;

        nextButton.gameObject.SetActive(currentSpreadIndex < spreads.Length - 1);

        if (currentSpreadIndex == spreads.Length - 1 && !bookshelfClueReported)
        {
            bookshelfClueReported = true;
            Level2PuzzleSystem.Instance?.FindBookshelfClue();
        }
    }

    // ───────────────────────────────────────────────────────────────────────────
    // Small opening animation when the book appears.
    // ───────────────────────────────────────────────────────────────────────────
    private IEnumerator OpenAnimation()
    {
        isAnimating = true;

        bookPanel.localScale = new Vector3(0.85f, 0.85f, 1f);

        float timer = 0f;

        while (timer < openAnimationTime)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / openAnimationTime;

            bookPanel.localScale = Vector3.Lerp(
                new Vector3(0.85f, 0.85f, 1f),
                Vector3.one,
                t
            );

            yield return null;
        }

        bookPanel.localScale = Vector3.one;
        isAnimating = false;
    }

    // ───────────────────────────────────────────────────────────────────────────
    // Real page flip effect:
    // 1. Shows TurningPage above the right page.
    // 2. Copies the current right page text onto TurningPage.
    // 3. Rotates TurningPage around its left pivot.
    // 4. Updates to the next spread.
    // 5. Hides TurningPage.
    // ───────────────────────────────────────────────────────────────────────────
    private IEnumerator PageFlipAnimation()
    {
        isAnimating = true;

        if (audioSource != null && pageTurnSound != null)
            audioSource.PlayOneShot(pageTurnSound);

        turningPage.gameObject.SetActive(true);
        turningText.text = rightText.text;
        turningPage.localRotation = Quaternion.Euler(0f, 0f, 0f);

        float timer = 0f;

        while (timer < pageTurnTime)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / pageTurnTime;

            float angle = Mathf.Lerp(0f, 180f, t);
            turningPage.localRotation = Quaternion.Euler(0f, angle, 0f);

            yield return null;
        }

        currentSpreadIndex++;
        UpdatePages();

        turningPage.localRotation = Quaternion.Euler(0f, 0f, 0f);
        turningPage.gameObject.SetActive(false);

        isAnimating = false;
    }
}