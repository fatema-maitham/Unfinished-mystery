using UnityEngine;
using UnityEngine.EventSystems;

public class NotebookToggle : MonoBehaviour
{
    public GameObject notebookPanel;
    public KeyCode toggleKey = KeyCode.Tab;

    [Header("Book Icon Corner Detection")]
    public RectTransform bookIconRect; // Drag your BookIcon here in Inspector
    public Camera uiCamera; // Leave empty if Canvas is Screen Space Overlay

    private bool _cursorFreedForIcon = false;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleNotebook();
            return;
        }

        // If notebook is already open, nothing to do here
        if (notebookPanel.activeSelf) return;

        // Check if mouse is hovering over the BookIcon area
        bool overIcon = IsMouseOverRect(bookIconRect);

        if (overIcon && !_cursorFreedForIcon)
        {
            _cursorFreedForIcon = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (!overIcon && _cursorFreedForIcon)
        {
            _cursorFreedForIcon = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    bool IsMouseOverRect(RectTransform rectTransform)
    {
        if (rectTransform == null) return false;

        // Temporarily free cursor position tracking
        Vector2 mousePos = Input.mousePosition;

        return RectTransformUtility.RectangleContainsScreenPoint(
            rectTransform,
            mousePos,
            uiCamera // null is fine for Screen Space Overlay
        );
    }

    public void ToggleNotebook()
    {
        bool isActive = !notebookPanel.activeSelf;
        notebookPanel.SetActive(isActive);

        if (isActive)
        {
            _cursorFreedForIcon = false;
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