using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverNode : MonoBehaviour
{
    [SerializeField] private float horizontalCurrent;
    [SerializeField] private float verticalCurrent;
    [SerializeField] private float riverWidth;

    [SerializeField] private List<RiverNode> nextNodes;

    //[SerializeField] private float rotationStart = 0.8f;

    public List<RiverNode> NextNodes
    {
        get { return nextNodes; }
    }

    public float HorizontalCurrent
    { 
        get { return horizontalCurrent; } 
    }

    public float VerticalCurrent
    {
        get { return verticalCurrent; }
    }

    public float RiverWidth
    {
        get { return riverWidth; }
    }

    //public float RotationStart
    //{
    //    get { return rotationStart; }
    //}

}
