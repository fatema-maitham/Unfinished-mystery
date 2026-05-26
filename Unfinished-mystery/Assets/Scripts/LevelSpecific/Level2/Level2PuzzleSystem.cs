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
    // This is the singleton instance.
    // Other scripts can access Level2PuzzleSystem.Instance to talk to this system.
    public static Level2PuzzleSystem Instance { get; private set; }

    [Header("Puzzle State")]

    // True after the player reads the first note at the start of the level.
    [SerializeField] private bool _firstNoteRead = false;

    // True after the player interacts with the TV and sees the TV message.
    [SerializeField] private bool _tvMessageSeen = false;

    // True after the player finds the clue hidden in the bookshelf.
    [SerializeField] private bool _bookshelfClueFound = false;

    // True after the player plays the gramophone message.
    [SerializeField] private bool _gramophonePlayed = false;

    // True after the player finds and picks up the SIM card.
    [SerializeField] private bool _simCardFound = false;

    // True after the player uses the SIM card on the phone and hears the message.
    [SerializeField] private bool _phoneMessageHeard = false;

    // True after the player enters the correct keypad code.
    [SerializeField] private bool _keypadCodeEntered = false;

    // True after the final door is unlocked.
    [SerializeField] private bool _doorUnlocked = false;

    // True after the player finishes the level.
    [SerializeField] private bool _levelComplete = false;

    [Header("Events")]

    // Invoked when the first note is read.
    public UnityEvent onFirstNoteRead;

    // Invoked when the TV message is seen.
    public UnityEvent onTVMessageSeen;

    // Invoked when the bookshelf clue is found.
    public UnityEvent onBookshelfClueFound;

    // Invoked when the gramophone message is played.
    public UnityEvent onGramophonePlayed;

    // Invoked when the SIM card is found.
    public UnityEvent onSimCardFound;

    // Invoked when the phone message is heard.
    public UnityEvent onPhoneMessageHeard;

    // Invoked when the correct keypad code is entered.
    public UnityEvent onKeypadCodeEntered;

    // Invoked when the door is unlocked.
    public UnityEvent onDoorUnlocked;

    // Invoked when the level is completed.
    public UnityEvent onLevelComplete;

    // Public read-only access for other scripts.
    public bool FirstNoteRead => _firstNoteRead;
    public bool TVMessageSeen => _tvMessageSeen;
    public bool BookshelfClueFound => _bookshelfClueFound;
    public bool GramophonePlayed => _gramophonePlayed;
    public bool SimCardFound => _simCardFound;
    public bool PhoneMessageHeard => _phoneMessageHeard;
    public bool KeypadCodeEntered => _keypadCodeEntered;
    public bool DoorUnlocked => _doorUnlocked;
    public bool LevelComplete => _levelComplete;

    private void Awake()
    {
        // If another Level2PuzzleSystem already exists, destroy this duplicate.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Save this object as the main Level2PuzzleSystem instance.
        Instance = this;
    }

    public void ReadFirstNote()
    {
        // Do nothing if the first note was already read before.
        if (_firstNoteRead) return;

        // Mark the first note as read.
        _firstNoteRead = true;

        // Print progress in the Console.
        Debug.Log("[Level2PuzzleSystem] First note read.");

        // Run any UnityEvents connected in the Inspector.
        onFirstNoteRead?.Invoke();
    }

    public void SeeTVMessage()
    {
        // The TV message should only count after the first note.
        if (!_firstNoteRead)
        {
            ShowBlocked("Read the first note first.");
            return;
        }

        // Do nothing if the TV message was already seen before.
        if (_tvMessageSeen) return;

        // Mark the TV message as seen.
        _tvMessageSeen = true;

        // Print progress in the Console.
        Debug.Log("[Level2PuzzleSystem] TV message seen.");

        // Run any UnityEvents connected in the Inspector.
        onTVMessageSeen?.Invoke();
    }

    public void FindBookshelfClue()
    {
        // The bookshelf clue should only count after the TV message.
        if (!_tvMessageSeen)
        {
            ShowBlocked("Check the television first.");
            return;
        }

        // Do nothing if the bookshelf clue was already found before.
        if (_bookshelfClueFound) return;

        // Mark the bookshelf clue as found.
        _bookshelfClueFound = true;

        // Print progress in the Console.
        Debug.Log("[Level2PuzzleSystem] Bookshelf clue found.");

        // Run any UnityEvents connected in the Inspector.
        onBookshelfClueFound?.Invoke();
    }

    public void PlayGramophone()
    {
        // The gramophone should only count after the bookshelf clue.
        if (!_bookshelfClueFound)
        {
            ShowBlocked("Find the bookshelf clue first.");
            return;
        }

        // Do nothing if the gramophone was already played before.
        if (_gramophonePlayed) return;

        // Mark the gramophone as played.
        _gramophonePlayed = true;

        // Print progress in the Console.
        Debug.Log("[Level2PuzzleSystem] Gramophone played.");

        // Run any UnityEvents connected in the Inspector.
        onGramophonePlayed?.Invoke();
    }

    public void FindSimCard()
    {
        // The SIM card should only count after the gramophone.
        if (!_gramophonePlayed)
        {
            ShowBlocked("Listen to the gramophone first.");
            return;
        }

        // Do nothing if the SIM card was already found before.
        if (_simCardFound) return;

        // Mark the SIM card as found.
        _simCardFound = true;

        // Print progress in the Console.
        Debug.Log("[Level2PuzzleSystem] SIM card found.");

        // Run any UnityEvents connected in the Inspector.
        onSimCardFound?.Invoke();
    }

    public void HearPhoneMessage()
    {
        // The phone message should only count after the SIM card is found.
        if (!_simCardFound)
        {
            ShowBlocked("Find the SIM card first.");
            return;
        }

        // Do nothing if the phone message was already heard before.
        if (_phoneMessageHeard) return;

        // Mark the phone message as heard.
        _phoneMessageHeard = true;

        // Print progress in the Console.
        Debug.Log("[Level2PuzzleSystem] Phone message heard.");

        // Run any UnityEvents connected in the Inspector.
        onPhoneMessageHeard?.Invoke();
    }

    public void EnterKeypadCode()
    {
        // The keypad code should only count after the phone message.
        if (!_phoneMessageHeard)
        {
            ShowBlocked("Hear the phone message first.");
            return;
        }

        // Do nothing if the keypad code was already entered before.
        if (_keypadCodeEntered) return;

        // Mark the keypad code as entered.
        _keypadCodeEntered = true;

        // Print progress in the Console.
        Debug.Log("[Level2PuzzleSystem] Keypad code entered.");

        // Run any UnityEvents connected in the Inspector.
        onKeypadCodeEntered?.Invoke();

        // Unlock the door after the correct keypad code is entered.
        UnlockDoor();
    }

    public void UnlockDoor()
    {
        // The door should only unlock after the keypad code is entered.
        if (!_keypadCodeEntered)
        {
            ShowBlocked("Enter the keypad code first.");
            return;
        }

        // Do nothing if the door was already unlocked before.
        if (_doorUnlocked) return;

        // Mark the door as unlocked.
        _doorUnlocked = true;

        // Print progress in the Console.
        Debug.Log("[Level2PuzzleSystem] Door unlocked.");

        // Run any UnityEvents connected in the Inspector.
        onDoorUnlocked?.Invoke();
    }

    public void CompleteLevel()
    {
        // The level should only complete after the door is unlocked.
        if (!_doorUnlocked)
        {
            ShowBlocked("Unlock the door first.");
            return;
        }

        // Do nothing if the level was already completed before.
        if (_levelComplete) return;

        // Mark the level as complete.
        _levelComplete = true;

        // Print progress in the Console.
        Debug.Log("[Level2PuzzleSystem] Level complete.");

        // Run any UnityEvents connected in the Inspector.
        onLevelComplete?.Invoke();
    }

    public static void ShowBlocked(string hint = "")
    {
        // Use a default message if no custom hint was provided.
        string message = string.IsNullOrEmpty(hint)
            ? "You can't do this yet. Look around more."
            : hint;

        // Show the blocked message in the Console.
        Debug.Log("[Level2PuzzleSystem] BLOCKED: " + message);
    }
}