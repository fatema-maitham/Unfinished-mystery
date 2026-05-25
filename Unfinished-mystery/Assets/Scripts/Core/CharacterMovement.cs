using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 4f;
    public float jumpHeight = 1f;
    public float gravity = -20f;
    public float rotationSpeed = 10f;

    [Header("Camera")]
    public Transform cameraFollowTarget;

    [Header("Ground Detection")]
    public LayerMask groundMask;


    [Header("Footstep Sound")]
    public AudioSource footstepAudioSource;

    private CharacterController controller;
    private Animator animator;
    private float verticalVelocity;

    private PlayerControls controls;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool isRunning;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled  += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => jumpPressed = true;
        controls.Player.Run.performed  += ctx => isRunning = true;
        controls.Player.Run.canceled   += ctx => isRunning = false;
    }

    void OnEnable()  => controls.Enable();
    void OnDisable() => controls.Disable();

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator   = GetComponent<Animator>();

        // Disable root motion so Mixamo animations don't fight the CharacterController
        animator.applyRootMotion = false;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        ApplyGravity();
        jumpPressed = false;
    }

    void HandleMovement()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            StopFootsteps();
            return;
        }

        float cameraYaw = cameraFollowTarget.eulerAngles.y;
        Quaternion cameraYawRotation = Quaternion.Euler(0, cameraYaw, 0);
        Vector3 moveDir = (cameraYawRotation * new Vector3(moveInput.x, 0, moveInput.y)).normalized;

        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        float animSpeed = 0f;

        if (moveDir.magnitude >= 0.1f && controller.isGrounded)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
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