using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverNode : MonoBehaviour
{
    [SerializeField] private Vector2 riverForce;
    [SerializeField] private Vector2 windForce;

    [SerializeField] private List<RiverNode> connections;

    public List<RiverNode> Connections
    {
        get { return Connections; }
    }
    
}
