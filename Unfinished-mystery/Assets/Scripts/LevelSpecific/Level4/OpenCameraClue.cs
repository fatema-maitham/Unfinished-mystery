using UnityEngine;

public class OpenCameraClue : MonoBehaviour
{
    public GameObject cameraClueUI;

    public void OnActivate()
    {
        if (cameraClueUI == null) return;

        cameraClueUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void OnActivate(GameObject source)
    {
        OnActivate();
    }

    void Update()
    {
        if (cameraClueUI != null && cameraClueUI.activeSelf && Input.GetKeyDown(KeyCode.Q))
        {
            cameraClueUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }
}