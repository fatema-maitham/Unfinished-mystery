using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class KeypadZoomInteract : MonoBehaviour
{
    public Transform player;
    public float interactDistance = 2f;

    public CinemachineCamera keypadCamera;
    public CinemachineCamera playerCamera;

    public GameObject promptText;

    private bool isZoomed = false;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        bool near = distance <= interactDistance;

        if (promptText != null)
            promptText.SetActive(near && !isZoomed);

        if (near && !isZoomed)
            EnterKeypadView();

        if (!near && isZoomed)
            ExitKeypadView();

        if (isZoomed && Keyboard.current.escapeKey.wasPressedThisFrame)
            ExitKeypadView();
    }

    void EnterKeypadView()
    {
        isZoomed = true;

        keypadCamera.Priority = 20;
        playerCamera.Priority = 10;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ExitKeypadView()
    {
        isZoomed = false;

        keypadCamera.Priority = 0;
        playerCamera.Priority = 20;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}