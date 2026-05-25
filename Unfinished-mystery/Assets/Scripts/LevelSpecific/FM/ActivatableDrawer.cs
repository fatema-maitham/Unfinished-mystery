using System.Collections;
using UnityEngine;

public class ActivatableDrawer : MonoBehaviour
{
    [Header("Prompt UI")]
    [SerializeField] private InteractionPromptUI promptUI;
    [SerializeField] private string promptText = "OPEN";
    [SerializeField] private string promptSubLabel = "Drawer";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 3.5f;

    [Header("Drawer Movement")]
    [SerializeField] private Transform drawerToMove;
    [SerializeField] private float openSpeed = 3f;

    [Header("Key")]
    [SerializeField] private GameObject keyObject;

    private bool isOpen;
    private bool showingPrompt;

    private Vector3 closedLocalPosition;
    private Vector3 openLocalPosition;

    private void Start()
    {
        if (drawerToMove == null)
            drawerToMove = transform;

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                player = playerObject.transform;
        }

        if (promptUI == null)
            promptUI = FindFirstObjectByType<InteractionPromptUI>();

        closedLocalPosition = drawerToMove.localPosition;
        openLocalPosition = closedLocalPosition + new Vector3(0f, 0f, 0.18f);

        if (keyObject != null)
            keyObject.SetActive(false);
    }

    private void Update()
    {
        if (isOpen || player == null || promptUI == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance)
        {
            promptUI.ShowPrompt(promptText, promptSubLabel);
            showingPrompt = true;

            if (Input.GetKeyDown(interactKey))
                OpenDrawer();
        }
        else
        {
            if (showingPrompt)
            {
                promptUI.HidePrompt();
                showingPrompt = false;
            }
        }
    }

    private void OpenDrawer()
    {
        isOpen = true;
        showingPrompt = false;

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
}