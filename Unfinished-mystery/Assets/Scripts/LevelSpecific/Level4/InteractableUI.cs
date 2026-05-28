using UnityEngine;

public class InteractableUI : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    public float interactDistance = 3f;

    [Header("References")]
    public GameObject uiCanvas;
    public ActivationPromptUI activationPrompt;

    [Header("Prompt Text")]
    public string labelText = "Examine";
    public string subLabelText = "";

    private bool isOpen = false;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        bool playerInRange = distance <= interactDistance;

        if (playerInRange && !isOpen)
            activationPrompt.ShowPrompt(labelText, subLabelText);
        else
            activationPrompt.HidePrompt();

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
            uiCanvas.SetActive(isOpen);

            if (isOpen)
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
}