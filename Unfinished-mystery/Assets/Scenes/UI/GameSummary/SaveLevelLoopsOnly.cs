using UnityEngine;

public class SaveLevelLoopsOnly : MonoBehaviour
{
    [Header("Which Level Is This?")]
    [SerializeField] private int levelNumber = 1;

    [Header("Loop Settings")]
    [SerializeField] private int maxLoops = 5;

    private bool saved = false;

    private void OnTriggerEnter(Collider other)
    {
        if (saved) return;
        if (!other.CompareTag("Player")) return;

        saved = true;

        LoopChangeSystem loopSystem = FindFirstObjectByType<LoopChangeSystem>();

        int loopsUsed = 1;

        if (loopSystem != null)
            loopsUsed = loopSystem.currentLoop;

        loopsUsed = Mathf.Clamp(loopsUsed, 1, maxLoops);

        PlayerPrefs.SetInt("Level" + levelNumber + "Loops", loopsUsed);
        PlayerPrefs.Save();
    }
}