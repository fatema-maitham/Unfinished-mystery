using System.Collections;
using UnityEngine;

public class CharacterBadgePopup : MonoBehaviour
{
    [SerializeField] private GameObject badgePanel;
    [SerializeField] private float minimumShowTime = 2f;

    private bool canClose;

    private void Start()
    {
        if (badgePanel != null)
            badgePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(AllowCloseAfterDelay());
    }

    private void Update()
    {
        if (!canClose) return;

        if (Input.GetMouseButtonDown(0) ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Return))
        {
            CloseBadge();
        }
    }

    private IEnumerator AllowCloseAfterDelay()
    {
        yield return new WaitForSecondsRealtime(minimumShowTime);
        canClose = true;
    }

    private void CloseBadge()
    {
        if (badgePanel != null)
            badgePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}