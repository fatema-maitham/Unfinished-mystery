using System.Collections;
using UnityEngine;

public class ActivatableDrawer : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 1.2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Prompt UI")]
    [SerializeField] private InteractionPromptUI promptUI;
    [SerializeField] private string promptText = "OPEN";
    [SerializeField] private string promptSubLabel = "Drawer";

    [Header("Drawer Movement")]
    [SerializeField] private Transform drawerToMove;
    [SerializeField] private Vector3 openOffset = new Vector3(0f, 0f, 0.65f);
    [SerializeField] private float openSpeed = 2.5f;

    [Header("Optional")]
    [SerializeField] private GameObject keyObject;

    private bool isOpen;
    private bool playerNear;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        if (drawerToMove == null)
            drawerToMove = transform;

        closedPosition = drawerToMove.localPosition;
        openPosition = closedPosition + openOffset;

        if (keyObject != null)
            keyObject.SetActive(false);
    }

    private void Update()
    {
        if (player == null || isOpen)
            return;

        playerNear = Vector3.Distance(player.position, transform.position) <= interactDistance;

        if (playerNear)
        {
            if (promptUI != null)
                promptUI.ShowPrompt(promptText, promptSubLabel);

            if (Input.GetKeyDown(interactKey))
                OpenDrawer();
        }
        else
        {
            if (promptUI != null)
                promptUI.HidePrompt();
        }
    }

    private void OpenDrawer()
    {
        isOpen = true;

        if (promptUI != null)
            promptUI.HidePrompt();

        if (keyObject != null)
            keyObject.SetActive(true);

        StartCoroutine(OpenRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        while (Vector3.Distance(drawerToMove.localPosition, openPosition) > 0.01f)
        {
            drawerToMove.localPosition = Vector3.Lerp(
                drawerToMove.localPosition,
                openPosition,
                Time.deltaTime * openSpeed
            );

            yield return null;
        }

        drawerToMove.localPosition = openPosition;
    }
}