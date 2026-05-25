using System.Collections;
using UnityEngine;

public class FirstNoteCloseTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup dialogCanvas;

    [Header("Settings")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float activationRadius = 1.8f;

    private Transform player;
    private bool waitingForClose = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (waitingForClose) return;
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance > activationRadius) return;

        if (Input.GetKeyDown(interactKey))
        {
            StartCoroutine(WaitUntilNoteCloses());
        }
    }

    private IEnumerator WaitUntilNoteCloses()
    {
        waitingForClose = true;

        yield return null;

        while (dialogCanvas != null && dialogCanvas.alpha > 0.01f)
        {
            yield return null;
        }

        Level2PuzzleSystem.Instance.ReadFirstNote();
    }
}