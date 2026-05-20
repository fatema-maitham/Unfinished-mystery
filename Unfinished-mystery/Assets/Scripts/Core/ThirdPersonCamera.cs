using UnityEngine;
using Unity.Cinemachine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public float horizontalSensitivity = 200f;
    public float verticalSensitivity = 150f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;

    [Header("References")]
    public Transform cameraFollowTarget;

    private float _xRotation = 0f;
    private float _yRotation = 0f;

    // Static so other scripts (UI buttons) can call it easily
    public static ThirdPersonCamera Instance { get; private set; }

    private bool _uiMode = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LockCursor();
    }

    void Update()
    {
        if (!_uiMode)
            HandleCameraRotation();

        HandleCursorToggle();
    }

    void HandleCameraRotation()
    {
        // Don't rotate camera if cursor is unlocked (notebook open, etc.)
        if (Cursor.lockState != CursorLockMode.Locked) return;

        float mouseX = Input.GetAxis("Mouse X") * horizontalSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * verticalSensitivity * Time.deltaTime;

        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, minVerticalAngle, maxVerticalAngle);

        cameraFollowTarget.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
    }
    void HandleCursorToggle()
    {
        // Press Escape to toggle UI mode
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_uiMode)
                ExitUIMode();
            else
                EnterUIMode();
        }
    }

    public void EnterUIMode()
    {
        _uiMode = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitUIMode()
    {
        _uiMode = false;
        LockCursor();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}