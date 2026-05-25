using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Central puzzle manager for Level 2.
/// Tracks the main story progress from the first note until the final door unlock.
/// </summary>
public class Level2PuzzleSystem : MonoBehaviour
{
    public static Level2PuzzleSystem Instance { get; private set; }

    // Main puzzle progress flags.
    [Header("Puzzle State")]
    [SerializeField] private bool _firstNoteRead = false;
    [SerializeField] private bool _tvMessageSeen = false;
    [SerializeField] private bool _bookshelfClueFound = false;
    [SerializeField] private bool _gramophonePlayed = false;
    [SerializeField] private bool _brassKeyFound = false;
    [SerializeField] private bool _evidenceBagOpened = false;
    [SerializeField] private bool _phoneNumberFound = false;
    [SerializeField] private bool _phoneMessageHeard = false;
    [SerializeField] private bool _clockCodeFound = false;
    [SerializeField] private bool _doorUnlocked = false;
    [SerializeField] private bool _levelComplete = false;

    // UnityEvents allow sounds, UI, VFX, and animations to be connected from the Inspector.
    [Header("Events")]
    public UnityEvent onFirstNoteRead;
    public UnityEvent onTVMessageSeen;
    public UnityEvent onBookshelfClueFound;
    public UnityEvent onGramophonePlayed;
    public UnityEvent onBrassKeyFound;
    public UnityEvent onEvidenceBagOpened;
    public UnityEvent onPhoneNumberFound;
    public UnityEvent onPhoneMessageHeard;
    public UnityEvent onClockCodeFound;
    public UnityEvent onDoorUnlocked;
    public UnityEvent onLevelComplete;

    // Public read-only access for other scripts.
    public bool FirstNoteRead => _firstNoteRead;
    public bool TVMessageSeen => _tvMessageSeen;
    public bool BookshelfClueFound => _bookshelfClueFound;
    public bool GramophonePlayed => _gramophonePlayed;
    public bool BrassKeyFound => _brassKeyFound;
    public bool EvidenceBagOpened => _evidenceBagOpened;
    public bool PhoneNumberFound => _phoneNumberFound;
    public bool PhoneMessageHeard => _phoneMessageHeard;
    public bool ClockCodeFound => _clockCodeFound;
    public bool DoorUnlocked => _doorUnlocked;
    public bool LevelComplete => _levelComplete;

    private void Awake()
    {
        // Keeps only one Level2PuzzleSystem active in the scene.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Called when the player reads the first note.
    public void ReadFirstNote()
    {
        if (_firstNoteRead) return;

        _firstNoteRead = true;
        Debug.Log("[Level2PuzzleSystem] First note read.");
        onFirstNoteRead?.Invoke();
    }

    // Called when the player sees the TV message.
    public void SeeTVMessage()
    {
        if (_tvMessageSeen) return;

        _tvMessageSeen = true;
        Debug.Log("[Level2PuzzleSystem] TV message seen.");
        onTVMessageSeen?.Invoke();
    }

    // Called when the bookshelf clue confirms the badge number.
    public void FindBookshelfClue()
    {
        if (_bookshelfClueFound) return;

        _bookshelfClueFound = true;
        Debug.Log("[Level2PuzzleSystem] Bookshelf clue found. Badge 259 confirmed.");
        onBookshelfClueFound?.Invoke();
    }

    // Called when the gramophone message plays.
    // Requires the bookshelf clue first.
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

    // Called when the player finds the brass key under the strongest light.
    // Requires the gramophone clue first.
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

    // Called when the player opens the evidence bag.
    // Requires the brass key first.
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

    // Called when the hidden phone number is found behind the wall frame.
    // Requires the evidence bag first.
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

    // Called after the player enters the phone number and hears Isla's message.
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

    // Called when the clock reveals the final code.
    // Requires the phone message first.
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

    // Called when the final door unlocks.
    // Requires the clock code first.
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

    // Called when the player exits the room.
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

    // Shows a blocked interaction message in the Console.
    // Later, this can be connected to the level UI message system.
    public static void ShowBlocked(string hint = "")
    {
        string message = string.IsNullOrEmpty(hint)
            ? "You can't do this yet. Look around more."
            : hint;

        Debug.Log("[Level2PuzzleSystem] BLOCKED: " + message);
    }
}