using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverNodeManager : MonoBehaviour
{
    [SerializeField] private List<RiverNode> riverNodes;

    public List<RiverNode> RiverNodes
    {
        get { return riverNodes; }
    }

    private void Awake()
    {
        riverNodes = new List<RiverNode>(FindObjectsOfType<RiverNode>());
    }
}
