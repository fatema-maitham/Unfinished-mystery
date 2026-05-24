using System.Collections;
using UnityEngine;

public class FirstNoteReadAfterClose : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float activationRadius = 1.8f;

    private Transform player;
    private bool alreadyTriggered = false;
    private bool waiting = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (alreadyTriggered) return;
        if (waiting) return;
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance > activationRadius) return;

        if (Input.GetKeyDown(interactKey))
        {
            StartCoroutine(WaitForNoteToOpenThenClose());
        }
    }

    private IEnumerator WaitForNoteToOpenThenClose()
    {
        waiting = true;

        // Wait until the note UI opens.
        while (Time.timeScale != 0f)
        {
            yield return null;
        }

        // Wait until the player presses E again and closes the note.
        while (Time.timeScale == 0f)
        {
            yield return null;
        }

        alreadyTriggered = true;

        Level2PuzzleSystem.Instance.ReadFirstNote();
    }
}