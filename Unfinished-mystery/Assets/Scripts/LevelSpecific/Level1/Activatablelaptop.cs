using UnityEngine;

/// <summary>
/// Puzzle 5 — Laptop
/// Blocked until the USB has been collected.
/// Boots on first interaction with USB in inventory.
/// Shows different screens depending on whether the file has been decrypted.
/// Player can return after decryption to see the updated screen.
/// </summary>
public class ActivatableLaptop : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label            = "Examine";
    [SerializeField] private string subLabel         = "Laptop";
    [SerializeField] private float  activationRadius = 1.5f;

    [Header("Screens")]
    [Tooltip("Shown after booting but before file is decrypted — encrypted file hint screen")]
    [SerializeField] private Sprite laptopLockedImage;
    [Tooltip("Shown after the file password is entered — open folder / evidence screen")]
    [SerializeField] private Sprite laptopBootedImage;

    [TextArea(2, 4)]
    [SerializeField] private string bootedText =
        "The laptop boots. A single folder opens:\n\"Kyrell_Flins_Plagiarism_Evidence\"\n\n" +
        "One file is locked: ForYourEyes.txt — password protected.\n\n" +
        "On screen: \"The password is the only honest number you ever taught me. " +
        "The sum of the first n primes, where n = the year you stopped being honest.\"";

    [TextArea(2, 3)]
    [SerializeField] private string decryptedText =
        "The plagiarism evidence is open on screen.\nShe compiled everything. He knew.";

    [Header("Blocked Messages")]
    [SerializeField] private string blockedNoUSB =
        "The laptop won't turn on. It needs something to boot from.";
    [SerializeField] private string blockedNoDrawer =
        "You can't do anything with it yet. You haven't found what makes it work.";

    // ── State ─────────────────────────────────────────────────────────────────
    private bool _booted = false;

    // ── IActivatable ──────────────────────────────────────────────────────────
    public string ActivationLabel  => _booted ? "View Files" : label;
    public string ActivationHint   => subLabel;
    public bool   CanActivate      => true;
    public float  ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        if (_booted)
        {
            ShowLaptopScreen();
            return;
        }

        if (!Level1PuzzleSystem.Instance.DrawerUnlocked)
        {
            Level1PuzzleSystem.ShowBlocked(blockedNoDrawer);
            return;
        }

        if (!Level1PuzzleSystem.Instance.USBFound)
        {
            Level1PuzzleSystem.ShowBlocked(blockedNoUSB);
            return;
        }

        _booted = true;
        Level1PuzzleSystem.Instance?.BootLaptop();
        ShowLaptopScreen();
    }

    private void ShowLaptopScreen()
    {
        if (!Level1PuzzleSystem.Instance.FileDecrypted)
        {
            if (laptopLockedImage != null)
                ActivationDialogUI.ShowImage(laptopLockedImage);
            else
                ActivationDialogUI.ShowText(bootedText, "Laptop");
        }
        else
        {
            if (laptopBootedImage != null)
                ActivationDialogUI.ShowImage(laptopBootedImage);
            else
                ActivationDialogUI.ShowText(decryptedText, "Laptop");
        }
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}