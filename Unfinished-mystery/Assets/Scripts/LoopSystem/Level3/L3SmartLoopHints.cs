using System.Collections;

using UnityEngine;

public class L3SmartLoopHints : MonoBehaviour
{
    [Header("Story Progress Source")]
    [SerializeField] private FilmProjectorUse projectorUse;

    [Header("Loop Settings")]
    [SerializeField] private int currentLoop = 1;
    [SerializeField] private int maxLoops = 5;

    [Header("Hint Objects")]
    [SerializeField] private L3ExitFlickerGuide exitRedLight;
    [SerializeField] private TVStaticSoundController radioStaticHint;
    [SerializeField] private GameObject mirrorLight;


    [Header("Loop 4/5 Atmosphere")]
    [SerializeField] private GameObject screenGlitchHint;


    [Header("Projector Screen Glitch")]
    [SerializeField] private FilmProjectorUse projectorScreen;
    [SerializeField] private float loop4GlitchInterval = 18f;
    [SerializeField] private float loop5GlitchInterval = 9f;
    [SerializeField] private float loop4GlitchDuration = 0.7f;
    [SerializeField] private float loop5GlitchDuration = 1f;


    [Header("Loop Light Flicker")]
    [SerializeField] private L3LoopLightFlicker loopLightFlicker;

    private Coroutine glitchRoutine;


    [Header("Loop Tension Hum")]
    [SerializeField] private AudioSource loopTensionHum;
    [SerializeField] private float loop4HumVolume = 0.08f;
    [SerializeField] private float loop5HumVolume = 0.14f;


    

    private void Start()
    {
        ApplyLoopHints();
    }

    public void SetCurrentLoop(int loopNumber)
    {
        currentLoop = Mathf.Clamp(loopNumber, 1, maxLoops);
        ApplyLoopHints();
    }

    public void ApplyLoopHints()
    {
        if (projectorUse == null)
            return;

        Debug.Log("SMART LOOP CHECK | Loop = " + currentLoop +
          " | Reel1Watched = " + projectorUse.reel1Watched +
          " | Reel2Collected = " + projectorUse.reel2Collected);

       bool exitHintOn =
            projectorUse.reel1Watched &&
            currentLoop >= 2 &&
            !projectorUse.reel2Collected &&
            !projectorUse.reel2Watched;

        bool finalHintOn =
            projectorUse.reel2Watched &&
            currentLoop >= 3 &&
            !projectorUse.reel3Collected;

        if (exitRedLight != null)
        {
            if (exitHintOn)
                exitRedLight.StartFlicker();
            else
                exitRedLight.ForceOff();
        }

        if (radioStaticHint != null && finalHintOn)
        {
            radioStaticHint.StartStatic();
        }

        if (mirrorLight != null)
        {
            mirrorLight.SetActive(finalHintOn);
        }
        


        HandleLoopGlitch();


        if (loopLightFlicker != null)
        {
            loopLightFlicker.SetCurrentLoop(currentLoop);
        }


                if (loopTensionHum != null)
        {
            if (currentLoop >= 5)
            {
                if (!loopTensionHum.isPlaying)
                    loopTensionHum.Play();

                loopTensionHum.volume = loop5HumVolume;
            }
            else if (currentLoop >= 4)
            {
                if (!loopTensionHum.isPlaying)
                    loopTensionHum.Play();

                loopTensionHum.volume = loop4HumVolume;
            }
            else
            {
                loopTensionHum.Stop();
            }
        }

    }


    private void HandleLoopGlitch()
{


    Debug.Log("HANDLE GLITCH | Current Loop = " + currentLoop);
    if (currentLoop < 4)
    {
        if (glitchRoutine != null)
        {
            StopCoroutine(glitchRoutine);
            glitchRoutine = null;
        }

        return;
    }

    if (glitchRoutine == null)
    {   
        Debug.Log("STARTING LOOP GLITCH ROUTINE");
        glitchRoutine = StartCoroutine(LoopGlitchRoutine());
    }
}

private IEnumerator LoopGlitchRoutine()
{
    while (true)
    {
        float waitTime = currentLoop >= 5
            ? loop5GlitchInterval
            : loop4GlitchInterval;

        float glitchDuration = currentLoop >= 5
            ? loop5GlitchDuration
            : loop4GlitchDuration;

        yield return new WaitForSeconds(waitTime);

        if (projectorScreen != null)
        {
            yield return StartCoroutine(
                projectorScreen.ShowLoopGlitch(glitchDuration)
            );
        }
    }
}


        [ContextMenu("Apply Loop Hints Now")]
    private void ApplyLoopHintsNow()
    {
        ApplyLoopHints();
    }
}