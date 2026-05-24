using UnityEngine;

/// <summary>
/// Tracks whether all three desk items have been examined.
/// Attach to the same empty GameObject as Level1PuzzleSystem.
/// Fires CompleteDeskPhase() the moment all three are done, in any order.
/// </summary>
public class DeskPhaseTracker : MonoBehaviour
{
    public static DeskPhaseTracker Instance { get; private set; }

    private bool _mugDone;
    private bool _examDone;
    private bool _memoDone;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;
    }

    public void RegisterMugExamined()  { _mugDone  = true; TryComplete(); }
    public void RegisterExamExamined() { _examDone = true; TryComplete(); }
    public void RegisterMemoExamined() { _memoDone = true; TryComplete(); }

    private void TryComplete()
    {
        if (_mugDone && _examDone && _memoDone)
            Level1PuzzleSystem.Instance?.CompleteDeskPhase();
    }
}