using UnityEngine;

public class PlayerLoopReset : MonoBehaviour, ILoopResettable
{
    Vector3 startPosition;
    Quaternion startRotation;

    Rigidbody rb;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
    }

    public void ResetState()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}