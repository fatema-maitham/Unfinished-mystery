using UnityEngine;

namespace InventoryFramework
{
    public class ItemPickupHandler : MonoBehaviour
    {
        public Hotbar hotbar;
        public Inventory inventory;

        public void PickupItem(Item item, int amount = 1)
        {
            if (item == null)
            {
                Debug.LogError("ItemPickupHandler: item is null!");
                return;
            }

            if (inventory == null)
            {
                Debug.LogError("ItemPickupHandler: Inventory is not assigned!");
                return;
            }

            Debug.Log("Trying to pick up: " + item.name + " amount: " + amount);

            bool addedToInventory = inventory.AddItem(item, amount);
            Debug.Log("Added to inventory? " + addedToInventory);

            if (!addedToInventory && hotbar != null)
            {
                bool addedToHotbar = hotbar.AddItem(item, amount);
                Debug.Log("Added to hotbar? " + addedToHotbar);

                if (!addedToHotbar)
                {
                    Debug.Log("Both inventory and hotbar are full!");
                }
            }

            HotbarUI hotbarUI = FindAnyObjectByType<HotbarUI>();
            if (hotbarUI != null)
            {
                hotbarUI.RefreshUI();
            }

            InventoryUI inventoryUI = FindAnyObjectByType<InventoryUI>();
            if (inventoryUI != null)
            {
                inventoryUI.RefreshUI();
            }
        }
    }
}