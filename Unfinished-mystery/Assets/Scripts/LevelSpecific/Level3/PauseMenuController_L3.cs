using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class PauseMenuController_L3 : MonoBehaviour
{
    [Header("Pause UI")]
    public GameObject pauseCanvas;

    [Header("Player Controls To Restore")]
    public CharacterMovement characterMovement;
    public ThirdPersonCamera thirdPersonCamera;

    [Header("Icon Objects")]
    public GameObject soundIconOnObject;
    public GameObject soundIconMuteObject;
    public GameObject musicIconOnObject;
    public GameObject musicIconMuteObject;

    [Header("UI Audio")]
    public AudioSource uiAudioSource;
    public AudioClip hoverClip;
    public AudioClip clickClip;

    [Header("Optional Level Music")]
    public AudioSource backgroundMusicSource;

    private bool isPaused = false;
    private bool soundMuted = false;
    private bool musicMuted = false;

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
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void LateUpdate()
    {
        if (!isPaused) return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PauseGame()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);

        isPaused = true;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (UIStateManager.Instance != null)
            UIStateManager.Instance.OpenPause();

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

        isPaused = false;
        Time.timeScale = 1f;

        while (Mouse.current != null && Mouse.current.leftButton.isPressed)
            yield return null;

        yield return new WaitForEndOfFrame();

        if (UIStateManager.Instance != null)
            UIStateManager.Instance.ClosePause();

        if (thirdPersonCamera != null)
            thirdPersonCamera.ExitUIMode();

        if (backgroundMusicSource != null && !musicMuted)
            backgroundMusicSource.UnPause();
    }

    public void RestartLevel()
    {
        StartCoroutine(RestartLevelAfterClick());
    }

    private IEnumerator RestartLevelAfterClick()
    {
        yield return new WaitForSecondsRealtime(0.15f);

        Time.timeScale = 1f;
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

    public void ToggleSound()
{
    soundMuted = !soundMuted;

    if (uiAudioSource != null)
        uiAudioSource.mute = soundMuted;

    UpdateAudioIcons();
}

    public void ToggleMusic()
    {
        musicMuted = !musicMuted;

        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.mute = musicMuted;
        }

        UpdateAudioIcons();
    }

    private void UpdateAudioIcons()
    {
        if (soundIconOnObject != null)
            soundIconOnObject.SetActive(!soundMuted);

        if (soundIconMuteObject != null)
            soundIconMuteObject.SetActive(soundMuted);

        if (musicIconOnObject != null)
            musicIconOnObject.SetActive(!musicMuted);

        if (musicIconMuteObject != null)
            musicIconMuteObject.SetActive(musicMuted);
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
}