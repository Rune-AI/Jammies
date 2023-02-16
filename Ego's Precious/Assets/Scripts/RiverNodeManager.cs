using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class RiverNodeManager : MonoBehaviour
{
    [SerializeField] private bool visualize;

    private List<RiverNode> riverNodes;

    public List<RiverNode> RiverNodes
    {
        get { return riverNodes; }
    }

    private void Awake()
    {
        riverNodes = new List<RiverNode>(FindObjectsOfType<RiverNode>());
    }

    private void Update()
    {
        if (!visualize)
        {
            return;
        }

        riverNodes = new List<RiverNode>(FindObjectsOfType<RiverNode>());
    }

    private void OnDrawGizmos()
    {
        if (!visualize)
        {
            return;
        }

        foreach (RiverNode node in riverNodes)
        {
            Gizmos.DrawSphere(node.transform.position, 0.5f);
            foreach(RiverNode nodeNeighbour in node.NextNodes)
            {
                Vector3 currentToNextNode = nodeNeighbour.transform.position - node.transform.position;
                Vector3 perpendicular = new Vector3(currentToNextNode.z, 0, -currentToNextNode.x);
                perpendicular.Normalize();

                Gizmos.DrawLine(node.transform.position, nodeNeighbour.transform.position);
                Gizmos.DrawLine(node.transform.position + perpendicular * node.RiverWidth / 2, nodeNeighbour.transform.position + perpendicular * nodeNeighbour.RiverWidth / 2);
                Gizmos.DrawLine(node.transform.position - perpendicular * node.RiverWidth / 2, nodeNeighbour.transform.position - perpendicular * nodeNeighbour.RiverWidth / 2 );
            }
        }
    }
}
