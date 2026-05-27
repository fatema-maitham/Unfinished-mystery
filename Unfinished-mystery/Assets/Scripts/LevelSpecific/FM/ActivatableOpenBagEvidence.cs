using UnityEngine;

public class ActivatableOpenBagEvidence : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label = "Read";
    [SerializeField] private string subLabel = "Evidence";
    [SerializeField] private float activationRadius = 1.5f;

    [Header("Evidence Images")]
    [SerializeField] private Sprite newspaperImage;
    [SerializeField] private Sprite phoneNumberImage;

    private bool newspaperShown;
    private bool phoneNumberShown;

    public string ActivationLabel
    {
        get
        {
            if (!newspaperShown)
                return "Read";

            if (!phoneNumberShown)
                return "Next";

            return "Read";
        }
    }

    public string ActivationHint => subLabel;
    public bool CanActivate => true;
    public float ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        if (!newspaperShown)
        {
            if (newspaperImage != null)
                ActivationDialogUI.ShowImage(newspaperImage);

            newspaperShown = true;
            return;
        }

        if (!phoneNumberShown)
        {
            if (phoneNumberImage != null)
                ActivationDialogUI.ShowImage(phoneNumberImage);

            phoneNumberShown = true;
            Level2PuzzleSystem.Instance?.FindPhoneNumber();

            return;
        }

        if (phoneNumberImage != null)
            ActivationDialogUI.ShowImage(phoneNumberImage);
    }

    public void OnActivatableFocus() { }

    public void OnActivatableBlur() { }
}