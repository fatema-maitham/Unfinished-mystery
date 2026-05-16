using UnityEngine;

public class ActivatableSign : MonoBehaviour, IActivatable
{
    [SerializeField] private string label    = "Read";
    [SerializeField] private string subLabel = "";
    [TextArea(3, 8)]
    [SerializeField] private string signText = "Enter your text here.";

    public string ActivationLabel => label;
    public string ActivationHint  => subLabel;
    public bool   CanActivate     => true;

    public void OnActivate(GameObject source)
    {
        ActivationDialogUI.ShowText(signText);
    }

    public void OnActivatableFocus()  { }
    public void OnActivatableBlur()   { }
}
