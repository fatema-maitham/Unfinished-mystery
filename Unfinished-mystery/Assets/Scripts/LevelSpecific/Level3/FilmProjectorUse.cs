using System.Collections;using System.Reflection;using TMPro;using UnityEngine;using UnityEngine.Video;using InventoryFramework;

public class FilmProjectorUse : MonoBehaviour{[Header("Watched Progress")]public bool reel1Watched = false;public bool reel2Watched = false;public bool reel3Watched = false;

[Header("Collected Progress")]
public bool reel1Collected = false;
public bool reel2Collected = false;
public bool reel3Collected = false;

[Header("Prompt UI")]
public InteractionPromptUI interactPrompt;

[Header("Inventory Items")]
public Item reel1Item;
public Item reel2Item;
public Item reel3Item;
public Hotbar hotbar;

[Header("Video")]
public VideoPlayer videoPlayer;
public VideoClip reel1Clip;
public VideoClip reel2Clip;
public VideoClip reel3Clip;

[Header("Screen Idle Image")]
public Renderer screenRenderer;
public Texture idleScreenTexture;

[Header("Message UI")]
public RectTransform sceneMessagePanel;
public CanvasGroup sceneMessageCanvasGroup;
public TMP_Text sceneMessageText;
public float messageStayTime = 6f;
public float errorStayTime = 3f;

[Header("Exit Door Clue")]
public L3ExitFlickerGuide exitRedWarningLight;

[Header("TV Final Clue")]
public TVStaticSoundController tvStaticController;

[Header("Optional Error Sound")]
public AudioSource glitchSound;


[Header("Final Code Reveal")]
[SerializeField] private Texture finalCodeTexture; // Screen_FinalCode_0427
[SerializeField] private float glitchDelay = 0.6f;

[TextArea(2, 3)]
[SerializeField] private string finalCodeMessage = "The final frame reveals a code.";




private bool playerInRange = false;
private bool videoPlaying = false;
private bool isReplay = false;
private int currentReelNumber = 0;

private void Start()
{
    if (interactPrompt != null)
        interactPrompt.HidePrompt();

    if (videoPlayer != null)
    {
        videoPlayer.Stop();
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    if (sceneMessagePanel != null)
        sceneMessagePanel.gameObject.SetActive(false);

    if (exitRedWarningLight != null)
        exitRedWarningLight.ForceOff();

    ShowIdleScreen();
}


private IEnumerator RevealFinalCodeSequence()
{
    yield return null;

    if (videoPlayer != null)
    {
        videoPlayer.Stop();
        videoPlayer.clip = null;
    }

    if (screenRenderer != null && finalCodeTexture != null)
    {
        screenRenderer.material.mainTexture = finalCodeTexture;
        screenRenderer.material.SetTexture("_BaseMap", finalCodeTexture);
    }

    if (exitRedWarningLight != null)
    {
        exitRedWarningLight.StartFlicker();
    }
}



public void NotifyReelCollected(int reelNumber)
{
    if (reelNumber == 1) reel1Collected = true;
    if (reelNumber == 2) reel2Collected = true;
    if (reelNumber == 3) reel3Collected = true;

    UpdatePrompt();
}

private void Update()
{
    if (playerInRange && !videoPlaying && Input.GetKeyDown(KeyCode.E))
    {
        HandleProjectorUse();
    }
}

private void HandleProjectorUse()
{
    int reelToUse = GetBestReelAction();

    if (reelToUse == 0)
        return;

    if (reelToUse == 2 && !reel1Watched)
        {
            StartCoroutine(ShowErrorMessage(
                "This reel does not belong here yet.\nFind the missing scene first."
            ));
            return;
        }

    if (reelToUse == 3 && !reel2Watched)
    {
        StartCoroutine(ShowErrorMessage(
            "The reel stutters...\nThis ending does not make sense yet."
        ));
        return;
    }

    bool alreadyWatched = IsReelWatched(reelToUse);

    if (alreadyWatched)
    {
        isReplay = true;
        PlayReel(reelToUse);
        return;
    }

    Item itemToRemove = GetReelItem(reelToUse);

    if (itemToRemove == null || hotbar == null)
        return;

    bool removed = hotbar.RemoveItem(itemToRemove, 1);
if (removed)
{
    isReplay = false;
    SetReelCollected(reelToUse, false);

    if (reelToUse == 3 && tvStaticController != null)
        tvStaticController.StopStaticPermanently();

        HotbarUI hotbarUI = FindAnyObjectByType<HotbarUI>();
        if (hotbarUI != null)
            hotbarUI.RefreshUI();

        PlayReel(reelToUse);
    }
    else
    {
        StartCoroutine(ShowErrorMessage("You need the correct film reel first."));
    }
}

private int GetBestReelAction()
{
    if (!reel1Watched)
    {
        if (HasReelAvailable(1)) return 1;
        if (HasReelAvailable(2)) return 2;
        if (HasReelAvailable(3)) return 3;
        return 0;
    }

    if (!reel2Watched)
    {
        if (HasReelAvailable(2)) return 2;
        return 1;
    }

    if (!reel3Watched)
    {
        if (HasReelAvailable(3)) return 3;
        return 2;
    }

    return 3;
}

private bool HasReelAvailable(int reelNumber)
{
    if (reelNumber == 1) return reel1Collected || InventoryHasItem(reel1Item);
    if (reelNumber == 2) return reel2Collected || InventoryHasItem(reel2Item);
    if (reelNumber == 3) return reel3Collected || InventoryHasItem(reel3Item);

    return false;
}

private void SetReelCollected(int reelNumber, bool value)
{
    if (reelNumber == 1) reel1Collected = value;
    if (reelNumber == 2) reel2Collected = value;
    if (reelNumber == 3) reel3Collected = value;
}

private void PlayReel(int reelNumber)
{
    currentReelNumber = reelNumber;
    videoPlaying = true;

    if (interactPrompt != null)
        interactPrompt.HidePrompt();

    if (videoPlayer == null)
        return;
    videoPlayer.enabled = true;
    videoPlayer.Stop();
    videoPlayer.time = 0;
    videoPlayer.clip = GetReelClip(reelNumber);
    videoPlayer.Play();
}

private void OnVideoFinished(VideoPlayer vp)
{
    videoPlaying = false;

    vp.Stop();

    if (currentReelNumber != 3)
    {
        ShowIdleScreen();
    }

    if (isReplay)
        {
            if (currentReelNumber == 3)
            {
                StartCoroutine(RevealFinalCodeSequence());
            }

            UpdatePrompt();
            return;
        }

    if (currentReelNumber == 1 && !reel1Watched)
    {
        reel1Watched = true;

        if (exitRedWarningLight != null)
        {
            if (!HasReelAvailable(2) && !reel2Watched)
                exitRedWarningLight.StartFlicker();
            else
                exitRedWarningLight.ForceOff();
        }

        StartCoroutine(ShowDiscoveryThenPrompt(
            "Scene 1 discovered: The girl was inside the cinema before closing."
        ));
    }
    else if (currentReelNumber == 2 && !reel2Watched)
    {
        reel2Watched = true;

        if (tvStaticController != null)
        {
            if (!reel3Collected && !reel3Watched)
                tvStaticController.StartStatic();
            else
                tvStaticController.StopStaticPermanently();
        }

        StartCoroutine(ShowDiscoveryThenPrompt(
            "Scene 2 discovered: Someone blocked the exit that night. Find the final reel."
        ));
    }
        else if (currentReelNumber == 3 && !reel3Watched)
        {
            reel3Watched = true;

            if (tvStaticController != null)
                tvStaticController.StopStaticPermanently();

            StartCoroutine(ShowDiscoveryThenPrompt(
                "Final reel discovered: Maya never escaped."));

            StartCoroutine(RevealFinalCodeSequence());
        }
        else
        {
            UpdatePrompt();
        }
}

private IEnumerator ShowDiscoveryThenPrompt(string message)
{
    yield return StartCoroutine(ShowMessage(message, messageStayTime));
    UpdatePrompt();
}

private IEnumerator ShowErrorMessage(string message)
{
    if (glitchSound != null)
        glitchSound.Play();

    ShowIdleScreen();
    yield return StartCoroutine(ShowMessage(message, errorStayTime));
    UpdatePrompt();
}

private IEnumerator ShowMessage(string message, float stayTime)
{

    if (interactPrompt != null)
    interactPrompt.HidePrompt();


    if (sceneMessagePanel == null)
        yield break;

    if (sceneMessageText != null)
        sceneMessageText.text = message;

    sceneMessagePanel.gameObject.SetActive(true);

    Vector2 shownPos = sceneMessagePanel.anchoredPosition;
    Vector2 hiddenPos = shownPos + new Vector2(700f, 0f);

    sceneMessagePanel.anchoredPosition = hiddenPos;

    if (sceneMessageCanvasGroup != null)
        sceneMessageCanvasGroup.alpha = 0f;

    float t = 0f;

    while (t < 1f)
    {
        t += Time.deltaTime * 3f;
        sceneMessagePanel.anchoredPosition = Vector2.Lerp(hiddenPos, shownPos, t);

        if (sceneMessageCanvasGroup != null)
            sceneMessageCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

        yield return null;
    }

    yield return new WaitForSeconds(stayTime);

    t = 0f;

    while (t < 1f)
    {
        t += Time.deltaTime * 3f;
        sceneMessagePanel.anchoredPosition = Vector2.Lerp(shownPos, hiddenPos, t);

        if (sceneMessageCanvasGroup != null)
            sceneMessageCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

        yield return null;
    }

    sceneMessagePanel.anchoredPosition = shownPos;
    sceneMessagePanel.gameObject.SetActive(false);
}

private void ShowIdleScreen()
{
    if (screenRenderer != null && idleScreenTexture != null)
    {
        screenRenderer.material.mainTexture = idleScreenTexture;
        screenRenderer.material.SetTexture("_BaseMap", idleScreenTexture);
    }
}

private void UpdatePrompt()
{
    if (!playerInRange || interactPrompt == null || videoPlaying)
        return;

    int reelAction = GetBestReelAction();

    if (reelAction == 0)
    {
        interactPrompt.HidePrompt();
        return;
    }

    if (IsReelWatched(reelAction))
    interactPrompt.ShowPrompt("REPLAY", "Reel " + reelAction);
   else
        interactPrompt.ShowPrompt("LOAD", "Reel " + reelAction);
        }

private bool IsReelWatched(int reelNumber)
{
    if (reelNumber == 1) return reel1Watched;
    if (reelNumber == 2) return reel2Watched;
    if (reelNumber == 3) return reel3Watched;
    return false;
}

private Item GetReelItem(int reelNumber)
{
    if (reelNumber == 1) return reel1Item;
    if (reelNumber == 2) return reel2Item;
    if (reelNumber == 3) return reel3Item;
    return null;
}

private VideoClip GetReelClip(int reelNumber)
{
    if (reelNumber == 1) return reel1Clip;
    if (reelNumber == 2) return reel2Clip;
    if (reelNumber == 3) return reel3Clip;
    return null;
}

private bool InventoryHasItem(Item item)
{
    if (hotbar == null || item == null)
        return false;

    MethodInfo[] methods = hotbar.GetType().GetMethods();

    foreach (MethodInfo method in methods)
    {
        if (method.Name == "HasItem")
        {
            ParameterInfo[] p = method.GetParameters();

            if (p.Length == 2)
            {
                object result = method.Invoke(hotbar, new object[] { item, 1 });
                if (result is bool hasItem) return hasItem;
            }

            if (p.Length == 1)
            {
                object result = method.Invoke(hotbar, new object[] { item });
                if (result is bool hasItem) return hasItem;
            }
        }

        if (method.Name == "ContainsItem")
        {
            ParameterInfo[] p = method.GetParameters();

            if (p.Length == 1)
            {
                object result = method.Invoke(hotbar, new object[] { item });
                if (result is bool hasItem) return hasItem;
            }
        }

        if (method.Name == "GetItemCount")
        {
            ParameterInfo[] p = method.GetParameters();

            if (p.Length == 1)
            {
                object result = method.Invoke(hotbar, new object[] { item });
                if (result is int count) return count > 0;
            }
        }
    }

    return false;
}

private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        playerInRange = true;
        UpdatePrompt();
    }
}

private void OnTriggerExit(Collider other)
{
    if (other.CompareTag("Player"))
    {
        playerInRange = false;

        if (interactPrompt != null)
            interactPrompt.HidePrompt();
    }
}

}