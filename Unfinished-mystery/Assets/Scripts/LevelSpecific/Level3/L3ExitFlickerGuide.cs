using System.Collections;
using UnityEngine;

public class L3ExitFlickerGuide : MonoBehaviour
{
    [Header("Light Reference")]
    public Light redLight;

    [Header("Flicker Settings")]
    public float lowIntensity = 0f;
    public float highIntensity = 2f;
    public float minBlinkTime = 0.05f;
    public float maxBlinkTime = 0.18f;
    public float minPauseTime = 0.6f;
    public float maxPauseTime = 1.8f;

    private Coroutine flickerRoutine;
    private bool isFlickering = false;
    private bool permanentlyStopped = false;

    private void Awake()
    {
        if (redLight == null)
            redLight = GetComponent<Light>();

        ForceOff();
    }

    private void Start()
    {
        ForceOff();
    }

    public void StartFlicker()
    {
        if (permanentlyStopped)
            return;

        if (redLight == null)
            return;

        if (isFlickering)
            return;

        isFlickering = true;
        flickerRoutine = StartCoroutine(FlickerLoop());
    }

    public void StopFlickerPermanently()
    {
        permanentlyStopped = true;
        ForceOff();
    }

    public void ForceOff()
    {
        isFlickering = false;

        if (flickerRoutine != null)
        {
            StopCoroutine(flickerRoutine);
            flickerRoutine = null;
        }

        if (redLight != null)
        {
            redLight.enabled = false;
            redLight.intensity = 0f;
        }
    }

    private IEnumerator FlickerLoop()
    {
        while (isFlickering && !permanentlyStopped)
        {
            int blinkCount = Random.Range(1, 4);

            for (int i = 0; i < blinkCount; i++)
            {
                if (!isFlickering || permanentlyStopped)
                    yield break;

                redLight.enabled = true;
                redLight.intensity = highIntensity;

                yield return new WaitForSeconds(Random.Range(minBlinkTime, maxBlinkTime));

                redLight.enabled = false;
                redLight.intensity = lowIntensity;

                yield return new WaitForSeconds(Random.Range(minBlinkTime, maxBlinkTime));
            }

            yield return new WaitForSeconds(Random.Range(minPauseTime, maxPauseTime));
        }

        ForceOff();
    }
}