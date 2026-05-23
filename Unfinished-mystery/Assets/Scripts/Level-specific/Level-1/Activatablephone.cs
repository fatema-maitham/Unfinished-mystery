using UnityEngine;

/// <summary>
/// Puzzle 7 — The Phone (Final Step)
/// The prompt does NOT appear until the file is decrypted — CanActivate
/// reads FileDecrypted from the puzzle manager every frame.
/// The phoneGlowVFX (your StarBlink prefab) activates at the same moment.
/// Sending the evidence fires CompleteLevel() and ends the loop.
/// </summary>
public class ActivatablePhone : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label            = "Send Evidence";
    [SerializeField] private string subLabel         = "Phone";
    [SerializeField] private float  activationRadius = 1.5f;

    [Header("Content")]
    [Tooltip("Optional image of the phone screen — Send Lynnette evidence to the university board")]
    [SerializeField] private Sprite phoneScreenImage;
    [TextArea(2, 4)]
    [SerializeField] private string completionText =
        "You forward the plagiarism evidence to the university board.\n\n" +
        "Loop broken.\n\nLevel complete.";

    [Header("Glow VFX")]
    [Tooltip("Assign your StarBlink prefab here — it enables when the phone becomes active")]
    [SerializeField] private GameObject phoneGlowVFX;

    // ── State ─────────────────────────────────────────────────────────────────
    private bool _sent = false;

    // ── IActivatable ──────────────────────────────────────────────────────────
    public string ActivationLabel  => label;
    public string ActivationHint   => subLabel;

    // No prompt appears at all until the file is decrypted
    public bool   CanActivate      => Level1PuzzleSystem.Instance != null
                                   && Level1PuzzleSystem.Instance.FileDecrypted;
    public float  ActivationRadius => activationRadius;

    // ── Unity ─────────────────────────────────────────────────────────────────
    private void Start()
    {
        // Make sure glow is off at start
        if (phoneGlowVFX != null)
            phoneGlowVFX.SetActive(false);
    }

    private void Update()
    {
        // Sync glow VFX with phone unlock state
        if (phoneGlowVFX != null)
        {
            bool shouldGlow = Level1PuzzleSystem.Instance != null
                           && Level1PuzzleSystem.Instance.FileDecrypted;
            if (phoneGlowVFX.activeSelf != shouldGlow)
                phoneGlowVFX.SetActive(shouldGlow);
        }
    }

    public void OnActivate(GameObject source)
    {
        if (_sent)
        {
            ActivationDialogUI.ShowText("The evidence has already been sent.", "Phone");
            return;
        }

        _sent = true;
        Level1PuzzleSystem.Instance?.CompleteLevel();

        if (phoneScreenImage != null)
            ActivationDialogUI.ShowImage(phoneScreenImage);
        else
            ActivationDialogUI.ShowText(completionText, "Phone");
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}