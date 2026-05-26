using System.Collections;
using UnityEngine;

public class PhoneSimCardInteraction : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 3f;

    [Header("Prompt UI")]
    [SerializeField] private ActivationPromptUI promptUI;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ringtoneClip;
    [SerializeField] private AudioClip phoneMessageClip;

    [Header("Inventory Icon To Hide")]
    [SerializeField] private GameObject simCardInventoryIconToHide;

    private bool phoneUsed;
    private bool showingMyPrompt;

    private void Update()
    {
        if (player == null || promptUI == null)
            return;

        bool nearPhone = Vector3.Distance(player.position, transform.position) <= interactDistance;

        if (!SimCardPickup.HasSimCard || phoneUsed || !nearPhone)
        {
            if (showingMyPrompt)
            {
                promptUI.HidePrompt();
                showingMyPrompt = false;
            }

            return;
        }

        promptUI.ShowPrompt("Press 0", "Use SIM Card");

        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            UsePhone();
        }
    }

    private void UsePhone()
    {
        phoneUsed = true;

        if (showingMyPrompt)
        {
            promptUI.HidePrompt();
            showingMyPrompt = false;
        }

        if (simCardInventoryIconToHide != null)
            simCardInventoryIconToHide.SetActive(false);

        StartCoroutine(PhoneSequence());
    }

    private IEnumerator PhoneSequence()
    {
        if (audioSource != null && ringtoneClip != null)
            audioSource.PlayOneShot(ringtoneClip);

        yield return new WaitForSeconds(7f);

        if (audioSource != null && phoneMessageClip != null)
            audioSource.PlayOneShot(phoneMessageClip);
    }
}