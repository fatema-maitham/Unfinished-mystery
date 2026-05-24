using UnityEngine;

public class ActivatableFinalPhoneNumberClue : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label = "Inspect";
    [SerializeField] private string subLabel = "Wall Frame";
    [SerializeField] private float activationRadius = 2.5f;

    [Header("Image")]
    [SerializeField] private Sprite phoneNumberPaperImage;

    [Header("Text")]
    [SerializeField] private string dialogTitle = "Torn paper - BACK";

    [TextArea(4, 10)]
    [SerializeField] private string dialogBody =
        "On the back, written in tiny pencil:\n\n" +
        "916-2847\n\n" +
        "\"Isla's number. Lana kept it hidden for years.\"";

    private bool imageShown = false;
    private bool clueFound = false;

    public string ActivationLabel => label;
    public string ActivationHint => imageShown ? "Read Back" : subLabel;
    public bool CanActivate => true;
    public float ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        if (!imageShown)
        {
            imageShown = true;

            if (phoneNumberPaperImage != null)
                ActivationDialogUI.ShowImage(phoneNumberPaperImage);
            else
                ActivationDialogUI.ShowText("The back of the torn paper is missing.", "Wall Frame");

            return;
        }

        ActivationDialogUI.ShowText(dialogBody, dialogTitle);

        if (!clueFound)
        {
            clueFound = true;
            Level2PuzzleSystem.Instance?.FindPhoneNumber();
        }
    }

    public void OnActivatableFocus() { }

    public void OnActivatableBlur() { }
}