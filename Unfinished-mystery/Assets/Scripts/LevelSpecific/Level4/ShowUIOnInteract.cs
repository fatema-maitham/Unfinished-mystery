using UnityEngine;

public class ShowUIOnInteract : MonoBehaviour
{
    public GameObject uiObject;

    void Start()
    {
        if (uiObject != null)
            uiObject.SetActive(false);
    }

    public void OnActivate(GameObject source)
    {
        if (uiObject != null)
            uiObject.SetActive(true);
    }
}