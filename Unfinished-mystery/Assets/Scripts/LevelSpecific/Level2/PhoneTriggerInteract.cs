using UnityEngine;

public class PhoneTriggerInteract : MonoBehaviour
{
    [Header("Prompt")]
    [SerializeField] private ActivationPromptUI promptUI;
    [SerializeField] private string label = "Use Phone";
    [SerializeField] private string hint = "Enter Number";

    [Header("Phone UI")]
    [SerializeField] private PhoneUI phoneUI;

    [Header("Input")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool playerInside;

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(interactKey))
        {
            promptUI.HidePrompt();
            phoneUI.OpenPhone();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        promptUI.ShowPrompt(label, hint);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        promptUI.HidePrompt();
    }
}