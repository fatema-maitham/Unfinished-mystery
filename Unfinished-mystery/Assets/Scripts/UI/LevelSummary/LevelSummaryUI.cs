using System.Collections;
using UnityEngine;
using TMPro;

public class LevelSummaryUI : MonoBehaviour
{
    [Header("Loop Text")]
    [SerializeField] private TMP_Text loopsText;
    [SerializeField] private int maxLoops = 5;

    [Header("Stars")]
    [SerializeField] private StarBounceUI star1;
    [SerializeField] private StarBounceUI star2;
    [SerializeField] private StarBounceUI star3;
    [SerializeField] private float delayBetweenStars = 0.2f;

    private void Start()
    {
        int savedMaxLoops = LevelResultData.MaxLoops > 0 ? LevelResultData.MaxLoops : maxLoops;
        int loopsUsed = Mathf.Clamp(LevelResultData.loopsUsed, 1, savedMaxLoops);

        if (loopsText != null)
            loopsText.text = "Loops Used: " + loopsUsed + " / " + savedMaxLoops;

        ResetAllStars();
        StartCoroutine(PlayStarsRoutine(GetStarCount(loopsUsed)));
    }

    private int GetStarCount(int loopsUsed)
    {
        if (loopsUsed <= 1) return 3;
        if (loopsUsed <= 3) return 2;
        return 1;
    }

    private void ResetAllStars()
    {
        if (star1 != null) star1.ResetStar();
        if (star2 != null) star2.ResetStar();
        if (star3 != null) star3.ResetStar();
    }

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
}