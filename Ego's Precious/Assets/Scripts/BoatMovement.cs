using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    private RiverNode currentNode;
    private RiverNode nextNode;

    private bool endGame;

    private Vector2 movementInput;

    private void Start()
    {
        float closestDistance = float.MaxValue;

        foreach (RiverNode riverNode in FindObjectsOfType<RiverNode>())
        {
            float distance = Vector3.Distance(transform.position, riverNode.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentNode = riverNode;
            }
        }

        if (!currentNode)
        {
            Debug.LogError("No start node found");
        }
        else
        {
            if(currentNode.NextNodes.Count == 0) 
            {
                Debug.LogError("closestnode doesn't have next node");
                return;
            }

            nextNode = currentNode.NextNodes[0];
            transform.position = currentNode.transform.position;

            transform.rotation = Quaternion.LookRotation(nextNode.transform.position - currentNode.transform.position);
        }
    }

    private void Update()
    {
        if (endGame)
        {
            return;
        }

        Vector3 currentToNextNode = nextNode.transform.position - currentNode.transform.position;
        currentToNextNode.Normalize();

        Vector3 ownPos = transform.position;
        ownPos.y = 0;       

        float t = VectorInverseLerp(currentNode.transform.position, nextNode.transform.position, ownPos);
        if (t > 0.98f)
        {
            currentNode = nextNode;

            if (currentNode.NextNodes.Count == 0)
            {
                endGame = true;
                return;
            }

            nextNode = currentNode.NextNodes[0];
        }


        float riverWidth = Mathf.Lerp(currentNode.RiverWidth, nextNode.RiverWidth, t);
        float horizontalForce = Mathf.Lerp(currentNode.HorizontalCurrent, nextNode.HorizontalCurrent, t);
        float verticalForce = Mathf.Lerp(currentNode.VerticalCurrent, nextNode.VerticalCurrent, t);

        
        Vector3 perpendicular = new Vector3(currentToNextNode.z, 0, -currentToNextNode.x);
        perpendicular.Normalize();

        Vector3 horizontalVelocity = perpendicular * horizontalForce + perpendicular * movementInput.x * moveSpeed;

        //normal movement
        transform.Translate(((moveSpeed + verticalForce) * Time.deltaTime * currentToNextNode), Space.World);

        //river current
        Vector3 posBetweenNodes = Vector3.Lerp(currentNode.transform.position, nextNode.transform.position, t);
        if (((Vector3.Cross((ownPos - currentNode.transform.position), currentToNextNode).y > 0 && horizontalForce > 0)
            || (Vector3.Cross((ownPos - currentNode.transform.position), currentToNextNode).y < 0 && horizontalForce < 0))
            || Vector3.Distance((ownPos + horizontalVelocity * Time.deltaTime), posBetweenNodes) < riverWidth / 2)
        {
            transform.Translate((horizontalVelocity * Time.deltaTime), Space.World);
        }
        else if (Vector3.Distance((ownPos + horizontalVelocity * Time.deltaTime), posBetweenNodes) > riverWidth / 2 + 0.1f) //too far so move back
        {
            transform.Translate((-horizontalVelocity * Time.deltaTime), Space.World);
        }

        Quaternion neededRotation = Quaternion.LookRotation(nextNode.transform.position - currentNode.transform.position);
        if(t > nextNode.RotationStart && nextNode.NextNodes.Count != 0)
        {
            neededRotation = Quaternion.LookRotation(nextNode.NextNodes[0].transform.position - nextNode.transform.position);
        }
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, neededRotation, Time.deltaTime * rotationSpeed);




        //Debug.Log("t: " + t);
        //Debug.Log("river width: " + riverWidth);
        Debug.Log("horizontalForce: " + horizontalForce);
        Debug.Log("posBetweenNodes: " + posBetweenNodes);
        //Debug.Log("verticalForce: " + verticalForce);
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }


    //https://answers.unity.com/questions/1271974/inverselerp-for-vector3.html
    float VectorInverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }
}
