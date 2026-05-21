using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NavKeypad
{
    public class Keypad : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent onAccessGranted;
        [SerializeField] private UnityEvent onAccessDenied;

        [Header("Combination Code")]
        [SerializeField] private int keypadCombo = 12345;

        [Header("Settings")]
        [SerializeField] private string accessGrantedText = "Granted";
        [SerializeField] private string accessDeniedText = "Denied";

        [Header("Visuals")]
        [SerializeField] private float displayResultTime = 1f;
        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f;

        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f);
        [SerializeField] private Color screenDeniedColor = Color.red;
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f);

        [Header("SoundFx")]
        [SerializeField] private AudioClip buttonClickedSfx;
        [SerializeField] private AudioClip accessDeniedSfx;
        [SerializeField] private AudioClip accessGrantedSfx;

        [Header("Component References")]
        [SerializeField] private Renderer panelMesh;
        [SerializeField] private TMP_Text keypadDisplayText;
        [SerializeField] private AudioSource audioSource;

        private string currentInput = "";
        private bool displayingResult = false;
        private bool accessWasGranted = false;

        private void Awake()
        {
            ClearInput();

            if (panelMesh != null)
                panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
            else
                Debug.LogError("Panel Mesh is missing!");

            if (keypadDisplayText == null)
                Debug.LogError("Keypad Display Text is missing!");
        }

        public void AddInput(string input)
        {
            Debug.Log("Keypad received input: " + input);

            if (audioSource != null && buttonClickedSfx != null)
                audioSource.PlayOneShot(buttonClickedSfx);

            if (displayingResult || accessWasGranted)
                return;

            input = input.ToLower();

            if (input == "enter")
            {
                CheckCombo();
                return;
            }

            if (currentInput.Length >= 9)
                return;

            currentInput += input;

            if (keypadDisplayText != null)
            {
                keypadDisplayText.text = currentInput;
                Debug.Log("Display updated to: " + currentInput);
            }
            else
            {
                Debug.LogError("Cannot update display. Keypad Display Text is missing!");
            }
        }

        public void CheckCombo()
        {
            Debug.Log("Checking combo: " + currentInput);

            if (int.TryParse(currentInput, out int currentKombo))
            {
                bool granted = currentKombo == keypadCombo;

                if (!displayingResult)
                    StartCoroutine(DisplayResultRoutine(granted));
            }
            else
            {
                Debug.LogWarning("Current input is not a number: " + currentInput);
            }
        }

        private IEnumerator DisplayResultRoutine(bool granted)
        {
            displayingResult = true;

            if (granted)
                AccessGranted();
            else
                AccessDenied();

            yield return new WaitForSeconds(displayResultTime);

            displayingResult = false;

            if (granted)
                yield break;

            ClearInput();

            if (panelMesh != null)
                panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
        }

        private void AccessDenied()
        {
            if (keypadDisplayText != null)
                keypadDisplayText.text = accessDeniedText;

            onAccessDenied?.Invoke();

            if (panelMesh != null)
                panelMesh.material.SetVector("_EmissionColor", screenDeniedColor * screenIntensity);

            if (audioSource != null && accessDeniedSfx != null)
                audioSource.PlayOneShot(accessDeniedSfx);
        }

        private void ClearInput()
        {
            currentInput = "";

            if (keypadDisplayText != null)
                keypadDisplayText.text = "";
        }

        private void AccessGranted()
        {
            accessWasGranted = true;

            if (keypadDisplayText != null)
                keypadDisplayText.text = accessGrantedText;

            onAccessGranted?.Invoke();

            if (panelMesh != null)
                panelMesh.material.SetVector("_EmissionColor", screenGrantedColor * screenIntensity);

            if (audioSource != null && accessGrantedSfx != null)
                audioSource.PlayOneShot(accessGrantedSfx);
        }
    }
}