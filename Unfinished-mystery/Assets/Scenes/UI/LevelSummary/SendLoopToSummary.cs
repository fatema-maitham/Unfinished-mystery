using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SendLoopToSummary : MonoBehaviour
{
    [SerializeField] private TMP_Text loopCounterText;
    [SerializeField] private string summarySceneName = "LevelSummary1";

    public void OpenSummary()
    {
        int loopNumber = 1;

        if (loopCounterText != null)
            int.TryParse(loopCounterText.text, out loopNumber);

        if (loopNumber < 1)
            loopNumber = 1;

        if (loopNumber > 5)
            loopNumber = 5;

        LevelLoopResultData.loopsUsed = loopNumber;

        SceneManager.LoadScene(summarySceneName);
    }
}