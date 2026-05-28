using UnityEngine;
using InventoryFramework;
using System.Collections.Generic;

public class InventoryKeyLoopReset : MonoBehaviour, ILoopResettable
{
    [Header("Key")]
    [SerializeField] private Item keyItem;
    [SerializeField] private GameObject keySceneObject;

    [Header("Inventory Data")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private Hotbar hotbar;

    
    private Vector3 keyStartLocalPosition;
    private Quaternion keyStartLocalRotation;
    private Vector3 keyStartLocalScale;

    private void Awake()
    {
        if (keySceneObject != null)
        {
            keyStartLocalPosition = keySceneObject.transform.localPosition;
            keyStartLocalRotation = keySceneObject.transform.localRotation;
            keyStartLocalScale = keySceneObject.transform.localScale;
        }
    }

    public void ResetState()
    {
        Debug.Log("KEY RESET STARTED");

        RemoveKeyFromSlots(inventory != null ? inventory.slots : null);
        RemoveKeyFromSlots(hotbar != null ? hotbar.slots : null);

        if (keySceneObject != null)
        {
            keySceneObject.SetActive(true);

            keySceneObject.transform.localPosition = keyStartLocalPosition;
            keySceneObject.transform.localRotation = keyStartLocalRotation;
            keySceneObject.transform.localScale = keyStartLocalScale;
        }

        

        Debug.Log("KEY RESET FINISHED");
    }

    private void RemoveKeyFromSlots(List<InventorySlot> slots)
    {
        if (slots == null || keyItem == null)
            return;

        foreach (InventorySlot slot in slots)
        {
            if (slot != null && !slot.IsEmpty && slot.item == keyItem)
            {
                slot.item = null;
                slot.count = 0;
            }
        }
    }

}