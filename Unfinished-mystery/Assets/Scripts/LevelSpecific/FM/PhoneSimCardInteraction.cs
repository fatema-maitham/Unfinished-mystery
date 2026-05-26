using System.Collections;
using UnityEngine;

public class PhoneSimCardInteraction : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 1.2f;
    [SerializeField] private KeyCode useKey = KeyCode.E;

    [Header("Prompt UI")]
    [SerializeField] private ActivationPromptUI promptUI;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ringtoneClip;
    [SerializeField] private AudioClip phoneMessageClip;
    [SerializeField] private float ringtoneDuration = 7f;

    [Header("Inventory Icon")]
    [SerializeField] private GameObject simCardInventoryIconToHide;

    [Header("Prompt Text")]
    [SerializeField] private string label = "Use";
    [SerializeField] private string subLabel = "Phone";

    private bool phoneUsed;
    private bool showingMyPrompt;

    private void Update()
    {
        if (player == null || promptUI == null)
            return;

        bool hasSimCard = SimCardPickup.HasSimCard;
        bool nearPhone = Vector3.Distance(player.position, transform.position) <= interactDistance;

        if (!hasSimCard || phoneUsed || !nearPhone)
        {
            if (showingMyPrompt)
            {
                promptUI.HidePrompt();
                showingMyPrompt = false;
            }

            return;
        }

        promptUI.ShowPrompt(label, subLabel);
        showingMyPrompt = true;

        if (Input.GetKeyDown(useKey))
            UsePhone();
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

        yield return new WaitForSeconds(ringtoneDuration);

        if (audioSource != null && phoneMessageClip != null)
            audioSource.PlayOneShot(phoneMessageClip);
    }
}