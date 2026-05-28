using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FakeLLMNoteSystem : MonoBehaviour
{
    [Header("Fake LLM UI")]
    [SerializeField] private GameObject fakeLLMMessagePanel;
    [SerializeField] private TMP_Text fakeLLMMessageText;

    [Header("Level Settings")]
    [Range(1, 5)]
    [SerializeField] private int levelNumber = 1;

    [Header("Loop Settings")]
    [SerializeField] private int currentLoop = 1;

    private void Start()
    {
        levelNumber = GetLevelNumberFromScene();
        currentLoop = 1;

        if (fakeLLMMessagePanel != null)
            fakeLLMMessagePanel.SetActive(true);

        if (fakeLLMMessageText != null)
            fakeLLMMessageText.text = GetMessage(levelNumber, currentLoop);
    }

    public void ShowFakeMessageForNextLoop()
    {
        currentLoop++;

        if (fakeLLMMessagePanel == null || fakeLLMMessageText == null)
            return;

        fakeLLMMessagePanel.SetActive(true);
        fakeLLMMessageText.text = GetMessage(levelNumber, currentLoop);
    }

    public void HideFakeMessage()
    {
        if (fakeLLMMessagePanel != null)
            fakeLLMMessagePanel.SetActive(false);
    }

    private int GetLevelNumberFromScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Contains("Level1")) return 1;
        if (sceneName.Contains("Level2")) return 2;
        if (sceneName.Contains("Level3")) return 3;
        if (sceneName.Contains("Level4")) return 4;
        if (sceneName.Contains("Level5")) return 5;

        return levelNumber;
    }

    private string GetMessage(int level, int loop)
    {
        if (level == 1)
        {
            if (loop == 1) return "The desk is the beginning. Check the exam, memo, and anything hidden under small objects.";
            if (loop == 2) return "The Fibonacci clue matters. The number 144 is not random.";
            if (loop == 3) return "The book points to pages and words. The words may become a drawer code.";
            if (loop == 4) return "The drawer evidence leads to the laptop. The USB may reveal what was deleted.";
            return "The final password is connected to primes and the number of students who believed in him.";
        }

        if (level == 2)
        {
            if (loop == 1) return "Start with the first note. Something in the room is refusing to be written.";
            if (loop == 2) return "The TV static is not just noise. Watch it until the hint finishes.";
            if (loop == 3) return "The painting, hidden book, gramophone, and drawer are connected.";
            if (loop == 4) return "Use the SIM card with the phone, then check the clock. The frozen time matters.";
            return "The clock stopped at 8:10. Try using that time on the final keypad.";
        }

        if (level == 3)
        {
            if (loop == 1) return "The projector needs reels. The story must be watched in order.";
            if (loop == 2) return "Reel 1 shows Maya was inside before closing. The exit may explain more.";
            if (loop == 3) return "Reel 2 reveals the exit was blocked. Search near the radio and mirror.";
            if (loop == 4) return "Reel 3 reveals the truth. Do not ignore the final frame.";
            return "The final frame shows 0427. Use it at the keypad.";
        }

        if (level == 4)
        {
            if (loop == 1) return "The old camera is the first clue. Study every photograph carefully.";
            if (loop == 2) return "The photos point toward hidden books around the room.";
            if (loop == 3) return "The highlighted words are numbers. Fifth, second, and eighth are important.";
            if (loop == 4) return "The code is 258. Use it to unlock the drawer.";
            return "Take the key from the drawer and unlock the exit door.";
        }

        if (level == 5)
        {
            if (loop == 1) return "The notebook riddle gives the first two digits of the lab code.";
            if (loop == 2) return "Oxygen and carbon are part of the answer. Think like a scientist.";
            if (loop == 3) return "Move the box. Something hidden cannot be seen under normal light.";
            if (loop == 4) return "Combine the notebook answer with the hidden two-digit clue.";
            return "Enter the full four-digit code on the keypad to escape the lab.";
        }

        return "Something from the last loop may help you escape.";
    }
}