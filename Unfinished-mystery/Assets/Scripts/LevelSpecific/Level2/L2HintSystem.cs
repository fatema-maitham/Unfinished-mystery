using UnityEngine;

// Hint system for Level 2 that gives different hints based on puzzle progress
public class L2HintSystem : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label = "Ask for Hint"; // Main prompt text
    [SerializeField] private string subLabel = "Hint"; // Small prompt text
    [SerializeField] private float activationRadius = 2f; // Distance needed to activate hint

    [Header("Speaker")]
    [Tooltip("Name shown above the hint text.")]
    [SerializeField] private string speakerName = "Inner Voice"; // Speaker name shown in dialog

    [Header("Hints — shown in order as puzzles are completed")]

    [Tooltip("Shown at the very start before the first note is read.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_Start =
        "Something in this room feels unfinished. Start with what was left behind."; // Hint before any puzzle progress

    [Tooltip("Shown after the first note is read, before the TV message.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_FirstNoteRead =
        "The room is quiet, but not completely silent. Follow the sound."; // Hint after first note

    [Tooltip("Shown after the TV message is seen, before the bookshelf clue.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_TVMessageSeen =
        "The message should lead you toward the library area."; // Hint after TV message

    [Tooltip("Shown after the bookshelf clue is found, before the gramophone.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_BookshelfFound =
        "Old stories can lead to old voices. Look for something that can play a recording."; // Hint after bookshelf clue

    [Tooltip("Shown after the gramophone is played, before the SIM card is found.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_GramophonePlayed =
        "The recording points to something hidden away. Check what can be opened."; // Hint after gramophone

    [Tooltip("Shown after the SIM card is found, before the phone message.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_SimCardFound =
        "A small missing piece can make a silent device speak again."; // Hint after SIM card

    [Tooltip("Shown after the phone message is heard, before the clock clue.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_PhoneMessageHeard =
        "After the message, pay attention to time."; // Hint after phone message

    [Tooltip("Shown after the clock clue is seen, before the keypad code.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_ClockClueSeen =
        "Use the stopped time as the door code."; // Hint after clock clue

    [Tooltip("Shown after the keypad code is entered, before the door opens.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_KeypadCodeEntered =
        "The keypad should open the way out."; // Hint after keypad code

    [Tooltip("Shown after the door is unlocked.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_DoorUnlocked =
        "You have everything you need to leave."; // Hint after door unlock

    [Tooltip("Shown after level is complete.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_Complete =
        "The truth is no longer trapped here."; // Hint after level complete

    public string ActivationLabel => label; // Prompt label used by activation system
    public string ActivationHint => subLabel; // Prompt hint used by activation system
    public bool CanActivate => true; // Hint object can always be activated
    public float ActivationRadius => activationRadius; // Activation range used by activation system

    public void OnActivate(GameObject source)
    {
        // Get the correct hint for current puzzle progress
        string hint = GetCurrentHint();

        // Show the hint inside the dialog UI
        ActivationDialogUI.ShowText(hint, speakerName);
    }

    private string GetCurrentHint()
    {
        // Get Level 2 puzzle progress system
        var ps = Level2PuzzleSystem.Instance;

        // If puzzle system is missing, show fallback hint
        if (ps == null)
        {
            Debug.LogWarning("[L2HintSystem] Level2PuzzleSystem not found in scene.", this);
            return "Something feels off. I can't focus right now.";
        }

        // Check latest progress first, then move backward
        if (ps.LevelComplete) return hint_Complete;
        if (ps.DoorUnlocked) return hint_DoorUnlocked;
        if (ps.KeypadCodeEntered) return hint_KeypadCodeEntered;
        if (ps.ClockClueSeen) return hint_ClockClueSeen;
        if (ps.PhoneMessageHeard) return hint_PhoneMessageHeard;
        if (ps.SimCardFound) return hint_SimCardFound;
        if (ps.GramophonePlayed) return hint_GramophonePlayed;
        if (ps.BookshelfClueFound) return hint_BookshelfFound;
        if (ps.TVMessageSeen) return hint_TVMessageSeen;
        if (ps.FirstNoteRead) return hint_FirstNoteRead;

        // Default hint before anything is completed
        return hint_Start;
    }

    // Optional focus function required by IActivatable
    public void OnActivatableFocus() { }

    // Optional blur function required by IActivatable
    public void OnActivatableBlur() { }
}