using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Manages the in-level HUD top bar.
/// Attach to the same GameObject as your HUD UIDocument.
/// Wire _carousel to the InstructionCarousel component in the scene.
/// </summary>
[RequireComponent(typeof(UIDocument))]
public class HUDManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InstructionCarousel carousel;

    private UIDocument _doc;
    private Button     _helpButton;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _doc.rootVisualElement;
        _helpButton = root.Q<Button>("btn-help");

        if (_helpButton == null)
        {
            Debug.LogWarning("[HUDManager] 'btn-help' button not found in UXML. " +
                             "Make sure your HUD UXML has a Button with name='btn-help'.");
            return;
        }

        _helpButton.clicked += OnHelpClicked;
    }

    private void OnDisable()
    {
        if (_helpButton != null)
            _helpButton.clicked -= OnHelpClicked;
    }

    private void OnHelpClicked()
    {
        if (carousel == null)
        {
            Debug.LogWarning("[HUDManager] InstructionCarousel reference is not set!");
            return;
        }
        carousel.Toggle();
    }
}
