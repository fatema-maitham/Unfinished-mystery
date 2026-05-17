using UnityEngine;
using TMPro;

public class RaycastKeypadInteractor : MonoBehaviour
{
    [Header("Raycast Settings")]
    public Camera playerCamera;
    public float interactDistance = 6f;

    [Header("Keypad Settings")]
    public string keypadTag = "Keypad";
    public string correctCode = "810";

    [Header("Door")]
    public GameObject finalDoor;

    [Header("Optional UI Text")]
    public TextMeshProUGUI messageText;

    private bool enteringCode = false;
    private string currentInput = "";

    void Start()
    {
        Debug.Log("RaycastKeypadInteractor STARTED on: " + gameObject.name);

        if (messageText != null)
            messageText.text = "";
    }

    void Update()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("Player Camera is NOT assigned!");
            return;
        }

        if (!enteringCode)
        {
            CheckForKeypad();
        }
        else
        {
            HandleCodeInput();
        }
    }

    void CheckForKeypad()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactDistance, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            Debug.Log("Raycast hit: " + hit.collider.name + " | Tag: " + hit.collider.tag);

            if (hit.collider.CompareTag(keypadTag))
            {
                if (messageText != null)
                    messageText.text = "Press E to use keypad";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    enteringCode = true;
                    currentInput = "";

                    if (messageText != null)
                        messageText.text = "Enter code: ";

                    Debug.Log("KEYPAD ACTIVE. Type 810 then press Enter.");
                }

                return;
            }
        }
        else
        {
            Debug.Log("Raycast hit NOTHING.");
        }

        if (messageText != null)
            messageText.text = "";
    }

    void HandleCodeInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) AddDigit("0");
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) AddDigit("1");
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) AddDigit("2");
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) AddDigit("3");
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) AddDigit("4");
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) AddDigit("5");
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) AddDigit("6");
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) AddDigit("7");
        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) AddDigit("8");
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) AddDigit("9");

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (currentInput.Length > 0)
                currentInput = currentInput.Substring(0, currentInput.Length - 1);

            UpdateMessage();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckCode();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            enteringCode = false;
            currentInput = "";

            if (messageText != null)
                messageText.text = "";

            Debug.Log("KEYPAD CANCELLED.");
        }
    }

    void AddDigit(string digit)
    {
        if (currentInput.Length >= 4)
            return;

        currentInput += digit;
        UpdateMessage();
    }

    void UpdateMessage()
    {
        if (messageText != null)
            messageText.text = "Enter code: " + currentInput;

        Debug.Log("Current input: " + currentInput);
    }

    void CheckCode()
    {
        if (currentInput == correctCode)
        {
            if (messageText != null)
                messageText.text = "GRANTED";

            Debug.Log("ACCESS GRANTED");

            if (finalDoor != null)
                finalDoor.SetActive(false);

            enteringCode = false;
        }
        else
        {
            if (messageText != null)
                messageText.text = "DENIED";

            Debug.Log("ACCESS DENIED");

            currentInput = "";
        }
    }
}