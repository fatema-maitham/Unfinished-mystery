using UnityEngine;

public class FloatingBookOrganic : MonoBehaviour
{
    [Header("Primary Float")]
    public float primaryAmplitude = 0.1f;
    public float primaryFrequency = 1.5f;

    [Header("Secondary Float (organic feel)")]
    public float secondaryAmplitude = 0.02f;
    public float secondaryFrequency = 2.3f;   

    [Header("Rotation")]
    public bool enableRotation = true;
    public float rotationSpeed = 20f;

    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        float primary   = Mathf.Sin(Time.time * primaryFrequency)   * primaryAmplitude;
        float secondary = Mathf.Sin(Time.time * secondaryFrequency) * secondaryAmplitude;

        transform.position = new Vector3(
            _startPos.x + secondary,   // subtle side drift
            _startPos.y + primary,     // main bob
            _startPos.z
        );

        if (enableRotation)
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}