using UnityEngine;

public class BookInteraction : MonoBehaviour
{
    [Header("Book UI")]
    [SerializeField] private BookUIController bookUI;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool playerInside;

    private void Update()
    {
        if (!playerInside)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            OpenBook();
        }
    }

    private void OpenBook()
    {
        if (bookUI != null)
        {
            bookUI.OpenBook();
        }
        else
        {
            Debug.LogWarning("[BookInteraction] Book UI is not assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;
    }
}