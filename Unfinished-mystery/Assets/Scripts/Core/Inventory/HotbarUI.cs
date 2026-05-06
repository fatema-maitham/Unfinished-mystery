using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryFramework
{
    public class HotbarUI : MonoBehaviour
    {
        public Hotbar hotbar;
        public Inventory inventory;
        public Transform slotParent;
        public ItemTooltip tooltip;
        public Transform toolsParent;

        public RectTransform dragLayer;
        public Canvas rootCanvas;

        private List<InventorySlotUI> slotUIs = new List<InventorySlotUI>();
        private int selectedIndex = 0;

        private void Start()
        {
            BuildSlots();
            RefreshUI();
        }

        private void BuildSlots()
        {
            slotUIs.Clear();

            if (slotParent == null)
            {
                Debug.LogError("HotbarUI: slotParent is not assigned!");
                return;
            }

            if (hotbar == null)
            {
                Debug.LogError("HotbarUI: hotbar is not assigned!");
                return;
            }

            foreach (Transform child in slotParent)
            {
                InventorySlotUI ui = child.GetComponent<InventorySlotUI>();

                if (ui != null)
                {
                    ui.tooltip = tooltip;
                    ui.SetupHotbar(hotbar, inventory, slotUIs.Count, this);
                    slotUIs.Add(ui);
                }
            }

            if (slotUIs.Count < hotbar.size)
            {
                Debug.LogWarning("HotbarUI: slot count in slotParent = " + slotUIs.Count + ", but hotbar.size = " + hotbar.size + ".");
            }
        }

        private void Update()
        {
            if (hotbar == null || slotUIs == null || slotUIs.Count == 0)
                return;

            int count = Mathf.Min(hotbar.size, slotUIs.Count);

            for (int i = 0; i < count; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    selectedIndex = i;
                    RefreshUI();
                }
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0f && count > 0)
            {
                selectedIndex = (selectedIndex + 1) % count;
                RefreshUI();
            }
            else if (scroll < 0f && count > 0)
            {
                selectedIndex = (selectedIndex - 1 + count) % count;
                RefreshUI();
            }
        }

        public void RefreshUI()
        {
            if (hotbar == null || hotbar.slots == null)
                return;

            if (slotUIs == null || slotUIs.Count == 0)
                return;

            int count = Mathf.Min(hotbar.slots.Count, slotUIs.Count);

            for (int i = 0; i < count; i++)
            {
                slotUIs[i].SetSlot(hotbar.slots[i]);

                Image bg = slotUIs[i].GetBackgroundImage();

                if (bg != null)
                {
                    bg.color = (i == selectedIndex) ? Color.yellow : Color.white;
                }
            }

            RefreshSelectedTool(count);
        }

        private void RefreshSelectedTool(int count)
        {
            if (toolsParent == null)
                return;

            for (int x = toolsParent.childCount - 1; x >= 0; x--)
            {
                Destroy(toolsParent.GetChild(x).gameObject);
            }

            if (selectedIndex < 0 || selectedIndex >= count)
                return;

            InventorySlot selectedSlot = slotUIs[selectedIndex].GetSlot();

            if (selectedSlot == null)
                return;

            if (selectedSlot.IsEmpty)
                return;

            if (selectedSlot.item == null)
                return;

            if (selectedSlot.item.model == null)
                return;

            Instantiate(selectedSlot.item.model, toolsParent);
        }
    }
}