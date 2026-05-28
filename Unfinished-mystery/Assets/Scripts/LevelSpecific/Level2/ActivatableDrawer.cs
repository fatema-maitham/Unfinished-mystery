using System.Collections;
using UnityEngine;
using TMPro;

public class ActivatableDrawer : MonoBehaviour, ILoopResettable
{
    [Header("New HUD Prompt")]
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TMP_Text keyHintText;
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private TMP_Text subLabelText;

    [Header("Prompt Text")]
    [SerializeField] private string keyText = "E";
    [SerializeField] private string promptText = "OPEN";
    [SerializeField] private string promptSubLabel = "Drawer";

    [Header("Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Drawer Movement")]
    [SerializeField] private Transform drawerToMove;
    [SerializeField] private Vector3 openOffset = new Vector3(0f, 0f, 0.45f);
    [SerializeField] private float openSpeed = 2.5f;

    [Header("Key")]
    [SerializeField] private GameObject keyObject;

    private bool isOpen;
    private bool showingPrompt;
    private Vector3 closedLocalPosition;
    private Vector3 openLocalPosition;
    private Coroutine openCoroutine;

    private void Awake()
    {
        if (drawerToMove == null)
            drawerToMove = transform;

        closedLocalPosition = drawerToMove.localPosition;
        openLocalPosition = closedLocalPosition + openOffset;
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                player = playerObject.transform;
        }

        ResetDrawerClosed();
    }

    private void Update()
    {
        if (isOpen || player == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance)
        {
            ShowPrompt();

            if (Input.GetKeyDown(interactKey))
                OpenDrawer();
        }
        else
        {
            HidePrompt();
        }
    }

    private void ShowPrompt()
    {
        if (showingPrompt)
            return;

        if (promptPanel != null)
            promptPanel.SetActive(true);

        if (keyHintText != null)
            keyHintText.text = keyText;

        if (labelText != null)
            labelText.text = promptText;

        if (subLabelText != null)
            subLabelText.text = promptSubLabel;

        showingPrompt = true;
    }

    private void HidePrompt()
    {
        if (promptPanel != null)
            promptPanel.SetActive(false);

        showingPrompt = false;
    }

    private void OpenDrawer()
    {
        if (isOpen)
            return;

        isOpen = true;
        HidePrompt();

        if (openCoroutine != null)
            StopCoroutine(openCoroutine);

        openCoroutine = StartCoroutine(OpenRoutine());
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

    public void ResetState()
    {
        ResetDrawerClosed();
    }

    private void ResetDrawerClosed()
    {
        if (openCoroutine != null)
        {
            StopCoroutine(openCoroutine);
            openCoroutine = null;
        }

        isOpen = false;
        showingPrompt = false;

        if (drawerToMove != null)
            drawerToMove.localPosition = closedLocalPosition;

        if (keyObject != null)
            keyObject.SetActive(false);

        HidePrompt();
    }
}