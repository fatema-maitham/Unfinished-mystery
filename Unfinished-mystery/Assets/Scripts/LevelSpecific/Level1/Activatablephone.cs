using UnityEngine;

/// <summary>
/// Puzzle 7 — The Phone (Final Step)
/// The prompt does NOT appear until the file is decrypted — CanActivate
/// reads FileDecrypted from the puzzle manager every frame.
/// The phoneGlowVFX (your StarBlink prefab) activates at the same moment.
/// Sending the evidence fires CompleteLevel() and ends the loop.
/// </summary>





public class ActivatablePhone : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label            = "Send Evidence";
    [SerializeField] private string subLabel         = "Phone";
    [SerializeField] private float  activationRadius = 1.5f;

    [Header("Content")]
    [SerializeField] private Sprite phoneScreenImage;
    [TextArea(2, 4)]
    [SerializeField] private string completionText =
        "You forward the plagiarism evidence to the university board.\n\n" +
        "Loop broken.\n\nLevel complete.";

    [Header("Glow VFX")]
    [SerializeField] private GameObject phoneGlowVFX;

    [Header("Summary Transition")]
    [SerializeField] private string summarySceneName = "LevelSummary1";
    [SerializeField] private int    maxLoops         = 5;
    [SerializeField] private float  sceneLoadDelay   = 2f; // seconds after dialog shows

    private bool _sent = false;

    public string ActivationLabel  => label;
    public string ActivationHint   => subLabel;
    public bool   CanActivate      => Level1PuzzleSystem.Instance != null
                                   && Level1PuzzleSystem.Instance.FileDecrypted;
    public float  ActivationRadius => activationRadius;

    private void Start()
    {
        if (phoneGlowVFX != null)
            phoneGlowVFX.SetActive(false);
    }

    private void Update()
    {
        if (phoneGlowVFX != null)
        {
            bool shouldGlow = Level1PuzzleSystem.Instance != null
                           && Level1PuzzleSystem.Instance.FileDecrypted;
            if (phoneGlowVFX.activeSelf != shouldGlow)
                phoneGlowVFX.SetActive(shouldGlow);
        }
    }

    public void OnActivate(GameObject source)
    {
        if (_sent)
        {
            ActivationDialogUI.ShowText("The evidence has already been sent.", "Phone");
            return;
        }

        _sent = true;
        Level1PuzzleSystem.Instance?.CompleteLevel();

        // Store loop data for the summary scene
        LoopChangeSystem loopSystem = FindFirstObjectByType<LoopChangeSystem>();
        int finalLoops = loopSystem != null ? loopSystem.currentLoop : 1;
        finalLoops = Mathf.Clamp(finalLoops, 1, maxLoops);

        LevelResultData.loopsUsed = finalLoops;
        LevelResultData.LoopsUsed = finalLoops;
        LevelResultData.MaxLoops  = maxLoops;

        // Show dialog, then load summary after delay
        if (phoneScreenImage != null)
            ActivationDialogUI.ShowImage(phoneScreenImage);
        else
            ActivationDialogUI.ShowText(completionText, "Phone");

        StartCoroutine(LoadSummaryAfterDelay());
    }

    private System.Collections.IEnumerator LoadSummaryAfterDelay()
    {
        yield return new WaitForSeconds(sceneLoadDelay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(summarySceneName);
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}