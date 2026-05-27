using UnityEngine;

public class FinalDoorInteract : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [SerializeField] private DoorOpenSimple doorOpenSimple;

    private bool doorOpened;

    private void Update()
    {
        if (player == null || doorOpenSimple == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance > interactDistance)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            TryOpenDoor();
        }
    }

    private void TryOpenDoor()
    {
        if (doorOpened)
            return;

        if (Level2PuzzleSystem.Instance == null)
            return;

        if (!Level2PuzzleSystem.Instance.DoorUnlocked)
        {
            Level2PuzzleSystem.ShowBlocked("The door is still locked.");
            return;
        }

        doorOpened = true;
        doorOpenSimple.OpenDoor();
    }
}