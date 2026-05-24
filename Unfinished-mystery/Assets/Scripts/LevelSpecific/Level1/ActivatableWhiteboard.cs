using UnityEngine;

// ═══════════════════════════════════════════════════════════════════════════════
// PUZZLE 2A — Whiteboard
// Blocked until desk phase is complete.
// Interaction: "tilt lamp toward board" reveals ghost equations.
// ═══════════════════════════════════════════════════════════════════════════════


/// <summary>
/// Puzzle 2A — Whiteboard
/// Blocked until desk phase is complete.
/// Tilting the lamp reveals ghost impressions of erased equations.
/// Key clue: φ = (1 + √5) / 2 ≈ 1.618 and F(12) = ?
/// This points the player toward Fibonacci and the number 144.
/// </summary>
public class ActivatableWhiteboard : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label            = "Tilt Lamp";
    [SerializeField] private string subLabel         = "Whiteboard";
    [SerializeField] private float  activationRadius = 1.8f;

    [Header("Content")]
    [Tooltip("Image showing ghost equations — φ formula and F(12) = ?")]
    [SerializeField] private Sprite ghostEquationImage;
    [TextArea(2, 5)]
    [SerializeField] private string revealText =
        "Tilting the lamp at an angle reveals ghost impressions of erased equations:\n\n" +
        "φ = (1 + √5) / 2 ≈ 1.618\n" +
        "F(12) = ?";

    [Header("Blocked Message")]
    [SerializeField] private string blockedMessage =
        "The whiteboard is covered in erased equations. You're not focused enough to make sense of it yet.";

    // ── State ─────────────────────────────────────────────────────────────────
    private bool _examined = false;

    // ── IActivatable ──────────────────────────────────────────────────────────
    public string ActivationLabel  => label;
    public string ActivationHint   => subLabel;
    public bool   CanActivate      => true;
    public float  ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        if (!Level1PuzzleSystem.Instance.DeskPhaseComplete)
        {
            Level1PuzzleSystem.ShowBlocked(blockedMessage);
            return;
        }

        if (ghostEquationImage != null)
            ActivationDialogUI.ShowImage(ghostEquationImage);
        else
            ActivationDialogUI.ShowText(revealText, "Whiteboard");

        if (!_examined)
        {
            _examined = true;
            BookshelfPhaseTracker.Instance?.RegisterWhiteboardExamined();
        }
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}