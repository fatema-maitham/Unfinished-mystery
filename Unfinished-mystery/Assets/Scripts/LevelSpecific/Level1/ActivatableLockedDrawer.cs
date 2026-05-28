using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using InventoryFramework;

/// <summary>
/// Puzzle 3 — Locked Desk Drawer
/// Two gate checks: desk phase AND bookshelf phase must both be complete.
/// Player enters 3-digit code 433 via a UI panel.
/// Code logic: first words on pages 12, 31, 53 of the book are WHEN (4), YOU (3), LIE (3).
///
/// On success:
///   1. Drawer slides open (local X animates to openLocalX, default -2.58).
///   2. USB Item is added directly to the player's inventory via ItemPickupHandler.
///   3. The USB 3D GameObject in the scene is revealed (SetActive true).
///   4. Level1PuzzleSystem.UnlockDrawer() is called.
///
/// Re-interacting after unlock shows the drawer contents dialog again.
/// Close the code panel with Escape without submitting.
/// </summary>
public class ActivatableLockedDrawer : MonoBehaviour, IActivatable
{
    [Header("Prompt")]
    [SerializeField] private string label            = "Examine Lock";
    [SerializeField] private string subLabel         = "Locked Drawer";
    [SerializeField] private float  activationRadius = 1.5f;

    [Header("Blocked Messages")]
    [SerializeField] private string blockedNoDeskMessage =
        "You can't open it yet. Examine the desk first.";
    [SerializeField] private string blockedNoBookshelfMessage =
        "A locked drawer. You need to figure out the combination first.";

    [Header("Code Entry UI")]
    [Tooltip("A panel under HUD_Canvas — needs InputField, Button (Submit), and TMP_Text (feedback)")]
    [SerializeField] private GameObject     codeEntryPanel;
    [SerializeField] private TMP_InputField codeInputField;
    [SerializeField] private Button         submitButton;
    [SerializeField] private TMP_Text       feedbackText;

    [Header("Correct Code")]
    [SerializeField] private string correctCode = "433";

    [Header("Drawer Animation")]
    [Tooltip("The Transform that slides open. Defaults to this GameObject's transform if left empty.")]
    [SerializeField] private Transform drawerTransform;
    [Tooltip("Target local X position when the drawer is fully open.")]
    [SerializeField] private float openLocalX = -2.58f;
    [Tooltip("How many seconds the slide animation takes.")]
    [SerializeField] private float slideDuration = 0.6f;

    [Header("USB Scene Object")]
    [Tooltip("The USB 3D GameObject hidden in the scene — revealed when the drawer opens.")]
    [SerializeField] private GameObject usbSceneObject;

    [Header("USB Inventory Item")]
    [Tooltip("The USB Item ScriptableObject asset to add to the player's inventory on unlock.")]
    [SerializeField] private Item usbItem;
    [SerializeField] private int  usbAmount = 1;

    [Header("On Success")]
    [Tooltip("Optional image of the open drawer contents.")]
    [SerializeField] private Sprite drawerContentsImage;
    [TextArea(2, 5)]
    [SerializeField] private string successText =
        "The drawer clicks open.\n\n" +
        "Inside: A USB drive labeled \"N.O. — Final Submission\"\n\n" +
        "A sticky note in Lynnette's handwriting:\n" +
        "\"It's all on the drive. Everything you told me to delete. I didn't.\"";

    // ── State ─────────────────────────────────────────────────────────────────
    private bool _unlocked = false;

    // ── IActivatable ──────────────────────────────────────────────────────────
    public string ActivationLabel  => _unlocked ? "Open" : label;
    public string ActivationHint   => subLabel;
    public bool   CanActivate      => true;
    public float  ActivationRadius => activationRadius;

    // ── Unity ─────────────────────────────────────────────────────────────────
    private void Awake()
    {
        // Fall back to this object's transform if none assigned
        if (drawerTransform == null)
            drawerTransform = transform;

        // Hide the USB scene object until the drawer is opened
        if (usbSceneObject != null)
            usbSceneObject.SetActive(false);
    }

    private void Start()
    {
        if (submitButton   != null) submitButton.onClick.AddListener(OnSubmitCode);
        if (codeEntryPanel != null) codeEntryPanel.SetActive(false);
    }

    // ── IActivatable — entry point ─────────────────────────────────────────────
    public void OnActivate(GameObject source)
    {
        if (_unlocked)
        {
            ShowDrawerContents();
            return;
        }

        if (!Level1PuzzleSystem.Instance.DeskPhaseComplete)
        {
            Level1PuzzleSystem.ShowBlocked(blockedNoDeskMessage);
            return;
        }

        if (!Level1PuzzleSystem.Instance.BookshelfPhaseComplete)
        {
            Level1PuzzleSystem.ShowBlocked(blockedNoBookshelfMessage);
            return;
        }

        OpenCodeEntry();
    }

    // ── Code Panel ────────────────────────────────────────────────────────────
    private void OpenCodeEntry()
    {
        if (codeEntryPanel != null)
        {
            codeEntryPanel.SetActive(true);
            UIStateManager.Instance?.OpenNotebook();
            Time.timeScale = 0f;

            if (feedbackText   != null) feedbackText.text = "";
            if (codeInputField != null)
            {
                codeInputField.text = "";
                codeInputField.ActivateInputField();
            }
        }
        else
        {
            ActivationDialogUI.ShowText(
                "Enter the 3-digit combination.\n\nHint: Count the letters in WHEN, YOU, LIE.",
                "Locked Drawer");
        }
    }

    private void OnSubmitCode()
    {
        if (codeInputField == null) return;

        string entered = codeInputField.text.Trim();

        if (entered == correctCode)
        {
            CloseCodeEntry();
            _unlocked = true;

            // 1. Notify puzzle system
            Level1PuzzleSystem.Instance?.UnlockDrawer();

            // 2. Slide the drawer open (uses unscaled time so it works after timeScale reset)
            StartCoroutine(SlideDrawerOpen());

            // 3. Add USB directly to player inventory
            GiveUSBToPlayer();

            // 4. Reveal the USB 3D object so the player can also interact with it
            if (usbSceneObject != null)
                usbSceneObject.SetActive(true);

            // 5. Show the success dialog
            ShowDrawerContents();
        }
        else
        {
            if (feedbackText != null)
                feedbackText.text = "Wrong combination. Try again.";

            if (codeInputField != null)
            {
                codeInputField.text = "";
                codeInputField.ActivateInputField();
            }
        }
    }

    private void CloseCodeEntry()
    {
        if (codeEntryPanel != null)
            codeEntryPanel.SetActive(false);

        UIStateManager.Instance?.CloseNotebook();
        Time.timeScale = 1f;
    }

    // ── Drawer Slide Animation ────────────────────────────────────────────────
    private IEnumerator SlideDrawerOpen()
    {
        float   elapsed  = 0f;
        float   startX   = drawerTransform.localPosition.x;
        Vector3 startPos = drawerTransform.localPosition;

        while (elapsed < slideDuration)
        {
            // Use unscaledDeltaTime so the animation still runs at timeScale 1 (restored above)
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);

            // Ease-out quad: starts fast, decelerates at the end
            t = 1f - (1f - t) * (1f - t);

            Vector3 pos = startPos;
            pos.x = Mathf.Lerp(startX, openLocalX, t);
            drawerTransform.localPosition = pos;

            yield return null;
        }

        // Snap to exact target in case of floating-point drift
        Vector3 final = drawerTransform.localPosition;
        final.x = openLocalX;
        drawerTransform.localPosition = final;
    }

    // ── Give USB to Player Inventory ──────────────────────────────────────────
    private void GiveUSBToPlayer()
    {
        if (usbItem == null)
        {
            Debug.LogWarning("[ActivatableLockedDrawer] usbItem is not assigned — USB not added to inventory.", this);
            return;
        }

        // Player is tagged "Player" and carries ItemPickupHandler (confirmed in Inspector)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("[ActivatableLockedDrawer] No GameObject with tag 'Player' found.", this);
            return;
        }

        ItemPickupHandler handler = player.GetComponent<ItemPickupHandler>();
        if (handler == null)
        {
            Debug.LogWarning("[ActivatableLockedDrawer] ItemPickupHandler not found on Player.", this);
            return;
        }

        handler.PickupItem(usbItem, usbAmount);
        Debug.Log("[ActivatableLockedDrawer] USB item added to player inventory via ItemPickupHandler.");
    }

    // ── Show Contents Dialog ──────────────────────────────────────────────────
    private void ShowDrawerContents()
    {
        if (drawerContentsImage != null)
            ActivationDialogUI.ShowImage(drawerContentsImage);
        else
            ActivationDialogUI.ShowText(successText, "Desk Drawer");
    }

    // ── Update — keyboard shortcuts while panel is open ───────────────────────
    private void Update()
    {
        if (codeEntryPanel != null && codeEntryPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                CloseCodeEntry();

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                OnSubmitCode();
        }
    }

    public void OnActivatableFocus() { }
    public void OnActivatableBlur()  { }
}