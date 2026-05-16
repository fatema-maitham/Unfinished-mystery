using UnityEngine;

public class ObjectLoopReset : MonoBehaviour, ILoopResettable
{
    Vector3 startPosition;
    Quaternion startRotation;
    Vector3 startScale;

    Rigidbody rb;

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;

        rb = GetComponent<Rigidbody>();
    }

    public void ResetState()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        transform.localScale = startScale;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        gameObject.SetActive(true);
    }
}