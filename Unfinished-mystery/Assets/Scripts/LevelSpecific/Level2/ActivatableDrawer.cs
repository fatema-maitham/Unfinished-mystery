using System.Collections;
using UnityEngine;
using TMPro;

// Drawer script: shows prompt, opens drawer, shows key, resets with loop
public class ActivatableDrawer : MonoBehaviour, ILoopResettable
{
    [Header("New HUD Prompt")]
    [SerializeField] private GameObject promptPanel; // UI panel that shows E prompt
    [SerializeField] private TMP_Text keyHintText;   // Text for key button
    [SerializeField] private TMP_Text labelText;     // Main prompt text
    [SerializeField] private TMP_Text subLabelText;  // Small prompt label

    [Header("Prompt Text")]
    [SerializeField] private string keyText = "E";              // Key shown to player
    [SerializeField] private string promptText = "OPEN";        // Action text
    [SerializeField] private string promptSubLabel = "Drawer";  // Object name

    [Header("Detection")]
    [SerializeField] private Transform player;              // Player transform
    [SerializeField] private float interactDistance = 2.5f;  // Distance needed to interact
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Interaction key

    [Header("Drawer Movement")]
    [SerializeField] private Transform drawerToMove; // The drawer part that moves
    [SerializeField] private Vector3 openOffset = new Vector3(0f, 0f, 0.45f); // How far drawer opens
    [SerializeField] private float openSpeed = 2.5f; // Drawer opening speed

    [Header("Key")]
    [SerializeField] private GameObject keyObject; // Key hidden inside drawer

    private bool isOpen; // True after drawer opens
    private bool showingPrompt; // Prevents prompt from being refreshed every frame
    private Vector3 closedLocalPosition; // Drawer original position
    private Vector3 openLocalPosition; // Drawer open position
    private Coroutine openCoroutine; // Stores drawer opening coroutine

    private void Awake()
    {
        // If no drawer part is assigned, move this object itself
        if (drawerToMove == null)
            drawerToMove = transform;

        // Save closed and open positions
        closedLocalPosition = drawerToMove.localPosition;
        openLocalPosition = closedLocalPosition + openOffset;
    }

    private void Start()
    {
        // Auto-find player if not assigned in Inspector
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                player = playerObject.transform;
        }

        // Start with drawer closed and key hidden
        ResetDrawerClosed();
    }

    private void Update()
    {
        // Stop checking if drawer is already open or player is missing
        if (isOpen || player == null)
            return;

        // Check distance between player and drawer
        float distance = Vector3.Distance(player.position, transform.position);

        // If player is close enough, show prompt and allow opening
        if (distance <= interactDistance)
        {
            ShowPrompt();

            if (Input.GetKeyDown(interactKey))
                OpenDrawer();
        }
        else
        {
            HidePrompt();
        }
    }

    private void ShowPrompt()
    {
        // Do not show again if already showing
        if (showingPrompt)
            return;

        // Enable prompt panel
        if (promptPanel != null)
            promptPanel.SetActive(true);

        // Set UI text
        if (keyHintText != null)
            keyHintText.text = keyText;

        if (labelText != null)
            labelText.text = promptText;

        if (subLabelText != null)
            subLabelText.text = promptSubLabel;

        showingPrompt = true;
    }

    private void HidePrompt()
    {
        // Hide prompt panel
        if (promptPanel != null)
            promptPanel.SetActive(false);

        showingPrompt = false;
    }

    private void OpenDrawer()
    {
        // Prevent opening more than once
        if (isOpen)
            return;

        isOpen = true;
        HidePrompt();

        // Stop old opening coroutine if it exists
        if (openCoroutine != null)
            StopCoroutine(openCoroutine);

        // Start smooth drawer opening
        openCoroutine = StartCoroutine(OpenRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        // Smoothly move drawer until it reaches open position
        while (Vector3.Distance(drawerToMove.localPosition, openLocalPosition) > 0.01f)
        {
            drawerToMove.localPosition = Vector3.Lerp(
                drawerToMove.localPosition,
                openLocalPosition,
                Time.deltaTime * openSpeed
            );

            yield return null;
        }

        // Snap exactly to open position
        drawerToMove.localPosition = openLocalPosition;

        // Show the key after drawer opens
        if (keyObject != null)
            keyObject.SetActive(true);
    }

    public void ResetState()
    {
        // Called by loop reset system
        ResetDrawerClosed();
    }

    private void ResetDrawerClosed()
    {
        // Stop opening animation if reset happens while drawer is moving
        if (openCoroutine != null)
        {
            StopCoroutine(openCoroutine);
            openCoroutine = null;
        }

        // Reset drawer state
        isOpen = false;
        showingPrompt = false;

        // Move drawer back to closed position
        if (drawerToMove != null)
            drawerToMove.localPosition = closedLocalPosition;

        // Hide key again
        if (keyObject != null)
            keyObject.SetActive(false);

        // Hide prompt
        HidePrompt();
    }
}