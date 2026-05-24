using UnityEngine;

public class L3DoorOpen : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform doorToMove;
    public Vector3 openRotation = new Vector3(0f, 90f, 0f);
    public float openSpeed = 2f;

    [Header("Exit White Glow")]
    public GameObject exitWhiteGlowPanel;
    public Light exitWhiteLight;
    public float exitLightIntensity = 15f;

    private bool isOpening = false;
    private Quaternion closedRotation;
    private Quaternion targetRotation;

    private void Start()
    {
        if (doorToMove == null)
            doorToMove = transform;

        closedRotation = doorToMove.rotation;
        targetRotation = closedRotation * Quaternion.Euler(openRotation);

        if (exitWhiteGlowPanel != null)
            exitWhiteGlowPanel.SetActive(false);

        if (exitWhiteLight != null)
            exitWhiteLight.enabled = false;
    }

    private void Update()
    {
        if (!isOpening)
            return;

        doorToMove.rotation = Quaternion.Slerp(
            doorToMove.rotation,
            targetRotation,
            Time.deltaTime * openSpeed
        );
    }

    public void OpenDoor()
    {
        isOpening = true;

        if (exitWhiteGlowPanel != null)
            exitWhiteGlowPanel.SetActive(true);

        if (exitWhiteLight != null)
        {
            exitWhiteLight.enabled = true;
            exitWhiteLight.intensity = exitLightIntensity;
        }
    }
}