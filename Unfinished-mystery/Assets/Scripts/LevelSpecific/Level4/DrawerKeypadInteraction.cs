using UnityEngine;

public class DrawerKeypadInteraction : MonoBehaviour, IActivatable
{
    [Header("Keypad")]
    [SerializeField] private GameObject keypadCanvas;

    [Header("Drawer")]
    [SerializeField] private Animator drawerAnimator;

    [Header("Interaction")]
    [SerializeField] private float activationRadius = 2.5f;

    private bool isUnlocked = false;
    private bool isOpen = false;

    public string ActivationLabel
{
    get
    {
        if (!isUnlocked)
            return "Use Keypad";

        if (!isOpen)
            return "Open Drawer";

        return "";
    }
}

    public string ActivationHint => "";
    public bool CanActivate => !isOpen;
    public float ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        if (!isUnlocked)
        {
            keypadCanvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            OpenDrawer();
        }
    }

    public void UnlockDrawer()
    {
        isUnlocked = true;

        if (keypadCanvas != null)
            keypadCanvas.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OpenDrawer()
    {
        isOpen = true;

        if (drawerAnimator != null)
            drawerAnimator.SetTrigger("Open");
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur() { }
}