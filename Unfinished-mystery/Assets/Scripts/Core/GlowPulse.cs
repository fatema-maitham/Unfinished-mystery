using UnityEngine;

public class GlowPulse : MonoBehaviour
{
    public float speed = 2f;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    void Update()
    {
        float s = Mathf.Lerp(minScale, maxScale, 
            (Mathf.Sin(Time.time * speed) + 1) / 2);
        transform.localScale = Vector3.one * s;
    }
}
