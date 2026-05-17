using System.Collections.Generic;
using UnityEngine;

namespace InventoryFramework
{
    public class Hotbar : MonoBehaviour
    {
        public int size = 9;
        public List<InventorySlot> slots;

        private void Awake()
        {
            slots = new List<InventorySlot>();

            for (int i = 0; i < size; i++)
            {
                slots.Add(new InventorySlot());
            }
        }

        public InventorySlot GetSlot(int index)
        {
            if (index < 0 || index >= slots.Count)
                return null;

            return slots[index];
        }

        public bool AddItem(Item newItem, int amount = 1)
        {
            if (newItem == null)
            {
                Debug.LogError("Hotbar: Cannot add null item.");
                return false;
            }

            // Always add the picked item to the first empty slot.
            // This means:
            // First pickup -> Slot 1
            // Second pickup -> Slot 2
            // Third pickup -> Slot 3
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].IsEmpty)
                {
                    slots[i].item = newItem;
                    slots[i].count = amount;

                    Debug.Log(newItem.name + " added to hotbar slot " + (i + 1));
                    return true;
                }
            }

            Debug.LogWarning("Hotbar is full. Could not add: " + newItem.name);
            return false;
        }

        public bool RemoveItem(Item itemToRemove, int amount = 1)
        {
            if (itemToRemove == null)
                return false;

            for (int i = 0; i < slots.Count; i++)
            {
                InventorySlot slot = slots[i];

                if (!slot.IsEmpty && slot.item == itemToRemove)
                {
                    slot.count -= amount;

                    if (slot.count <= 0)
                    {
                        slot.item = null;
                        slot.count = 0;
                    }

                    Debug.Log(itemToRemove.name + " removed from hotbar slot " + (i + 1));
                    return true;
                }
            }

            return false;
        }
    }
}