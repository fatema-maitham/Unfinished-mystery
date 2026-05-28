using UnityEngine;

public class ActivatableClock : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label = "Check";
    [SerializeField] private string subLabel = "Clock";
    [SerializeField] private float activationRadius = 1.5f;

    [Header("Clock Message")]
    [TextArea(2, 4)]
    [SerializeField] private string clockMessage = "The clock is frozen at 8:10.";

    private bool hasCheckedClock;

    public string ActivationLabel => label;
    public string ActivationHint => subLabel;
    public bool CanActivate => !hasCheckedClock;
    public float ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        if (hasCheckedClock)
            return;

        if (Level2PuzzleSystem.Instance == null)
            return;

        if (!Level2PuzzleSystem.Instance.PhoneMessageHeard)
        {
            Level2PuzzleSystem.ShowBlocked("The clock means nothing yet.");
            return;
        }

        hasCheckedClock = true;

        ActivationDialogUI.ShowText(clockMessage, "Clock");

        Level2PuzzleSystem.Instance.SeeClockClue();
    }

    public void OnActivatableFocus() { }

    public void OnActivatableBlur() { }
}