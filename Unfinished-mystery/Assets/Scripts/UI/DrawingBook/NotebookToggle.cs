using UnityEngine;

public class NotebookToggle : MonoBehaviour
{
    public GameObject notebookPanel;
    public KeyCode toggleKey = KeyCode.Tab; // Change to whatever key you prefer

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            ToggleNotebook();
    }

    public void ToggleNotebook()
    {
        bool isActive = !notebookPanel.activeSelf;
        notebookPanel.SetActive(isActive);

        if (isActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}