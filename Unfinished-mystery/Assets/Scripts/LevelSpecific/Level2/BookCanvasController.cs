using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ═══════════════════════════════════════════════════════════════════════════════
// LEVEL 2 — CLUE BOOK CANVAS CONTROLLER
// Controls the visual book UI.
// Shows left/right page content.
// Handles Next and Back page navigation.
// Plays page-turn audio.
// Uses a small bounce animation instead of a real TurningPage object.
// When the player reaches the final spread, the bookshelf clue becomes complete.
// Attach this script to: ClueBookCanvas
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

    private int currentSpreadIndex = 0;
    private bool isAnimating = false;
    private bool bookshelfClueReported = false;

  private void Start()
{
if (bookPanel != null)
    bookPanel.gameObject.SetActive(false);

    if (bookPanel != null)
        bookPanel.gameObject.SetActive(false);

    if (nextButton != null)
        nextButton.onClick.AddListener(NextSpread);

    if (backButton != null)
        backButton.onClick.AddListener(PreviousSpread);
}

    // ───────────────────────────────────────────────────────────────────────────
    // Opens the book from the first spread.
    // Called by BookInteract when the player presses E near the bookshelf book.
    // ───────────────────────────────────────────────────────────────────────────
public void OpenBook()
{
    Debug.Log("[BookCanvasController] OpenBook called");

    currentSpreadIndex = 0;
    bookshelfClueReported = false;

    bookPanel.gameObject.SetActive(true);

    UpdatePages();
    StartCoroutine(OpenAnimation());
}

    // ───────────────────────────────────────────────────────────────────────────
    // Closes the book canvas only.
    // Player movement is restored from BookInteract.CloseBook().
    // ───────────────────────────────────────────────────────────────────────────
public void CloseBook()
{
    bookPanel.gameObject.SetActive(false);
}

    // ───────────────────────────────────────────────────────────────────────────
    // Moves to the next spread.
    // Plays page flip sound and a small book bounce animation.
    // ───────────────────────────────────────────────────────────────────────────
    public void NextSpread()
    {
        if (isAnimating)
            return;

        if (spreads == null || currentSpreadIndex >= spreads.Length - 1)
            return;

        currentSpreadIndex++;
        StartCoroutine(PageChangeAnimation());
    }

    // ───────────────────────────────────────────────────────────────────────────
    // Moves to the previous spread.
    // Lets the player go back and reread earlier clues.
    // ───────────────────────────────────────────────────────────────────────────
    public void PreviousSpread()
    {
        if (isAnimating)
            return;

        if (currentSpreadIndex <= 0)
            return;

        currentSpreadIndex--;
        StartCoroutine(PageChangeAnimation());
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
            leftText.text = spread.leftText;

        if (rightText != null)
            rightText.text = spread.rightText;

        if (leftText != null)
            leftText.fontSize = spread.bigText ? 46 : 28;

        if (rightText != null)
            rightText.fontSize = spread.bigText ? 46 : 30;

        if (nextButton != null)
            nextButton.gameObject.SetActive(currentSpreadIndex < spreads.Length - 1);

        if (backButton != null)
            backButton.gameObject.SetActive(currentSpreadIndex > 0);

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

        if (bookPanel != null)
            bookPanel.localScale = new Vector3(0.85f, 0.85f, 1f);

        float timer = 0f;

        while (timer < openAnimationTime)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / openAnimationTime;

            if (bookPanel != null)
            {
                bookPanel.localScale = Vector3.Lerp(
                    new Vector3(0.85f, 0.85f, 1f),
                    Vector3.one,
                    t
                );
            }

            yield return null;
        }

        if (bookPanel != null)
            bookPanel.localScale = Vector3.one;

        isAnimating = false;
    }

    // ───────────────────────────────────────────────────────────────────────────
    // Simple page change animation.
    // This replaces the real TurningPage object to keep setup easy.
    // It plays page-turn audio, slightly shrinks the book, updates the pages,
    // then returns the book to normal size.
    // ───────────────────────────────────────────────────────────────────────────
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