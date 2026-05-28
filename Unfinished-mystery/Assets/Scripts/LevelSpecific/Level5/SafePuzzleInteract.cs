using UnityEngine;

public class SafePuzzleInteract : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform player;
    public float interactDistance = 2.5f;
    public KeyCode playKey = KeyCode.E;

    [Header("Prompt UI")]
    public GameObject promptUI;
    public string label = "Unlock";
    public string subLabel = "Safe";

    [Header("Puzzle Dependency")]
    // Drag your Note/Clue UI panel here to verify if the player looked at it first!
    public GameObject clueCheckPanel;

    [Header("Success Feedback")]
    public AudioSource successAudio;
    public GameObject rewardObject;

    private bool isPlayerNearby = false;
    private bool promptShowing = false;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        isPlayerNearby = distance <= interactDistance;

        // Manage UI Interaction Prompts seamlessly
        if (isPlayerNearby && !promptShowing)
        {
            // Call your UI manager here to show text matching your custom Label & SubLabel
            promptUI.SetActive(true);
            promptShowing = true;
        }
        else if (!isPlayerNearby && promptShowing)
        {
            promptUI.SetActive(false);
            promptShowing = false;
        }

        // Action trigger execution
        if (isPlayerNearby && Input.GetKeyDown(playKey))
        {
            ExecuteInteraction();
        }
    }

    void ExecuteInteraction()
    {
        if (clueCheckPanel != null && !clueCheckPanel.activeSelf)
        {
            Debug.Log("The safe is locked. I need to find the combination clue first.");
            return;
        }

        // ppuzzle success state
        Debug.Log("Safe Unlocked successfully!");
        if (successAudio != null) successAudio.Play();
        if (rewardObject != null) rewardObject.SetActive(true);

        this.enabled = false;
        if (promptUI != null) promptUI.SetActive(false);
    }
}