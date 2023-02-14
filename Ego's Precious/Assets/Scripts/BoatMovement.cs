using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    private RiverNode currentNode;
    private RiverNode nextNode;

    private void Start()
    {
        List<RiverNode> riverNodes = new List<RiverNode> { };

        RiverNode closestNode = null;
        float closestDistance = float.MaxValue;

        foreach (RiverNode riverNode in riverNodes)
        {
            if (Vector3.Distance(transform.position, riverNode.transform.position) < closestDistance)
            {
                closestNode = riverNode;
            }
        }

        currentNode = closestNode;
    }

    private void Update()
    {
        Vector3 velocity = nextNode.transform.position - currentNode.transform.position;
        velocity.Normalize();

        transform.position += speed * Time.deltaTime * velocity;

        Vector3 ownPos = transform.position;
        ownPos.y = 0;
        if (Vector3.Distance(ownPos, nextNode.transform.position) < 0.1f)
        {
            currentNode = nextNode;

            nextNode = currentNode.Connections[0];

        }
    }
}
