using UnityEngine;

public class BookUIController : MonoBehaviour
{
    [Header("Pages")]
    [SerializeField] private GameObject page01;
    [SerializeField] private GameObject page02;
    [SerializeField] private GameObject page03;

    [Header("Buttons")]
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject backButton;

    private int currentPage = 1;

    private void Start()
    {
        CloseBook();
    }

    public void OpenBook()
    {
        currentPage = 1;

        gameObject.SetActive(true);
        ShowCurrentPage();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseBook()
    {
        gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void NextPage()
    {
        if (currentPage >= 3)
            return;

        currentPage++;
        ShowCurrentPage();
    }

    public void BackPage()
    {
        if (currentPage <= 1)
            return;

        currentPage--;
        ShowCurrentPage();
    }

    private void ShowCurrentPage()
    {
        if (page01 != null)
            page01.SetActive(currentPage == 1);

        if (page02 != null)
            page02.SetActive(currentPage == 2);

        if (page03 != null)
            page03.SetActive(currentPage == 3);

        if (nextButton != null)
            nextButton.SetActive(currentPage < 3);

        if (backButton != null)
            backButton.SetActive(currentPage > 1);
    }
}