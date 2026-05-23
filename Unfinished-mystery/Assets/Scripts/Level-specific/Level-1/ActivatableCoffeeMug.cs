using UnityEngine;

// ═══════════════════════════════════════════════════════════════════════════════
// PUZZLE 1A — Coffee Mug
// Moving it reveals a torn piece of paper. Completing all three desk items
// (mug + exam + memo) calls CompleteDeskPhase().
// ═══════════════════════════════════════════════════════════════════════════════
public class ActivatableCoffeeMug : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label         = "Examine";
    [SerializeField] private string subLabel      = "Coffee Mug";
    [SerializeField] private float  activationRadius = 1.5f;

    [Header("Content")]
    [SerializeField] private Sprite tornPaperImage; // assign in Inspector
    [TextArea(2, 4)]
    [SerializeField] private string revealText =
        "Moving the mug reveals a torn piece of paper underneath.\n" +
        "\"Prof. F — I know what the sequence means. I found the pattern in your own thesis. You hid it in plain sight. — L\"";

    [Header("State")]
    [SerializeField] private GameObject tornPaperObject; // optional: enable a prop in the scene

    private bool _examined = false;

    public string ActivationLabel  => label;
    public string ActivationHint   => _examined ? "Already examined" : subLabel;
    public bool   CanActivate      => true;
    public float  ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        if (tornPaperImage != null)
            ActivationDialogUI.ShowImage(tornPaperImage);
        else
            ActivationDialogUI.ShowText(revealText, "Coffee Mug");

        if (!_examined)
        {
            _examined = true;
            if (tornPaperObject != null) tornPaperObject.SetActive(true);
            DeskPhaseTracker.Instance?.RegisterMugExamined();
        }
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}


// ═══════════════════════════════════════════════════════════════════════════════
// PUZZLE 1B — Lynnette's Exam Paper
// ═══════════════════════════════════════════════════════════════════════════════
// public class ActivatableExamPaper : MonoBehaviour, IActivatable
// {
//     [Header("Prompt")]
//     [SerializeField] private string label         = "Read";
//     [SerializeField] private string subLabel      = "Graded Exam";
//     [SerializeField] private float  activationRadius = 1.5f;
//
//     [Header("Content")]
//     [SerializeField] private Sprite examFrontImage;
//     [SerializeField] private Sprite examBackImage;  // the pencil message side
//     [TextArea(2, 4)]
//     [SerializeField] private string backText =
//         "On the back, in tiny pencil:\n\"Fibonacci starts with love, not logic. 1, 1, 2, 3 — but what comes after fear?\"";
//
//     private bool _flipped   = false;
//     private bool _examined  = false;
//
//     public string ActivationLabel  => _flipped ? "Flip Back" : "Flip Over";
//     public string ActivationHint   => subLabel;
//     public bool   CanActivate      => true;
//     public float  ActivationRadius => activationRadius;
//
//     public void OnActivate(GameObject source)
//     {
//         if (!_flipped)
//         {
//             // Show front first
//             if (examFrontImage != null)
//                 ActivationDialogUI.ShowImage(examFrontImage);
//             else
//                 ActivationDialogUI.ShowText("Lynnette's exam — perfect score. Voss crossed it out and wrote \"SEE ME\" in red.", "Exam Paper");
//             _flipped = true;
//         }
//         else
//         {
//             // Show back with hidden clue
//             if (examBackImage != null)
//                 ActivationDialogUI.ShowImage(examBackImage);
//             else
//                 ActivationDialogUI.ShowText(backText, "Exam Paper — Back");
//
//             if (!_examined)
//             {
//                 _examined = true;
//                 DeskPhaseTracker.Instance?.RegisterExamExamined();
//             }
//         }
//     }
//
//     public void OnActivatableFocus() { }
//     public void OnActivatableBlur()  { }
// }


// ═══════════════════════════════════════════════════════════════════════════════
// PUZZLE 1C — University Memo
// Shows the watermark number 144 and March 5th signature date.
// ═══════════════════════════════════════════════════════════════════════════════
// public class ActivatableUniversityMemo : MonoBehaviour, IActivatable
// {
//     [Header("Prompt")]
//     [SerializeField] private string label         = "Read";
//     [SerializeField] private string subLabel      = "University Memo";
//     [SerializeField] private float  activationRadius = 1.5f;
//
//     [Header("Content")]
//     [SerializeField] private Sprite memoImage;
//     [TextArea(2, 5)]
//     [SerializeField] private string memoText =
//         "Nadia Orin has been formally withdrawn from the program.\nSigned: Professor K. Flins — March 5th.\n\n" +
//         "Looking closely, a faint number is watermarked into the paper: 144.";
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
//         if (memoImage != null)
//             ActivationDialogUI.ShowImage(memoImage);
//         else
//             ActivationDialogUI.ShowText(memoText, "University Memo");
//
//         if (!_examined)
//         {
//             _examined = true;
//             DeskPhaseTracker.Instance?.RegisterMemoExamined();
//         }
//     }
//
//     public void OnActivatableFocus() { }
//     public void OnActivatableBlur()  { }
// }


// ═══════════════════════════════════════════════════════════════════════════════
// DESK PHASE TRACKER — helper singleton that watches all three desk items
// and fires CompleteDeskPhase() when all three are examined.
// Attach to the same empty GameObject as Level1PuzzleSystem, or any scene object.
// ═══════════════════════════════════════════════════════════════════════════════
// public class DeskPhaseTracker : MonoBehaviour
// {
//     public static DeskPhaseTracker Instance { get; private set; }
//
//     private bool _mugDone, _examDone, _memoDone;
//
//     private void Awake()
//     {
//         if (Instance != null && Instance != this) { Destroy(this); return; }
//         Instance = this;
//     }
//
//     public void RegisterMugExamined()  { _mugDone  = true; TryComplete(); }
//     public void RegisterExamExamined() { _examDone = true; TryComplete(); }
//     public void RegisterMemoExamined() { _memoDone = true; TryComplete(); }
//
//     private void TryComplete()
//     {
//         if (_mugDone && _examDone && _memoDone)
//             Level1PuzzleSystem.Instance?.CompleteDeskPhase();
//     }
// }