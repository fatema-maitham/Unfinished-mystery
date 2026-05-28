using UnityEngine;

public class Level4KeyReset : MonoBehaviour, ILoopResettable
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;

    private void Awake()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        startScale = transform.localScale;
    }

    public void ResetState()
    {
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
        transform.localScale = startScale;

        gameObject.SetActive(true);
    }
}