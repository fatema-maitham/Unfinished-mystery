using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivatableFinalPhoneNumberClue : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label = "Inspect";
    [SerializeField] private string subLabel = "Wall Frame";
    [SerializeField] private float activationRadius = 8f;

    [Header("Existing Dialog UI")]
    [SerializeField] private GameObject dialogRoot;
    [SerializeField] private GameObject textPanel;
    [SerializeField] private GameObject imagePanel;

    [SerializeField] private Image displayImage;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogBodyText;
    [SerializeField] private TMP_Text continueHintText;

    [Header("Phone Number Image")]
    [SerializeField] private Sprite phoneNumberNoteImage;

    [Header("Text")]
    [SerializeField] private string dialogTitle = "Torn paper - BACK";

    [TextArea(4, 10)]
    [SerializeField] private string dialogBody =
        "On the back, written in tiny pencil:\n\n" +
        "916-2847\n\n" +
        "\"Isla's number. Lana kept it hidden for years.\"";

    private int step = 0;
    private bool phoneNumberFound = false;

    public string ActivationLabel => step == 0 ? label : "Continue";
    public string ActivationHint => step == 0 ? subLabel : "Phone Number";
    public bool CanActivate => true;
    public float ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        if (step == 0)
        {
            ShowImagePanel();
            step = 1;
            return;
        }

        if (step == 1)
        {
            ShowTextPanel();
            step = 2;

            if (!phoneNumberFound)
            {
                phoneNumberFound = true;
                Level2PuzzleSystem.Instance?.FindPhoneNumber();
            }

            return;
        }

        ClosePanel();
        step = 0;
    }

    private void ShowImagePanel()
    {
        if (dialogRoot != null)
            dialogRoot.SetActive(true);

        if (imagePanel != null)
            imagePanel.SetActive(true);

        if (textPanel != null)
            textPanel.SetActive(false);

        if (displayImage != null && phoneNumberNoteImage != null)
            displayImage.sprite = phoneNumberNoteImage;
    }

    private void ShowTextPanel()
    {
        if (dialogRoot != null)
            dialogRoot.SetActive(true);

        if (imagePanel != null)
            imagePanel.SetActive(false);

        if (textPanel != null)
            textPanel.SetActive(true);

        if (speakerNameText != null)
            speakerNameText.text = dialogTitle;

        if (dialogBodyText != null)
            dialogBodyText.text = dialogBody;

        if (continueHintText != null)
            continueHintText.text = "Press E to close";
    }

private void ClosePanel()
{
    if (imagePanel != null) imagePanel.SetActive(false);
    if (textPanel != null) textPanel.SetActive(false);
}

    public void OnActivatableFocus() { }

    public void OnActivatableBlur() { }
}