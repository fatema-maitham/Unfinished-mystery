using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class KeypadZoomInteract : MonoBehaviour
{
    public Transform player;
    public MonoBehaviour playerMovementScript;

    public CinemachineCamera keypadCamera;
    public CinemachineCamera playerCamera;

    public float interactDistance = 3f;

    private bool isZoomed = false;

    void Update()
    {
        Debug.Log("KeypadZoomInteract running");

        if (player == null)
        {
            Debug.LogError("Player is missing");
            return;
        }

        if (keypadCamera == null)
        {
            Debug.LogError("Keypad Camera is missing");
            return;
        }

        if (playerCamera == null)
        {
            Debug.LogError("Player Camera is missing");
            return;
        }

        float distance = Vector3.Distance(player.position, transform.position);
        Debug.Log("Distance to keypad = " + distance);

        if (distance <= interactDistance)
        {
            Debug.Log("Player is near keypad");

            if (Keyboard.current.zKey.wasPressedThisFrame)
            {
                Debug.Log("Z pressed. Switching camera.");

                isZoomed = !isZoomed;

                keypadCamera.Priority = isZoomed ? 20 : 0;
                playerCamera.Priority = isZoomed ? 0 : 10;

                if (playerMovementScript != null)
                {
                    playerMovementScript.enabled = !isZoomed;
                    Debug.Log("Player movement enabled = " + playerMovementScript.enabled);
                }
            }
        }
    }
}