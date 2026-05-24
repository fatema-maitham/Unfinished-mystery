using UnityEngine;


/// <summary>
/// Puzzle 1B — Lynnette's Exam Paper
/// First press shows the front (graded exam with SEE ME).
/// Second press shows the back (hidden pencil message — clue for later puzzles).
/// Only the second press counts as "examined" for the DeskPhaseTracker.
/// </summary>
public class ActivatableExamPaper : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label         = "Read";
    [SerializeField] private string subLabel      = "Graded Exam";
    [SerializeField] private float  activationRadius = 1.5f;

    [Header("Front Side")]
    [Tooltip("Image of the graded exam — perfect score crossed out with SEE ME in red")]
    [SerializeField] private Sprite examFrontImage;
    [TextArea(2, 4)]
    [SerializeField] private string frontText =
        "Lynnette's exam. Perfect score — but Voss crossed it out and wrote \"SEE ME\" in red ink.";

    [Header("Back Side")]
    [Tooltip("Image of the pencil message Lynnette wrote on the back")]
    [SerializeField] private Sprite examBackImage;
    [TextArea(2, 4)]
    [SerializeField] private string backText =
        "On the back, written in tiny pencil:\n\n" +
        "\"Fibonacci starts with love, not logic. 1, 1, 2, 3 — but what comes after fear?\"";

    // ── State ─────────────────────────────────────────────────────────────────
    private bool _flipped  = false; // true after first press (front shown)
    private bool _examined = false; // true after second press (back shown)

    // ── IActivatable ──────────────────────────────────────────────────────────
    public string ActivationLabel  => _flipped ? "Flip Back" : "Flip Over";
    public string ActivationHint   => subLabel;
    public bool   CanActivate      => true;
    public float  ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        if (!_flipped)
        {
            // First press — show front
            if (examFrontImage != null)
                ActivationDialogUI.ShowImage(examFrontImage);
            else
                ActivationDialogUI.ShowText(frontText, "Exam Paper");

            _flipped = true;
        }
        else
        {
            // Second press — show back with the hidden clue
            if (examBackImage != null)
                ActivationDialogUI.ShowImage(examBackImage);
            else
                ActivationDialogUI.ShowText(backText, "Exam Paper — Back");

            // Only register as examined on the back side
            if (!_examined)
            {
                _examined = true;
                DeskPhaseTracker.Instance?.RegisterExamExamined();
            }
        }
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}