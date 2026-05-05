using UnityEngine;
using InventoryFramework;

public class FilmReelPickup : MonoBehaviour
{
    public GameObject interactPrompt;

    [Header("Inventory Item")]
    public Item item;
    public int amount = 1;

    private bool playerInRange = false;
    private ItemPickupHandler pickupHandler;

    private void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUpReel();
        }
    }

    private void PickUpReel()
    {
        Debug.Log("E pressed while near reel");

        if (pickupHandler == null)
        {
            Debug.LogError("ItemPickupHandler not found on Player!");
            return;
        }

        if (item == null)
        {
            Debug.LogError("FilmReelPickup item is NOT assigned!");
            return;
        }

        pickupHandler.PickupItem(item, amount);

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        Debug.Log("Film reel picked up and added to inventory!");

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            pickupHandler = other.GetComponent<ItemPickupHandler>();

            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pickupHandler = null;

            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }
}