using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonStyleSimple : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    private Image buttonImage;
    private TMP_Text buttonText;
    private RectTransform buttonRect;

    [Header("Colors")]
    [SerializeField] private Color normalColor = new Color32(184, 90, 73, 255);
    [SerializeField] private Color hoverColor = new Color32(170, 83, 67, 255);
    [SerializeField] private Color pressedColor = new Color32(145, 70, 58, 255);

    private bool isPointerOver;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        buttonRect = GetComponent<RectTransform>();
        buttonText = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        SetState(normalColor, Vector3.one);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        SetState(hoverColor, new Vector3(1.04f, 1.04f, 1f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        SetState(normalColor, Vector3.one);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetState(pressedColor, new Vector3(0.98f, 0.98f, 1f));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetState(isPointerOver ? hoverColor : normalColor,
                 isPointerOver ? new Vector3(1.04f, 1.04f, 1f) : Vector3.one);
    }

    private void SetState(Color color, Vector3 scale)
    {
        buttonImage.color = color;
        buttonRect.localScale = scale;
    }
}