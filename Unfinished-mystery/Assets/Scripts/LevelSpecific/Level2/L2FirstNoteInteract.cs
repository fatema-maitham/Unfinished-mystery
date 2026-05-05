using UnityEngine;

public class L2FirstNoteInteract : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private L2NoteUI noteUI;

    [Header("Note Message")]
    [TextArea(3, 8)]
    [SerializeField] private string noteMessage =
        "I hid the truth in the things I could not throw away.";

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string promptMessage = "Press E to read";

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;

    private bool playerInside;
    private bool noteRead;

    private void Start()
    {
        if (showDebugLogs)
        {
            Debug.Log("[FirstNote] Script started on object: " + gameObject.name);
        }

        if (noteUI == null)
        {
            Debug.LogWarning("[FirstNote] noteUI is NOT assigned in Inspector.");
        }
        else
        {
            Debug.Log("[FirstNote] noteUI assigned correctly: " + noteUI.name);
        }

        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogWarning("[FirstNote] No Collider found on this object.");
        }
        else
        {
            Debug.Log("[FirstNote] Collider found. Is Trigger = " + col.isTrigger);
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("[FirstNote] No Rigidbody found. Add Rigidbody and set Is Kinematic ON.");
        }
        else
        {
            Debug.Log("[FirstNote] Rigidbody found. Is Kinematic = " + rb.isKinematic + ", Use Gravity = " + rb.useGravity);
        }
    }

    private void Update()
    {
        if (!playerInside)
            return;

        if (showDebugLogs)
        {
            Debug.Log("[FirstNote] Player is inside trigger. Waiting for key: " + interactKey);
        }

        if (Input.GetKeyDown(interactKey))
        {
            if (showDebugLogs)
            {
                Debug.Log("[FirstNote] E key pressed.");
            }

            ReadNote();
        }
    }

    private void ReadNote()
    {
        if (noteUI == null)
        {
            Debug.LogWarning("[FirstNote] Cannot show note. noteUI is not assigned.");
            return;
        }

        noteUI.ShowNote(noteMessage);
        noteRead = true;

        Debug.Log("[FirstNote] First Note read. UI should be open now.");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("[FirstNote] Something entered trigger: " + other.name + " | Tag: " + other.tag);

        if (!other.CompareTag("Player"))
        {
            Debug.Log("[FirstNote] Entered object is NOT tagged Player. Ignoring it.");
            return;
        }

        playerInside = true;

        Debug.Log("[FirstNote] Player entered trigger.");

        if (noteUI != null)
        {
            noteUI.ShowPrompt(promptMessage);
            Debug.Log("[FirstNote] Prompt should be shown now.");
        }
        else
        {
            Debug.LogWarning("[FirstNote] noteUI missing, cannot show prompt.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("[FirstNote] Something exited trigger: " + other.name + " | Tag: " + other.tag);

        if (!other.CompareTag("Player"))
        {
            Debug.Log("[FirstNote] Exited object is NOT tagged Player. Ignoring it.");
            return;
        }

        playerInside = false;

        Debug.Log("[FirstNote] Player exited trigger.");

        if (noteUI != null)
        {
            noteUI.HidePrompt();
            Debug.Log("[FirstNote] Prompt hidden.");
        }
    }

    public bool IsNoteRead()
    {
        return noteRead;
    }
}