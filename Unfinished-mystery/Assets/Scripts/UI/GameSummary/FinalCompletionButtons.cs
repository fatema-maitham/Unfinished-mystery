using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalCompletionButtons : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text totalLoopsText;

    [Header("Star Images")]
    [SerializeField] private GameObject star1;
    [SerializeField] private GameObject star2;
    [SerializeField] private GameObject star3;

    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string playAgainSceneName = "Level1";

    private void Start()
    {
        int totalLoops = 0;

        for (int i = 1; i <= 5; i++)
            totalLoops += PlayerPrefs.GetInt("Level" + i + "Loops", 1);

        totalLoopsText.text = "Total LOOPS USED: " + totalLoops + " / 25";

        ShowStars(GetStarCount(totalLoops));
    }

    private int GetStarCount(int totalLoops)
    {
        if (totalLoops <= 5) return 3;
        if (totalLoops <= 15) return 2;
        return 1;
    }

    private void ShowStars(int stars)
    {
        star1.SetActive(stars >= 1);
        star2.SetActive(stars >= 2);
        star3.SetActive(stars >= 3);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(playAgainSceneName);
    }
}