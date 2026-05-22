using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class KeypadZoomInteract : MonoBehaviour
{
    public Transform player;
    public MonoBehaviour playerMovementScript;

    public CinemachineCamera keypadCamera;
    public CinemachineCamera playerCamera;
    public Camera mainCamera;

    public float interactDistance = 20f;
    public float clickDistance = 100f;

    private bool isZoomed = false;

    void Update()
    {
        if (player == null || keypadCamera == null || playerCamera == null || mainCamera == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance && Keyboard.current.zKey.wasPressedThisFrame)
        {
            isZoomed = !isZoomed;

            keypadCamera.Priority = isZoomed ? 100 : 0;
            playerCamera.Priority = isZoomed ? 0 : 10;

            if (playerMovementScript != null)
                playerMovementScript.enabled = !isZoomed;

            if (isZoomed)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            Debug.Log("Zoom mode = " + isZoomed);
        }

        if (isZoomed && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ClickKeypadButton();
        }
    }

    void ClickKeypadButton()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        RaycastHit[] hits = Physics.RaycastAll(ray, clickDistance, ~0, QueryTriggerInteraction.Collide);

        if (hits.Length == 0)
        {
            Debug.Log("Raycast hit nothing");
            return;
        }

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            Debug.Log("Raycast hit: " + hit.collider.name);

            NavKeypad.KeypadButton button = hit.collider.GetComponent<NavKeypad.KeypadButton>();

            if (button == null)
                button = hit.collider.GetComponentInParent<NavKeypad.KeypadButton>();

            if (button != null)
            {
                Debug.Log("Pressed keypad button: " + button.name);
                button.PressButton();
                return;
            }
        }

        Debug.Log("No KeypadButton found in raycast hits");
    }
}