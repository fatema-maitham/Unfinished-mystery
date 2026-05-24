using UnityEngine;

/// <summary>
/// Puzzle 2B — Number Theory and Cryptography by K. Flins
/// Blocked until desk phase is complete.
/// Pulling it out reveals Fragment 2 of Lynnette's message and the book dedication.
/// Key clues:
///   - "For the 7 students who believed in me" → n = 7 (used for file password)
///   - Pages 12, 31, 53, 97 → first words WHEN YOU LIE NUMBERS
///   - Letter count 4, 3, 3 → drawer code 433
/// </summary>
public class ActivatableBook : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label            = "Pull Out";
    [SerializeField] private string subLabel         = "Number Theory and Cryptography";
    [SerializeField] private float  activationRadius = 1.5f;

    [Header("Content")]
    [Tooltip("Image of the folded note — Fragment 2 of Lynnette's message")]
    [SerializeField] private Sprite bookNoteImage;
    [TextArea(2, 6)]
    [SerializeField] private string noteText =
        "A folded note falls out — Fragment 2 of Lynnette's message:\n\n" +
        "\"Your cipher uses prime positions. I mapped them. Pages 12, 31, 53, 97. " +
        "First word on each page. I hid the last piece where you always leave your secrets.\"\n\n" +
        "The book spine reads: \"For the 7 students who believed in me.\"";

    [Header("Blocked Message")]
    [SerializeField] private string blockedMessage =
        "You notice the book is slightly out of place, but you're not ready to investigate the bookshelf yet.";

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

        if (bookNoteImage != null)
            ActivationDialogUI.ShowImage(bookNoteImage);
        else
            ActivationDialogUI.ShowText(noteText, "Number Theory and Cryptography");

        if (!_examined)
        {
            _examined = true;
            BookshelfPhaseTracker.Instance?.RegisterBookExamined();
        }
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}