using UnityEngine;
using UnityEngine.SceneManagement;

public class SendLoopToSummary : MonoBehaviour
{
    [Header("Summary Scene")]
    [SerializeField] private string summarySceneName = "Level2Summary";

    [Header("Loop Settings")]
    [SerializeField] private int maxLoops = 5;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        LoopChangeSystem loopSystem = FindFirstObjectByType<LoopChangeSystem>();

        int finalLoops = 1;

        if (loopSystem != null)
            finalLoops = loopSystem.currentLoop;

        finalLoops = Mathf.Clamp(finalLoops, 1, maxLoops);

        LevelResultData.loopsUsed = finalLoops;
        LevelResultData.LoopsUsed = finalLoops;
        LevelResultData.MaxLoops = maxLoops;

        SceneManager.LoadScene(summarySceneName);
    }
}