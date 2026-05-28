// using System.Collections;
// using UnityEngine;
// using TMPro;
// using UnityEngine.EventSystems;

// public class WritingBookKeyboard : MonoBehaviour
// {
//     [Header("UI")]
//     [SerializeField] private GameObject     canvasWriting;
//     [SerializeField] private TMP_InputField writingInputField;

//     [Header("Key")]
//     [SerializeField] private KeyCode openCloseKey = KeyCode.LeftControl;

//     private bool isOpen;

//     private void Start()
//     {
//         canvasWriting.SetActive(false);
//         isOpen = false;
//     }

//     private void Update()
//     {
//         if (Input.GetKeyDown(openCloseKey))
//             ToggleBook();

//         if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
//             CloseBook();

//         // Re-assert cursor every frame while open
//         if (isOpen)
//         {
//             Cursor.lockState = CursorLockMode.None;
//             Cursor.visible   = true;
//         }
//     }

//     public void ToggleBook()
//     {
//         if (isOpen) CloseBook();
//         else        OpenBook();
//     }

//     public void OpenBook()
//     {
//         isOpen = true;
//         canvasWriting.SetActive(true);
//         UIStateManager.Instance?.OpenNotebook();
//         StartCoroutine(FocusInputNextFrame());
//     }

//     public void CloseBook()
//     {
//         isOpen = false;
//         canvasWriting.SetActive(false);
//         UIStateManager.Instance?.CloseNotebook();
//     }

//     private IEnumerator FocusInputNextFrame()
//     {
//         // Wait one unscaled frame so the EventSystem settles
//         // before we try to activate the field — prevents cursor disappearing
//         yield return new WaitForSecondsRealtime(0.05f);

//         Cursor.lockState = CursorLockMode.None;
//         Cursor.visible   = true;

//         if (writingInputField != null)
//         {
//             EventSystem.current?.SetSelectedGameObject(writingInputField.gameObject);
//             writingInputField.ActivateInputField();
//         }
//     }

//     public void ClearWriting()
//     {
//         writingInputField.text = "";
//         StartCoroutine(FocusInputNextFrame());
//     }
// }

using UnityEngine;
using TMPro;

public class WritingBookKeyboard : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject canvasWriting;
    [SerializeField] private TMP_InputField writingInputField;

    [Header("Key")]
    [SerializeField] private KeyCode openCloseKey = KeyCode.LeftControl;

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
            ToggleBook();
        }
    }

    public void ToggleBook()
    {
        if (isOpen)
            CloseBook();
        else
            OpenBook();
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