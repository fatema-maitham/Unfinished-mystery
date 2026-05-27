using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class L3EvidenceBoardUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject evidenceBoardPanel;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private Button continueButton;

    [Header("Player")]
    [SerializeField] private MonoBehaviour playerMovement;
    [SerializeField] private ThirdPersonCamera thirdPersonCamera;

    [Header("Typewriter")]
    [TextArea(8, 15)]
    [SerializeField] private string fullText;

    [SerializeField] private float typingSpeed = 0.02f;

    [SerializeField] private GameObject overlayPanel;

    [Header("Typing Sound")]
    [SerializeField] private AudioSource typingAudioSource;


    private bool boardOpen = false;

    private bool isTyping = false;

    private void Start()
    {
        if (evidenceBoardPanel != null)
            evidenceBoardPanel.SetActive(false);

        if (continueButton != null)
            continueButton.gameObject.SetActive(false);
    }

    public void ShowEvidenceBoard()

    {
        Debug.Log("EVIDENCE BOARD OPENED");
        if (evidenceBoardPanel != null)
            evidenceBoardPanel.SetActive(true);

        if (overlayPanel != null)
            overlayPanel.SetActive(true);

        boardOpen = true;

        if (bodyText != null)
            bodyText.text = "";

        if (continueButton != null)
            continueButton.gameObject.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (thirdPersonCamera != null)
            thirdPersonCamera.EnterUIMode();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (typingAudioSource != null)
        typingAudioSource.Play();

        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        isTyping = true;

        foreach (char c in fullText)
        {
            bodyText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        if (typingAudioSource != null)
            typingAudioSource.Stop();

        if (continueButton != null)
            continueButton.gameObject.SetActive(true);
    }

    public void CloseEvidenceBoard()
    {

            boardOpen = false;

        if (evidenceBoardPanel != null)
            evidenceBoardPanel.SetActive(false);
        
        if (overlayPanel != null)
            overlayPanel.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (thirdPersonCamera != null)
            thirdPersonCamera.ExitUIMode();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void LateUpdate()
    {
        if (!boardOpen)
            return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}