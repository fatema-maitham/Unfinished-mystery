using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhoneUI : MonoBehaviour
{
    [Header("Main UI")]
    [SerializeField] private GameObject phonePanel;
    [SerializeField] private TMP_Text displayText;

    [Header("Number Buttons")]
    [SerializeField] private Button button0;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private Button button4;
    [SerializeField] private Button button5;
    [SerializeField] private Button button6;
    [SerializeField] private Button button7;
    [SerializeField] private Button button8;
    [SerializeField] private Button button9;

    [Header("Action Buttons")]
    [SerializeField] private Button callButton;
    [SerializeField] private Button clearButton;
    [SerializeField] private Button closeButton;

    [Header("Correct Number")]
    [SerializeField] private string correctNumber = "9162847";

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ringClip;
    [SerializeField] private AudioClip islaMessageClip;
    [SerializeField] private AudioClip wrongNumberClip;
    [SerializeField] private AudioClip tickClip;

    [Header("After Call")]
    [SerializeField] private GameObject clockLight;

    private string currentNumber = "";
    private bool callFinished = false;

    private void Awake()
    {
        phonePanel.SetActive(false);

        button0.onClick.AddListener(() => AddDigit("0"));
        button1.onClick.AddListener(() => AddDigit("1"));
        button2.onClick.AddListener(() => AddDigit("2"));
        button3.onClick.AddListener(() => AddDigit("3"));
        button4.onClick.AddListener(() => AddDigit("4"));
        button5.onClick.AddListener(() => AddDigit("5"));
        button6.onClick.AddListener(() => AddDigit("6"));
        button7.onClick.AddListener(() => AddDigit("7"));
        button8.onClick.AddListener(() => AddDigit("8"));
        button9.onClick.AddListener(() => AddDigit("9"));

        callButton.onClick.AddListener(CallNumber);
        clearButton.onClick.AddListener(ClearNumber);
        closeButton.onClick.AddListener(ClosePhone);

        UpdateDisplay();
    }

    public void OpenPhone()
    {
        phonePanel.SetActive(true);
        currentNumber = "";
        UpdateDisplay();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void AddDigit(string digit)
    {
        if (callFinished) return;
        if (currentNumber.Length >= 7) return;

        currentNumber += digit;
        UpdateDisplay();
    }

    private void ClearNumber()
    {
        if (callFinished) return;

        currentNumber = "";
        UpdateDisplay();
    }

    private void CallNumber()
    {
        if (callFinished) return;

        if (currentNumber == correctNumber)
        {
            StartCoroutine(CorrectCallSequence());
        }
        else
        {
            StartCoroutine(WrongNumberSequence());
        }
    }

    private IEnumerator WrongNumberSequence()
    {
        displayText.text = "NO SIGNAL";

        if (audioSource != null && wrongNumberClip != null)
            audioSource.PlayOneShot(wrongNumberClip);

        yield return new WaitForSeconds(1.2f);

        currentNumber = "";
        UpdateDisplay();
    }

    private IEnumerator CorrectCallSequence()
    {
        callFinished = true;
        SetButtons(false);

        displayText.text = "DIALING...";

        if (audioSource != null && ringClip != null)
        {
            audioSource.PlayOneShot(ringClip);
            yield return new WaitForSeconds(ringClip.length);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        displayText.text = "CONNECTED";

        yield return new WaitForSeconds(0.5f);

        if (audioSource != null && islaMessageClip != null)
        {
            audioSource.PlayOneShot(islaMessageClip);
            yield return new WaitForSeconds(islaMessageClip.length);
        }
        else
        {
            yield return new WaitForSeconds(5f);
        }

        displayText.text = "CALL ENDED";

        yield return new WaitForSeconds(1f);

        ClosePhone();

        yield return new WaitForSeconds(0.5f);

        if (audioSource != null && tickClip != null)
            audioSource.PlayOneShot(tickClip);

        if (clockLight != null)
            clockLight.SetActive(true);
    }

    private void ClosePhone()
    {
        phonePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UpdateDisplay()
    {
        if (string.IsNullOrEmpty(currentNumber))
        {
            displayText.text = "ENTER NUMBER";
            return;
        }

        if (currentNumber.Length <= 3)
            displayText.text = currentNumber;
        else
            displayText.text = currentNumber.Substring(0, 3) + "-" + currentNumber.Substring(3);
    }

    private void SetButtons(bool state)
    {
        button0.interactable = state;
        button1.interactable = state;
        button2.interactable = state;
        button3.interactable = state;
        button4.interactable = state;
        button5.interactable = state;
        button6.interactable = state;
        button7.interactable = state;
        button8.interactable = state;
        button9.interactable = state;

        callButton.interactable = state;
        clearButton.interactable = state;
        closeButton.interactable = state;
    }
}