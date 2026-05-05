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

            if (hotbar == null)
            {
                Debug.LogError("ItemPickupHandler: Hotbar is not assigned!");
                return;
            }

            Debug.Log("Trying to pick up: " + item.name + " amount: " + amount);

            bool addedToHotbar = hotbar.AddItem(item, amount);
            Debug.Log("Added to hotbar? " + addedToHotbar);

            if (!addedToHotbar && inventory != null)
            {
                bool addedToInventory = inventory.AddItem(item, amount);
                Debug.Log("Added to inventory? " + addedToInventory);

                if (!addedToInventory)
                {
                    Debug.Log("Both hotbar and inventory full!");
                }
            }

            HotbarUI hotbarUI = FindAnyObjectByType<HotbarUI>();
            if (hotbarUI != null)
                hotbarUI.RefreshUI();

            InventoryUI inventoryUI = FindAnyObjectByType<InventoryUI>();
            if (inventoryUI != null)
                inventoryUI.RefreshUI();
        }
    }
}