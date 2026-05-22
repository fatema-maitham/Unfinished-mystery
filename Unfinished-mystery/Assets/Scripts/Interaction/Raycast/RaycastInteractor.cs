using TMPro;
using UnityEngine;

public class RaycastInteractor : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float interactDistance = 4f;
    [SerializeField] private LayerMask interactableLayer = ~0;

    [Header("Input")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("UI")]
    [SerializeField] private TMP_Text interactionPromptText;

    private Camera playerCamera;
    private IInteractable currentInteractable;

    private void Awake()
    {
        playerCamera = GetComponent<Camera>();

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Start()
    {
        HidePrompt();
    }

    private void Update()
    {
        if (NoteController.IsAnyNoteOpen)
        {
            currentInteractable = null;
            HidePrompt();
            return;
        }

        CheckForInteractable();

        if (currentInteractable != null && Input.GetKeyDown(interactKey))
        {
            currentInteractable.Interact();
        }
    }

    private void CheckForInteractable()
    {
        currentInteractable = null;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable == null)
            {
                interactable = hit.collider.GetComponentInParent<IInteractable>();
            }

            if (interactable != null)
            {
                currentInteractable = interactable;
                ShowPrompt(interactable.GetPromptText());
                return;
            }
        }

        HidePrompt();
    }

    private void ShowPrompt(string message)
    {
        if (interactionPromptText == null)
            return;

        interactionPromptText.gameObject.SetActive(true);
        interactionPromptText.text = message;
    }

    private void HidePrompt()
    {
        if (interactionPromptText == null)
            return;

        interactionPromptText.text = "";
        interactionPromptText.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Camera cam = GetComponent<Camera>();

        if (cam == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(cam.transform.position, cam.transform.forward * interactDistance);
    }
}