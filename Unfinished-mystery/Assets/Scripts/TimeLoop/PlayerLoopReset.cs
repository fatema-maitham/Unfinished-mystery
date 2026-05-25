using System.Collections;
using UnityEngine;

public class PlayerLoopReset : MonoBehaviour, ILoopResettable
{
    public Transform respawnPoint;

    Rigidbody rb;
    CharacterController characterController;
    Animator animator;

    MonoBehaviour[] movementScripts;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        movementScripts = GetComponents<MonoBehaviour>();
    }

    public void ResetState()
    {
        StartCoroutine(ResetPlayerRoutine());
    }

    IEnumerator ResetPlayerRoutine()
    {
        SetMovementEnabled(false);
        StopWalkingAnimation();

        if (characterController != null)
            characterController.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;

        yield return null;

        StopWalkingAnimation();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;
        }

        if (characterController != null)
            characterController.enabled = true;

        SetMovementEnabled(true);
    }

    public void SetMovementEnabled(bool enabled)
    {
        foreach (MonoBehaviour script in movementScripts)
        {
            if (script != this)
                script.enabled = enabled;
        }

        if (!enabled)
            StopWalkingAnimation();
    }
    

    void StopWalkingAnimation()
    {
        if (animator == null)
            return;

        animator.SetFloat("Speed", 0f);
        animator.SetBool("IsWalking", false);
        animator.Play("Idle");
    }
}