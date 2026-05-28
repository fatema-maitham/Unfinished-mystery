using UnityEngine;

// Opens a close-up keypad UI when the player looks at the keypad and presses E
public class KeypadCloseUpMode : MonoBehaviour
{
    [Header("Raycast")]
    public Camera playerCamera; // Camera used to raycast from the player's view
    public float interactDistance = 5f; // Maximum distance to interact with keypad
    public string keypadTag = "Keypad"; // Tag required on the keypad object

    [Header("Close Up Keypad")]
    public GameObject closeUpKeypad; // Close-up keypad UI/object shown when opened

    [Header("Optional: Disable movement while using keypad")]
    public Behaviour[] disableWhileOpen; // Movement/camera scripts disabled while keypad is open

    private bool isOpen = false; // Tracks if keypad close-up is currently open

    void Start()
    {
        // Hide keypad close-up at the start
        if (closeUpKeypad != null)
            closeUpKeypad.SetActive(false);

        // Start gameplay with cursor locked and hidden
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // If keypad is closed, check if player is looking at it
        if (!isOpen)
        {
            CheckForKeypad();
        }
        else
        {
            // Close keypad with Escape while it is open
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseKeypad();
            }
        }
    }

    void CheckForKeypad()
    {
        // Stop if camera is missing
        if (playerCamera == null)
            return;

        // Create ray from camera position forward
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        // Check what the player is looking at
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            // Debug shows what object the ray is hitting
            Debug.Log("Ray hit: " + hit.collider.name + " | Tag: " + hit.collider.tag);

            // If ray hits an object tagged Keypad, allow opening with E
            if (hit.collider.CompareTag(keypadTag))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    OpenKeypad();
                }
            }
        }
    }

    void OpenKeypad()
    {
        // Mark keypad as open
        isOpen = true;

        // Show close-up keypad
        if (closeUpKeypad != null)
            closeUpKeypad.SetActive(true);

        // Disable assigned player movement/camera behaviours
        foreach (Behaviour behaviour in disableWhileOpen)
        {
            if (behaviour != null)
                behaviour.enabled = false;
        }

        // Unlock cursor for keypad UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Keypad close-up opened.");
    }

    public void CloseKeypad()
    {
        // Mark keypad as closed
        isOpen = false;

        // Hide close-up keypad
        if (closeUpKeypad != null)
            closeUpKeypad.SetActive(false);

        // Re-enable assigned player movement/camera behaviours
        foreach (Behaviour behaviour in disableWhileOpen)
        {
            if (behaviour != null)
                behaviour.enabled = true;
        }

        // Lock cursor again for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Keypad close-up closed.");
    }
}