using UnityEngine;

public class KeypadScaleOnApproach : MonoBehaviour
{
    public Transform player;
    public float triggerDistance = 2.0f;

    public Vector3 normalScale = Vector3.one;
    public Vector3 closeScale = new Vector3(1.8f, 1.8f, 1.8f);

    public float scaleSpeed = 6f;

    void Start()
    {
        normalScale = transform.localScale;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        Vector3 targetScale = distance <= triggerDistance ? closeScale : normalScale;

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * scaleSpeed
        );
    }
}