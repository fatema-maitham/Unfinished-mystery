using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraClueUI : MonoBehaviour
{
    public Image photoImage;

    public Sprite[] photos;

    public TMP_Text clueText;

    private int currentIndex = 0;

    private string[] clues =
    {
        "The <color=#7A1F1F>fifth</color> shadow was missing from the shelf.",

        "The <color=#7A1F1F>second</color> truth was hidden beneath the dust.",

        "The <color=#7A1F1F>eighth</color> page carried the final warning."
    };

    void Start()
    {
        UpdateClue();
    }

    public void NextPhoto()
    {
        currentIndex++;

        if (currentIndex >= photos.Length)
            currentIndex = 0;

        UpdateClue();
    }

    public void PreviousPhoto()
    {
        currentIndex--;

        if (currentIndex < 0)
            currentIndex = photos.Length - 1;

        UpdateClue();
    }

    void UpdateClue()
    {
        photoImage.sprite = photos[currentIndex];
        clueText.text = clues[currentIndex];
    }
}