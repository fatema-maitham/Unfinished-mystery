using UnityEngine;

/// <summary>
/// Updated Puzzle 5 — Laptop
/// Now opens LaptopScreenUI (full desktop UI) instead of ActivationDialogUI.
/// Replace your existing ActivatableLaptop.cs with this file.
/// </summary>
public class ActivatableLaptop : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label            = "Examine";
    [SerializeField] private string subLabel         = "Laptop";
    [SerializeField] private float  activationRadius = 1.5f;

    [Header("Blocked Messages")]
    [SerializeField] private string blockedNoUSB =
        "The laptop won't turn on. It needs something to boot from.";
    [SerializeField] private string blockedNoDrawer =
        "You can't do anything with it yet. You haven't found what makes it work.";

    private bool _booted = false;

    public string ActivationLabel  => _booted ? "Use Laptop" : label;
    public string ActivationHint   => subLabel;
    public bool   CanActivate      => true;
    public float  ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        if (_booted)
        {
            OpenLaptopScreen();
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
        OpenLaptopScreen();
    }

    private void OpenLaptopScreen()
    {
        if (LaptopScreenUI.Instance != null)
            LaptopScreenUI.Instance.Open();
        else
            Debug.LogWarning("[ActivatableLaptop] LaptopScreenUI not found in scene.", this);
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}