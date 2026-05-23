using UnityEngine;
using InventoryFramework;

// ═══════════════════════════════════════════════════════════════════════════════
// PUZZLE 4 — USB Drive
// Appears inside the drawer after it's unlocked.
// Player picks it up — it goes into their inventory via the existing system.
// Uses IActivatable so it fits the interaction prompt.
// ═══════════════════════════════════════════════════════════════════════════════
public class ActivatableUSB : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label         = "Pick Up";
    [SerializeField] private string subLabel      = "USB — N.O. Final Submission";
    [SerializeField] private float  activationRadius = 1.2f;

    [Header("Inventory")]
    [Tooltip("Assign the USB Item asset from your Project window")]
    [SerializeField] private Item usbItem;
    [SerializeField] private int  amount = 1;

    [Header("Blocked Message")]
    [SerializeField] private string blockedMessage =
        "The drawer is locked. Find the combination first.";

    private bool _collected = false;

    public string ActivationLabel  => label;
    public string ActivationHint   => subLabel;
    public bool   CanActivate      => !_collected;
    public float  ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        if (!Level1PuzzleSystem.Instance.DrawerUnlocked)
        {
            Level1PuzzleSystem.ShowBlocked(blockedMessage);
            return;
        }

        if (_collected) return;

        // Add to inventory using the existing ItemPickupHandler on the player
        var handler = source.GetComponent<ItemPickupHandler>();
        if (handler != null && usbItem != null)
        {
            handler.PickupItem(usbItem, amount);
        }

        _collected = true;
        Level1PuzzleSystem.Instance?.CollectUSB();

        ActivationDialogUI.ShowText(
            "You picked up the USB drive.\nMaybe it'll boot the laptop.",
            "USB Drive");

        // Hide the object after pickup
        gameObject.SetActive(false);
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}


// ═══════════════════════════════════════════════════════════════════════════════
// PUZZLE 5 — Laptop
// Blocked until USB is in inventory.
// Player submits the USB item to boot the laptop.
// ═══════════════════════════════════════════════════════════════════════════════
public class ActivatableLaptop : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label         = "Examine";
    [SerializeField] private string subLabel      = "Laptop";
    [SerializeField] private float  activationRadius = 1.5f;

    [Header("Inventory Check")]
    [Tooltip("The USB Item asset — must match what ActivatableUSB uses")]
    [SerializeField] private Item usbItem;

    [Header("Content")]
    [SerializeField] private Sprite laptopBootedImage;   // folder contents screen
    [SerializeField] private Sprite laptopLockedImage;   // encrypted file screen
    [TextArea(2, 4)]
    [SerializeField] private string bootedText =
        "The laptop boots. A single folder opens:\n\"Kyrell_Flins_Plagiarism_Evidence\"\n\n" +
        "She compiled everything. One file is locked: ForYourEyes.txt — password protected.\n" +
        "Hint on screen: \"The password is the only honest number you ever taught me.\"";

    [Header("Blocked Messages")]
    [SerializeField] private string blockedNoUSB =
        "The laptop won't turn on. It needs something to boot from.";
    [SerializeField] private string blockedNoDrawer =
        "You can't open it yet. You haven't found what makes it work.";

    private bool _booted = false;

    public string ActivationLabel  => _booted ? "View Files" : label;
    public string ActivationHint   => subLabel;
    public bool   CanActivate      => true;
    public float  ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        // Already booted — go straight to file puzzle
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

        // Boot the laptop
        _booted = true;
        Level1PuzzleSystem.Instance?.BootLaptop();
        ShowLaptopScreen();
    }

    private void ShowLaptopScreen()
    {
        if (!Level1PuzzleSystem.Instance.FileDecrypted)
        {
            // Show locked file state
            if (laptopLockedImage != null)
                ActivationDialogUI.ShowImage(laptopLockedImage);
            else
                ActivationDialogUI.ShowText(bootedText, "Laptop");
        }
        else
        {
            // File already decrypted — show evidence
            if (laptopBootedImage != null)
                ActivationDialogUI.ShowImage(laptopBootedImage);
            else
                ActivationDialogUI.ShowText("The plagiarism evidence is open on screen.", "Laptop");
        }
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}