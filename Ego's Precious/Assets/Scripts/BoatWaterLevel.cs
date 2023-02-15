using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatWaterLevel : MonoBehaviour
{
    [SerializeField] private int waterLevelRisingTreshold = 2;
    [SerializeField] private float waterRiseSpeed = 0.05f;
    [SerializeField] private float minDepthBeforeSubmerged = 1;
    [SerializeField] private float maxDepthBeforeSubmerged = 10;

    [SerializeField] public int holeCount = 3;

    private float waterLevelPercentage;

    private List<BoatFloater> boatFloaters;

    public float WaterLevelPercentage
    {
        get { return waterLevelPercentage; }
    }

    private void Awake()
    {
        boatFloaters = new List<BoatFloater>(gameObject.GetComponentsInChildren<BoatFloater>());
    }

    private void Update()
    {
        waterLevelPercentage += (holeCount - (waterLevelRisingTreshold - 1)) * Time.deltaTime * waterRiseSpeed;

        waterLevelPercentage = Mathf.Clamp01(waterLevelPercentage);

        float depthBeforeSubmerged = minDepthBeforeSubmerged + waterLevelPercentage * (maxDepthBeforeSubmerged - minDepthBeforeSubmerged);
        foreach (BoatFloater boatFloater in boatFloaters)
        {
            boatFloater.depthBeforeSublerged = depthBeforeSubmerged;
        }
    }
}
