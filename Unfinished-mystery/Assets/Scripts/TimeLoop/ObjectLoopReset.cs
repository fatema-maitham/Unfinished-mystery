using UnityEngine;

public class ObjectLoopReset : MonoBehaviour, ILoopResettable
{
    private Vector3 startLocalPosition;
    private Quaternion startLocalRotation;
    private Vector3 startLocalScale;
    private bool startActive;

    private Rigidbody rb;
    private Animator animator;

    void Awake()
    {
        SaveStartState();
    }

    private void SaveStartState()
    {
        startLocalPosition = transform.localPosition;
        startLocalRotation = transform.localRotation;
        startLocalScale = transform.localScale;
        startActive = gameObject.activeSelf;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public void ResetState()
    {
        gameObject.SetActive(startActive);

        transform.localPosition = startLocalPosition;
        transform.localRotation = startLocalRotation;
        transform.localScale = startLocalScale;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
        }

        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }
    }
}