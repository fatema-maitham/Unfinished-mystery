using UnityEngine;
using InventoryFramework;

/// <summary>
/// Level 2 only.
/// Picks up the brass key, adds it to inventory, then marks Brass Key Found
/// in Level2PuzzleSystem.
/// </summary>
public class Level2KeyPickup : MonoBehaviour
{
    [Header("Prompt")]
    [SerializeField] private InteractionPromptUI promptUI;
    [SerializeField] private string promptText = "PICK UP";
    [SerializeField] private string promptSubLabel = "Brass Key";

    [Header("Inventory Item")]
    [SerializeField] private Item item;
    [SerializeField] private int amount = 1;

    [Header("Pickup Settings")]
    [SerializeField] private bool destroyAfterPickup = true;

    private bool playerInRange;
    private ItemPickupHandler pickupHandler;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
            PickUpKey();
    }

    private void PickUpKey()
    {
        if (pickupHandler == null)
        {
            Debug.LogError("[Level2KeyPickup] ItemPickupHandler not found on Player.");
            return;
        }

        if (item == null)
        {
            Debug.LogError("[Level2KeyPickup] Key item is not assigned.");
            return;
        }

        pickupHandler.PickupItem(item, amount);

        if (promptUI != null)
            promptUI.HidePrompt();

        Level2PuzzleSystem.Instance?.FindBrassKey();

        Debug.Log("[Level2KeyPickup] Brass key picked up.");

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