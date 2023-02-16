using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatWaterLevel : MonoBehaviour
{
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private int waterLevelRisingTreshold = 2;
    [SerializeField] private float waterRiseSpeed = 0.05f;
    [SerializeField] private float minDepthBeforeSubmerged = 1;
    [SerializeField] private float maxDepthBeforeSubmerged = 10;

    [SerializeField] private int holeCount = 0;

    private float waterLevelPercentage;

    private List<BoatFloater> boatFloaters;

    private BoatMovement boatMovement;

    public float WaterLevelPercentage
    {
        get { return waterLevelPercentage; }
    }

    private void Awake()
    {
        boatFloaters = new List<BoatFloater>(gameObject.GetComponentsInChildren<BoatFloater>());

        boatMovement = GetComponent<BoatMovement>();

        if (!loseScreen)
        {
            Debug.LogError("loseScreen not assigned");
        }
    }

    private void Update()
    {
        holeCount = HoleAndStickerManager.instance.GetActiveHoleCount();

        waterLevelPercentage += (holeCount - (waterLevelRisingTreshold - 1)) * Time.deltaTime * waterRiseSpeed;

        if(waterLevelPercentage > 1)
        {
            boatMovement.EndGame = true;
            if (loseScreen)
            {
                loseScreen.SetActive(true);
            }
            waterLevelPercentage = 1;
        }
        if(waterLevelPercentage < 0)
        {
            waterLevelPercentage = 0;
        }

        float depthBeforeSubmerged = minDepthBeforeSubmerged + waterLevelPercentage * (maxDepthBeforeSubmerged - minDepthBeforeSubmerged);
        foreach (BoatFloater boatFloater in boatFloaters)
        {
            boatFloater.depthBeforeSublerged = depthBeforeSubmerged;
        }
    }
}
