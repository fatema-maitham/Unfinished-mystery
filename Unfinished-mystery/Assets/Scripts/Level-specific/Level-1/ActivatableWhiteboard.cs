using UnityEngine;

// ═══════════════════════════════════════════════════════════════════════════════
// PUZZLE 2A — Whiteboard
// Blocked until desk phase is complete.
// Interaction: "tilt lamp toward board" reveals ghost equations.
// ═══════════════════════════════════════════════════════════════════════════════
using UnityEngine;

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

// ═══════════════════════════════════════════════════════════════════════════════
// PUZZLE 2B — The Book (Number Theory and Cryptography by K. Flins)
// Blocked until desk phase is complete.
// Pulling it out reveals a folded note — Fragment 2 of Lynnette's message.
// ═══════════════════════════════════════════════════════════════════════════════
// public class ActivatableBook : MonoBehaviour, IActivatable
// {
//     [Header("Prompt")]
//     [SerializeField] private string label         = "Pull Out";
//     [SerializeField] private string subLabel      = "Number Theory and Cryptography";
//     [SerializeField] private float  activationRadius = 1.5f;
//
//     [Header("Content")]
//     [SerializeField] private Sprite bookNoteImage; // Fragment 2 image
//     [TextArea(2, 6)]
//     [SerializeField] private string noteText =
//         "A folded note falls out — Fragment 2:\n\n" +
//         "\"Your cipher uses prime positions. I mapped them. Pages 12, 31, 53, 97. " +
//         "First word on each page. I hid the last piece where you always leave your secrets.\"\n\n" +
//         "The book spine shows a dedication: \"For the 7 students who believed in me.\"";
//
//     [Header("Blocked Message")]
//     [SerializeField] private string blockedMessage =
//         "You notice the book is slightly out of place, but you're not ready to investigate the bookshelf yet.";
//
//     private bool _examined = false;
//
//     public string ActivationLabel  => label;
//     public string ActivationHint   => subLabel;
//     public bool   CanActivate      => true;
//     public float  ActivationRadius => activationRadius;
//
//     public void OnActivate(GameObject source)
//     {
//         if (!Level1PuzzleSystem.Instance.DeskPhaseComplete)
//         {
//             Level1PuzzleSystem.ShowBlocked(blockedMessage);
//             return;
//         }
//
//         if (bookNoteImage != null)
//             ActivationDialogUI.ShowImage(bookNoteImage);
//         else
//             ActivationDialogUI.ShowText(noteText, "Number Theory and Cryptography");
//
//         if (!_examined)
//         {
//             _examined = true;
//             BookshelfPhaseTracker.Instance?.RegisterBookExamined();
//         }
//     }
//
//     public void OnActivatableFocus() { }
//     public void OnActivatableBlur()  { }
// }


// ═══════════════════════════════════════════════════════════════════════════════
// BOOKSHELF PHASE TRACKER
// Both whiteboard and book must be examined to complete Puzzle 2.
// ═══════════════════════════════════════════════════════════════════════════════
// public class BookshelfPhaseTracker : MonoBehaviour
// {
//     public static BookshelfPhaseTracker Instance { get; private set; }
//
//     private bool _whiteboardDone, _bookDone;
//
//     private void Awake()
//     {
//         if (Instance != null && Instance != this) { Destroy(this); return; }
//         Instance = this;
//     }
//
//     public void RegisterWhiteboardExamined() { _whiteboardDone = true; TryComplete(); }
//     public void RegisterBookExamined()       { _bookDone       = true; TryComplete(); }
//
//     private void TryComplete()
//     {
//         if (_whiteboardDone && _bookDone)
//             Level1PuzzleSystem.Instance?.CompleteBookshelfPhase();
//     }
// }