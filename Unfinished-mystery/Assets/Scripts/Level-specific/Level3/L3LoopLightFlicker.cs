using System.Collections;
using UnityEngine;

public class L3LoopLightFlicker : MonoBehaviour
{
    [Header("Lights")]
    [SerializeField] private Light[] flickerLights;

    [Header("Loop Settings")]
    [SerializeField] private int currentLoop = 1;

    [Header("Flicker Timing")]
    [SerializeField] private float loop4OffTime = 0.18f;
    [SerializeField] private float loop4OnTime = 0.35f;
    [SerializeField] private float loop5OffTime = 0.12f;
    [SerializeField] private float loop5OnTime = 0.18f;

    private Coroutine flickerRoutine;
    private int lastLoop = -1;

    private void Start()
    {
        ApplyLoopFlicker();
    }

    private void Update()
    {
        if (lastLoop != currentLoop)
        {
            ApplyLoopFlicker();
        }
    }

    public void SetCurrentLoop(int loop)
    {
        currentLoop = loop;
        ApplyLoopFlicker();
    }

    public void ApplyLoopFlicker()
    {
        lastLoop = currentLoop;

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
        {
            float offTime = currentLoop >= 5 ? loop5OffTime : loop4OffTime;
            float onTime = currentLoop >= 5 ? loop5OnTime : loop4OnTime;

            foreach (Light lightObj in flickerLights)
            {
                if (lightObj != null)
                    lightObj.enabled = false;
            }

            yield return new WaitForSeconds(offTime);

            foreach (Light lightObj in flickerLights)
            {
                if (lightObj != null)
                    lightObj.enabled = true;
            }

            yield return new WaitForSeconds(onTime);
        }
    }

    private void ResetLights()
    {
        foreach (Light lightObj in flickerLights)
        {
            if (lightObj != null)
                lightObj.enabled = true;
        }
    }
}