using UnityEngine;

public class BoxInteraction : MonoBehaviour
{
    [Header("Your Existing UI")]
    [SerializeField] private GameObject interactionPanel; // Drag your UI panel here

    [Header("Movement Settings")]
    [SerializeField] private Vector3 moveOffset = new Vector3(2f, 0f, 0f); // Direction & distance to slide
    [SerializeField] private float moveSpeed = 3f;                      // How fast it slides
    [SerializeField] private KeyCode interactKey = KeyCode.E;           // Key to press

    private bool playerInRange = false;
    private bool isMoved = false;
    private Vector3 targetPosition;

    void Start()
    {
        // Ensure the UI starts hidden
        if (interactionPanel != null)
            interactionPanel.SetActive(false);

        // Calculate where the box will slide to
        targetPosition = transform.position + moveOffset;
    }

    void Update()
    {
        // If player is close, hasn't moved the box yet, and presses 'E'
        if (playerInRange && !isMoved && Input.GetKeyDown(interactKey))
        {
            isMoved = true;

            // Hide your interaction UI since the action is done
            if (interactionPanel != null)
                interactionPanel.SetActive(false);
        }

        // Smoothly slide the box to the target position once triggered
        if (isMoved && transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    // Show UI when player is close
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isMoved)
        {
            playerInRange = true;
            if (interactionPanel != null)
                interactionPanel.SetActive(true);
        }
    }

    // Hide UI if player walks away without interacting
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionPanel != null)
                interactionPanel.SetActive(false);
        }
    }
}