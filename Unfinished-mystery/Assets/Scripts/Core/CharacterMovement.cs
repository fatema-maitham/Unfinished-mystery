using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed     = 1.5f;
    public float runSpeed      = 4f;
    public float jumpHeight    = 1f;
    public float gravity       = -20f;
    public float rotationSpeed = 10f;

    [Header("Camera")]
    public Transform cameraFollowTarget;

    [Header("Ground Detection")]
    public LayerMask groundMask;

    [Header("Footstep Sound")]
    public AudioSource footstepAudioSource;

    private CharacterController controller;
    private Animator             animator;
    private float                verticalVelocity;

    private PlayerControls controls;
    private Vector2        moveInput;
    private bool           jumpPressed;
    private bool           isRunning;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput   = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled  += ctx => moveInput   = Vector2.zero;
        controls.Player.Jump.performed += ctx => jumpPressed = true;
        controls.Player.Run.performed  += ctx => isRunning   = true;
        controls.Player.Run.canceled   += ctx => isRunning   = false;
    }

    void OnEnable()
    {
        controls.Enable();

        // Subscribe to UI state changes so we can zero input immediately
        if (UIStateManager.Instance != null)
        {
            UIStateManager.Instance.OnPauseStateChanged    += OnUIStateChanged;
            UIStateManager.Instance.OnNotebookStateChanged += OnUIStateChanged;
        }
    }

    void OnDisable()
    {
        controls.Disable();

        if (UIStateManager.Instance != null)
        {
            UIStateManager.Instance.OnPauseStateChanged    -= OnUIStateChanged;
            UIStateManager.Instance.OnNotebookStateChanged -= OnUIStateChanged;
        }
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator   = GetComponent<Animator>();
        animator.applyRootMotion = false;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        ApplyGravity();
        jumpPressed = false;
    }

    // ── UI State Callback ─────────────────────────────────────────────────────

    /// Called whenever pause OR notebook opens/closes.
    /// Immediately zeroes movement so the character stops.
    private void OnUIStateChanged(bool isOpen)
    {
        if (isOpen)
        {
            moveInput   = Vector2.zero;
            isRunning   = false;
            jumpPressed = false;
            StopFootsteps();
            animator.SetFloat("Speed", 0f);
        }
    }

    // ── Movement ──────────────────────────────────────────────────────────────

    void HandleMovement()
    {
        // Block all movement while any UI is open
        if (UIStateManager.Instance != null && UIStateManager.Instance.IsAnyUIOpen)
        {
            StopFootsteps();
            animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
            return;
        }

        float      cameraYaw    = cameraFollowTarget.eulerAngles.y;
        Quaternion cameraYawRot = Quaternion.Euler(0, cameraYaw, 0);
        Vector3    moveDir      = (cameraYawRot * new Vector3(moveInput.x, 0, moveInput.y)).normalized;

        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        float animSpeed    = 0f;

        if (moveDir.magnitude >= 0.1f && controller.isGrounded)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(moveDir),
                rotationSpeed * Time.deltaTime
            );

            controller.Move(moveDir * currentSpeed * Time.deltaTime);
            animSpeed = isRunning ? 1f : 0.5f;

            if (footstepAudioSource != null && !footstepAudioSource.isPlaying)
                footstepAudioSource.Play();
        }
        else
        {
            StopFootsteps();
        }

        animator.SetFloat("Speed", animSpeed, 0.1f, Time.deltaTime);
    }

    void HandleJump()
    {
        if (UIStateManager.Instance != null && UIStateManager.Instance.IsAnyUIOpen)
            return;

        if (jumpPressed && controller.isGrounded)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;
        else
            verticalVelocity += gravity * Time.deltaTime;

        controller.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    void StopFootsteps()
    {
        if (footstepAudioSource != null && footstepAudioSource.isPlaying)
            footstepAudioSource.Stop();
    }
}