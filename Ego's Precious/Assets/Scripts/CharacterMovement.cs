using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float downWardForce = 1;
    [SerializeField] private List<BoxCollider> boatColliders;
    [SerializeField] private List<Vector2> boundsPolygon;

    //private CharacterController characterController;

    private Rigidbody ownRigidbody;

    private Vector2 movementInput;

    private Vector3 nextMoveVector;

    private GameObject boat;

    private void Awake()
    {
        boat = transform.parent.gameObject;

        ownRigidbody = GetComponent<Rigidbody>();

        //characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        //Vector3 moveVector = transform.parent.transform.TransformVector(new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.deltaTime);
        //characterController.Move(moveVector);

        //transform.localPosition += new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.fixedDeltaTime;

        //BoxCollider hitCollider = null;
        //foreach (BoxCollider collider in boatColliders)
        //{
        //    if (transform.localPosition.x < collider.center.x + collider.size.x / 2
        //        && transform.localPosition.x > collider.center.x - collider.size.x / 2
        //        && transform.localPosition.y < collider.center.y + collider.size.y / 2
        //        && transform.localPosition.y > collider.center.y - collider.size.y / 2
        //        && transform.localPosition.z < collider.center.z + collider.size.z / 2
        //        && transform.localPosition.z > collider.center.z - collider.size.z / 2)

        //    {
        //        hitCollider = collider;
        //        break;
        //    }
        //}

        //if (hitCollider)
        //{
        //    Bounds bounds = new Bounds();
        //    bounds.center = hitCollider.center;
        //    bounds.size = hitCollider.size;

        //    transform.localPosition = GetClosestPointOnBounds(bounds, transform.localPosition);
        //}

        nextMoveVector = new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.fixedDeltaTime;
        Vector3 newPosition = transform.localPosition + nextMoveVector * 10;

        bool colliding = false;
        foreach (BoxCollider collider in boatColliders)
        {
            if (newPosition.x < collider.center.x + collider.size.x / 2
                && newPosition.x > collider.center.x - collider.size.x / 2
                && newPosition.y < collider.center.y + collider.size.y / 2
                && newPosition.y > collider.center.y - collider.size.y / 2
                && newPosition.z < collider.center.z + collider.size.z / 2
                && newPosition.z > collider.center.z - collider.size.z / 2)

            {
                colliding = true;
                break;
            }
        }

        if (!colliding)
        {
            transform.localPosition += nextMoveVector;
        }


        //nextMoveVector = new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.fixedDeltaTime;
        //nextMoveVector = transform.parent.transform.TransformDirection(nextMoveVector);
        //Vector3 newPosition = transform.position + nextMoveVector * 10;

        //bool colliding = false;
        //foreach (Collider collider in boatColliders)
        //{
        //    if (collider.bounds.Contains(newPosition))
        //    {
        //        colliding = true;
        //        break;
        //    }
        //}

        //if (!colliding)
        //{
        //    transform.position += nextMoveVector;
        //}


        //nextMoveVector = new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.fixedDeltaTime;
        //Vector3 newPosition = transform.localPosition + nextMoveVector * 10;

        //newPosition = transform.parent.transform.TransformPoint(newPosition);

        //bool colliding = false;
        //foreach (Collider collider in boatColliders)
        //{
        //    if (collider.bounds.Contains(newPosition))
        //    {
        //        colliding = true;
        //        break;
        //    }
        //}

        //if (!colliding)
        //{
        //    transform.localPosition += nextMoveVector;
        //}

        //transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.deltaTime);

        //if (newPosition.x < maxMoveLimit.x && newPosition.x > minMoveLimit.x && newPosition.y < maxMoveLimit.y && newPosition.y > minMoveLimit.y)
        //{
        //    transform.Translate(nextMoveVector);
        //}

        //ownRigidbody.velocity = Vector3.zero;

        //nextMoveVector = new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.deltaTime;

        //transform.Translate(nextMoveVector);

        //transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.deltaTime);

        rigidBody.AddForceAtPosition(Physics.gravity * downWardForce, transform.position, ForceMode.Acceleration);

        //Vector3 moveDirection = boat.transform.TransformDirection(new Vector3(movementInput.x, 0, movementInput.y));
        //moveDirection.y = 0;
        //moveDirection.Normalize();
        //rigidBody.AddForce(moveDirection * moveSpeed * Time.deltaTime);
        //rigidBody.AddForce(new Vector3(0,-downWardForce,0) * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }



    bool IsPointInPolygon(Vector2 p, List<Vector2> polygon)
    {
        if (polygon.Count < 2)
        {
            return false;
        }
        // 1. First do a simple test with axis aligned bounding box around the polygon
        float xMin = polygon[0].x;
        float xMax = polygon[0].x;
        float yMin = polygon[0].y;
        float yMax = polygon[0].y;
        for (int i = 1; i < polygon.Count; ++i)
        {
            if (xMin > polygon[i].x)
            {
                xMin = polygon[i].x;
            }
            if (xMax < polygon[i].x)
            {
                xMax = polygon[i].x;
            }
            if (yMin > polygon[i].y)
            {
                yMin = polygon[i].y;
            }
            if (yMax < polygon[i].y)
            {
                yMax = polygon[i].y;
            }
        }
        if (p.x < xMin || p.x > xMax || p.y < yMin || p.y > yMax)
        {
            return false;
        }

        // 2. Draw a virtual ray from anywhere outside the polygon to the point 
        //    and count how often it hits any side of the polygon. 
        //    If the number of hits is even, it's outside of the polygon, if it's odd, it's inside.
        int numberOfIntersectionPoints = 0;
        Vector2 p2 = new Vector2(xMax + 10.0f, p.y); // Horizontal line from point to point outside polygon (p2)

        // Count the number of intersection points
        float lambda1 = 0;
        float lambda2 = 0;
        for (int i = 0; i < polygon.Count; ++i)
        {
            if (IntersectLineSegments(polygon[i], polygon[(i + 1) % polygon.Count], p, p2, ref lambda1, ref lambda2, 0.00001f))
            {
                if (lambda1 > 0 && lambda1 <= 1 && lambda2 > 0 && lambda2 <= 1)
                {
                    ++numberOfIntersectionPoints;
                }
            }
        }
        if (numberOfIntersectionPoints % 2 == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    bool IntersectLineSegments(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2, ref float outLambda1, ref float outLambda2, float epsilon)
    {
        bool intersecting = false;

        Vector2 p1p2 = p2 - p1;
        Vector2 q1q2 = q2 - q1;

        // Cross product to determine if parallel
        float denom = Vector3.Cross(p1p2, q1q2).z;


        // Don't divide by zero
        if (Mathf.Abs(denom) > epsilon)
        {
            intersecting = true;

            Vector2 p1q1 = q1 - p1;

            float num1 = Vector3.Cross(p1q1, q1q2).z;
            float num2 = Vector3.Cross(p1q1, p1p2).z;
            outLambda1 = num1 / denom;
            outLambda2 = num2 / denom;
        }

        else // are parallel
        {
            // Connect start points
            Vector2 p1q1 = q1 - p1;

            // Cross product to determine if segments and the line connecting their start points are parallel, 
            // if so, than they are on a line
            // if not, then there is no intersection
            if (Mathf.Abs(Vector3.Cross(p1q1, q1q2).z) > epsilon)
            {
                return false;
            }

            // Check the 4 conditions
            outLambda1 = 0;
            outLambda2 = 0;
            if (IsPointOnLineSegment(p1, q1, q2) ||
                IsPointOnLineSegment(p2, q1, q2) ||
                IsPointOnLineSegment(q1, p1, p2) ||
                IsPointOnLineSegment(q2, p1, p2))
            {
                intersecting = true;
            }
        }
        return intersecting;
    }

    bool IsPointOnLineSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ap = p - a;
        Vector2 bp = p - b;
        // If not on same line, return false
        if (Mathf.Abs(Vector3.Cross(ap, bp).z) > 0.001f)
        {
            return false;
        }

        // Both vectors must point in opposite directions if p is between a and b
        if (Vector2.Dot(ap, bp) > 0)
        {
            return false;
        }

        return true;
    }





    ////https://gist.github.com/thebne/370c2bfbb89134403f4cd0462768b194
    //Vector3 GetClosestPointOnBounds(Bounds bounds, Vector3 point)
    //{
    //    var boundPoint1 = bounds.min;
    //    var boundPoint2 = bounds.max;
    //    var points = new Vector3[] {
    //        new Vector3(boundPoint1.x, boundPoint1.y),
    //        new Vector3(boundPoint1.x, boundPoint2.y),
    //        new Vector3(boundPoint2.x, boundPoint2.y),
    //        new Vector3(boundPoint2.x, boundPoint1.y),
    //    };

    //    return new Vector3[]
    //    {
    //        GetClosestPoint(point, points[0], points[1]),
    //        GetClosestPoint(point, points[1], points[2]),
    //        GetClosestPoint(point, points[2], points[3]),
    //        GetClosestPoint(point, points[3], points[0]),
    //    }.OrderBy(l => l.sqrMagnitude).First();
    //}

    //Vector3 GetClosestPoint(Vector3 point, Vector3 line_start, Vector3 line_end)
    //{
    //    Vector3 line_direction = line_end - line_start;
    //    float line_length = line_direction.magnitude;
    //    line_direction.Normalize();
    //    float project_length = Mathf.Clamp(Vector3.Dot(point - line_start, line_direction), 0f, line_length);
    //    return line_start + line_direction * project_length;
    //}
}
