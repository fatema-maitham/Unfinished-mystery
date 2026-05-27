using UnityEngine;

public class NotebookToggle : MonoBehaviour
{
    public GameObject notebookPanel;
    public KeyCode toggleKey = KeyCode.Tab;

    [Header("Book Icon Corner Detection")]
    public RectTransform bookIconRect;
    public Camera uiCamera; // Leave null for Screen Space Overlay

    void Start()
    {
        if (notebookPanel != null)
            notebookPanel.SetActive(false);
    }

    void Update()
    {
        // Tab toggles notebook
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleNotebook();
            return;
        }

        // ESC closes notebook (but NOT pause — PauseMenuController handles ESC separately)
        if (Input.GetKeyDown(KeyCode.Escape) && notebookPanel.activeSelf)
        {
            CloseNotebook();
        }
    }

    public void ToggleNotebook()
    {
        if (notebookPanel.activeSelf)
            CloseNotebook();
        else
            OpenNotebook();
    }

    public void OpenNotebook()
    {
        notebookPanel.SetActive(true);
        UIStateManager.Instance.OpenNotebook();
    }

    public void CloseNotebook()
    {
        notebookPanel.SetActive(false);
        UIStateManager.Instance.CloseNotebook();
    }
}