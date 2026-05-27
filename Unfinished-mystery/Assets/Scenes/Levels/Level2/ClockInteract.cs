using UnityEngine;

public class ClockInteract : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool playerInside;
    private bool hasCheckedClock;

    private void Update()
    {
        if (!playerInside) return;
        if (hasCheckedClock) return;

        if (Input.GetKeyDown(interactKey))
        {
            CheckClock();
        }
    }

    private void CheckClock()
    {
        if (Level2PuzzleSystem.Instance == null)
            return;

        if (!Level2PuzzleSystem.Instance.PhoneMessageHeard)
        {
            Level2PuzzleSystem.ShowBlocked("The clock means nothing yet.");
            return;
        }

        hasCheckedClock = true;

        ActivationDialogUI.ShowText("The clock is frozen at 8:10.");

        Level2PuzzleSystem.Instance.SeeClockClue();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}