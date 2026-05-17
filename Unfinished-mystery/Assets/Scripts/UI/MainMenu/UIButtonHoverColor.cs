using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Text Reference")]
    [SerializeField] private TMP_Text buttonText;

    [Header("Colors")]
    // Normal X color: soft off-white, fits the dark overlay.
    [SerializeField] private Color normalColor = new Color32(235, 230, 215, 255);

    // Hover X color: light gray, subtle and clean.
    [SerializeField] private Color hoverColor = new Color32(190, 190, 190, 255);

    private void Awake()
    {
        if (buttonText == null)
        {
            buttonText = GetComponentInChildren<TMP_Text>();
        }

        SetNormalColor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetHoverColor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetNormalColor();
    }

    private void SetNormalColor()
    {
        if (buttonText != null)
        {
            buttonText.color = normalColor;
        }
    }

    private void SetHoverColor()
    {
        if (buttonText != null)
        {
            buttonText.color = hoverColor;
        }
    }
}