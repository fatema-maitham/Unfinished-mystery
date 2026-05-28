using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class KeypadZoomInteract : MonoBehaviour
{
    public Transform player;
    public NavKeypad.Keypad keypad;

    public CinemachineCamera keypadCamera;
    public CinemachineCamera playerCamera;
    public Camera mainCamera;

    public float interactDistance = 3f;
    public float clickDistance = 100f;
    public LayerMask keypadButtonLayer = ~0;

    private bool isZoomed;
    private bool exiting;

    private void Start()
    {
        NormalCamera();
    }

    private void Update()
    {
        if (player == null || keypadCamera == null || playerCamera == null)
            return;

        if (!isZoomed)
        {
            float distance = Vector3.Distance(player.position, transform.position);

            if (distance <= interactDistance &&
                Keyboard.current != null &&
                Keyboard.current.zKey.wasPressedThisFrame)
            {
                EnterZoom();
            }

            return;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (!exiting &&
            Keyboard.current != null &&
            (Keyboard.current.zKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame))
        {
            StartCoroutine(ExitZoomSafe());
            return;
        }

        if (!exiting && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ClickKeypadButton();
        }
    }

    private void EnterZoom()
    {
        exiting = false;
        isZoomed = true;

        keypadCamera.Priority = 100;
        playerCamera.Priority = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator ExitZoomSafe()
    {
        exiting = true;
        isZoomed = false;

        if (keypadCamera != null)
            keypadCamera.Priority = 0;

        if (playerCamera != null)
            playerCamera.Priority = 10;

        while (Mouse.current != null && Mouse.current.leftButton.isPressed)
            yield return null;

        yield return null;
        yield return null;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        exiting = false;
    }

    private void NormalCamera()
    {
        isZoomed = false;
        exiting = false;

        if (keypadCamera != null)
            keypadCamera.Priority = 0;

        if (playerCamera != null)
            playerCamera.Priority = 10;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ClickKeypadButton()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null || Mouse.current == null)
            return;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        RaycastHit[] hits = Physics.RaycastAll(
            ray,
            clickDistance,
            keypadButtonLayer,
            QueryTriggerInteraction.Collide
        );

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