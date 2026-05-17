using UnityEngine;

public class KeypadCloseUpMode : MonoBehaviour
{
    [Header("Raycast")]
    public Camera playerCamera;
    public float interactDistance = 5f;
    public string keypadTag = "Keypad";

    [Header("Close Up Keypad")]
    public GameObject closeUpKeypad;

    [Header("Optional: Disable movement while using keypad")]
    public Behaviour[] disableWhileOpen;

    private bool isOpen = false;

    void Start()
    {
        if (closeUpKeypad != null)
            closeUpKeypad.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!isOpen)
        {
            CheckForKeypad();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseKeypad();
            }
        }
    }

    void CheckForKeypad()
    {
        if (playerCamera == null)
            return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            Debug.Log("Ray hit: " + hit.collider.name + " | Tag: " + hit.collider.tag);

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
        isOpen = true;

        if (closeUpKeypad != null)
            closeUpKeypad.SetActive(true);

        foreach (Behaviour behaviour in disableWhileOpen)
        {
            if (behaviour != null)
                behaviour.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Keypad close-up opened.");
    }

    public void CloseKeypad()
    {
        isOpen = false;

        if (closeUpKeypad != null)
            closeUpKeypad.SetActive(false);

        foreach (Behaviour behaviour in disableWhileOpen)
        {
            if (behaviour != null)
                behaviour.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Keypad close-up closed.");
    }
}