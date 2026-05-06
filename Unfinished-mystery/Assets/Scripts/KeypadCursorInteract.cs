using UnityEngine;

public class KeypadCursorInteract : MonoBehaviour
{
    [Header("Optional Prompt")]
    public GameObject pressEText;

    private bool playerNearby;
    private bool keypadMode;

    void Start()
    {
        if (pressEText != null)
            pressEText.SetActive(false);

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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ExitKeypadMode()
    {
        keypadMode = false;

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