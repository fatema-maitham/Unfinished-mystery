using UnityEngine;

public class TVStaticSoundController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource staticAudioSource;

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

        if (staticAudioSource == null)
            return;

        if (!staticAudioSource.isPlaying)
            staticAudioSource.Play();
    }

    public void StopStatic()
    {
        if (staticAudioSource != null)
            staticAudioSource.Stop();
    }

    public void StopStaticPermanently()
    {
        permanentlyStopped = true;
        StopStatic();
    }
}