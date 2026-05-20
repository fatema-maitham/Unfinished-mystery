using System.Collections.Generic;
using UnityEngine;

public class LoopChangeSystem : MonoBehaviour
{
    [System.Serializable]
    public class LoopObject
    {
        public string objectName;
        public GameObject target;

        [Header("Position for Each Loop")]
        public Vector3[] loopPositions = new Vector3[10];

        [Header("Active / Hidden for Each Loop")]
        public bool[] loopActiveStates = new bool[10];
    }

    [Header("Loop Settings")]
    public int currentLoop = 1;
    public int maxLoops = 10;

    [Header("Objects That Change Each Loop")]
    public List<LoopObject> loopObjects = new List<LoopObject>();

    private void Start()
    {
        ApplyLoopChanges();
    }

    public void GoToNextLoop()
    {
        currentLoop++;

        if (currentLoop > maxLoops)
            currentLoop = maxLoops;

        ApplyLoopChanges();
    }

    public void SetLoop(int loopNumber)
    {
        currentLoop = Mathf.Clamp(loopNumber, 1, maxLoops);
        ApplyLoopChanges();
    }

    private void ApplyLoopChanges()
    {
        int loopIndex = currentLoop - 1;

        foreach (LoopObject item in loopObjects)
        {
            if (item.target == null)
                continue;

            if (item.loopPositions != null && item.loopPositions.Length > loopIndex)
            {
                item.target.transform.position = item.loopPositions[loopIndex];
            }

            if (item.loopActiveStates != null && item.loopActiveStates.Length > loopIndex)
            {
                item.target.SetActive(item.loopActiveStates[loopIndex]);
            }
        }
    }
}