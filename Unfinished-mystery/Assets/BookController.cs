using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private MonoBehaviour playerMovement;

    [Header("UI")]
    [SerializeField] private GameObject bookCanvas;
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TMP_Text promptTextUI;
    [SerializeField] private TMP_Text bookPageText;
    [SerializeField] private TMP_Text pageNumberText;

    [Header("Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button closeButton;

    [Header("Book Pages")]
    [TextArea(4, 10)]
    [SerializeField] private string[] pages =
    {
        "PAGE 1\n\nInvestigation Note\n\nHe never left the scene.\nHe stayed in uniform.",

        "PAGE 2\n\nThe child remembered a number:\n\n259\n\nTwo windows. Five books. Nine marks.",

        "PAGE 3\n\nOfficer Photograph\n\nBadge Number: 259\n\nThe drawing was not random.\nIt was a memory."
    };

    private int currentPage;
    private bool bookOpen;

    private void Start()
    {
        bookCanvas.SetActive(false);
        promptPanel.SetActive(false);

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PreviousPage);
        closeButton.onClick.AddListener(CloseBook);
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        bool nearBook = distance <= interactDistance;

        if (!bookOpen)
        {
            promptPanel.SetActive(nearBook);

            if (nearBook)
                promptTextUI.text = "PRESS E TO TAKE BOOK";
        }

        if (nearBook && !bookOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenBook();
        }

        if (bookOpen && Input.GetKeyDown(KeyCode.X))
        {
            CloseBook();
        }

        if (bookOpen && Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextPage();
        }

        if (bookOpen && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousPage();
        }
    }

    private void OpenBook()
    {
        bookOpen = true;
        currentPage = 0;

        bookCanvas.SetActive(true);
        promptPanel.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        UpdatePage();
    }

    private void CloseBook()
    {
        bookOpen = false;

        bookCanvas.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void NextPage()
    {
        if (!bookOpen)
            return;

        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            UpdatePage();
        }
    }

    private void PreviousPage()
    {
        if (!bookOpen)
            return;

        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    private void UpdatePage()
    {
        bookPageText.text = pages[currentPage];
        pageNumberText.text = "Page " + (currentPage + 1) + " / " + pages.Length;

        prevButton.gameObject.SetActive(currentPage > 0);
        nextButton.gameObject.SetActive(currentPage < pages.Length - 1);
    }
}