using UnityEngine;

public class KeypadInteractionZone : MonoBehaviour
{
    [Header("Player")]
    public GameObject playerController;

    [Header("Keypad Camera Optional")]
    public Camera playerCamera;
    public Camera keypadCamera;

    [Header("Prompt UI Optional")]
    public GameObject pressEText;

    private bool playerNearby = false;
    private bool keypadMode = false;

    void Start()
    {
        if (pressEText != null)
            pressEText.SetActive(false);

        if (keypadCamera != null)
            keypadCamera.enabled = false;

        if (playerCamera != null)
            playerCamera.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            EnterKeypadMode();
        }

        if (keypadMode && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitKeypadMode();
        }
    }

    void EnterKeypadMode()
    {
        keypadMode = true;

        if (pressEText != null)
            pressEText.SetActive(false);

        if (playerController != null)
            playerController.SetActive(false);

        if (playerCamera != null)
            playerCamera.enabled = false;

        if (keypadCamera != null)
            keypadCamera.enabled = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitKeypadMode()
    {
        keypadMode = false;

        if (playerController != null)
            playerController.SetActive(true);

        if (playerCamera != null)
            playerCamera.enabled = true;

        if (keypadCamera != null)
            keypadCamera.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            if (pressEText != null)
                pressEText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;

            if (pressEText != null)
                pressEText.SetActive(false);
        }
    }
}