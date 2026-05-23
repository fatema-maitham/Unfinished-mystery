using TMPro;
using UnityEngine;

public class BookInteract : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private MonoBehaviour playerMovement;

    [Header("Book")]
    [SerializeField] private BookCanvasController bookCanvasController;

    [Header("Prompt")]
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TMP_Text promptTextUI;

    private bool bookOpen;

    private void Start()
    {
        promptPanel.SetActive(false);
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

        if (bookOpen && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape)))
        {
            CloseBook();
        }
    }

    private void OpenBook()
    {
        bookOpen = true;
        promptPanel.SetActive(false);

        bookCanvasController.OpenBook();

        if (playerMovement != null)
            playerMovement.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseBook()
    {
        bookOpen = false;

        bookCanvasController.CloseBook();

        if (playerMovement != null)
            playerMovement.enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}