using UnityEngine;
using UnityEngine.Events;

// ═══════════════════════════════════════════════════════════════════════════════
// LEVEL 2 PUZZLE SYSTEM — The Detective
// Central state manager for Level 2.
// Tracks the story flow from the first note to the final locked door.
// Attach this script to an empty GameObject called "Level2PuzzleSystem".
// ═══════════════════════════════════════════════════════════════════════════════
public class Level2PuzzleSystem : MonoBehaviour
{
    public static Level2PuzzleSystem Instance { get; private set; }

    // ── Story / Puzzle State Flags ────────────────────────────────────────────
    [Header("Puzzle State")]
    [SerializeField] private bool _firstNoteRead         = false;
    [SerializeField] private bool _tvMessageSeen         = false;
    [SerializeField] private bool _childDrawingFound     = false;
    [SerializeField] private bool _bookshelfClueFound    = false;
    [SerializeField] private bool _gramophonePlayed      = false;
    [SerializeField] private bool _brassKeyFound         = false;
    [SerializeField] private bool _evidenceBagOpened     = false;
    [SerializeField] private bool _phoneNumberFound      = false;
    [SerializeField] private bool _phoneMessageHeard     = false;
    [SerializeField] private bool _clockCodeFound        = false;
    [SerializeField] private bool _doorUnlocked          = false;
    [SerializeField] private bool _levelComplete         = false;

    // ── Events ────────────────────────────────────────────────────────────────
    [Header("Events")]
    public UnityEvent onFirstNoteRead;
    public UnityEvent onTVMessageSeen;
    public UnityEvent onChildDrawingFound;
    public UnityEvent onBookshelfClueFound;
    public UnityEvent onGramophonePlayed;
    public UnityEvent onBrassKeyFound;
    public UnityEvent onEvidenceBagOpened;
    public UnityEvent onPhoneNumberFound;
    public UnityEvent onPhoneMessageHeard;
    public UnityEvent onClockCodeFound;
    public UnityEvent onDoorUnlocked;
    public UnityEvent onLevelComplete;

    // ── Public Readers ────────────────────────────────────────────────────────
    public bool FirstNoteRead      => _firstNoteRead;
    public bool TVMessageSeen      => _tvMessageSeen;
    public bool ChildDrawingFound  => _childDrawingFound;
    public bool BookshelfClueFound => _bookshelfClueFound;
    public bool GramophonePlayed   => _gramophonePlayed;
    public bool BrassKeyFound      => _brassKeyFound;
    public bool EvidenceBagOpened  => _evidenceBagOpened;
    public bool PhoneNumberFound   => _phoneNumberFound;
    public bool PhoneMessageHeard  => _phoneMessageHeard;
    public bool ClockCodeFound     => _clockCodeFound;
    public bool DoorUnlocked       => _doorUnlocked;
    public bool LevelComplete      => _levelComplete;

    // ── Unity ─────────────────────────────────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // ACT 0 — FIRST NOTE
    // The player reads: "I hid the truth in the things I could not throw away."
    // ═══════════════════════════════════════════════════════════════════════════
    public void ReadFirstNote()
    {
        if (_firstNoteRead) return;

        _firstNoteRead = true;
        Debug.Log("[Level2PuzzleSystem] First note read.");
        onFirstNoteRead?.Invoke();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // ACT 1A — TV MESSAGE
    // TV shows: "She drew what I refused to write."
    // ═══════════════════════════════════════════════════════════════════════════
    public void SeeTVMessage()
    {
        if (_tvMessageSeen) return;

        _tvMessageSeen = true;
        Debug.Log("[Level2PuzzleSystem] TV message seen.");
        onTVMessageSeen?.Invoke();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // ACT 1B — CHILD DRAWING
    // The drawing reveals 2 windows, 5 books, 9 red marks = 259.
    // ═══════════════════════════════════════════════════════════════════════════
    public void FindChildDrawing()
    {
        if (_childDrawingFound) return;

        _childDrawingFound = true;
        Debug.Log("[Level2PuzzleSystem] Child drawing found. Code clue: 259.");
        onChildDrawingFound?.Invoke();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // ACT 1C — BOOKSHELF CLUE
    // The book confirms: "He never left the scene. He stayed in uniform."
    // Badge number 259 connects the drawing to the police officer.
    // ═══════════════════════════════════════════════════════════════════════════
    public void FindBookshelfClue()
    {
        if (_bookshelfClueFound) return;

        _bookshelfClueFound = true;
        Debug.Log("[Level2PuzzleSystem] Bookshelf clue found. Badge 259 confirmed.");
        onBookshelfClueFound?.Invoke();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // ACT 2A — GRAMOPHONE MESSAGE
    // Can be played after the bookshelf clue is found.
    // Message points the player toward the strongest light.
    // ═══════════════════════════════════════════════════════════════════════════
    public void PlayGramophone()
    {
        if (!_bookshelfClueFound)
        {
            ShowBlocked("You need to understand the badge number first.");
            return;
        }

        if (_gramophonePlayed) return;

        _gramophonePlayed = true;
        Debug.Log("[Level2PuzzleSystem] Gramophone message played.");
        onGramophonePlayed?.Invoke();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // ACT 2B — KEY UNDER LIGHT
    // The player finds the brass key under the strongest light.
    // ═══════════════════════════════════════════════════════════════════════════
    public void FindBrassKey()
    {
        if (!_gramophonePlayed)
        {
            ShowBlocked("The next truth only appears where light is strongest.");
            return;
        }

        if (_brassKeyFound) return;

        _brassKeyFound = true;
        Debug.Log("[Level2PuzzleSystem] Brass key found.");
        onBrassKeyFound?.Invoke();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // ACT 3 — EVIDENCE BAG
    // Requires the brass key.
    // Contains case file, police photo, newspaper clipping, and torn note.
    // ═══════════════════════════════════════════════════════════════════════════
    public void OpenEvidenceBag()
    {
        if (!_brassKeyFound)
        {
            ShowBlocked("The bag is locked. You need a key.");
            return;
        }

        if (_evidenceBagOpened) return;

        _evidenceBagOpened = true;
        Debug.Log("[Level2PuzzleSystem] Evidence bag opened.");
        onEvidenceBagOpened?.Invoke();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // ACT 4A — PHONE NUMBER
    // Hidden behind the framed picture: 916-2847.
    // ═══════════════════════════════════════════════════════════════════════════
    public void FindPhoneNumber()
    {
        if (!_evidenceBagOpened)
        {
            ShowBlocked("You need to uncover the evidence first.");
            return;
        }

        if (_phoneNumberFound) return;

        _phoneNumberFound = true;
        Debug.Log("[Level2PuzzleSystem] Phone number found: 916-2847.");
        onPhoneNumberFound?.Invoke();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // ACT 4B — PHONE CALL / ISLA MESSAGE
    // Requires phone number 916-2847.
    // ═══════════════════════════════════════════════════════════════════════════
    public void HearPhoneMessage()
    {
        if (!_phoneNumberFound)
        {
            ShowBlocked("You do not know the number yet.");
            return;
        }

        if (_phoneMessageHeard) return;

        _phoneMessageHeard = true;
        Debug.Log("[Level2PuzzleSystem] Isla phone message heard.");
        onPhoneMessageHeard?.Invoke();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // ACT 5A — CLOCK CODE
    // After Isla's message, the clock reveals the final code: 1143.
    // ═══════════════════════════════════════════════════════════════════════════
    public void FindClockCode()
    {
        if (!_phoneMessageHeard)
        {
            ShowBlocked("The room is not ready to reveal the final code yet.");
            return;
        }

        if (_clockCodeFound) return;

        _clockCodeFound = true;
        Debug.Log("[Level2PuzzleSystem] Clock code found: 1143.");
        onClockCodeFound?.Invoke();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // ACT 5B — LOCKED DOOR
    // Door unlocks only after the final clock code is discovered.
    // ═══════════════════════════════════════════════════════════════════════════
    public void UnlockDoor()
    {
        if (!_clockCodeFound)
        {
            ShowBlocked("You still do not know the final code.");
            return;
        }

        if (_doorUnlocked) return;

        _doorUnlocked = true;
        Debug.Log("[Level2PuzzleSystem] Door unlocked.");
        onDoorUnlocked?.Invoke();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // LEVEL COMPLETE
    // Called when Lana exits the room.
    // ═══════════════════════════════════════════════════════════════════════════
    public void CompleteLevel()
    {
        if (!_doorUnlocked)
        {
            ShowBlocked("The exit is still locked.");
            return;
        }

        if (_levelComplete) return;

        _levelComplete = true;
        Debug.Log("[Level2PuzzleSystem] Level 2 complete.");
        onLevelComplete?.Invoke();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // BLOCKED MESSAGE HELPER
    // Use this when the player tries to interact with something too early.
    // Replace Debug.Log with your UI message system later if needed.
    // ═══════════════════════════════════════════════════════════════════════════
    public static void ShowBlocked(string hint = "")
    {
        string message = string.IsNullOrEmpty(hint)
            ? "You can't do this yet. Look around more."
            : hint;

        Debug.Log("[Level2PuzzleSystem] BLOCKED: " + message);
    }
}