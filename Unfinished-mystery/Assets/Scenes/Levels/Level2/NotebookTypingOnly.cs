using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NotebookTypingOnly : MonoBehaviour
{
    [Header("Notebook")]
    [SerializeField] private GameObject notebookPanel;
    [SerializeField] private TMP_InputField notebookInputField;

    [Header("Text Style")]
    [SerializeField] private TMP_FontAsset font;
    [SerializeField] private float fontSize = 28f;
    [SerializeField] private Color textColor = Color.black;
    [SerializeField] private TextAlignmentOptions alignment = TextAlignmentOptions.TopLeft;

    [Header("Writing Settings")]
    [SerializeField] private bool autoFocusWhenOpen = true;
    [SerializeField] private string placeholderText = "Write your notes here...";

    private TMP_Text inputText;
    private TMP_Text placeholderTextComponent;

    private void Awake()
    {
        if (notebookInputField == null)
            return;

        inputText = notebookInputField.textComponent;
        placeholderTextComponent = notebookInputField.placeholder as TMP_Text;

        ApplyStyle();
    }

    private void Update()
    {
        if (notebookPanel == null || notebookInputField == null)
            return;

        if (!notebookPanel.activeSelf)
            return;

        if (autoFocusWhenOpen && !notebookInputField.isFocused)
        {
            EventSystem.current.SetSelectedGameObject(notebookInputField.gameObject);
            notebookInputField.ActivateInputField();
        }
    }

    private void OnValidate()
    {
        if (notebookInputField == null)
            return;

        inputText = notebookInputField.textComponent;
        placeholderTextComponent = notebookInputField.placeholder as TMP_Text;

        ApplyStyle();
    }

    private void ApplyStyle()
    {
        if (inputText != null)
        {
            if (font != null)
                inputText.font = font;

            inputText.fontSize = fontSize;
            inputText.color = textColor;
            inputText.alignment = alignment;
        }

        if (placeholderTextComponent != null)
        {
            if (font != null)
                placeholderTextComponent.font = font;

            placeholderTextComponent.fontSize = fontSize;
            placeholderTextComponent.color = new Color(textColor.r, textColor.g, textColor.b, 0.35f);
            placeholderTextComponent.alignment = alignment;
            placeholderTextComponent.text = placeholderText;
        }

        notebookInputField.lineType = TMP_InputField.LineType.MultiLineNewline;
        notebookInputField.characterLimit = 0;
    }
}