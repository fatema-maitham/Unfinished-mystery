using UnityEngine;

// ═══════════════════════════════════════════════════════════════════════════════
// LEVEL RESULT DATA
// Stores the summary information from the completed level.
// The LevelSummary scene reads this data and displays it on the UI.
// ═══════════════════════════════════════════════════════════════════════════════
public static class LevelResultData
{
    public static string title;
    public static string levelName;
    public static string characterName;
    public static string role;
    public static int loopsUsed;
    public static string resultMessage;
    public static Sprite portrait;

    public static string nextSceneName;
    public static string replaySceneName;
}