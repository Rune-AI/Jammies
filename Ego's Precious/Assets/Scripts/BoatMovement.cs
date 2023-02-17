using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static UnityEngine.Rendering.DebugUI;

public class BoatMovement : MonoBehaviour
{
    //public GameObject visual;
    //public GameObject visualBezier;

    [SerializeField] private GameObject creditSceneManager;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float tiltedMoveSpeed = 10;
    [SerializeField] private float sidePushSpeed = 2;

    private RiverNodeManager riverNodeManager;

    private List<RiverNode> riverNodes = Enumerable.Repeat<RiverNode>(null, 4).ToList(); //fill list with 4 empty nodes

    private bool endGame = false;

    public bool EndGame
    {
        get { return endGame; }
        set { endGame = value; }
    }

    private Vector2 movementInput;

    private void Awake()
    {
        riverNodeManager = FindObjectOfType<RiverNodeManager>();

        if (!creditSceneManager)
        {
            Debug.LogError("winScreen not assigned");
        }
    }

    private void Start()
    {
        float closestDistance = float.MaxValue;

        foreach (RiverNode riverNode in riverNodeManager.RiverNodes)
        {
            float distance = Vector3.Distance(transform.position, riverNode.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                riverNodes[0] = riverNode; //current
                riverNodes[1] = riverNode; //current
            }
        }

        if (!riverNodes[1])
        {
            Debug.LogError("No start node found");
            endGame = true;
        }
        else
        {
            if(riverNodes[1].NextNodes.Count == 0) 
            {
                Debug.LogError("startnode doesn't have next node");
                endGame = true;
                return;
            }
            if (riverNodes[1].NextNodes.Count > 1)
            {
                Debug.LogError("startnode isn't allowed to have more than 1 next node");
                endGame = true;
                return;
            }

            riverNodes[2] = riverNodes[1].NextNodes[0];
            Vector3 position = riverNodes[1].transform.position;
            position.y = transform.position.y;
            transform.position = position;

            transform.rotation = Quaternion.LookRotation(riverNodes[2].transform.position - riverNodes[1].transform.position);

            if (riverNodes[2].NextNodes.Count == 0)
            {
                riverNodes[3] = riverNodes[2];
            }
            else
            {
                riverNodes[3] = riverNodes[2].NextNodes[0];
            }

            foreach(RiverNode riverNode in riverNodes)
            {
                if (!riverNode)
                {
                    Debug.LogError("empty node assigned");
                    endGame = true;
                    return;
                }
            }
        }
    }

    private void Update()
    {
        if (endGame)
        {
            return;
        }

        movementInput.x = (transform.rotation.eulerAngles.z - 180) / 180;

        if(movementInput.x > 0)
        {
            movementInput.x = 1 - movementInput.x;
            movementInput.x = Mathf.Sqrt(movementInput.x);
        }
        else if(movementInput.x < 0)
        {
            movementInput.x = -1 - movementInput.x;
            movementInput.x = -Mathf.Sqrt(Mathf.Abs(movementInput.x));
        }

        if (Mathf.Abs(movementInput.x) < 0.02f)
        {
            movementInput.x = 0;
        }

        movementInput.x *= tiltedMoveSpeed;


        Vector3 currentToNextNode = riverNodes[2].transform.position - riverNodes[1].transform.position;
        currentToNextNode.Normalize();

        Vector3 ownPos = transform.position;
        ownPos.y = 0;       

        float t = Mathf.Clamp01(VectorInverseLerp(riverNodes[1].transform.position, riverNodes[2].transform.position, ownPos));
        float reachedNodeValue = 0.99f;
        if (t >= reachedNodeValue)
        {
            riverNodes[0] = riverNodes[1];
            riverNodes[1] = riverNodes[2];

            if (riverNodes[1].NextNodes.Count == 0)
            {
                Debug.Log("endgame");
                endGame = true;
                if (creditSceneManager)
                {
                    creditSceneManager.SetActive(true);
                }
                return;
            }

            //set node 2
            float closestDistance = float.MaxValue;
            foreach (RiverNode riverNode in riverNodes[1].NextNodes) 
            {
                Vector3 direction = riverNode.transform.position - riverNodes[1].transform.position;
                direction.Normalize();

                float distance = Vector3.Distance((riverNodes[1].transform.position + direction), ownPos);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    riverNodes[2] = riverNode;
                    
                }
            }

            //set node 3
            if (riverNodes[2].NextNodes.Count == 0)
            {
                riverNodes[3] = riverNodes[2];
            }
            else
            {                
                closestDistance = float.MaxValue;
                foreach (RiverNode riverNode in riverNodes[2].NextNodes)
                {
                    Vector3 direction = riverNode.transform.position - riverNodes[2].transform.position;
                    direction.Normalize();

                    float distance = Vector3.Distance((riverNodes[2].transform.position + direction), ownPos);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        riverNodes[3] = riverNode;
                    }
                }
            }

            //recalc t
            t = Mathf.Clamp01(VectorInverseLerp(riverNodes[1].transform.position, riverNodes[2].transform.position, ownPos));
        }


        float riverWidth = Mathf.Lerp(riverNodes[1].RiverWidth, riverNodes[2].RiverWidth, t);
        float horizontalForce = Mathf.Lerp(riverNodes[1].HorizontalCurrent, riverNodes[2].HorizontalCurrent, t);
        float verticalForce = Mathf.Lerp(riverNodes[1].VerticalCurrent, riverNodes[2].VerticalCurrent, t);

        
        Vector3 perpendicular = new Vector3(currentToNextNode.z, 0, -currentToNextNode.x);
        perpendicular.Normalize();

        Vector3 horizontalVelocity = perpendicular * horizontalForce + perpendicular * movementInput.x;

        //normal movement
        transform.Translate(((moveSpeed + verticalForce) * Time.deltaTime * currentToNextNode), Space.World);

        //river current
        Vector3 posBetweenNodes = GetPositionBetweenNodes(t);
        Vector3 posBetweenNodesProjected = GetBezierPointProjectedOnPerpendicularRiverGraph(posBetweenNodes, perpendicular, Vector3.Lerp(riverNodes[1].transform.position, riverNodes[2].transform.position, t));
        //visualBezier.transform.position = posBetweenNodes;
        //visual.transform.position = Vector3.Lerp(riverNodes[1].transform.position, riverNodes[2].transform.position, t);
        if (((Vector3.Cross((ownPos - riverNodes[1].transform.position), currentToNextNode).y > 0 && horizontalForce + movementInput.x > 0)
            || (Vector3.Cross((ownPos - riverNodes[1].transform.position), currentToNextNode).y < 0 && horizontalForce + movementInput.x < 0))
            || Vector3.Distance((ownPos + horizontalVelocity * Time.deltaTime), posBetweenNodesProjected) < riverWidth / 2)
        {
            transform.Translate((horizontalVelocity * Time.deltaTime), Space.World);
        }
        else if (Vector3.Distance((ownPos + horizontalVelocity * Time.deltaTime), posBetweenNodesProjected) > riverWidth / 2 + 0.1f) //too far so move back
        {
            Vector3 direction = posBetweenNodesProjected - ownPos;
            direction.Normalize();
            transform.Translate((direction * sidePushSpeed * Time.deltaTime), Space.World);
        }

        Quaternion neededRotation = Quaternion.LookRotation(GetPositionBetweenNodes(t + (1 - reachedNodeValue)) - posBetweenNodes);

        //Vector3 rotation = transform.eulerAngles;
        //rotation.y = neededRotation.eulerAngles.y;
        //transform.eulerAngles = rotation;

        Vector3 eulers = Quaternion.RotateTowards(transform.rotation, neededRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        eulers.x = transform.eulerAngles.x;
        eulers.z = transform.eulerAngles.z;
        transform.eulerAngles = eulers;

        //Debug.Log("t: " + t);
        //Debug.Log("river width: " + riverWidth);
        //Debug.Log("horizontalForce: " + horizontalForce);
        //Debug.Log("posBetweenNodes: " + posBetweenNodes);
        //Debug.Log("verticalForce: " + verticalForce);
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }


    //https://apoorvaj.io/cubic-bezier-through-four-points/
    Vector3 GetPositionBetweenNodes(float t)
    {
        const float alpha = 0.5f;

        Vector3 p0 = riverNodes[0].transform.position;
        Vector3 p1 = riverNodes[1].transform.position;
        Vector3 p2 = riverNodes[2].transform.position;
        Vector3 p3 = riverNodes[3].transform.position;

        if(p0 == p1)
        {
            p0 += (p1 - p2).normalized * 0.01f;
        }
        if (p1 == p2)
        {
            p1 += (p2 - p3).normalized * 0.01f;
        }
        if (p2 == p3)
        {
            p2 += (p1 - p2).normalized * 0.01f; //cant use p4 cause we dont have it
        }

        float d1 = Mathf.Pow((p1 - p0).magnitude, alpha);
        float d2 = Mathf.Pow((p2 - p1).magnitude, alpha);
        float d3 = Mathf.Pow((p3 - p2).magnitude, alpha);

        Vector3 t1 = (Mathf.Pow(d1, 2) * p2 - Mathf.Pow(d2, 2) * p0 + (2 * Mathf.Pow(d1, 2) + 3 * d1 * d2 + Mathf.Pow(d2, 2)) * p1) / (3 * d1 * (d1 + d2));
        Vector3 t2 = (Mathf.Pow(d3, 2) * p1 - Mathf.Pow(d2, 2) * p3 + (2 * Mathf.Pow(d3, 2) + 3 * d3 * d2 + Mathf.Pow(d2, 2)) * p2) / (3 * d3 * (d3 + d2));

        return p1 * Mathf.Pow((1 - t), 3) + t1 * 3 * Mathf.Pow((1 - t), 2) * t + t2 * 3 * (1 - t) * Mathf.Pow(t, 2) + p2 * Mathf.Pow(t, 3);
    }


    Vector3 GetBezierPointProjectedOnPerpendicularRiverGraph(Vector3 bezierPos, Vector3 perpendicularVector, Vector3 boatPosOnRiverGraph)
    {
        Vector3 lineP1 = boatPosOnRiverGraph;
        Vector3 lineP2 = boatPosOnRiverGraph + perpendicularVector;
        return Vector3.Project((bezierPos - lineP1), (lineP2 - lineP1)) + lineP1;
    }












//https://answers.unity.com/questions/1271974/inverselerp-for-vector3.html
float VectorInverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    float BezierInverseLerp(Vector3 position)
    {
        const float alpha = 0.5f;

        Vector3 p0 = riverNodes[0].transform.position;
        Vector3 p1 = riverNodes[1].transform.position;
        Vector3 p2 = riverNodes[2].transform.position;
        Vector3 p3 = riverNodes[3].transform.position;

        if (p0 == p1)
        {
            p0 += (p1 - p2).normalized * 0.01f;
        }
        if (p1 == p2)
        {
            p1 += (p2 - p3).normalized * 0.01f;
        }
        if (p2 == p3)
        {
            p2 += (p1 - p2).normalized * 0.01f; //cant use p4 cause we dont have it
        }

        float d1 = Mathf.Pow((p1 - p0).magnitude, alpha);
        float d2 = Mathf.Pow((p2 - p1).magnitude, alpha);
        float d3 = Mathf.Pow((p3 - p2).magnitude, alpha);

        Vector3 t1 = (Mathf.Pow(d1, 2) * p2 - Mathf.Pow(d2, 2) * p0 + (2 * Mathf.Pow(d1, 2) + 3 * d1 * d2 + Mathf.Pow(d2, 2)) * p1) / (3 * d1 * (d1 + d2));
        Vector3 t2 = (Mathf.Pow(d3, 2) * p1 - Mathf.Pow(d2, 2) * p3 + (2 * Mathf.Pow(d3, 2) + 3 * d3 * d2 + Mathf.Pow(d2, 2)) * p2) / (3 * d3 * (d3 + d2));



        return VectorInverseLerp(t1, t2, position);
    }
}
