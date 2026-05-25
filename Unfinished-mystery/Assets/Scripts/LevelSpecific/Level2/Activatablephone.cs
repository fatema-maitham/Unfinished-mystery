using UnityEngine;

public class Activatablephone : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string activationLabel = "Use Phone";
    [SerializeField] private string activationHint = "Enter Number";
    [SerializeField] private float activationRadius = 3f;

    [Header("Phone UI")]
    [SerializeField] private PhoneUI phoneUI;

    public string ActivationLabel => activationLabel;
    public string ActivationHint => activationHint;
    public bool CanActivate => phoneUI != null;
    public float ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        if (phoneUI == null)
        {
            Debug.LogWarning("Phone UI is not assigned on Activatablephone.");
            return;
        }

        phoneUI.OpenPhone();
    }

    public void OnActivatableFocus()
    {
        // Optional: add highlight later
    }

    public void OnActivatableBlur()
    {
        // Optional: remove highlight later
    }
}