using UnityEngine;

public class L2HintSystem : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label = "Ask for Hint";
    [SerializeField] private string subLabel = "Hint";
    [SerializeField] private float activationRadius = 2f;

    [Header("Speaker")]
    [Tooltip("Name shown above the hint text.")]
    [SerializeField] private string speakerName = "Inner Voice";

    [Header("Hints — shown in order as puzzles are completed")]

    [Tooltip("Shown at the very start before the first note is read.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_Start =
        "Something in this room feels unfinished. Start with what was left behind.";

    [Tooltip("Shown after the first note is read, before the TV message.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_FirstNoteRead =
        "The room is quiet, but not completely silent. Follow the sound.";

    [Tooltip("Shown after the TV message is seen, before the bookshelf clue.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_TVMessageSeen =
        "The message should lead you toward the library area.";

    [Tooltip("Shown after the bookshelf clue is found, before the gramophone.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_BookshelfFound =
        "Old stories can lead to old voices. Look for something that can play a recording.";

    [Tooltip("Shown after the gramophone is played, before the SIM card is found.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_GramophonePlayed =
        "The recording points to something hidden away. Check what can be opened.";

    [Tooltip("Shown after the SIM card is found, before the phone message.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_SimCardFound =
        "A small missing piece can make a silent device speak again.";

    [Tooltip("Shown after the phone message is heard, before the clock clue.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_PhoneMessageHeard =
        "After the message, pay attention to time.";

    [Tooltip("Shown after the clock clue is seen, before the keypad code.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_ClockClueSeen =
        "Use the stopped time as the door code.";

    [Tooltip("Shown after the keypad code is entered, before the door opens.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_KeypadCodeEntered =
        "The keypad should open the way out.";

    [Tooltip("Shown after the door is unlocked.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_DoorUnlocked =
        "You have everything you need to leave.";

    [Tooltip("Shown after level is complete.")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_Complete =
        "The truth is no longer trapped here.";

    public string ActivationLabel => label;
    public string ActivationHint => subLabel;
    public bool CanActivate => true;
    public float ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        string hint = GetCurrentHint();
        ActivationDialogUI.ShowText(hint, speakerName);
    }

    private string GetCurrentHint()
    {
        var ps = Level2PuzzleSystem.Instance;

        if (ps == null)
        {
            Debug.LogWarning("[L2HintSystem] Level2PuzzleSystem not found in scene.", this);
            return "Something feels off. I can't focus right now.";
        }

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

        return hint_Start;
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur() { }
}