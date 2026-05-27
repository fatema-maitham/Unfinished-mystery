using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Central puzzle state manager for Level 2.
/// Put this script on an empty GameObject named Level2PuzzleSystem.
/// This script stores the main progress of the whole level.
/// Other scripts call these public methods when the player finishes each puzzle step.
/// </summary>
public class Level2PuzzleSystem : MonoBehaviour
{
    // Singleton instance so other scripts can call:
    // Level2PuzzleSystem.Instance.MethodName();
    public static Level2PuzzleSystem Instance { get; private set; }

    [Header("Puzzle State")]

    // True after the player reads the first note.
    [SerializeField] private bool _firstNoteRead = false;

    // True after the player checks the TV message.
    [SerializeField] private bool _tvMessageSeen = false;

    // True after the player finds the hidden book/library clue.
    [SerializeField] private bool _bookshelfClueFound = false;

    // True after the player plays the gramophone message.
    [SerializeField] private bool _gramophonePlayed = false;

    // True after the player picks up the SIM card from the drawer.
    [SerializeField] private bool _simCardFound = false;

    // True after the player uses the SIM card on the phone and hears the message.
    [SerializeField] private bool _phoneMessageHeard = false;

    // True after the player checks the stopped clock.
    [SerializeField] private bool _clockClueSeen = false;

    // True after the player enters the correct keypad code.
    [SerializeField] private bool _keypadCodeEntered = false;

    // True after the final door is unlocked.
    [SerializeField] private bool _doorUnlocked = false;

    // True after the player exits / completes the level.
    [SerializeField] private bool _levelComplete = false;

    [Header("Events")]

    // These events can be connected in the Inspector if needed.
    public UnityEvent onFirstNoteRead;
    public UnityEvent onTVMessageSeen;
    public UnityEvent onBookshelfClueFound;
    public UnityEvent onGramophonePlayed;
    public UnityEvent onSimCardFound;
    public UnityEvent onPhoneMessageHeard;
    public UnityEvent onClockClueSeen;
    public UnityEvent onKeypadCodeEntered;
    public UnityEvent onDoorUnlocked;
    public UnityEvent onLevelComplete;

    // Public read-only access.
    public bool FirstNoteRead => _firstNoteRead;
    public bool TVMessageSeen => _tvMessageSeen;
    public bool BookshelfClueFound => _bookshelfClueFound;
    public bool GramophonePlayed => _gramophonePlayed;
    public bool SimCardFound => _simCardFound;
    public bool PhoneMessageHeard => _phoneMessageHeard;
    public bool ClockClueSeen => _clockClueSeen;
    public bool KeypadCodeEntered => _keypadCodeEntered;
    public bool DoorUnlocked => _doorUnlocked;
    public bool LevelComplete => _levelComplete;

    private void Awake()
    {
        // Prevent duplicate puzzle systems in the same scene.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Save this object as the active puzzle system.
        Instance = this;
    }

    public void ReadFirstNote()
    {
        // Stop if this step was already completed.
        if (_firstNoteRead) return;

        _firstNoteRead = true;

        Debug.Log("[Level2PuzzleSystem] First note read.");
        onFirstNoteRead?.Invoke();
    }

    public void SeeTVMessage()
    {
        // TV should only count after the first note.
        if (!_firstNoteRead)
        {
            ShowBlocked("Read the first note first.");
            return;
        }

        if (_tvMessageSeen) return;

        _tvMessageSeen = true;

        Debug.Log("[Level2PuzzleSystem] TV message seen.");
        onTVMessageSeen?.Invoke();
    }

    public void FindBookshelfClue()
    {
        // Bookshelf clue should only count after the TV message.
        if (!_tvMessageSeen)
        {
            ShowBlocked("Check the television first.");
            return;
        }

        if (_bookshelfClueFound) return;

        _bookshelfClueFound = true;

        Debug.Log("[Level2PuzzleSystem] Bookshelf clue found.");
        onBookshelfClueFound?.Invoke();
    }

    public void PlayGramophone()
    {
        // Gramophone should only count after the bookshelf clue.
        if (!_bookshelfClueFound)
        {
            ShowBlocked("Find the bookshelf clue first.");
            return;
        }

        if (_gramophonePlayed) return;

        _gramophonePlayed = true;

        Debug.Log("[Level2PuzzleSystem] Gramophone played.");
        onGramophonePlayed?.Invoke();
    }

    public void FindSimCard()
    {
        // SIM card should only count after the gramophone clue.
        if (!_gramophonePlayed)
        {
            ShowBlocked("Listen to the gramophone first.");
            return;
        }

        if (_simCardFound) return;

        _simCardFound = true;

        Debug.Log("[Level2PuzzleSystem] SIM card found.");
        onSimCardFound?.Invoke();
    }

    public void HearPhoneMessage()
    {
        // Phone message should only count after the SIM card is found.
        if (!_simCardFound)
        {
            ShowBlocked("Find the SIM card first.");
            return;
        }

        if (_phoneMessageHeard) return;

        _phoneMessageHeard = true;

        Debug.Log("[Level2PuzzleSystem] Phone message heard.");
        onPhoneMessageHeard?.Invoke();
    }

    public void SeeClockClue()
    {
        // Clock clue should only count after hearing the phone message.
        if (!_phoneMessageHeard)
        {
            ShowBlocked("Hear the phone message first.");
            return;
        }

        if (_clockClueSeen) return;

        _clockClueSeen = true;

        Debug.Log("[Level2PuzzleSystem] Clock clue seen.");
        onClockClueSeen?.Invoke();
    }

    public void EnterKeypadCode()
    {
        // Keypad should only count after the player checks the stopped clock.
        if (!_clockClueSeen)
        {
            ShowBlocked("Check the clock first.");
            return;
        }

        if (_keypadCodeEntered) return;

        _keypadCodeEntered = true;

        Debug.Log("[Level2PuzzleSystem] Keypad code entered.");
        onKeypadCodeEntered?.Invoke();

        // Correct keypad code unlocks the door.
        UnlockDoor();
    }

    public void UnlockDoor()
    {
        // Door should only unlock after the keypad code is entered.
        if (!_keypadCodeEntered)
        {
            ShowBlocked("Enter the keypad code first.");
            return;
        }

        if (_doorUnlocked) return;

        _doorUnlocked = true;

        Debug.Log("[Level2PuzzleSystem] Door unlocked.");
        onDoorUnlocked?.Invoke();
    }

    public void CompleteLevel()
    {
        // Level should only finish after the door is unlocked.
        if (!_doorUnlocked)
        {
            ShowBlocked("Unlock the door first.");
            return;
        }

        if (_levelComplete) return;

        _levelComplete = true;

        Debug.Log("[Level2PuzzleSystem] Level complete.");
        onLevelComplete?.Invoke();
    }

    public static void ShowBlocked(string hint = "")
    {
        // Default blocked message if no custom message is given.
        string message = string.IsNullOrEmpty(hint)
            ? "You can't do this yet. Look around more."
            : hint;

        Debug.Log("[Level2PuzzleSystem] BLOCKED: " + message);
    }
}