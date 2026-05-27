using System.Collections;
using UnityEngine;

public class PhoneSimCardInteraction : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 1.2f;

    [Header("Phone Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ringtoneClip;
    [SerializeField] private AudioClip phoneMessageClip;

    [Header("Clock Audio After Phone Message")]
    [SerializeField] private AudioSource clockAudioSource;
    [SerializeField] private AudioClip clockSoundClip;

    [Header("Inventory Icon To Hide")]
    [SerializeField] private GameObject simCardInventoryIconToHide;

    private bool phoneUsed;

    private void Update()
    {
        if (player == null)
            return;

        bool nearPhone = Vector3.Distance(player.position, transform.position) <= interactDistance;

        if (!SimCardPickup.HasSimCard || phoneUsed || !nearPhone)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            UsePhone();
        }
    }

    private void UsePhone()
    {
        phoneUsed = true;

        if (simCardInventoryIconToHide != null)
            simCardInventoryIconToHide.SetActive(false);

        StartCoroutine(PhoneSequence());
    }

    private IEnumerator PhoneSequence()
    {
        if (audioSource != null && ringtoneClip != null)
        {
            audioSource.PlayOneShot(ringtoneClip);
        }

        yield return new WaitForSeconds(7f);

        if (audioSource != null && phoneMessageClip != null)
        {
            audioSource.PlayOneShot(phoneMessageClip);
            yield return new WaitForSeconds(phoneMessageClip.length);
        }

        if (Level2PuzzleSystem.Instance != null)
        {
            Level2PuzzleSystem.Instance.HearPhoneMessage();
        }

        if (clockAudioSource != null && clockSoundClip != null)
        {
            clockAudioSource.PlayOneShot(clockSoundClip);
        }
    }
}