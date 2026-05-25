using UnityEngine;

public class ActivatableEvidenceBag : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string lockedLabel = "Unlock";
    [SerializeField] private string subLabel = "Evidence Bag";
    [SerializeField] private float activationRadius = 1.5f;

    [Header("Input")]
    [SerializeField] private KeyCode selectKeySlot = KeyCode.Alpha1;

    [Header("Bag Objects")]
    [SerializeField] private GameObject closedBag;
    [SerializeField] private GameObject openBag;

    [Header("Blocked Message")]
    [SerializeField] private string noKeyMessage = "You need the key to unlock this bag.";

    private bool keySelected;
    private bool opened;

    public string ActivationLabel => opened ? "Opened" : lockedLabel;
    public string ActivationHint => subLabel;
    public bool CanActivate => !opened;
    public float ActivationRadius => activationRadius;

    private void Start()
    {
        if (closedBag != null)
            closedBag.SetActive(true);

        if (openBag != null)
            openBag.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(selectKeySlot))
            keySelected = true;
    }

    public void OnActivate(GameObject source)
    {
        if (opened)
            return;

        if (!keySelected)
        {
            ActivationDialogUI.ShowText(noKeyMessage, "Locked Bag");
            return;
        }

        OpenBag();
    }

    private void OpenBag()
    {
        opened = true;

        if (closedBag != null)
            closedBag.SetActive(false);

        if (openBag != null)
            openBag.SetActive(true);

        Level2PuzzleSystem.Instance?.OpenEvidenceBag();

        Debug.Log("[ActivatableEvidenceBag] Evidence bag opened.");
    }

    public void OnActivatableFocus() { }

    public void OnActivatableBlur() { }
}