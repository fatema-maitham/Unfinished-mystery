using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using TMPro;

public class KeypadZoomInteract : MonoBehaviour
{
    [Header("Keypad State")]
    public bool finalUnlock;
    public bool requiresFinalReel = true;
    public bool keypadUnlocked;

    [Header("Player & Controls")]
    public Transform player;
    public MonoBehaviour playerMovementScript;
    // NEW: Drag the script that controls your camera looking/rotation here (e.g., ThirdPersonController or LookScript)
    public MonoBehaviour playerCameraLookScript;

    [Header("Cameras")]
    public CinemachineCamera keypadCamera;
    public CinemachineCamera playerCamera;
    public Camera mainCamera;

    [Header("Interaction Settings")]
    public float interactDistance = 2f;
    public float clickDistance = 100f;

    [Header("Prompt UI")]
    public GameObject interactPrompt;
    public string promptAction = "ENTER";
    public string promptSubLabel = "Keypad";
    public TMP_Text keypadKeyText;

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

        if (interactPrompt != null)
        {
            interactPrompt.SetActive(nearKeypad && !isZoomed);
        }

        if (!isZoomed)
        {
            if (nearKeypad && Keyboard.current != null && Keyboard.current.zKey.wasPressedThisFrame)
                EnterZoom();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

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

        // NEW: Disable the camera look script so clicks don't warp the mouse
        if (playerCameraLookScript != null)
            playerCameraLookScript.enabled = false;

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

        // NEW: Re-enable the camera look script when leaving the keypad
        if (playerCameraLookScript != null)
            playerCameraLookScript.enabled = true;

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