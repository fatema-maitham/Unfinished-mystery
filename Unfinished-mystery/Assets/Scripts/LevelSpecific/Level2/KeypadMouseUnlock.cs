using UnityEngine;

public class KeypadMouseUnlock : MonoBehaviour
{
    private bool mouseFree = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            mouseFree = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Mouse unlocked for keypad.");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mouseFree = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("Mouse locked.");
        }
    }
}