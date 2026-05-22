using UnityEngine;

public class ActivatableSign : MonoBehaviour, IActivatable
{
    public enum DisplayMode { Text, Image }

    [Header("Prompt")]
    [SerializeField] private string label    = "Read";
    [SerializeField] private string subLabel = "";

    [Header("Display Mode")]
    [Tooltip("Choose Text to show a dialogue. Choose Image to show a sprite (letter, map, etc.)")]
    [SerializeField] private DisplayMode displayMode = DisplayMode.Text;

    [Header("Text Settings")]
    [Tooltip("Shown when Display Mode is set to Text")]
    [TextArea(3, 8)]
    [SerializeField] private string signText = "Enter your text here.";
    [Tooltip("Optional speaker name shown above the text e.g. Notice Board, Amber")]
    [SerializeField] private string speakerName = "";

    [Header("Image Settings")]
    [Tooltip("Shown when Display Mode is set to Image. Assign a Sprite from your Project window.")]
    [SerializeField] private Sprite revealImage;

    [Header("Detection")]
    [Tooltip("How close the player must be to see the prompt.")]
    [SerializeField] private float activationRadius = 1.5f;

    public string ActivationLabel  => label;
    public string ActivationHint   => subLabel;
    public bool   CanActivate      => true;
    public float  ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        switch (displayMode)
        {
            case DisplayMode.Text:
                ActivationDialogUI.ShowText(signText, speakerName);
                break;

            case DisplayMode.Image:
                if (revealImage != null)
                    ActivationDialogUI.ShowImage(revealImage);
                else
                    Debug.LogWarning($"[ActivatableSign] '{gameObject.name}' is set to Image mode but has no sprite assigned.", this);
                break;
        }
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}