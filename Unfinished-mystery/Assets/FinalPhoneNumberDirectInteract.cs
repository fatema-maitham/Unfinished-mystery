using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalPhoneNumberDirectInteract : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 5f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Prompt")]
    [SerializeField] private ActivationPromptUI promptUI;
    [SerializeField] private string promptLabel = "Inspect";
    [SerializeField] private string promptSubLabel = "Wall Frame";

    [Header("Dialog UI")]
    [SerializeField] private GameObject dialogRoot;
    [SerializeField] private GameObject imagePanel;
    [SerializeField] private GameObject textPanel;
    [SerializeField] private Image displayImage;

    [Header("Text UI")]
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogBodyText;
    [SerializeField] private TMP_Text continueHintText;

    [Header("Content")]
    [SerializeField] private Sprite phoneNumberNoteImage;
    [SerializeField] private string dialogTitle = "Torn paper - BACK";

    [TextArea(4, 10)]
    [SerializeField] private string dialogBody =
        "On the back, written in tiny pencil:\n\n" +
        "916-2847\n\n" +
        "\"Isla's number. Lana kept it hidden for years.\"";

    private int step = 0;
    private bool promptShown = false;
    private bool clueFound = false;
    private float nextInputTime = 0f;

    private GameObject promptPanel;

    private void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        if (promptUI == null)
            promptUI = FindFirstObjectByType<ActivationPromptUI>();

        if (promptUI != null)
        {
            Transform foundPromptPanel = promptUI.transform.Find("PromptPanel");
            if (foundPromptPanel != null)
                promptPanel = foundPromptPanel.gameObject;
        }
    }

    private void Start()
    {
        CloseAll();
    }

    private void Update()
    {
        if (player == null || promptUI == null)
            return;

        if (step == 1)
        {
            if (Time.time >= nextInputTime && Input.GetKeyDown(interactKey))
                ShowBlackText();

            return;
        }

        if (step == 2)
        {
            if (Time.time >= nextInputTime && Input.GetKeyDown(interactKey))
                CloseAll();

            return;
        }

        float distance = Vector3.Distance(player.position, transform.position);
        bool near = distance <= interactDistance;

        if (near)
        {
            if (!promptShown)
            {
                promptShown = true;

                if (promptPanel != null)
                    promptPanel.SetActive(true);

                promptUI.ShowPrompt(promptLabel, promptSubLabel);
            }

            if (Input.GetKeyDown(interactKey))
                ShowNoteImage();
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

    private void ShowNoteImage()
    {
        step = 1;
        nextInputTime = Time.time + 0.35f;

        promptShown = false;

        if (promptPanel != null)
            promptPanel.SetActive(false);

        dialogRoot.SetActive(true);
        dialogRoot.transform.SetAsLastSibling();

        textPanel.SetActive(false);
        imagePanel.SetActive(true);
        imagePanel.transform.SetAsLastSibling();

        RectTransform rootRect = dialogRoot.GetComponent<RectTransform>();
        ForceFullScreen(rootRect);

        RectTransform panelRect = imagePanel.GetComponent<RectTransform>();
        ForceFullScreen(panelRect);

        displayImage.gameObject.SetActive(true);
        displayImage.enabled = true;
        displayImage.sprite = phoneNumberNoteImage;
        displayImage.color = Color.white;
        displayImage.preserveAspect = true;
        displayImage.raycastTarget = false;
        displayImage.transform.SetAsLastSibling();

        RectTransform img = displayImage.GetComponent<RectTransform>();
        img.anchorMin = new Vector2(0.5f, 0.5f);
        img.anchorMax = new Vector2(0.5f, 0.5f);
        img.pivot = new Vector2(0.5f, 0.5f);
        img.anchoredPosition = new Vector2(0f, 120f);
        img.sizeDelta = new Vector2(900f, 650f);
        img.localScale = Vector3.one;

        Debug.Log("[FinalPhoneNumber] Note image shown.");
    }

    private void ShowBlackText()
    {
        step = 2;
        nextInputTime = Time.time + 0.35f;

        dialogRoot.SetActive(true);
        
        dialogRoot.transform.SetParent(GameObject.Find("HUD_Canvas").transform, false);
dialogRoot.transform.SetAsLastSibling();
        imagePanel.SetActive(false);

        textPanel.SetActive(true);
        textPanel.transform.SetAsLastSibling();

        ForceFullScreen(dialogRoot.GetComponent<RectTransform>());
        ForceFullScreen(textPanel.GetComponent<RectTransform>());

        speakerNameText.text = dialogTitle;
        dialogBodyText.text = dialogBody;
        continueHintText.text = "Press E to close";

        speakerNameText.color = Color.white;
        dialogBodyText.color = Color.white;
        continueHintText.color = Color.white;

        ForceTextBox(speakerNameText.GetComponent<RectTransform>(), new Vector2(0f, 150f), new Vector2(900f, 80f));
        ForceTextBox(dialogBodyText.GetComponent<RectTransform>(), new Vector2(0f, 0f), new Vector2(900f, 260f));
        ForceTextBox(continueHintText.GetComponent<RectTransform>(), new Vector2(0f, -210f), new Vector2(700f, 60f));

        speakerNameText.alignment = TextAlignmentOptions.Center;
        dialogBodyText.alignment = TextAlignmentOptions.Center;
        continueHintText.alignment = TextAlignmentOptions.Center;

        speakerNameText.fontSize = 38;
        dialogBodyText.fontSize = 30;
        continueHintText.fontSize = 24;

        if (!clueFound)
        {
            clueFound = true;
            Level2PuzzleSystem.Instance?.FindPhoneNumber();
        }

        Debug.Log("[FinalPhoneNumber] Black text panel shown.");
    }

    private void CloseAll()
    {
        step = 0;

        if (dialogRoot != null)
            dialogRoot.SetActive(false);

        if (imagePanel != null)
            imagePanel.SetActive(false);

        if (textPanel != null)
            textPanel.SetActive(false);

        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    private void ForceFullScreen(RectTransform rect)
    {
        if (rect == null)
            return;

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;
    }

    private void ForceTextBox(RectTransform rect, Vector2 position, Vector2 size)
    {
        if (rect == null)
            return;

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        rect.localScale = Vector3.one;
    }
}