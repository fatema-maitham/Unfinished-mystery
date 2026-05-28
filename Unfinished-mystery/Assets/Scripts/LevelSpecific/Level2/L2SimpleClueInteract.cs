// using UnityEngine;

// public class L2SimpleClueInteract : MonoBehaviour
// {
//     [Header("UI Reference")]
//     [SerializeField] private L2NoteUI noteUI;

//     [Header("Clue Message")]
//     [TextArea(3, 8)]
//     [SerializeField] private string clueMessage;

//     [Header("Interaction")]
//     [SerializeField] private KeyCode interactKey = KeyCode.E;
//     [SerializeField] private string promptMessage = "Press E to interact";

//     private bool playerInside;

//     private void Update()
//     {
//         if (!playerInside)
//             return;

//         if (Input.GetKeyDown(interactKey))
//         {
//             ShowClue();
//         }
//     }

//     private void ShowClue()
//     {
//         if (noteUI == null)
//         {
//             Debug.LogWarning(gameObject.name + ": Note UI is not assigned.");
//             return;
//         }

//         noteUI.ShowNote(clueMessage);
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         Debug.Log(gameObject.name + " entered by: " + other.name + " | Tag: " + other.tag);

//         if (!other.CompareTag("Player"))
//             return;

//         playerInside = true;

//         if (noteUI != null)
//             noteUI.ShowPrompt(promptMessage);
//     }

//     private void OnTriggerExit(Collider other)
//     {
//         if (!other.CompareTag("Player"))
//             return;

//         playerInside = false;

//         if (noteUI != null)
//             noteUI.HidePrompt();
//     }
// }