using System.Collections;
using UnityEngine;

public class L3Reel2Echo : MonoBehaviour
{
    [Header("Echo Audio")]
    [SerializeField] private AudioSource doorRattleAudio;

    [Header("Settings")]
    [SerializeField] private float echoDuration = 2f;

    public void PlayEcho()
    {
        StartCoroutine(PlayEchoRoutine());
    }

    private IEnumerator PlayEchoRoutine()
    {
        if (doorRattleAudio != null)
            doorRattleAudio.Play();

        yield return new WaitForSeconds(echoDuration);
    }
}