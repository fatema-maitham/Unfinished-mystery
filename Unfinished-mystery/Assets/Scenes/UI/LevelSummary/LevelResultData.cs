using UnityEngine;

public static class LevelResultData
{
    public static string title = "";
    public static string levelName = "";
    public static string characterName = "";
    public static string role = "";
    public static int loopsUsed = 0;
    public static string resultMessage = "";
    public static Sprite portrait = null;
    public static string nextSceneName = "";
    public static string replaySceneName = "";

    // عشان لو عندج سكربتات قديمة تستخدم هالأسماء بعد
    public static int StarsEarned = 0;
    public static int LoopsUsed = 0;
    public static int MaxLoops = 5;
    public static string LevelName = "";
    public static string ReplaySceneName = "";
    public static string ContinueSceneName = "";
}