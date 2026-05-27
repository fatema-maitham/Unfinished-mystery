using UnityEngine;

public class L1hintSystem : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label            = "Ask for Hint";
    [SerializeField] private string subLabel         = "Hint";
    [SerializeField] private float  activationRadius = 2f;

    [Header("Speaker")]
    [Tooltip("Name shown above the hint text. e.g. Magic Ball, Helper, Inner Voice")]
    [SerializeField] private string speakerName = "Inner Voice";

    [Header("Hints — shown in order as puzzles are completed")]
    [Tooltip("Shown at the very start before anything is examined")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_Start =
        "You wake up slumped at your desk. Something feels wrong. Look around — the desk holds more than it seems.";

    [Tooltip("Shown after desk phase is complete, before bookshelf")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_DeskDone =
        "The memo, the exam, the note under the mug — it all points to something hidden in this room. Check the bookshelf.";

    [Tooltip("Shown after bookshelf phase is complete, before drawer")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_BookshelfDone =
        "WHEN YOU LIE, NUMBERS REMEMBER. Count the letters in each word. That's your combination.";

    [Tooltip("Shown after drawer is unlocked, before USB is picked up")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_DrawerUnlocked =
        "The drawer is open. There's something inside — pick it up. It might be the key to the laptop.";

    [Tooltip("Shown after USB is collected, before laptop is booted")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_USBFound =
        "You have the drive. Plug it into the laptop on the desk.";

    [Tooltip("Shown after laptop is booted, before file is decrypted")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_LaptopBooted =
        "One file is locked. The password is a number — honest, mathematical. " +
        "Think about what n means. The book told you. The dedication told you.";

    [Tooltip("Shown after file is decrypted, before sending evidence")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_FileDecrypted =
        "You have everything. The phone on the desk just lit up. It's time to make this right.";

    [Tooltip("Shown after level is complete")]
    [TextArea(2, 4)]
    [SerializeField] private string hint_Complete =
        "The loop is broken. The evidence is out there. Whatever comes next — you chose honesty.";

    // ── IActivatable ──────────────────────────────────────────────────────────
    public string ActivationLabel  => label;
    public string ActivationHint   => subLabel;
    public bool   CanActivate      => true;
    public float  ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        string hint = GetCurrentHint();
        ActivationDialogUI.ShowText(hint, speakerName);
    }

    // ── Hint Logic ────────────────────────────────────────────────────────────
    private string GetCurrentHint()
    {
        var ps = Level1PuzzleSystem.Instance;

        if (ps == null)
        {
            Debug.LogWarning("[HintSystem] Level1PuzzleSystem not found in scene.", this);
            return "Something feels off. I can't think straight right now.";
        }

        if (ps.LevelComplete)    return hint_Complete;
        if (ps.FileDecrypted)    return hint_FileDecrypted;
        if (ps.LaptopBooted)     return hint_LaptopBooted;
        if (ps.USBFound)         return hint_USBFound;
        if (ps.DrawerUnlocked)   return hint_DrawerUnlocked;
        if (ps.BookshelfPhaseComplete) return hint_BookshelfDone;
        if (ps.DeskPhaseComplete)      return hint_DeskDone;

        return hint_Start;
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}