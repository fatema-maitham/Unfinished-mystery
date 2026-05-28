using UnityEngine;
using InventoryFramework; 

public class UVFlashlightController : MonoBehaviour
{
    [Header("Inventory Tracking")]
    [SerializeField] private ItemPickupHandler playerPickupHandler; 
    [SerializeField] private Item flashlightItemReference;         

    [Header("Flashlight Visuals")]
    [SerializeField] private GameObject handheldFlashlightObject; 
    [SerializeField] private GameObject uvCameraOverlay;         

    [Header("Input Controls")]
    [SerializeField] private KeyCode toggleKey = KeyCode.F;      

    private bool isFlashlightOn = false;

    void Start()
    {
        // Keep everything hidden at spawn
        if (handheldFlashlightObject != null) handheldFlashlightObject.SetActive(false);
        if (uvCameraOverlay != null) uvCameraOverlay.SetActive(false);
    }

    void Update()
    {
        
        if (!HasFlashlightInInventory())
        {
            if (isFlashlightOn) TurnOffFlashlight();
            return;
        }

      
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleFlashlight();
        }
    }

    private bool HasFlashlightInInventory()
    {
        if (playerPickupHandler == null || flashlightItemReference == null) return false;

        // 1. Check if the flashlight is sitting in your Hotbar slots
        if (playerPickupHandler.hotbar != null)
        {
            foreach (InventorySlot slot in playerPickupHandler.hotbar.slots)
            {
                if (!slot.IsEmpty && slot.item == flashlightItemReference)
                {
                    return true; // Found it in the hotbar!
                }
            }
        }

        // 2. Fallback: Check the main inventory canvas if it's full/routed there
        if (playerPickupHandler.inventory != null)
        {
            foreach (InventorySlot slot in playerPickupHandler.inventory.slots)
            {
                if (!slot.IsEmpty && slot.item == flashlightItemReference)
                {
                    return true; // Found it in the backup inventory!
                }
            }
        }

        return false; // Not found anywhere yet
    }

    private void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;

        if (handheldFlashlightObject != null) handheldFlashlightObject.SetActive(isFlashlightOn);
        if (uvCameraOverlay != null) uvCameraOverlay.SetActive(isFlashlightOn);

        Debug.Log("UV Flashlight Toggled: " + isFlashlightOn);
    }

    private void TurnOffFlashlight()
    {
        isFlashlightOn = false;
        if (handheldFlashlightObject != null) handheldFlashlightObject.SetActive(false);
        if (uvCameraOverlay != null) uvCameraOverlay.SetActive(false);
    }
}