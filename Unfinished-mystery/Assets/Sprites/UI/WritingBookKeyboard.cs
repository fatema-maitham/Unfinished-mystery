using UnityEngine;
using TMPro;

public class WritingBookKeyboard : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject canvasWriting;
    [SerializeField] private TMP_InputField writingInputField;

    [Header("Key")]
    [SerializeField] private KeyCode openCloseKey = KeyCode.N;

    private bool isOpen;

    private void Start()
    {
        canvasWriting.SetActive(false);
        isOpen = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(openCloseKey))
        {
            if (isOpen)
                CloseBook();
            else
                OpenBook();
        }
    }

    public void OpenBook()
    {
        isOpen = true;
        canvasWriting.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        writingInputField.ActivateInputField();
        writingInputField.Select();
    }

    public void CloseBook()
    {
        isOpen = false;
        canvasWriting.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ClearWriting()
    {
        writingInputField.text = "";
        writingInputField.ActivateInputField();
    }
}