using UnityEngine;

public class BookGlow : MonoBehaviour
{
    [Header("Glow Settings")]
    public Color glowColor = new Color(0.4f, 0.6f, 1f);
    public float minIntensity = 0.5f;
    public float maxIntensity = 2.5f;
    public float pulseFrequency = 1.5f;   // match floatFrequency for sync

    private Material _mat;

    void Start()
    {
        _mat = GetComponent<Renderer>().material;
        _mat.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * pulseFrequency) + 1f) / 2f; // 0–1
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        _mat.SetColor("_EmissionColor", glowColor * intensity);
    }

    void OnDestroy()
    {
        // Clean up instanced material
        if (_mat != null) Destroy(_mat);
    }
}