using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class KeypadZoomInteract : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    public MonoBehaviour playerMovementScript;

    [Header("Cameras")]
    public CinemachineCamera keypadCamera;
    public CinemachineCamera playerCamera;
    public Camera mainCamera;

    [Header("Interaction")]
    public float interactDistance = 2f;
    public float clickDistance = 100f;

    private bool isZoomed;

    private void Start()
    {
        if (keypadCamera != null)
            keypadCamera.Priority = 0;

        if (playerCamera != null)
            playerCamera.Priority = 10;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (player == null || keypadCamera == null || playerCamera == null || mainCamera == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);
        bool nearKeypad = distance <= interactDistance;

        if (!isZoomed)
        {
            if (nearKeypad && Keyboard.current != null && Keyboard.current.zKey.wasPressedThisFrame)
                EnterZoom();
        }
        else
        {
            if (Keyboard.current != null && Keyboard.current.zKey.wasPressedThisFrame)
                ExitZoom();

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                ClickKeypadButton();
        }
    }

    private void EnterZoom()
    {
        isZoomed = true;

        keypadCamera.Priority = 100;
        playerCamera.Priority = 0;

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ExitZoom()
    {
        isZoomed = false;

        keypadCamera.Priority = 0;
        playerCamera.Priority = 10;

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ClickKeypadButton()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit[] hits = Physics.RaycastAll(ray, clickDistance, ~0, QueryTriggerInteraction.Collide);

        if (hits.Length == 0)
            return;

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            NavKeypad.KeypadButton button = hit.collider.GetComponent<NavKeypad.KeypadButton>();

            if (button == null)
                button = hit.collider.GetComponentInParent<NavKeypad.KeypadButton>();

            if (button != null)
            {
                button.PressButton();
                return;
            }
        }
    }
}