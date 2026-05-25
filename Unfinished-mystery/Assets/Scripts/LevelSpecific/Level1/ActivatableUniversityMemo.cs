using UnityEngine;

/// <summary>
/// Puzzle 1C — University Memo
/// Shows the formal withdrawal of Nadia Orin signed by Voss.
/// Key details: signature date March 5th (day Lynnette disappeared),
/// and watermark number 144 — the 12th Fibonacci number, clue for later.
/// Registers with DeskPhaseTracker on first examination.
/// </summary>
public class ActivatableUniversityMemo : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label            = "Read";
    [SerializeField] private string subLabel         = "University Memo";
    [SerializeField] private float  activationRadius = 1.5f;

    [Header("Content")]
    [Tooltip("Optional image of the memo — make sure watermark 144 is visible in the design")]
    [SerializeField] private Sprite memoImage;
    [TextArea(2, 5)]
    [SerializeField] private string memoText =
        "Nadia Orin has been formally withdrawn from the program.\n" +
        "Signed: Professor K. Flins — March 5th.\n\n" +
        "Looking closely, a faint number is watermarked into the paper: 144.";

    // ── State ─────────────────────────────────────────────────────────────────
    private bool _examined = false;

    // ── IActivatable ──────────────────────────────────────────────────────────
    public string ActivationLabel  => label;
    public string ActivationHint   => subLabel;
    public bool   CanActivate      => true;
    public float  ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        if (memoImage != null)
            ActivationDialogUI.ShowImage(memoImage);
        else
            ActivationDialogUI.ShowText(memoText, "University Memo");

        if (!_examined)
        {
            _examined = true;
            DeskPhaseTracker.Instance?.RegisterMemoExamined();
        }
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}