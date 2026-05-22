using UnityEngine;


/// <summary>
/// Alternative interactable contract — naming is fully isolated from IInteractable.
/// Implement this on any GameObject that should be interactable in the ALT system.
/// </summary>
using UnityEngine;

public interface IActivatable
{
    string ActivationLabel { get; }
    string ActivationHint  { get; }
    bool   CanActivate     { get; }
    float  ActivationRadius { get; }  // ← add this

    void OnActivate(GameObject source);
    void OnActivatableFocus();
    void OnActivatableBlur();
}