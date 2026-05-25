using System.Collections;
using UnityEngine;

public class ActivatableDrawer : MonoBehaviour
{
    [Header("Prompt UI")]
    [SerializeField] private InteractionPromptUI promptUI;
    [SerializeField] private string promptText = "OPEN";
    [SerializeField] private string promptSubLabel = "Drawer";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Drawer Movement")]
    [SerializeField] private Transform drawerToMove;
    [SerializeField] private float openDistance = 0.28f;
    [SerializeField] private float openSpeed = 3f;

    [Header("Key")]
    [SerializeField] private GameObject keyObject;

    private bool playerInside;
    private bool isOpen;

    private Vector3 closedLocalPosition;
    private Vector3 openLocalPosition;

    private void Start()
    {
        if (drawerToMove == null)
            drawerToMove = transform;

        closedLocalPosition = drawerToMove.localPosition;

        // Move the drawer forward using its LOCAL Z axis.
        // If it opens the wrong way, change +openDistance to -openDistance below.
openLocalPosition = closedLocalPosition + new Vector3(0f, 0f, 0.18f);
        if (keyObject != null)
            keyObject.SetActive(false);
    }

    private void Update()
    {
        if (!playerInside || isOpen)
            return;

        if (Input.GetKeyDown(interactKey))
            OpenDrawer();
    }

    private void OpenDrawer()
    {
        isOpen = true;

        if (promptUI != null)
            promptUI.HidePrompt();

        StartCoroutine(OpenRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        while (Vector3.Distance(drawerToMove.localPosition, openLocalPosition) > 0.01f)
        {
            drawerToMove.localPosition = Vector3.Lerp(
                drawerToMove.localPosition,
                openLocalPosition,
                Time.deltaTime * openSpeed
            );

            yield return null;
        }

        drawerToMove.localPosition = openLocalPosition;

        if (keyObject != null)
            keyObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isOpen)
            return;

        playerInside = true;

        if (promptUI != null)
            promptUI.ShowPrompt(promptText, promptSubLabel);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        if (promptUI != null)
            promptUI.HidePrompt();
    }
}