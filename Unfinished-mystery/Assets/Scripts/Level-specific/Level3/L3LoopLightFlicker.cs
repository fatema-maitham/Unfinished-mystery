using System.Collections;
using UnityEngine;

public class L3LoopLightFlicker : MonoBehaviour
{
    [Header("Lights")]
    [SerializeField] private Light[] flickerLights;

    [Header("Loop Settings")]
    [SerializeField] private int currentLoop = 1;

    [Header("Loop 4 Flicker")]
    [SerializeField] private float loop4MinIntensity = 1.2f;
    [SerializeField] private float loop4MaxIntensity = 2f;
    [SerializeField] private float loop4Speed = 0.08f;

    [Header("Loop 5 Flicker")]
    [SerializeField] private float loop5MinIntensity = 0.8f;
    [SerializeField] private float loop5MaxIntensity = 2.5f;
    [SerializeField] private float loop5Speed = 0.04f;

    private Coroutine flickerRoutine;

    private void Start()
    {
        ApplyLoopFlicker();
    }

    public void SetCurrentLoop(int loop)
    {
        currentLoop = loop;
        Debug.Log("LIGHT FLICKER LOOP SET TO: " + currentLoop);
        ApplyLoopFlicker();
    }

    public void ApplyLoopFlicker()
    {
        if (flickerRoutine != null)
        {
            StopCoroutine(flickerRoutine);
            flickerRoutine = null;
        }

        if (currentLoop >= 4)
        {
            flickerRoutine = StartCoroutine(FlickerRoutine());
        }
        else
        {
            ResetLights();
        }
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        Debug.Log("LIGHT FLICKER RUNNING | Loop = " + currentLoop);
        {
            float minIntensity;
            float maxIntensity;
            float speed;

            if (currentLoop >= 5)
            {
                minIntensity = loop5MinIntensity;
                maxIntensity = loop5MaxIntensity;
                speed = loop5Speed;
            }
            else
            {
                minIntensity = loop4MinIntensity;
                maxIntensity = loop4MaxIntensity;
                speed = loop4Speed;
            }

            foreach (Light lightObj in flickerLights)
            {
                if (lightObj != null)
                {
                    lightObj.intensity =
                        Random.Range(minIntensity, maxIntensity);
                }
            }

            

            yield return new WaitForSeconds(speed);
        }
    }

    private void ResetLights()
    {
        foreach (Light lightObj in flickerLights)
        {
            if (lightObj != null)
            {
                lightObj.intensity = 2f;
            }
        }
    }
}