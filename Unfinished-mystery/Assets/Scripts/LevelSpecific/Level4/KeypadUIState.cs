using UnityEngine;

public class KeypadUIState : MonoBehaviour
{
    [SerializeField] private MonoBehaviour characterMovement;
    [SerializeField] private MonoBehaviour cameraMovement;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (characterMovement != null)
            characterMovement.enabled = false;

        if (cameraMovement != null)
            cameraMovement.enabled = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (characterMovement != null)
            characterMovement.enabled = true;

        if (cameraMovement != null)
            cameraMovement.enabled = true;
    }
}