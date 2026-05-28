using UnityEngine;

public class DoorOpenSimple : MonoBehaviour
{
    public float openAngle = -90f;
    public float speed = 3f;

    private bool open = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            open ? openRotation : closedRotation,
            Time.deltaTime * speed
        );
    }

    public void OpenDoor()
    {
        open = true;
    }
}