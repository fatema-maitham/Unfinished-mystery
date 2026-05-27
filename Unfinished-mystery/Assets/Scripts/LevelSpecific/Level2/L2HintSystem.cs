using UnityEngine;
using TMPro;
using System.Collections;

public class L2HintSystem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject hintPanel;
    [SerializeField] private TMP_Text hintBodyText;

    [Header("Progress")]
    [SerializeField] private Level2PuzzleSystem puzzleSystem;

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

    private string GetCurrentHint()
    {
        if (puzzleSystem == null)
            return "Keep looking around the room.";

        if (!puzzleSystem.FirstNoteRead)
            return "Start by checking the note on the floor.";

        if (!puzzleSystem.TVMessageSeen)
            return "The static sound is coming from something nearby.";

        if (!puzzleSystem.BookshelfClueFound)
            return "The TV message should lead you to the library area.";

        if (!puzzleSystem.GramophonePlayed)
            return "The hidden book points toward an old sound device.";

        if (!puzzleSystem.SimCardFound)
            return "Check what the drawer revealed.";

        if (!puzzleSystem.PhoneMessageHeard)
            return "The small item you found belongs in another device.";

        if (!puzzleSystem.KeypadCodeEntered)
            return "After the message, pay attention to time.";

        if (!puzzleSystem.DoorUnlocked)
            return "Use the stopped time as the door code.";

        return "You have everything you need to leave.";
    }

    private IEnumerator HideHintAfterDelay()
    {
        yield return new WaitForSeconds(autoHideTime);

        if (hintPanel != null)
            hintPanel.SetActive(false);
    }
}