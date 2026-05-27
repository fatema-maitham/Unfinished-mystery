using UnityEngine;
using Unity.Cinemachine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public float horizontalSensitivity = 200f;
    public float verticalSensitivity   = 150f;
    public float minVerticalAngle      = -30f;
    public float maxVerticalAngle      =  60f;

    [Header("References")]
    public Transform cameraFollowTarget;

    private float _xRotation = 0f;
    private float _yRotation = 0f;

    public static ThirdPersonCamera Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Don't touch cursor here — UIStateManager owns it.
        // On game start no UI is open, so it should already be locked.
    }

    void Update()
    {
        // Only rotate camera when no UI panel is open
        if (UIStateManager.Instance != null && !UIStateManager.Instance.IsAnyUIOpen)
            HandleCameraRotation();
    }

    void HandleCameraRotation()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        float mouseX = Input.GetAxis("Mouse X") * horizontalSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * verticalSensitivity   * Time.deltaTime;

        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation  = Mathf.Clamp(_xRotation, minVerticalAngle, maxVerticalAngle);

        cameraFollowTarget.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
    }

    // Called by PauseMenuController after resume coroutine finishes
    public void ExitUIMode()
    {
        // Cursor already handled by UIStateManager; nothing extra needed.
    }

    // Legacy helpers kept for any other callers
    public void EnterUIMode() { }
}