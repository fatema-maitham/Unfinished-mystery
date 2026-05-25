using UnityEngine;
using InventoryFramework;

public class ItemPickup : MonoBehaviour
{
    [Header("Prompt")]
    [SerializeField] private InteractionPromptUI promptUI;
    [SerializeField] private string promptText = "PICK UP";
    [SerializeField] private string promptSubLabel = "Item";

    [Header("Inventory Item")]
    [SerializeField] private Item item;
    [SerializeField] private int amount = 1;

    [Header("Pickup Settings")]
    [SerializeField] private bool destroyAfterPickup = true;

    [Header("Pickup Sound")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float pickupSoundVolume = 0.7f;
    

    [Header("Optional Level 3 Events")]
    [SerializeField] private L3ExitFlickerGuide flickerToStopOnPickup;
    [SerializeField] private L3ExitInspect inspectToDisableOnPickup;
    [SerializeField] private TVStaticSoundController tvStaticToStopOnPickup;
    [SerializeField] private FilmProjectorUse projectorToNotify;
    [SerializeField] private int collectedReelNumber = 0;

    private bool playerInRange = false;
    private ItemPickupHandler pickupHandler;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUpItem();
        }
    }

    private void PickUpItem()
    {
        if (pickupHandler == null)
        {
            Debug.LogError("ItemPickupHandler not found on Player!");
            return;
        }

        if (item == null)
        {
            Debug.LogError("Item is not assigned on " + gameObject.name);
            return;
        }

        pickupHandler.PickupItem(item, amount);

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(
                pickupSound,
                transform.position,
                pickupSoundVolume
            );
        }

        if (promptUI != null)
            promptUI.HidePrompt();

        if (projectorToNotify != null && collectedReelNumber > 0)
            projectorToNotify.NotifyReelCollected(collectedReelNumber);

        if (flickerToStopOnPickup != null)
            flickerToStopOnPickup.StopFlickerPermanently();

        if (tvStaticToStopOnPickup != null)
            tvStaticToStopOnPickup.StopStaticPermanently();

        if (inspectToDisableOnPickup != null)
            inspectToDisableOnPickup.PauseInspectBriefly(0.6f);

        Debug.Log(item.name + " picked up and added to inventory.");

        if (destroyAfterPickup)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;
        pickupHandler = other.GetComponent<ItemPickupHandler>();

        if (promptUI != null)
            promptUI.ShowPrompt(promptText, promptSubLabel);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;
        pickupHandler = null;

        if (promptUI != null)
            promptUI.HidePrompt();
    }
}