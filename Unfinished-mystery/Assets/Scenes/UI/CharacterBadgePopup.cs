using System.Collections;
using UnityEngine;

public class CharacterBadgePopup : MonoBehaviour
{
    [SerializeField] private GameObject badgePanel;
    [SerializeField] private float minimumShowTime = 2f;

    private bool canClose;

    void Start()
    {
        Time.timeScale = 0f;

        if (badgePanel != null)
            badgePanel.SetActive(true);

        StartCoroutine(AllowCloseAfterDelay());
    }

    void Update()
    {
        if (!canClose) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
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

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}