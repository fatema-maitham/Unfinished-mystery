using UnityEngine;
using InventoryFramework;

public class SimCardPickup : MonoBehaviour
{
    public static bool HasSimCard { get; private set; }

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
            PickUpSimCard();
    }

    private void PickUpSimCard()
    {
        if (pickupHandler == null)
        {
            Debug.LogError("[SimCardPickup] ItemPickupHandler not found on Player.");
            return;
        }

        if (item == null)
        {
            Debug.LogError("[SimCardPickup] SIM Card item is not assigned.");
            return;
        }

        pickupHandler.PickupItem(item, amount);

        HasSimCard = true;

        if (Level2PuzzleSystem.Instance != null)
        {
            Level2PuzzleSystem.Instance.FindSimCard();
        }

        Debug.Log("[SimCardPickup] SIM Card picked up.");

        if (destroyAfterPickup)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        pickupHandler = other.GetComponent<ItemPickupHandler>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        pickupHandler = null;
    }
}