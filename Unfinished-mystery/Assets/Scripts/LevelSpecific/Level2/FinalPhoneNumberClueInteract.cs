using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalPhoneNumberClueInteract : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Prompt UI")]
    [SerializeField] private ActivationPromptUI promptUI;
    [SerializeField] private string promptLabel = "Inspect";
    [SerializeField] private string promptSubLabel = "Wall Frame";

    [Header("DialogRoot UI")]
    [SerializeField] private GameObject dialogRoot;
    [SerializeField] private GameObject imagePanel;
    [SerializeField] private GameObject textPanel;

    [Header("Image UI")]
    [SerializeField] private Image displayImage;
    [SerializeField] private Sprite phoneNumberPaperImage;

    [Header("Text UI")]
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogBodyText;
    [SerializeField] private TMP_Text continueHintText;

    [Header("Dialog Text")]
    [SerializeField] private string dialogTitle = "Torn paper - BACK";

    [TextArea(4, 10)]
    [SerializeField] private string dialogBody =
        "On the back, written in tiny pencil:\n\n" +
        "916-2847\n\n" +
        "\"Isla's number. Lana kept it hidden for years.\"";

    [Header("Input Safety")]
    [SerializeField] private float inputDelay = 0.35f;

    private enum ClueState { Closed, ShowingImage, ShowingText }

    private ClueState state = ClueState.Closed;
    private float nextInputTime;
    private bool clueFound;
    private bool promptShown;

    private void Awake()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

        if (promptUI == null)
            promptUI = FindFirstObjectByType<ActivationPromptUI>();
    }

    private void Start()
    {
        HideDialogOnly();
    }

    private void Update()
    {
        if (player == null || promptUI == null)
            return;

        if (state == ClueState.ShowingImage)
        {
            if (Time.unscaledTime >= nextInputTime && Input.GetKeyDown(interactKey))
                ShowTextPanel();

            return;
        }

        if (state == ClueState.ShowingText)
        {
            if (Time.unscaledTime >= nextInputTime && Input.GetKeyDown(interactKey))
                CloseEverything();

            return;
        }

        float distance = Vector3.Distance(player.position, transform.position);
        bool nearFrame = distance <= interactDistance;

        if (nearFrame)
        {
            if (!promptShown)
            {
                promptShown = true;
                promptUI.ShowPrompt(promptLabel, promptSubLabel);
            }

            if (Input.GetKeyDown(interactKey))
                ShowImagePanel();
        }
        else
        {
            if (promptShown)
            {
                promptShown = false;
                promptUI.HidePrompt();
            }
        }
    }

private void ShowImagePanel()
{
    state = ClueState.ShowingImage;
    nextInputTime = Time.unscaledTime + inputDelay;

    promptShown = false;
    promptUI.HidePrompt();

    dialogRoot.SetActive(true);
    textPanel.SetActive(false);
    imagePanel.SetActive(true);

    dialogRoot.transform.SetAsLastSibling();
    imagePanel.transform.SetAsLastSibling();

    displayImage.gameObject.SetActive(true);
    displayImage.enabled = true;
    displayImage.sprite = phoneNumberPaperImage;
    displayImage.color = Color.white;
    displayImage.preserveAspect = true;
    displayImage.transform.SetParent(dialogRoot.transform, false);
    displayImage.transform.SetAsLastSibling();

    RectTransform img = displayImage.GetComponent<RectTransform>();
    img.anchorMin = new Vector2(0.5f, 0.5f);
    img.anchorMax = new Vector2(0.5f, 0.5f);
    img.pivot = new Vector2(0.5f, 0.5f);
    img.anchoredPosition = Vector2.zero;
    img.sizeDelta = new Vector2(650f, 450f);
    img.localScale = Vector3.one;

    Debug.Log("[FinalPhoneNumberClue] Image shown CENTER.");
}

    private void ShowTextPanel()
    {
        state = ClueState.ShowingText;
        nextInputTime = Time.unscaledTime + inputDelay;

        imagePanel.SetActive(false);

        dialogRoot.SetActive(true);
        dialogRoot.transform.SetAsLastSibling();

        textPanel.SetActive(true);
        textPanel.transform.SetAsLastSibling();

        speakerNameText.text = dialogTitle;
        dialogBodyText.text = dialogBody;
        continueHintText.text = "Press E to close";

        if (!clueFound)
        {
            clueFound = true;
            Level2PuzzleSystem.Instance?.FindPhoneNumber();
        }

        Debug.Log("[FinalPhoneNumberClue] Text shown.");
    }

    private void CloseEverything()
    {
        state = ClueState.Closed;
        HideDialogOnly();
        Debug.Log("[FinalPhoneNumberClue] Closed.");
    }

    private void HideDialogOnly()
    {
        if (dialogRoot != null)
            dialogRoot.SetActive(false);

        if (imagePanel != null)
            imagePanel.SetActive(false);

        if (textPanel != null)
            textPanel.SetActive(false);
    }
}