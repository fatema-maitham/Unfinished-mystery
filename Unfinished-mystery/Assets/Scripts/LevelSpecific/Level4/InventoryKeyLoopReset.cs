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

    [Header("UI Refresh")]
    [SerializeField] private MonoBehaviour hotbarUI;
    [SerializeField] private MonoBehaviour inventoryUI;

    private Vector3 startLocalPosition;
    private Quaternion startLocalRotation;
    private Vector3 startLocalScale;

    private bool keyHidden = false;

    private void Awake()
    {
        if (keySceneObject != null)
        {
            startLocalPosition = keySceneObject.transform.localPosition;
            startLocalRotation = keySceneObject.transform.localRotation;
            startLocalScale = keySceneObject.transform.localScale;
        }
    }

    private void Update()
    {
        if (keyHidden || keySceneObject == null)
            return;

        if (HasKey())
        {
            keyHidden = true;
            HideKeyObject();
            RefreshUI();

            Debug.Log("KEY HIDDEN AFTER PICKUP");
        }
    }

    public void ResetState()
    {
        Debug.Log("KEY RESET STARTED");

        RemoveKeyFromSlots(inventory != null ? inventory.slots : null);
        RemoveKeyFromSlots(hotbar != null ? hotbar.slots : null);

        keyHidden = false;

        if (keySceneObject != null)
        {
            keySceneObject.SetActive(true);

            keySceneObject.transform.localPosition = startLocalPosition;
            keySceneObject.transform.localRotation = startLocalRotation;
            keySceneObject.transform.localScale = startLocalScale;

            ShowKeyVisuals();
            SetKeyPickupEnabled(false);
        }

        RefreshUI();

        Debug.Log("KEY RESET FINISHED");
    }

    public void MakeKeyPickupAvailable()
    {
        if (keySceneObject == null)
            return;

        keySceneObject.SetActive(true);

        ShowKeyVisuals();
        SetKeyPickupEnabled(true);

        Debug.Log("KEY PICKUP ENABLED");
    }

    private void HideKeyObject()
    {
        if (keySceneObject == null)
            return;

        SetKeyPickupEnabled(false);
        HideKeyVisuals();
        keySceneObject.SetActive(false);
    }

    private void ShowKeyVisuals()
    {
        foreach (Renderer rend in keySceneObject.GetComponentsInChildren<Renderer>(true))
        {
            if (rend != null)
                rend.enabled = true;
        }
    }

    private void HideKeyVisuals()
    {
        foreach (Renderer rend in keySceneObject.GetComponentsInChildren<Renderer>(true))
        {
            if (rend != null)
                rend.enabled = false;
        }
    }

    private void SetKeyPickupEnabled(bool enabled)
    {
        foreach (Collider col in keySceneObject.GetComponentsInChildren<Collider>(true))
        {
            if (col != null)
                col.enabled = enabled;
        }

        foreach (MonoBehaviour script in keySceneObject.GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (script == this)
                continue;

            string scriptName = script.GetType().Name;

            if (scriptName.Contains("Pickup") || scriptName.Contains("ItemPickup"))
                script.enabled = enabled;
        }
    }

    private bool HasKey()
    {
        if (keyItem == null)
            return false;

        if (ContainsKey(inventory != null ? inventory.slots : null))
            return true;

        if (ContainsKey(hotbar != null ? hotbar.slots : null))
            return true;

        return false;
    }

    private bool ContainsKey(List<InventorySlot> slots)
    {
        if (slots == null)
            return false;

        foreach (InventorySlot slot in slots)
        {
            if (slot != null && !slot.IsEmpty && slot.item == keyItem)
                return true;
        }

        return false;
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

    private void RefreshUI()
    {
        if (hotbarUI != null)
            hotbarUI.Invoke("RefreshUI", 0f);

        if (inventoryUI != null)
            inventoryUI.Invoke("RefreshUI", 0f);
    }
}