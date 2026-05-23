using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookCanvasController : MonoBehaviour
{
    [System.Serializable]
    public class BookSpread
    {
        public Sprite leftImage;

        [TextArea(3, 10)]
        public string leftText;

        [TextArea(3, 10)]
        public string rightText;
    }

    [Header("Main UI")]
    [SerializeField] private GameObject bookCanvas;
    [SerializeField] private RectTransform bookPanel;

    [Header("Left Page")]
    [SerializeField] private Image leftImage;
    [SerializeField] private TMP_Text leftText;

    [Header("Right Page")]
    [SerializeField] private TMP_Text rightText;

    [Header("Turning Page")]
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

    [Header("Animation")]
    [SerializeField] private float openAnimationTime = 0.2f;
    [SerializeField] private float pageTurnTime = 0.45f;

    private int currentSpreadIndex;
    private bool isAnimating;

    private void Start()
    {
        bookCanvas.SetActive(false);
        turningPage.gameObject.SetActive(false);

        nextButton.onClick.AddListener(NextSpread);
        closeButton.onClick.AddListener(CloseBook);
    }

    public void OpenBook()
    {
        currentSpreadIndex = 0;
        bookCanvas.SetActive(true);
        turningPage.gameObject.SetActive(false);

        UpdatePages();
        StartCoroutine(OpenAnimation());
    }

    public void CloseBook()
    {
        bookCanvas.SetActive(false);
        turningPage.gameObject.SetActive(false);
    }

    public void NextSpread()
    {
        if (isAnimating)
            return;

        if (currentSpreadIndex >= spreads.Length - 1)
            return;

        StartCoroutine(PageFlipAnimation());
    }

    private void UpdatePages()
    {
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

        nextButton.gameObject.SetActive(currentSpreadIndex < spreads.Length - 1);
    }

    private IEnumerator OpenAnimation()
    {
        isAnimating = true;

        bookPanel.localScale = new Vector3(0.85f, 0.85f, 1f);

        float timer = 0f;

        while (timer < openAnimationTime)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / openAnimationTime;
            bookPanel.localScale = Vector3.Lerp(new Vector3(0.85f, 0.85f, 1f), Vector3.one, t);
            yield return null;
        }

        bookPanel.localScale = Vector3.one;
        isAnimating = false;
    }

    private IEnumerator PageFlipAnimation()
    {
        isAnimating = true;

        if (audioSource != null && pageTurnSound != null)
            audioSource.PlayOneShot(pageTurnSound);

        turningPage.gameObject.SetActive(true);
        turningText.text = rightText.text;

        turningPage.localRotation = Quaternion.Euler(0, 0, 0);

        float timer = 0f;

        while (timer < pageTurnTime)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / pageTurnTime;

            float angle = Mathf.Lerp(0f, 180f, t);
            turningPage.localRotation = Quaternion.Euler(0, angle, 0);

            yield return null;
        }

        currentSpreadIndex++;
        UpdatePages();

        turningPage.localRotation = Quaternion.Euler(0, 0, 0);
        turningPage.gameObject.SetActive(false);

        isAnimating = false;
    }
}