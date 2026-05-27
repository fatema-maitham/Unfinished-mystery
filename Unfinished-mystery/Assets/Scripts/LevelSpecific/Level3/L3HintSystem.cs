using UnityEngine;
using TMPro;
using System.Collections;

public class L3HintSystem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject hintPanel;
    [SerializeField] private TMP_Text hintBodyText;

    [Header("Progress")]
    [SerializeField] private FilmProjectorUse projectorUse;
    [SerializeField] private bool accessGranted;

    [Header("Settings")]
    [SerializeField] private float autoHideTime = 5f;

    private Coroutine hideCoroutine;

    private void Start()
    {
        if (hintPanel != null)
            hintPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ShowHint();
        }
    }

    public void ShowHint()
    {
        if (hintPanel == null || hintBodyText == null)
            return;

        hintPanel.SetActive(true);
        hintBodyText.text = GetCurrentHint();

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideHintAfterDelay());
    }

    public void SetAccessGranted()
    {
        accessGranted = true;
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

    private IEnumerator HideHintAfterDelay()
    {
        yield return new WaitForSeconds(autoHideTime);

        if (hintPanel != null)
            hintPanel.SetActive(false);
    }
}