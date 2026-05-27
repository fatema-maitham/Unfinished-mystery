using UnityEngine;

public class L3Reel3Echo : MonoBehaviour
{
    [Header("Echo Audio")]
    [SerializeField] private AudioSource reel3EchoAudio;

    public void PlayEcho()
    {
        if (reel3EchoAudio != null)
            reel3EchoAudio.Play();
    }
}