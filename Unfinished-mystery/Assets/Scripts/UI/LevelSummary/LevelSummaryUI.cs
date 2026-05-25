using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// ═══════════════════════════════════════════════════════════════════════════════
// LEVEL SUMMARY UI
// Reads completed level data from LevelResultData.
// Displays level number, revealed identity, role, loops used, result message,
// character portrait, and star rating.
// Max loops in this game version = 5.
// Attach this script to: LevelSummaryManager.
// ═══════════════════════════════════════════════════════════════════════════════
public class LevelSummaryUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject summaryPanel;

    [Header("Texts")]
    public TMP_Text titleText;
    public TMP_Text levelText;
    public TMP_Text identityText;
    public TMP_Text roleText;
    public TMP_Text loopsText;
    public TMP_Text resultText;

    [Header("Portrait")]
    public Image portraitImage;
    public Sprite defaultPortrait;

    [Header("Stars")]
    public StarBounceUI star1;
    public StarBounceUI star2;
    public StarBounceUI star3;
    public float delayBetweenStars = 0.2f;

    private const int MaxLoops = 5;

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        
        ApplyDefaultIfNeeded();

        Sprite portraitToUse = LevelResultData.portrait != null
            ? LevelResultData.portrait
            : defaultPortrait;

        ShowSummary(
            LevelResultData.title,
            LevelResultData.levelName,
            LevelResultData.characterName,
            LevelResultData.role,
            LevelResultData.loopsUsed,
            LevelResultData.resultMessage,
            portraitToUse
        );
    }

    // ═══════════════════════════════════════════════════════════════════════════════
    // DEFAULT DATA
    // Used only if the level forgot to send data before opening LevelSummary.
    // This prevents the summary screen from appearing empty.
    // ═══════════════════════════════════════════════════════════════════════════════
    private void ApplyDefaultIfNeeded()
    {
        if (string.IsNullOrEmpty(LevelResultData.title))
            LevelResultData.title = "LEVEL COMPLETE";

        if (string.IsNullOrEmpty(LevelResultData.levelName))
            LevelResultData.levelName = "LEVEL 1";

        if (string.IsNullOrEmpty(LevelResultData.characterName))
            LevelResultData.characterName = "Kyryll Flins";

        if (string.IsNullOrEmpty(LevelResultData.role))
            LevelResultData.role = "Mathematics Professor";

        if (LevelResultData.loopsUsed < 1)
            LevelResultData.loopsUsed = 1;

        if (LevelResultData.loopsUsed > MaxLoops)
            LevelResultData.loopsUsed = MaxLoops;

        if (string.IsNullOrEmpty(LevelResultData.resultMessage))
            LevelResultData.resultMessage = "The truth has been uncovered.";

        if (string.IsNullOrEmpty(LevelResultData.nextSceneName))
            LevelResultData.nextSceneName = "Level2CharacterPage";

        if (string.IsNullOrEmpty(LevelResultData.replaySceneName))
            LevelResultData.replaySceneName = "Level1";
    }

    // ═══════════════════════════════════════════════════════════════════════════════
    // SHOW SUMMARY
    // Updates all UI elements with the level result data.
    // ═══════════════════════════════════════════════════════════════════════════════
    public void ShowSummary(
        string title,
        string level,
        string characterName,
        string role,
        int loopsUsed,
        string resultMessage,
        Sprite portrait)
    {
        loopsUsed = Mathf.Clamp(loopsUsed, 1, MaxLoops);

        if (summaryPanel != null)
            summaryPanel.SetActive(true);

        if (titleText != null)
            titleText.text = title;

        if (levelText != null)
            levelText.text = level;

        if (identityText != null)
            identityText.text = "IDENTITY REVEALED: " + characterName;

        if (roleText != null)
            roleText.text = role;

        if (loopsText != null)
            loopsText.text = "Loops Used: " + loopsUsed + " / " + MaxLoops;

        if (resultText != null)
            resultText.text = resultMessage;

        if (portraitImage != null)
        {
            Sprite finalPortrait = portrait != null ? portrait : defaultPortrait;

            portraitImage.sprite = finalPortrait;
            portraitImage.overrideSprite = finalPortrait;
            portraitImage.color = Color.white;
            portraitImage.enabled = true;
            portraitImage.preserveAspect = true;
        }

        ResetAllStars();

        if (gameObject.activeInHierarchy)
            StartCoroutine(PlayStarsRoutine(GetStarCount(loopsUsed)));
    }

    // ═══════════════════════════════════════════════════════════════════════════════
    // STAR COUNT
    // 1 loop      = 3 stars
    // 2–3 loops   = 2 stars
    // 4–5 loops   = 1 star
    // ═══════════════════════════════════════════════════════════════════════════════
    private int GetStarCount(int loopsUsed)
    {
        loopsUsed = Mathf.Clamp(loopsUsed, 1, MaxLoops);

        if (loopsUsed == 1) return 3;
        if (loopsUsed <= 3) return 2;
        return 1;
    }

    // ═══════════════════════════════════════════════════════════════════════════════
    // RESET STARS
    // Resets all stars before playing the earned star animation.
    // ═══════════════════════════════════════════════════════════════════════════════
    private void ResetAllStars()
    {
        if (star1 != null) star1.ResetStar();
        if (star2 != null) star2.ResetStar();
        if (star3 != null) star3.ResetStar();
    }

    // ═══════════════════════════════════════════════════════════════════════════════
    // PLAY STAR ANIMATION
    // Plays only the stars earned by the player.
    // ═══════════════════════════════════════════════════════════════════════════════
    private IEnumerator PlayStarsRoutine(int starCount)
    {
        if (starCount >= 1 && star1 != null)
        {
            yield return star1.PlayAnimation();
            yield return new WaitForSecondsRealtime(delayBetweenStars);
        }

        if (starCount >= 2 && star2 != null)
        {
            yield return star2.PlayAnimation();
            yield return new WaitForSecondsRealtime(delayBetweenStars);
        }

        if (starCount >= 3 && star3 != null)
        {
            yield return star3.PlayAnimation();
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════════
    // CONTINUE BUTTON
    // Loads the next scene after the summary.
    // ═══════════════════════════════════════════════════════════════════════════════
    public void Continue()
    {
        SceneManager.LoadScene(LevelResultData.nextSceneName);
    }

    // ═══════════════════════════════════════════════════════════════════════════════
    // REPLAY LEVEL BUTTON
    // Reloads the completed level.
    // ═══════════════════════════════════════════════════════════════════════════════
    public void ReplayLevel()
    {
        SceneManager.LoadScene(LevelResultData.replaySceneName);
    }
}