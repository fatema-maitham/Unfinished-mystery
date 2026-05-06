using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("UI Elements")]
    public Button fullscreenButton;
    public TMP_Text fullscreenText;

    public Slider musicSlider;
    public Slider sfxSlider;

    public Button applyButton;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    // Saved/applied settings
    private float savedMusic;
    private float savedSFX;
    private bool savedFullscreen;

    // Current selected setting in UI before pressing Apply
    private bool pendingFullscreen;

    private void OnEnable()
    {
        SetupSliders();
        LoadSavedSettings();
        RefreshUIFromSaved();
        ApplySavedAudio();
        ApplySavedDisplay();
        AddListeners();
        UpdateApplyButtonState();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void SetupSliders()
    {
        if (musicSlider != null)
        {
            musicSlider.minValue = 0f;
            musicSlider.maxValue = 1f;
            musicSlider.wholeNumbers = false;
        }

        if (sfxSlider != null)
        {
            sfxSlider.minValue = 0f;
            sfxSlider.maxValue = 1f;
            sfxSlider.wholeNumbers = false;
        }
    }

    private void LoadSavedSettings()
    {
        savedMusic = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        savedSFX = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        savedFullscreen = PlayerPrefs.GetInt("Fullscreen", 0) == 1;

        pendingFullscreen = savedFullscreen;
    }

    private void RefreshUIFromSaved()
    {
        RemoveListeners();

        if (musicSlider != null)
            musicSlider.value = savedMusic;

        if (sfxSlider != null)
            sfxSlider.value = savedSFX;

        RefreshFullscreenButtonText();

        AddListeners();
    }

    private void AddListeners()
    {
        if (fullscreenButton != null)
        {
            fullscreenButton.onClick.RemoveAllListeners();
            fullscreenButton.onClick.AddListener(ToggleFullscreenSelection);
        }

        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
        }

        if (applyButton != null)
        {
            applyButton.onClick.RemoveAllListeners();
            applyButton.onClick.AddListener(ApplySettings);
        }
    }

    private void RemoveListeners()
    {
        if (fullscreenButton != null)
            fullscreenButton.onClick.RemoveAllListeners();

        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveAllListeners();

        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveAllListeners();

        if (applyButton != null)
            applyButton.onClick.RemoveAllListeners();
    }

    private void RefreshFullscreenButtonText()
    {
        if (fullscreenText == null) return;

        // If fullscreen is currently selected, button shows OFF because clicking it will turn fullscreen off.
        // If windowed is currently selected, button shows ON because clicking it will turn fullscreen on.
        fullscreenText.text = pendingFullscreen ? "OFF" : "ON";
    }

    private void OnMusicSliderChanged(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;

        UpdateApplyButtonState();
    }

    private void OnSfxSliderChanged(float value)
    {
        if (sfxSource != null)
            sfxSource.volume = value;

        UpdateApplyButtonState();
    }

    private void ToggleFullscreenSelection()
    {
        pendingFullscreen = !pendingFullscreen;

        RefreshFullscreenButtonText();
        UpdateApplyButtonState();
    }

    private void ApplySavedAudio()
    {
        if (musicSource != null)
            musicSource.volume = savedMusic;

        if (sfxSource != null)
            sfxSource.volume = savedSFX;
    }

    private void ApplySavedDisplay()
    {
        FullScreenMode mode = savedFullscreen
            ? FullScreenMode.FullScreenWindow
            : FullScreenMode.Windowed;

        Screen.fullScreenMode = mode;
        Screen.fullScreen = savedFullscreen;
    }

    private void UpdateApplyButtonState()
    {
        bool musicChanged = musicSlider != null && !Mathf.Approximately(musicSlider.value, savedMusic);
        bool sfxChanged = sfxSlider != null && !Mathf.Approximately(sfxSlider.value, savedSFX);
        bool fullscreenChanged = pendingFullscreen != savedFullscreen;

        bool hasChanges = musicChanged || sfxChanged || fullscreenChanged;

        if (applyButton != null)
            applyButton.interactable = hasChanges;
    }

    public void ApplySettings()
    {
        savedMusic = musicSlider != null ? musicSlider.value : savedMusic;
        savedSFX = sfxSlider != null ? sfxSlider.value : savedSFX;
        savedFullscreen = pendingFullscreen;

        PlayerPrefs.SetFloat("MusicVolume", savedMusic);
        PlayerPrefs.SetFloat("SFXVolume", savedSFX);
        PlayerPrefs.SetInt("Fullscreen", savedFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        ApplySavedAudio();
        ApplySavedDisplay();

        UpdateApplyButtonState();

        Debug.Log("Settings applied successfully.");
    }

    public void ReturnToMenu()
    {
        // Revert UI back to saved/applied values if player did not press Apply.
        pendingFullscreen = savedFullscreen;

        RemoveListeners();

        if (musicSlider != null)
            musicSlider.value = savedMusic;

        if (sfxSlider != null)
            sfxSlider.value = savedSFX;

        RefreshFullscreenButtonText();

        AddListeners();

        ApplySavedAudio();
        ApplySavedDisplay();

        UpdateApplyButtonState();
    }
}