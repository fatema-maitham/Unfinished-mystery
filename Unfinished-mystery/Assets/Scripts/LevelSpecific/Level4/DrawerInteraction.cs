using UnityEngine;

public class DrawerInteraction : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    public float interactDistance = 3f;

    [Header("References")]
    public GameObject keypadCanvas;
    public ActivationPromptUI activationPrompt;

    private bool isOpen = false;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(
            player.position, transform.position);
        bool playerInRange = distance <= interactDistance;

        if (playerInRange && !isOpen)
            activationPrompt.ShowPrompt("Drawer", "Enter Code");
        else
            activationPrompt.HidePrompt();

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
            keypadCanvas.SetActive(isOpen);
        }
    }
}