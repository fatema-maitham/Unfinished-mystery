using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Central puzzle state manager for Level 1.
/// Attach to an empty GameObject called "Level1PuzzleSystem" in the scene.
/// All puzzle objects read from and report to this singleton.
/// </summary>
public class Level1PuzzleSystem : MonoBehaviour
{
    public static Level1PuzzleSystem Instance { get; private set; }

    // ── Puzzle State Flags ────────────────────────────────────────────────────
    [Header("Puzzle State (read-only in play mode)")]
    [SerializeField] private bool _deskPhaseComplete      = false; // Puzzle 1: coffee mug + exam + memo
    [SerializeField] private bool _bookshelfPhaseComplete = false; // Puzzle 2: whiteboard + book + note
    [SerializeField] private bool _drawerUnlocked         = false; // Puzzle 3: drawer code 433
    [SerializeField] private bool _usbFound               = false; // Puzzle 4: USB retrieved from drawer
    [SerializeField] private bool _laptopBooted           = false; // Puzzle 5: USB inserted into laptop
    [SerializeField] private bool _fileDecrypted          = false; // Puzzle 6: password 58 entered
    [SerializeField] private bool _levelComplete          = false; // Final: evidence sent

    // ── Events (other scripts can subscribe) ──────────────────────────────────
    [Header("Events")]
    public UnityEvent onDeskPhaseComplete;
    public UnityEvent onBookshelfPhaseComplete;
    public UnityEvent onDrawerUnlocked;
    public UnityEvent onUSBFound;
    public UnityEvent onLaptopBooted;
    public UnityEvent onFileDecrypted;
    public UnityEvent onLevelComplete;

    // ── Public Readers ────────────────────────────────────────────────────────
    public bool DeskPhaseComplete      => _deskPhaseComplete;
    public bool BookshelfPhaseComplete => _bookshelfPhaseComplete;
    public bool DrawerUnlocked         => _drawerUnlocked;
    public bool USBFound               => _usbFound;
    public bool LaptopBooted           => _laptopBooted;
    public bool FileDecrypted          => _fileDecrypted;
    public bool LevelComplete          => _levelComplete;

    // ── Unity ─────────────────────────────────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // ── State Setters (called by puzzle objects) ──────────────────────────────

    public void CompleteDeskPhase()
    {
        if (_deskPhaseComplete) return;
        _deskPhaseComplete = true;
        Debug.Log("[PuzzleSystem] Desk phase complete.");
        onDeskPhaseComplete?.Invoke();
    }

    public void CompleteBookshelfPhase()
    {
        if (_bookshelfPhaseComplete) return;
        _bookshelfPhaseComplete = true;
        Debug.Log("[PuzzleSystem] Bookshelf phase complete.");
        onBookshelfPhaseComplete?.Invoke();
    }

    public void UnlockDrawer()
    {
        if (_drawerUnlocked) return;
        _drawerUnlocked = true;
        Debug.Log("[PuzzleSystem] Drawer unlocked.");
        onDrawerUnlocked?.Invoke();
    }

    public void CollectUSB()
    {
        if (_usbFound) return;
        _usbFound = true;
        Debug.Log("[PuzzleSystem] USB collected.");
        onUSBFound?.Invoke();
    }

    public void BootLaptop()
    {
        if (_laptopBooted) return;
        _laptopBooted = true;
        Debug.Log("[PuzzleSystem] Laptop booted.");
        onLaptopBooted?.Invoke();
    }

    public void DecryptFile()
    {
        if (_fileDecrypted) return;
        _fileDecrypted = true;
        Debug.Log("[PuzzleSystem] File decrypted.");
        onFileDecrypted?.Invoke();
    }

    public void CompleteLevel()
    {
        if (_levelComplete) return;
        _levelComplete = true;
        Debug.Log("[PuzzleSystem] Level complete!");
        onLevelComplete?.Invoke();
    }

    // ── Helper: blocked message ───────────────────────────────────────────────
    /// <summary>
    /// Call this from any puzzle object when the player tries to interact too early.
    /// Pass a custom hint or leave empty for the default.
    /// </summary>
    public static void ShowBlocked(string hint = "")
    {
        string msg = string.IsNullOrEmpty(hint)
            ? "You can't do this yet. Look around more."
            : hint;
        ActivationDialogUI.ShowText(msg);
    }
}