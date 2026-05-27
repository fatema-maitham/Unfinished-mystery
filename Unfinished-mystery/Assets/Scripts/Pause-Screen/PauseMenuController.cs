using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class PauseMenuController : MonoBehaviour
{
    [Header("Pause UI")]
    public GameObject pauseCanvas;

    [Header("Player Controls To Restore")]
    public CharacterMovement characterMovement;
    public ThirdPersonCamera thirdPersonCamera;

    [Header("Icon Images")]
    public Image soundIconImage;
    public Image musicIconImage;

    [Header("Sound Icons")]
    public Sprite soundOnIcon;
    public Sprite soundOffIcon;

    [Header("Music Icons")]
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;

    [Header("UI Audio")]
    public AudioSource uiAudioSource;
    public AudioClip hoverClip;
    public AudioClip clickClip;

    [Header("Optional Level Music")]
    public AudioSource backgroundMusicSource;

    [Header("Optional Levels Scene")]
    public string levelsSceneName = "";

    private bool isPaused    = false;
    private bool soundMuted  = false;
    private bool musicMuted  = false;

    private void Start()
    {
        Time.timeScale = 1f;

        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);

        UpdateAudioIcons();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) ResumeGame();
            else          PauseGame();
        }
    }

    // ── Pause / Resume ────────────────────────────────────────────────────────

    public void PauseGame()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);

        isPaused       = true;
        Time.timeScale = 0f;

        UIStateManager.Instance.OpenPause();   // ← cursor handled here

        if (backgroundMusicSource != null && !musicMuted)
            backgroundMusicSource.Pause();
    }

    public void ResumeGame()
    {
        StartCoroutine(ResumeGameRoutine());
    }

    private IEnumerator ResumeGameRoutine()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);

        isPaused       = false;
        Time.timeScale = 1f;

        // Wait until the mouse button that clicked Resume is released
        while (Mouse.current != null && Mouse.current.leftButton.isPressed)
            yield return null;

        yield return new WaitForEndOfFrame();

        UIStateManager.Instance.ClosePause();  // ← cursor handled here

        if (thirdPersonCamera != null)
            thirdPersonCamera.ExitUIMode();

        if (backgroundMusicSource != null && !musicMuted)
            backgroundMusicSource.UnPause();
    }

    // ── Navigation ────────────────────────────────────────────────────────────

    public void RestartLevel()
    {
        StartCoroutine(RestartLevelAfterClick());
    }

    private IEnumerator RestartLevelAfterClick()
    {
        yield return new WaitForSecondsRealtime(0.15f);

        Time.timeScale   = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToLevels()
    {
        Time.timeScale = 1f;

        if (UIStateManager.Instance != null)
            UIStateManager.Instance.ClosePause();

        SceneManager.LoadScene("LevelsBook");
    }


        public void ExitGame()
    {
        Time.timeScale = 1f;

        if (UIStateManager.Instance != null)
            UIStateManager.Instance.ClosePause();

        SceneManager.LoadScene("MainMenu");
    }


    // ── Audio ─────────────────────────────────────────────────────────────────

    public void ToggleSound()
    {
        soundMuted            = !soundMuted;
        AudioListener.volume  = soundMuted ? 0f : 1f;
        UpdateAudioIcons();
    }

    public void ToggleMusic()
    {
        musicMuted = !musicMuted;

        AudioSource[] all = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource a in all)
            if (a != null) a.mute = musicMuted;

        UpdateAudioIcons();
    }

    public void PlayHoverSound()
    {
        if (!soundMuted && uiAudioSource != null && hoverClip != null)
            uiAudioSource.PlayOneShot(hoverClip);
    }

    public void PlayClickSound()
    {
        if (!soundMuted && uiAudioSource != null && clickClip != null)
            uiAudioSource.PlayOneShot(clickClip);
    }

    private void UpdateAudioIcons()
    {
        if (soundIconImage != null)
            soundIconImage.sprite = soundMuted ? soundOffIcon : soundOnIcon;

        if (musicIconImage != null)
            musicIconImage.sprite = musicMuted ? musicOffIcon : musicOnIcon;
    }
}