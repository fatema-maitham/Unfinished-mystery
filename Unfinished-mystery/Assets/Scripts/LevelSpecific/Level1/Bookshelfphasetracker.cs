using UnityEngine;

/// <summary>
/// Tracks whether both bookshelf items have been examined.
/// Attach to the same empty GameObject as Level1PuzzleSystem.
/// Fires CompleteBookshelfPhase() when both whiteboard and book are done.
/// </summary>
public class BookshelfPhaseTracker : MonoBehaviour
{
    public static BookshelfPhaseTracker Instance { get; private set; }

    private bool _whiteboardDone;
    private bool _bookDone;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;
    }

    public void RegisterWhiteboardExamined() { _whiteboardDone = true; TryComplete(); }
    public void RegisterBookExamined()       { _bookDone       = true; TryComplete(); }

    private void TryComplete()
    {
        if (_whiteboardDone && _bookDone)
            Level1PuzzleSystem.Instance?.CompleteBookshelfPhase();
    }
}