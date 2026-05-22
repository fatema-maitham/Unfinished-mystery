using UnityEngine;
using InventoryFramework;

public class ItemPickup : MonoBehaviour
{
    [Header("Prompt")]
    [SerializeField] private InteractionPromptUI promptUI;
    [SerializeField] private string promptText = "Pick up item";

    [Header("Inventory Item")]
    [SerializeField] private Item item;
    [SerializeField] private int amount = 1;

    [Header("Pickup Settings")]
    [SerializeField] private bool destroyAfterPickup = true;

    [Header("Optional Level 3 Events")]
    [SerializeField] private L3ExitFlickerGuide flickerToStopOnPickup;
    [SerializeField] private L3ExitInspect inspectToDisableOnPickup;

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

        if (promptUI != null)
            promptUI.HidePrompt();

        if (flickerToStopOnPickup != null)
            flickerToStopOnPickup.StopFlickerPermanently();

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
            promptUI.ShowPrompt(promptText);
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