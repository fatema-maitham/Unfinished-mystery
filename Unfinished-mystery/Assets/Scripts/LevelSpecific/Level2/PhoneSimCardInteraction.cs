using System.Collections;
using UnityEngine;

// Handles using the SIM card with the phone, then plays the phone message and clock sound
public class PhoneSimCardInteraction : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player; // Player transform used for distance check
    [SerializeField] private float interactDistance = 1.2f; // Distance needed to use the phone

    [Header("Phone Audio")]
    [SerializeField] private AudioSource audioSource; // Audio source for ringtone and phone message
    [SerializeField] private AudioClip ringtoneClip; // Ringtone audio clip
    [SerializeField] private AudioClip phoneMessageClip; // Phone message audio clip

    [Header("Clock Audio After Phone Message")]
    [SerializeField] private AudioSource clockAudioSource; // Audio source for clock sound
    [SerializeField] private AudioClip clockSoundClip; // Clock sound after phone message

    [Header("Inventory Icon To Hide")]
    [SerializeField] private GameObject simCardInventoryIconToHide; // SIM card inventory icon hidden after use

    private bool phoneUsed; // Prevents the phone from being used more than once

    private void Update()
    {
        // Stop if player reference is missing
        if (player == null)
            return;

        // Check if player is close enough to the phone
        bool nearPhone = Vector3.Distance(player.position, transform.position) <= interactDistance;

        // Stop if the SIM card is not collected, phone is already used, or player is too far
        if (!SimCardPickup.HasSimCard || phoneUsed || !nearPhone)
            return;

        // Press 0 to use the SIM card with the phone
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            UsePhone();
        }
    }

    private void UsePhone()
    {
        // Mark phone as used
        phoneUsed = true;

        // Hide SIM card icon from inventory
        if (simCardInventoryIconToHide != null)
            simCardInventoryIconToHide.SetActive(false);

        // Start the phone sequence
        StartCoroutine(PhoneSequence());
    }

    private IEnumerator PhoneSequence()
    {
        // Play ringtone first
        if (audioSource != null && ringtoneClip != null)
        {
            audioSource.PlayOneShot(ringtoneClip);
        }

        // Wait before the phone message starts
        yield return new WaitForSeconds(7f);

        // Play the phone message
        if (audioSource != null && phoneMessageClip != null)
        {
            audioSource.PlayOneShot(phoneMessageClip);
            yield return new WaitForSeconds(phoneMessageClip.length);
        }

        // Tell the Level 2 puzzle system that the phone message was heard
        if (Level2PuzzleSystem.Instance != null)
        {
            Level2PuzzleSystem.Instance.HearPhoneMessage();
        }

        // Play the clock sound after the phone message
        if (clockAudioSource != null && clockSoundClip != null)
        {
            clockAudioSource.PlayOneShot(clockSoundClip);
        }
    }
}