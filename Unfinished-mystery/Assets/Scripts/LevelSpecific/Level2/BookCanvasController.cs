using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Controls the book UI, page navigation, page animation, and final bookshelf clue report
public class BookCanvasController : MonoBehaviour
{
    [System.Serializable]
    public class BookSpread
    {
        public Sprite leftImage; // Image shown on the left page

        [TextArea(3, 10)]
        public string leftText; // Text shown on the left page

        [TextArea(3, 10)]
        public string rightText; // Text shown on the right page

        public bool bigText; // Makes page text bigger for special pages
    }

    [Header("Main UI")]
    [SerializeField] private GameObject bookCanvas; // Main book canvas object
    [SerializeField] private RectTransform bookPanel; // Main book panel that appears on screen

    [Header("Left Page")]
    [SerializeField] private Image leftImage; // Left page image UI
    [SerializeField] private TMP_Text leftText; // Left page text UI

    [Header("Right Page")]
    [SerializeField] private TMP_Text rightText; // Right page text UI

    [Header("Buttons")]
    [SerializeField] private Button nextButton; // Button for next page
    [SerializeField] private Button backButton; // Button for previous page

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource; // Audio source for page sound
    [SerializeField] private AudioClip pageTurnSound; // Sound played when page changes

    [Header("Book Pages")]
    [SerializeField] private BookSpread[] spreads; // All book page spreads

    [Header("Animation Settings")]
    [SerializeField] private float openAnimationTime = 0.2f; // Time for book opening animation
    [SerializeField] private float pageBounceTime = 0.18f; // Small delay after page change

    private int currentSpreadIndex; // Current page spread number
    private bool isAnimating; // Prevents page spam during animation
    private bool reachedFinalSpread; // True when player reaches last spread
    private bool bookshelfClueReported; // Prevents reporting the clue more than once

    private void Start()
    {
        // Hide the book panel at the start
        if (bookPanel != null)
            bookPanel.gameObject.SetActive(false);

        // Connect next button to next page function
        if (nextButton != null)
            nextButton.onClick.AddListener(NextSpread);

        // Connect back button to previous page function
        if (backButton != null)
            backButton.onClick.AddListener(PreviousSpread);
    }

    private void Update()
    {
        // Stop if book is not open
        if (bookPanel == null || !bookPanel.gameObject.activeSelf)
            return;

        // Unlock and show cursor while book is open
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Right arrow goes to next spread
        if (Input.GetKeyDown(KeyCode.RightArrow))
            NextSpread();

        // Left arrow goes to previous spread
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            PreviousSpread();

        // Mouse scroll down goes to next spread
        if (Input.mouseScrollDelta.y < 0f)
            NextSpread();

        // Mouse scroll up goes to previous spread
        if (Input.mouseScrollDelta.y > 0f)
            PreviousSpread();
    }

    private bool ClickedButton(Button button)
    {
        // Return false if button cannot be clicked
        if (button == null || !button.gameObject.activeInHierarchy || !button.interactable)
            return false;

        RectTransform rect = button.GetComponent<RectTransform>();

        // Check if mouse position is inside the button rectangle
        return RectTransformUtility.RectangleContainsScreenPoint(
            rect,
            Input.mousePosition,
            null
        );
    }

    public void OpenBook()
    {
        // Start from first spread every time book opens
        currentSpreadIndex = 0;
        reachedFinalSpread = false;

        // Make sure canvas is enabled and appears above other UI
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = true;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999;
        }

        // Make sure canvas group can be seen and clicked
        CanvasGroup group = GetComponent<CanvasGroup>();
        if (group != null)
        {
            group.alpha = 1f;
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        // Show and position the book panel
        if (bookPanel != null)
        {
            bookPanel.gameObject.SetActive(true);
            bookPanel.SetAsLastSibling();
            bookPanel.anchoredPosition = new Vector2(0f, 80f);
            bookPanel.sizeDelta = new Vector2(1055f, 623f);
            bookPanel.localScale = Vector3.one;
        }

        // Keep next button above other UI objects
        if (nextButton != null)
            nextButton.transform.SetAsLastSibling();

        // Keep back button above other UI objects
        if (backButton != null)
            backButton.transform.SetAsLastSibling();

        // Load the first page and play opening animation
        UpdatePages();
        StartCoroutine(OpenAnimation());
    }

    public void CloseBook()
    {
        // Hide the book panel
        if (bookPanel != null)
            bookPanel.gameObject.SetActive(false);

        // If player reached the final spread, report bookshelf clue once
        if (reachedFinalSpread && !bookshelfClueReported)
        {
            bookshelfClueReported = true;
            Level2PuzzleSystem.Instance?.FindBookshelfClue();
        }
    }

    public void NextSpread()
    {
        // Prevent page change while animating
        if (isAnimating) return;

        // Stop if there is no next spread
        if (spreads == null || currentSpreadIndex >= spreads.Length - 1) return;

        // Move to next spread
        currentSpreadIndex++;
        StartCoroutine(PageChangeAnimation());
    }

    public void PreviousSpread()
    {
        // Prevent page change while animating
        if (isAnimating) return;

        // Stop if already on first spread
        if (currentSpreadIndex <= 0) return;

        // Move to previous spread
        currentSpreadIndex--;
        StartCoroutine(PageChangeAnimation());
    }

    private void UpdatePages()
    {
        // Stop if no spreads exist
        if (spreads == null || spreads.Length == 0)
            return;

        BookSpread spread = spreads[currentSpreadIndex];

        // Update left image
        if (leftImage != null)
        {
            leftImage.gameObject.SetActive(spread.leftImage != null);
            leftImage.sprite = spread.leftImage;
        }

        // Update left text and font size
        if (leftText != null)
        {
            leftText.text = spread.leftText;
            leftText.fontSize = spread.bigText ? 46 : 28;
        }

        // Update right text and font size
        if (rightText != null)
        {
            rightText.text = spread.rightText;
            rightText.fontSize = spread.bigText ? 46 : 30;
        }

        // Show next button only when there is another spread
        if (nextButton != null)
            nextButton.gameObject.SetActive(currentSpreadIndex < spreads.Length - 1);

        // Show back button only after the first spread
        if (backButton != null)
            backButton.gameObject.SetActive(currentSpreadIndex > 0);

        // Mark that player reached the final spread
        if (currentSpreadIndex == spreads.Length - 1)
            reachedFinalSpread = true;
    }

    private IEnumerator OpenAnimation()
    {
        isAnimating = true;

        // Start slightly smaller for pop-in animation
        if (bookPanel != null)
            bookPanel.localScale = new Vector3(0.85f, 0.85f, 1f);

        float timer = 0f;

        // Scale the book panel up smoothly
        while (timer < openAnimationTime)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / openAnimationTime;

            if (bookPanel != null)
                bookPanel.localScale = Vector3.Lerp(new Vector3(0.85f, 0.85f, 1f), Vector3.one, t);

            yield return null;
        }

        // Snap to final scale
        if (bookPanel != null)
            bookPanel.localScale = Vector3.one;

        isAnimating = false;
    }

    private IEnumerator PageChangeAnimation()
    {
        isAnimating = true;

        // Play page turn sound
        if (audioSource != null && pageTurnSound != null)
            audioSource.PlayOneShot(pageTurnSound);

        // Change page content
        UpdatePages();

        // Wait a small moment before allowing another page change
        yield return new WaitForSecondsRealtime(pageBounceTime);

        isAnimating = false;
    }
}