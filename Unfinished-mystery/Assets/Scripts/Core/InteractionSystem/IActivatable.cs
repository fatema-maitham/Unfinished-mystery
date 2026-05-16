using UnityEngine;


/// <summary>
/// Alternative interactable contract — naming is fully isolated from IInteractable.
/// Implement this on any GameObject that should be interactable in the ALT system.
/// </summary>
public interface IActivatable
{
    string ActivationLabel    { get; }
    string ActivationHint     { get; }   // equivalent to InteractionSubLabel

    void OnActivate(GameObject source);
    void OnActivatableFocus();
    void OnActivatableBlur();

    bool CanActivate { get; }            // equivalent to IsInteractable
}