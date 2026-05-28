using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TypingInputControlFix : MonoBehaviour
{
    [Header("Notebook Input Field")]
    public TMP_InputField inputField;

    [Header("Player Scripts To Disable While Typing")]
    public MonoBehaviour[] scriptsToDisable;

    private bool isTyping = false;

    void Update()
    {
        if (inputField == null)
            return;

        bool selected =
            EventSystem.current != null &&
            EventSystem.current.currentSelectedGameObject == inputField.gameObject;

        if (selected)
        {
            if (!isTyping)
            {
                isTyping = true;
                SetScripts(false);
            }
        }
        else
        {
            if (isTyping)
            {
                isTyping = false;
                SetScripts(true);
            }
        }
    }

    private void OnDisable()
    {
        isTyping = false;
        SetScripts(true);
    }

    private void OnDestroy()
    {
        isTyping = false;
        SetScripts(true);
    }

public void ForceStopTyping()
{
    isTyping = false;
    SetScripts(true);

    if (inputField != null)
    {
        inputField.text = inputField.text;
        inputField.DeactivateInputField();
        inputField.ReleaseSelection();
    }

    if (EventSystem.current != null)
        EventSystem.current.SetSelectedGameObject(null);

    if (UIStateManager.Instance != null)
        UIStateManager.Instance.CloseNotebook();
}

    private void SetScripts(bool enabled)
    {
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = enabled;
        }
    }
}