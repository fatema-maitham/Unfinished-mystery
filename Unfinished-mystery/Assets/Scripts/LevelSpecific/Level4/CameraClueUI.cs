using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraClueUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image photoImage;
    [SerializeField] private RectTransform photoFrame;
    [SerializeField] private TMP_Text clueText;

    [Header("Photos")]
    [SerializeField] private Sprite[] photos;

    [Header("Photo Scale")]
    [SerializeField] private float photoScale = 0.88f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSound;

    private int currentIndex = 0;

    private readonly string[] clues =
    {
        "The <color=#7A1F1F>fifth</color> shadow was missing from the shelf.",
        "The <color=#7A1F1F>second</color> truth was hidden beneath the dust.",
        "The <color=#7A1F1F>eighth</color> page carried the final warning."
    };

    private void Start()
    {
        UpdateClue();
    }

    public void NextPhoto()
    {
        PlayClickSound();

        currentIndex++;

        if (currentIndex >= photos.Length)
            currentIndex = 0;

        UpdateClue();
    }

    public void PreviousPhoto()
    {
        PlayClickSound();

        currentIndex--;

        if (currentIndex < 0)
            currentIndex = photos.Length - 1;

        UpdateClue();
    }

    private void UpdateClue()
    {
        if (photos == null || photos.Length == 0)
            return;

        if (photoImage != null)
        {
            photoImage.sprite = photos[currentIndex];
            photoImage.preserveAspect = false;
            FitPhotoToFrame();
        }

        if (clueText != null && currentIndex < clues.Length)
            clueText.text = clues[currentIndex];
    }

    private void FitPhotoToFrame()
    {
        if (photoImage == null || photoFrame == null || photoImage.sprite == null)
            return;

        RectTransform photoRect = photoImage.rectTransform;

        float frameWidth = photoFrame.rect.width;
        float frameHeight = photoFrame.rect.height;

        float spriteWidth = photoImage.sprite.rect.width;
        float spriteHeight = photoImage.sprite.rect.height;

        float frameRatio = frameWidth / frameHeight;
        float spriteRatio = spriteWidth / spriteHeight;

        float finalWidth;
        float finalHeight;

        if (spriteRatio > frameRatio)
        {
            finalHeight = frameHeight;
            finalWidth = frameHeight * spriteRatio;
        }
        else
        {
            finalWidth = frameWidth;
            finalHeight = frameWidth / spriteRatio;
        }

        photoRect.anchorMin = new Vector2(0.5f, 0.5f);
        photoRect.anchorMax = new Vector2(0.5f, 0.5f);
        photoRect.pivot = new Vector2(0.5f, 0.5f);
        photoRect.anchoredPosition = Vector2.zero;

        photoRect.sizeDelta = new Vector2(
            finalWidth * photoScale,
            finalHeight * photoScale
        );
    }

    private void PlayClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
            audioSource.PlayOneShot(buttonClickSound);
    }
}