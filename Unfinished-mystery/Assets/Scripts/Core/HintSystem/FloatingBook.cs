using UnityEngine;

public class FloatingBook : MonoBehaviour
{
    [Header("Float Settings")]
    public float floatAmplitude = 0.1f;   // how high it bobs
    public float floatFrequency = 1.5f;   // speed of bobbing

    [Header("Rotation Settings")]
    public bool enableRotation = true;
    public float rotationSpeed = 20f;     // degrees per second

    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        // Sine wave on Y axis
        float newY = _startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(_startPos.x, newY, _startPos.z);

        // Optional slow spin
        if (enableRotation)
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}