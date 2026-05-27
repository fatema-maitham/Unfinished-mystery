using UnityEngine;
using TMPro;

public class CameraInteract : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject cameraClueUI;
    public TMP_Text promptText;

    [Header("Settings")]
    public float interactDistance = 2.5f;

    private bool isViewing = false;

    void Start()
    {
        cameraClueUI.SetActive(false);
        promptText.gameObject.SetActive(false);
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        bool nearCamera = distance <= interactDistance;

        // Show interaction prompt
        if (nearCamera && !isViewing)
        {
            promptText.gameObject.SetActive(true);
            promptText.text = "[E] View Camera";

            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenCameraUI();
            }
        }
        else if (!isViewing)
        {
            promptText.gameObject.SetActive(false);
        }

        // Close UI
        if (isViewing && Input.GetKeyDown(KeyCode.Q))
        {
            CloseCameraUI();
        }
    }

    void OpenCameraUI()
    {
        isViewing = true;

        cameraClueUI.SetActive(true);
        promptText.gameObject.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;
    }

    void CloseCameraUI()
    {
        isViewing = false;

        cameraClueUI.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;
    }
}