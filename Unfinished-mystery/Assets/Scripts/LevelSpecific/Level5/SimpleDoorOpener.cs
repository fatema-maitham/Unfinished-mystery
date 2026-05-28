using System.Collections;
using UnityEngine;

public class SimpleDoorOpener : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector3 openOffset = new Vector3(0, -3f, 0); // Slides down by default
    [SerializeField] private float speed = 2f;

    private Vector3 closedPosition;
    private Vector3 targetPosition;
    private bool isOpening = false;

    void Start()
    {
        // save the exact spot the door is currently sitting at
        closedPosition = transform.localPosition;
        // calculate where it should go when opened
        targetPosition = closedPosition + openOffset;
    }

    // this is the public function the Keypad will trigger
    public void OpenDoor()
    {
        if (!isOpening)
        {
            StartCoroutine(SlideDoorRoutine());
        }
    }

    private IEnumerator SlideDoorRoutine()
    {
        isOpening = true;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            transform.localPosition = Vector3.Lerp(closedPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }

        // snap precisely to final open position
        transform.localPosition = targetPosition;
    }
}