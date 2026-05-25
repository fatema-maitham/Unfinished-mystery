using UnityEngine;

// ═══════════════════════════════════════════════════════════════════════════════
// LEVEL 2 — HIDDEN WALL FRAME INTERACTION
// The player inspects the suspicious wall frame.
// When E is pressed, the hidden phone number is revealed.
// Attach this script to: painting_3
// ═══════════════════════════════════════════════════════════════════════════════
public class HiddenWallFrameInteract : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Prompt UI")]
    [SerializeField] private ActivationPromptUI promptUI;

    [Header("Hidden Number Object")]
    [SerializeField] private GameObject hiddenNumberVisual;

    [Header("Prompt")]
    [SerializeField] private string label = "Inspect";
    [SerializeField] private string subLabel = "Wall Frame";

    private bool numberRevealed = false;

    private void Start()
    {
        if (hiddenNumberVisual != null)
            hiddenNumberVisual.SetActive(false);
    }

    private void Update()
    {
        if (player == null || promptUI == null)
            return;

        bool nearFrame = Vector3.Distance(player.position, transform.position) <= interactDistance;

        if (!nearFrame || numberRevealed)
        {
            promptUI.HidePrompt();
            return;
        }

        promptUI.ShowPrompt(label, subLabel);

        if (Input.GetKeyDown(interactKey))
            RevealNumber();
    }

    private void RevealNumber()
    {
        numberRevealed = true;
        promptUI.HidePrompt();

        if (hiddenNumberVisual != null)
            hiddenNumberVisual.SetActive(true);

        Level2PuzzleSystem.Instance?.FindPhoneNumber();
    }
}