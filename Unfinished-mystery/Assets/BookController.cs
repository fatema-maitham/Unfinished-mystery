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

        public bool bigText;
    }

    [Header("Main UI")]
    [SerializeField] private GameObject bookCanvas;
    [SerializeField] private RectTransform bookPanel;

    [Header("Content")]
    [SerializeField] private Image leftImage;
    [SerializeField] private TMP_Text leftText;
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

    [Header("Pages")]
    [SerializeField] private BookSpread[] spreads;

    [Header("Animation")]
    [SerializeField] private float pageTurnTime = 0.45f;

    private int currentIndex;
    private bool isAnimating;

    private void Start()
    {
        bookCanvas.SetActive(false);
        turningPage.gameObject.SetActive(false);

        nextButton.onClick.AddListener(NextSpread);
    }

    public void OpenBook()
    {
        currentIndex = 0;
        bookCanvas.SetActive(true);
        turningPage.gameObject.SetActive(false);
        UpdatePage();
    }

    public void CloseBook()
    {
        bookCanvas.SetActive(false);
        turningPage.gameObject.SetActive(false);
    }

    public void NextSpread()
    {
        if (isAnimating) return;
        if (currentIndex >= spreads.Length - 1) return;

        StartCoroutine(PageFlip());
    }

    private void UpdatePage()
    {
        BookSpread spread = spreads[currentIndex];

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

        int size = spread.bigText ? 46 : 30;
        leftText.fontSize = size;
        rightText.fontSize = size;

        nextButton.gameObject.SetActive(currentIndex < spreads.Length - 1);
    }

    private IEnumerator PageFlip()
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

        currentIndex++;
        UpdatePage();

        turningPage.localRotation = Quaternion.Euler(0, 0, 0);
        turningPage.gameObject.SetActive(false);

        isAnimating = false;
    }
}