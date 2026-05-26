using UnityEngine;

public class L3ProjectorBreakdown : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private GameObject sparkEffect;
    [SerializeField] private AudioSource breakdownAudio;

    private bool hasPlayed = false;

    public void PlayBreakdown()
    {
        if (hasPlayed)
            return;

        hasPlayed = true;

        if (smokeEffect != null)
            smokeEffect.SetActive(true);

        if (sparkEffect != null)
            sparkEffect.SetActive(true);

        if (breakdownAudio != null)
            breakdownAudio.Play();
    }
}