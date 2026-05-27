using UnityEngine;
using TMPro;

public class L3HintBookSystem : MonoBehaviour
{
    [Header("Prompt")]
    [SerializeField] private InteractionPromptUI promptUI;
    [SerializeField] private string label = "Ask for Hint";
    [SerializeField] private string subLabel = "Hint";

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float activationRadius = 4f;

    [Header("Hint UI")]
    [SerializeField] private GameObject hintPanel;
    [SerializeField] private TMP_Text hintBodyText;

    [Header("Progress")]
    [SerializeField] private FilmProjectorUse projectorUse;
    [SerializeField] private bool accessGranted = false;

    private bool showingMyPrompt = false;
    private bool hintOpen = false;

    private void Start()
    {
        if (hintPanel != null)
            hintPanel.SetActive(false);
    }

    private void Update()
    {
        if (player == null || hintPanel == null)
            return;

        if (hintOpen && Input.GetKeyDown(KeyCode.E))
        {
            CloseHint();
            return;
        }

        float distance = Vector3.Distance(player.position, transform.position);
        bool playerInRange = distance <= activationRadius;

        if (playerInRange)
        {
            if (promptUI != null && !showingMyPrompt && !hintOpen)
            {
                promptUI.ShowPrompt(label, subLabel);
                showingMyPrompt = true;
            }

            if (!hintOpen && Input.GetKeyDown(KeyCode.E))
            {
                OpenHint();
            }
        }
        else
        {
            if (promptUI != null && showingMyPrompt)
            {
                promptUI.HidePrompt();
                showingMyPrompt = false;
            }
        }
    }

    public void SetAccessGranted()
    {
        accessGranted = true;
    }

    private void OpenHint()
    {
        if (hintBodyText != null)
            hintBodyText.text = GetCurrentHint();

        hintPanel.SetActive(true);
        hintOpen = true;

        if (promptUI != null)
            promptUI.HidePrompt();

        showingMyPrompt = false;
    }

    private void CloseHint()
    {
        hintPanel.SetActive(false);
        hintOpen = false;
    }

    private string GetCurrentHint()
    {
        if (projectorUse == null)
            return "Keep investigating the cinema.";

        if (!projectorUse.reel1Watched)
            return "Start with the first reel. The cinema screen is waiting.";

        if (projectorUse.reel1Watched && !projectorUse.reel2Watched)
            return "The exit area may explain what happened next.";

        if (projectorUse.reel2Watched && !projectorUse.reel3Watched)
            return "Something near the mirror is trying to guide you.";

        if (projectorUse.reel3Watched && !accessGranted)
            return "The final frame stayed on the screen for a reason.";

        if (accessGranted)
            return "The exit is open. Leave the cinema.";

        return "Keep investigating the cinema.";
    }
}