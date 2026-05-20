using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private TMP_Text actionText;

    [Header("Default Settings")]
    [SerializeField] private string interactionKey = "E";

    private void Awake()
    {
        if (keyText != null)
        {
            keyText.text = interactionKey;
        }

        HidePrompt();
    }

    public void ShowPrompt(string action)
    {
        if (promptPanel != null)
        {
            promptPanel.SetActive(true);
        }

        if (keyText != null)
        {
            keyText.text = interactionKey;
        }

        if (actionText != null)
        {
            actionText.text = action;
        }
    }

    public void HidePrompt()
    {
        if (promptPanel != null)
        {
            promptPanel.SetActive(false);
        }
    }
}