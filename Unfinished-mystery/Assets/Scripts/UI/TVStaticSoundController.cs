using UnityEngine;

public class TVStaticSoundController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource staticAudioSource;

    [Header("Light")]
    public Light staticLight;

    private bool permanentlyStopped = false;

    private void Awake()
    {
        if (staticAudioSource == null)
            staticAudioSource = GetComponent<AudioSource>();

        StopStatic();
    }

    private void Start()
    {
        StopStatic();
    }

    public void StartStatic()
    {
        if (permanentlyStopped)
            return;

        if (staticAudioSource != null && !staticAudioSource.isPlaying)
            staticAudioSource.Play();

        if (staticLight != null)
            staticLight.enabled = true;
    }

    public void StopStatic()
    {
        if (staticAudioSource != null)
            staticAudioSource.Stop();

        if (staticLight != null)
            staticLight.enabled = false;
    }

    public void StopStaticPermanently()
    {
        permanentlyStopped = true;
        StopStatic();
    }
}