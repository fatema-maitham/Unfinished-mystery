using UnityEngine;

// Unlocks the mouse with E and locks it again with Escape
public class KeypadMouseUnlock : MonoBehaviour
{
    private bool mouseFree = false; // Tracks if the mouse is currently unlocked

    void Update()
    {
        // Press E to unlock and show the mouse
        if (Input.GetKeyDown(KeyCode.E))
        {
            mouseFree = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Mouse unlocked for keypad.");
        }

        // Press Escape to lock and hide the mouse again
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mouseFree = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("Mouse locked.");
        }
    }
}