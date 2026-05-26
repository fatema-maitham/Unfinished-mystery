using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using TMPro;
public class KeypadZoomInteract : MonoBehaviour
{

[Header("Final Unlock")]
public bool requiresFinalReel = true;
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
    private bool isZoomed = false;
    private bool keypadPromptShowing = false;
private void Start()
{
    requiresFinalReel = true;
    keypadUnlocked = false;
    keypadPromptShowing = false;
}

//     private void Update()
//     {
//         if (player == null || keypadCamera == null || playerCamera == null || mainCamera == null)
//             return;

//         float distance = Vector3.Distance(player.position, transform.position);
//         bool canInteract = distance <= interactDistance;

//         if (!keypadUnlocked)
//             return;


//                 if (isZoomed)
//         {
//             if (thirdPersonCamera != null)
//                 thirdPersonCamera.enabled = false;

//             Cursor.lockState = CursorLockMode.None;
//             Cursor.visible = true;

//             if (interactPrompt != null && keypadPromptShowing)
//             {
//                 interactPrompt.HidePrompt();
//                 keypadPromptShowing = false;
//             }
//         }
//         else
//         {
//             if (thirdPersonCamera != null)
//             {
//                 thirdPersonCamera.enabled = true;
//                 thirdPersonCamera.ExitUIMode();
//             }

//             Cursor.lockState = CursorLockMode.Locked;
//             Cursor.visible = false;
//         }

//         if (!isZoomed)
//         {
//             if (canInteract)
//             {
//                 if (interactPrompt != null)
// {
//     interactPrompt.ShowPrompt(promptAction, promptSubLabel);

//     if (keypadKeyText != null)
//         keypadKeyText.text = "Z";

//     keypadPromptShowing = true;
// }
//             }
//             else
//             {
//                 if (interactPrompt != null && keypadPromptShowing)
//                 {
//                     interactPrompt.HidePrompt();
//                     keypadPromptShowing = false;
//                 }
//             }
//         }

//         if (canInteract && Keyboard.current.zKey.wasPressedThisFrame)
//             ToggleZoom();

//         if (isZoomed && Mouse.current.leftButton.wasPressedThisFrame)
//             ClickKeypadButton();
//     }

    // private void ToggleZoom()
    // {
    //     isZoomed = !isZoomed;

    //     keypadCamera.Priority = isZoomed ? 100 : 0;
    //     playerCamera.Priority = isZoomed ? 0 : 10;

    //     if (playerMovementScript != null)
    //         playerMovementScript.enabled = !isZoomed;

    //     if (isZoomed)
    //     {
    //         Cursor.lockState = CursorLockMode.None;
    //         Cursor.visible = true;

    //         if (interactPrompt != null && keypadPromptShowing)
    //         {
    //             interactPrompt.HidePrompt();
    //             keypadPromptShowing = false;
    //         }
    //     }
    //     else
    //     {
    //         Cursor.lockState = CursorLockMode.Locked;
    //         Cursor.visible = false;
    //     }
    // }


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
            if (interactPrompt != null && keypadPromptShowing)
            {
                interactPrompt.HidePrompt();
                keypadPromptShowing = false;
            }
        }
    }

    if (canInteract && Keyboard.current.zKey.wasPressedThisFrame)
    {
        ToggleZoom();
    }

    if (isZoomed && Mouse.current.leftButton.wasPressedThisFrame)
    {
        ClickKeypadButton();
    }
}

private void LateUpdate()
{
    if (!isZoomed)
        return;

    if (Time.timeScale == 0f)
        return;

    if (Cursor.lockState != CursorLockMode.None)
        Cursor.lockState = CursorLockMode.None;

    if (!Cursor.visible)
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

        if (interactPrompt != null && keypadPromptShowing)
        {
            interactPrompt.HidePrompt();
            keypadPromptShowing = false;
        }
    }
    else
            {
                if (thirdPersonCamera != null)
                    thirdPersonCamera.ExitUIMode();

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
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
    Debug.Log("Keypad prompt unlocked after Reel 3.");
}
}