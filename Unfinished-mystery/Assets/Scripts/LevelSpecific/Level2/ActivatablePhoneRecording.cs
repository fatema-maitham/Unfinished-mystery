using System.Collections;
using UnityEngine;

public class ActivatablePhoneRecording : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string activationLabel = "Listen";
    [SerializeField] private string activationHint = "Phone";
    [SerializeField] private float activationRadius = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSource phoneAudioSource;
    [SerializeField] private AudioClip lanaRecordingClip;
    [SerializeField] private AudioSource clockAudioSource;
    [SerializeField] private AudioClip clockStrongSound;

    [Header("After Recording")]
    [SerializeField] private GameObject clockObjectToEnableInspect;

    private bool hasPlayed;

    public string ActivationLabel => activationLabel;
    public string ActivationHint => activationHint;
    public bool CanActivate => !hasPlayed;
    public float ActivationRadius => activationRadius;

    public void OnActivate(GameObject source)
    {
        if (hasPlayed) return;

        hasPlayed = true;
        StartCoroutine(PlayPhoneThenClock());
    }

    private IEnumerator PlayPhoneThenClock()
    {
        if (phoneAudioSource != null && lanaRecordingClip != null)
        {
            phoneAudioSource.clip = lanaRecordingClip;
            phoneAudioSource.Play();

            yield return new WaitForSeconds(lanaRecordingClip.length);
        }

        yield return new WaitForSeconds(0.5f);

        if (clockAudioSource != null && clockStrongSound != null)
        {
            clockAudioSource.PlayOneShot(clockStrongSound);
        }

        if (clockObjectToEnableInspect != null)
        {
            clockObjectToEnableInspect.SetActive(true);
        }
    }

    public void OnActivatableFocus() { }

    public void OnActivatableBlur() { }
}