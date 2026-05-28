using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the in-level HUD top bar — uGUI version.
/// Attach to any HUD GameObject.
/// Wire btnHelp and carousel in the Inspector.
/// </summary>
public class HUDManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button              btnHelp;
    [SerializeField] private InstructionCarousel carousel;

    private void OnEnable()  => btnHelp.onClick.AddListener(OnHelpClicked);
    private void OnDisable() => btnHelp.onClick.RemoveListener(OnHelpClicked);

    private void OnHelpClicked()
    {
        if (carousel == null)
        {
            Debug.LogWarning("[HUDManager] InstructionCarousel reference not set!");
            return;
        }
        carousel.Toggle();
    }
}
