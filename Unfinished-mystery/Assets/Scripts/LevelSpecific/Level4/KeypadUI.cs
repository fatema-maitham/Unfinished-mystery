using TMPro;
using UnityEngine;

public class KeypadUI : MonoBehaviour
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
    private bool solved = false;

    private void OnEnable()
    {
        if (solved)
        {
            gameObject.SetActive(false);
            return;
        }

        currentCode = "";
        displayText.text = "";
        resultText.text = "";

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            CloseKeypad();
    }

    public void PressKey(string number)
    {
        if (currentCode.Length >= maxDigits) return;

        currentCode += number;
        displayText.text = currentCode;
        resultText.text = "";

        PlaySound(buttonClickSound);
    }

    public void ClearCode()
    {
        currentCode = "";
        displayText.text = "";
        resultText.text = "";

        PlaySound(buttonClickSound);
    }

    public void EnterCode()
    {
        if (currentCode == correctCode)
        {
            solved = true;

            resultText.text = "GRANTED";
            resultText.color = Color.green;

            PlaySound(grantedSound);

            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (drawerAnimator != null)
                drawerAnimator.SetTrigger("Open");

            if (drawerInteractableUI != null)
                drawerInteractableUI.enabled = false;

            gameObject.SetActive(false);
        }
        else
        {
            resultText.text = "DENIED";
            resultText.color = Color.red;

            currentCode = "";
            displayText.text = "";

            PlaySound(deniedSound);
        }
    }

    public void CloseKeypad()
    {
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        gameObject.SetActive(false);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}