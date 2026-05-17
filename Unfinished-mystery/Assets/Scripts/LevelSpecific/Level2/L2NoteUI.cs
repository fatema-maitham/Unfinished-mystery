using TMPro;
using UnityEngine;

public class L2NoteUI : MonoBehaviour
{
    [Header("Note UI")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private TMP_Text bodyText;

    [Header("Prompt UI")]
    [SerializeField] private GameObject promptObject;
    [SerializeField] private TMP_Text promptText;

    private void Start()
    {
        HideNote();
        HidePrompt();
    }

    public void ShowPrompt(string message)
    {
        if (promptObject != null)
            promptObject.SetActive(true);

        if (promptText != null)
            promptText.text = message;
    }

    public void HidePrompt()
    {
        if (promptObject != null)
            promptObject.SetActive(false);
    }

    public void ShowNote(string body)
    {
        if (notePanel != null)
            notePanel.SetActive(true);

        if (bodyText != null)
            bodyText.text = body;

        HidePrompt();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideNote()
    {
        if (notePanel != null)
            notePanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}