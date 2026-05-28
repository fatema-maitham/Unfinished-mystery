using TMPro;
using UnityEngine;

public class KeypadUI : MonoBehaviour, ILoopResettable
{
    [Header("Code")]
    [SerializeField] private string correctCode = "258";
    [SerializeField] private int maxDigits = 3;

    [Header("UI")]
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private TMP_Text resultText;

    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip deniedSound;
    [SerializeField] private AudioClip grantedSound;

    [Header("Drawer")]
    [SerializeField] private Animator drawerAnimator;
    [SerializeField] private MonoBehaviour drawerInteractableUI;

    private string currentCode = "";
    private bool solvedThisLoop = false;

    private void OnEnable()
    {
        if (solvedThisLoop)
        {
            gameObject.SetActive(false);
            return;
        }

        currentCode = "";

        if (displayText != null)
            displayText.text = "";

        if (resultText != null)
            resultText.text = "";

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            CloseKeypad();
    }

    public void PressKey(string number)
    {
        if (currentCode.Length >= maxDigits)
            return;

        currentCode += number;

        if (displayText != null)
            displayText.text = currentCode;

        if (resultText != null)
            resultText.text = "";

        PlaySound(buttonClickSound);
    }

    public void ClearCode()
    {
        currentCode = "";

        if (displayText != null)
            displayText.text = "";

        if (resultText != null)
            resultText.text = "";

        PlaySound(buttonClickSound);
    }

    public void EnterCode()
    {
        if (currentCode == correctCode)
        {
            solvedThisLoop = true;

            if (resultText != null)
            {
                resultText.text = "GRANTED";
                resultText.color = Color.green;
            }

            PlaySound(grantedSound);

            if (drawerAnimator != null)
                drawerAnimator.SetTrigger("Open");

            CloseKeypad();
        }
        else
        {
            if (resultText != null)
            {
                resultText.text = "DENIED";
                resultText.color = Color.red;
            }

            currentCode = "";

            if (displayText != null)
                displayText.text = "";

            PlaySound(deniedSound);
        }
    }

    public void CloseKeypad()
    {
       // Cursor.visible = false;
       // Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;

        gameObject.SetActive(false);
    }

    public void ResetState()
    {
        solvedThisLoop = false;
        currentCode = "";

        if (displayText != null)
            displayText.text = "";

        if (resultText != null)
            resultText.text = "";

        //Cursor.visible = false;
       // Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;

        gameObject.SetActive(false);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}