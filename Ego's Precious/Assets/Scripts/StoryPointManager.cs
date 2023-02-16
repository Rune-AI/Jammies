using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPointManager : MonoBehaviour
{
    [SerializeField] private Transform boat;
    [SerializeField] private List<float> StoryPointZTriggers;
    [SerializeField] private List<GameObject> StoryPoints;
    [SerializeField] private List<float> StoryPointLength;

    private void Update()
    {
        for (int i = 0; i < StoryPointZTriggers.Count; i++)
        {
            if (StoryPointZTriggers[i] < boat.position.z)
            {
                float distance = boat.position.z - StoryPointZTriggers[i];
                if (distance < StoryPointLength[i])
                {
                    StoryPoints[i].SetActive(true);
                }
                else
                {
                    StoryPoints[i].SetActive(false);
                    //Destroy(StoryPoints[i]);
                }
            }
            else
            {
                StoryPoints[i].SetActive(false);
            }
        }
    
    }
}
