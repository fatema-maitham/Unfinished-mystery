using UnityEngine;

/// <summary>
/// Single source of truth for UI mode across Pause, Notebook, and any future UI.
/// All scripts call UIStateManager instead of setting Cursor state directly.
/// </summary>
public class UIStateManager : MonoBehaviour
{
    public static UIStateManager Instance { get; private set; }

    private bool _pauseOpen    = false;
    private bool _notebookOpen = false;

    public event System.Action<bool> OnPauseStateChanged;
    public event System.Action<bool> OnNotebookStateChanged;

    public bool IsAnyUIOpen => _pauseOpen || _notebookOpen;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        // Authoritative starting state: game is running, cursor locked
        _pauseOpen    = false;
        _notebookOpen = false;
        ApplyCursorState();
    }

    // ── Pause ─────────────────────────────────────────────────────────────────

    public void OpenPause()
    {
        if (_pauseOpen) return;
        _pauseOpen = true;
        ApplyCursorState();
        OnPauseStateChanged?.Invoke(true);
    }

    public void ClosePause()
    {
        if (!_pauseOpen) return;
        _pauseOpen = false;
        ApplyCursorState();
        OnPauseStateChanged?.Invoke(false);
    }

    // ── Notebook ──────────────────────────────────────────────────────────────

    public void OpenNotebook()
    {
        if (_notebookOpen) return;
        _notebookOpen = true;
        ApplyCursorState();
        OnNotebookStateChanged?.Invoke(true);
    }

    public void CloseNotebook()
    {
        if (!_notebookOpen) return;
        _notebookOpen = false;
        ApplyCursorState();
        OnNotebookStateChanged?.Invoke(false);
    }

    // ── Internal ──────────────────────────────────────────────────────────────

    private void ApplyCursorState()
    {
        if (IsAnyUIOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible   = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;
        }
    }
}