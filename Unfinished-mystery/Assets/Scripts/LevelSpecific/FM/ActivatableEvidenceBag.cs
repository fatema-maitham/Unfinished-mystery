using UnityEngine;

public class ActivatableEvidenceBag : MonoBehaviour
{
    [Header("Prompt UI")]
    [SerializeField] private InteractionPromptUI promptUI;
    [SerializeField] private string lockedPromptText = "LOCKED";
    [SerializeField] private string lockedSubLabel = "You need the key to unlock this bag.";
    [SerializeField] private string openPromptText = "UNLOCK";
    [SerializeField] private string openSubLabel = "Evidence Bag";

    [Header("Input")]
    [SerializeField] private KeyCode selectKeySlot = KeyCode.Alpha1;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Bag Objects")]
    [SerializeField] private GameObject closedBag;
    [SerializeField] private GameObject openBag;

    private bool playerInside;
    private bool keySelected;
    private bool opened;

    private void Start()
    {
        if (closedBag != null)
            closedBag.SetActive(true);

        if (openBag != null)
            openBag.SetActive(false);
    }

    private void Update()
    {
        if (opened)
            return;

        if (Input.GetKeyDown(selectKeySlot))
            keySelected = true;

        if (!playerInside)
            return;

        if (promptUI != null)
        {
            if (keySelected)
                promptUI.ShowPrompt(openPromptText, openSubLabel);
            else
                promptUI.ShowPrompt(lockedPromptText, lockedSubLabel);
        }

        if (keySelected && Input.GetKeyDown(interactKey))
            OpenBag();
    }

    private void OpenBag()
    {
        opened = true;

        if (promptUI != null)
            promptUI.HidePrompt();

        if (closedBag != null)
            closedBag.SetActive(false);

        if (openBag != null)
            openBag.SetActive(true);

        Level2PuzzleSystem.Instance?.OpenEvidenceBag();

        Debug.Log("[ActivatableEvidenceBag] Evidence bag opened.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || opened)
            return;

        playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        if (promptUI != null)
            promptUI.HidePrompt();
    }
}