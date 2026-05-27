using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using TMPro;

public class L3KeypadZoomInteract : MonoBehaviour
{
    [Header("Final Unlock")]
    public bool keypadUnlocked = false;

    [Header("Player")]
    public Transform player;
    public MonoBehaviour playerMovementScript;
    public ThirdPersonCamera thirdPersonCamera;

    [Header("Cameras")]
    public CinemachineCamera keypadCamera;
    public CinemachineCamera playerCamera;
    public Camera mainCamera;

    [Header("Interaction")]
    public float interactDistance = 3f;
    public float clickDistance = 100f;

    [Header("Prompt")]
    public InteractionPromptUI interactPrompt;
    public string promptAction = "ENTER";
    public string promptSubLabel = "Keypad";
    public TMP_Text keypadKeyText;

    [Header("Evidence Board")]
    [SerializeField] private L3EvidenceBoardUI evidenceBoardUI;

    private bool isZoomed = false;
    private bool keypadPromptShowing = false;
    private bool pendingEvidenceBoard = false;

    private void Start()
    {
        keypadUnlocked = false;
        keypadPromptShowing = false;

        if (keypadCamera != null)
            keypadCamera.Priority = 0;

        if (playerCamera != null)
            playerCamera.Priority = 10;
    }

    private void Update()
    {
        if (player == null || keypadCamera == null || playerCamera == null || mainCamera == null)
            return;

        if (!keypadUnlocked)
            return;

        float distance = Vector3.Distance(player.position, transform.position);
        bool canInteract = distance <= interactDistance;

        if (!isZoomed)
        {
            if (canInteract)
            {
                if (interactPrompt != null)
                {
                    interactPrompt.ShowPrompt(promptAction, promptSubLabel);

                    if (keypadKeyText != null)
                        keypadKeyText.text = "Z";

                    keypadPromptShowing = true;
                }
            }
            else
            {
                HidePrompt();
            }
        }

        if (canInteract && Keyboard.current != null && Keyboard.current.zKey.wasPressedThisFrame)
            ToggleZoom();

        if (isZoomed && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            ClickKeypadButton();
    }

    private void LateUpdate()
    {
        if (!isZoomed)
            return;

        if (Time.timeScale == 0f)
            return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ToggleZoom()
    {
        isZoomed = !isZoomed;

        keypadCamera.Priority = isZoomed ? 100 : 0;
        playerCamera.Priority = isZoomed ? 0 : 10;

        if (playerMovementScript != null)
            playerMovementScript.enabled = !isZoomed;

        if (isZoomed)
        {
            if (thirdPersonCamera != null)
                thirdPersonCamera.EnterUIMode();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            HidePrompt();
        }
        else
        {
            if (thirdPersonCamera != null)
                thirdPersonCamera.ExitUIMode();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (pendingEvidenceBoard && evidenceBoardUI != null)
            {
                pendingEvidenceBoard = false;
                evidenceBoardUI.ShowEvidenceBoard();
            }
        }
    }

    private void HidePrompt()
    {
        if (interactPrompt != null && keypadPromptShowing)
        {
            interactPrompt.HidePrompt();
            keypadPromptShowing = false;
        }
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

    public void UnlockKeypadPrompt()
    {
        keypadUnlocked = true;
        Debug.Log("Level 3 keypad prompt unlocked after Reel 3.");
    }

    public void TriggerEvidenceBoard()
    {
        Debug.Log("EVIDENCE BOARD REQUESTED");
        pendingEvidenceBoard = true;
    }
}