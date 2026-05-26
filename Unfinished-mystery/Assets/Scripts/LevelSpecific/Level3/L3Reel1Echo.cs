using System.Collections;
using UnityEngine;

public class L3Reel1Echo : MonoBehaviour
{
    [Header("Echo Objects")]
    [SerializeField] private GameObject shadowObject;
    [SerializeField] private AudioSource doorEchoAudio;

    [Header("Settings")]
    [SerializeField] private float echoDuration = 3f;

    public void PlayEcho()
    {
        StartCoroutine(PlayEchoRoutine());
    }

    private IEnumerator PlayEchoRoutine()
    {
        if (shadowObject != null)
            shadowObject.SetActive(true);

        if (doorEchoAudio != null)
            doorEchoAudio.Play();

        yield return new WaitForSeconds(echoDuration);

        if (shadowObject != null)
            shadowObject.SetActive(false);
    }
}