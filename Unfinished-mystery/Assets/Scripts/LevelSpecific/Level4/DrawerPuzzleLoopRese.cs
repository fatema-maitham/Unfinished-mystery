using System.Collections;
using UnityEngine;

public class DrawerPuzzleLoopReset : MonoBehaviour, ILoopResettable
{
    [SerializeField] private DrawerKeypadInteraction drawerPuzzle;
    [SerializeField] private MonoBehaviour interactableUI;
    [SerializeField] private KeypadUI keypadUI;

    public void ResetState()
    {
        if (drawerPuzzle != null)
            drawerPuzzle.ResetDrawerPuzzle();

        if (keypadUI != null)
            keypadUI.ResetState();

        StartCoroutine(RefreshInteraction());
    }

    private IEnumerator RefreshInteraction()
    {
        yield return null;

        if (interactableUI != null)
        {
            interactableUI.enabled = false;
            yield return null;
            interactableUI.enabled = true;
        }
    }
}