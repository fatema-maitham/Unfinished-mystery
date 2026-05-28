using UnityEngine;
using UnityEngine.SceneManagement;
using InventoryFramework;
using TMPro;

public class DoorWithKeyRequirement : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 3f;

    [Header("Inventory")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private Hotbar hotbar;
    [SerializeField] private Item requiredKey;

    [Header("Locked Popup")]
    [SerializeField] private TMP_Text lockedPopupText;
    [SerializeField] private float popupTime = 2f;

    [Header("Scene")]
    [SerializeField] private string summarySceneName = "LevelSummary4";

    [Header("Loop Settings")]
    [SerializeField] private int maxLoops = 5;

    private bool triggered = false;
    private float popupTimer = 0f;

    private void Start()
    {
        HideLockedPopup();
    }

    private void Update()
    {
        if (triggered || player == null)
            return;

        if (popupTimer > 0f)
        {
            popupTimer -= Time.deltaTime;

            if (popupTimer <= 0f)
                HideLockedPopup();
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > interactDistance)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!HasRequiredKey())
            {
                ShowLockedPopup("Door Is Locked");
                return;
            }

            triggered = true;
            HideLockedPopup();

            int finalLoops = 1;
            LoopChangeSystem loopSystem = FindFirstObjectByType<LoopChangeSystem>();

            if (loopSystem != null)
                finalLoops = loopSystem.currentLoop;

            finalLoops = Mathf.Clamp(finalLoops, 1, maxLoops);

            LevelResultData.loopsUsed = finalLoops;
            LevelResultData.LoopsUsed = finalLoops;
            LevelResultData.MaxLoops = maxLoops;

            SceneManager.LoadScene(summarySceneName);
        }
    }

    private void ShowLockedPopup(string message)
    {
        if (lockedPopupText == null)
            return;

        lockedPopupText.text = message;
        lockedPopupText.gameObject.SetActive(true);
        popupTimer = popupTime;
    }

    private void HideLockedPopup()
    {
        if (lockedPopupText != null)
            lockedPopupText.gameObject.SetActive(false);
    }

    private bool HasRequiredKey()
    {
        if (requiredKey == null)
            return false;

        if (inventory != null && inventory.slots != null)
        {
            foreach (InventorySlot slot in inventory.slots)
            {
                if (slot != null && !slot.IsEmpty && slot.item == requiredKey)
                    return true;
            }
        }

        if (hotbar != null && hotbar.slots != null)
        {
            foreach (InventorySlot slot in hotbar.slots)
            {
                if (slot != null && !slot.IsEmpty && slot.item == requiredKey)
                    return true;
            }
        }

        return false;
    }
}