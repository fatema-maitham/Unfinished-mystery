using UnityEngine;

public class DirectFirstNoteInteract : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private ActivationPromptUI promptUI;

    [Header("Prompt")]
    [SerializeField] private string label = "Read";
    [SerializeField] private string subLabel = "First Note";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Note")]
    [SerializeField] private Sprite noteImage;
    [SerializeField] private float interactDistance = 3f;

    private bool playerNear;

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        playerNear = distance <= interactDistance;

        if (playerNear)
        {
            promptUI?.ShowPrompt(label, subLabel);

            if (Input.GetKeyDown(interactKey))
            {
                if (noteImage != null)
                    ActivationDialogUI.ShowImage(noteImage);

                Level2PuzzleSystem.Instance?.ReadFirstNote();
            }
        }
        else
        {
            promptUI?.HidePrompt();
        }
    }
}