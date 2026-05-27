using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoopChangeSystem : MonoBehaviour
{
    [System.Serializable]
    public class LoopObject
    {
        public string objectName;
        public GameObject target;

        [Header("Use Local Transform")]
        public bool useLocalPosition = true;

        [Header("Position for Each Loop")]
        public Vector3[] loopPositions = new Vector3[5];

        [Header("Rotation for Each Loop")]
        public Vector3[] loopRotations = new Vector3[5];

        [Header("Active / Hidden for Each Loop")]
        public bool[] loopActiveStates = new bool[5];
    }

    [Header("Loop Settings")]
    public int currentLoop = 1;
    public int maxLoops = 5;

    [Header("UI Text")]
    public TMP_Text loopText;

    [Header("Objects That Change Each Loop")]
    public List<LoopObject> loopObjects = new List<LoopObject>();

    private void Start()
    {
        currentLoop = Mathf.Clamp(currentLoop, 1, maxLoops);
        ApplyLoopChanges();
        UpdateLoopUI();
    }

    public void GoToNextLoop()
    {
        currentLoop++;

        if (currentLoop > maxLoops)
            currentLoop = maxLoops;

        ApplyLoopChanges();
        UpdateLoopUI();
    }

    public void SetLoop(int loopNumber)
    {
        currentLoop = Mathf.Clamp(loopNumber, 1, maxLoops);

        ApplyLoopChanges();
        UpdateLoopUI();
    }

    private void ApplyLoopChanges()
    {
        int loopIndex = currentLoop - 1;

        foreach (LoopObject item in loopObjects)
        {
            if (item == null || item.target == null)
                continue;

            if (item.loopActiveStates != null && item.loopActiveStates.Length > loopIndex)
            {
                item.target.SetActive(item.loopActiveStates[loopIndex]);
            }

            if (item.loopPositions != null && item.loopPositions.Length > loopIndex)
            {
                if (item.useLocalPosition)
                    item.target.transform.localPosition = item.loopPositions[loopIndex];
                else
                    item.target.transform.position = item.loopPositions[loopIndex];
            }

            if (item.loopRotations != null && item.loopRotations.Length > loopIndex)
            {
                if (item.useLocalPosition)
                    item.target.transform.localRotation = Quaternion.Euler(item.loopRotations[loopIndex]);
                else
                    item.target.transform.rotation = Quaternion.Euler(item.loopRotations[loopIndex]);
            }
        }
    }

    private void UpdateLoopUI()
    {
        if (loopText != null)
        {
            loopText.text = "Loop " + currentLoop + " / " + maxLoops;
        }
    }
}