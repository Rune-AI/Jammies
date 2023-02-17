using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Numerics;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private bool visualize;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float rotateSpeed = 1;
    [SerializeField] private float downWardForce = 1;
    //[SerializeField] private List<BoxCollider> boatColliders;
    [SerializeField] private List<Vector2> outerBoundsPolygon;
    [SerializeField] private List<Vector2> innerBoundsPolygon;

    [SerializeField] private GameObject cameraGameObject;

    //private CharacterController characterController;

    //private Rigidbody ownRigidbody;

    private Vector2 movementInput;

    //private Vector3 nextMoveVector;

    //private GameObject boat;

    private Animator animator;
    public GameObject charMesh;

    private void Awake()
    {
        animator = charMesh.GetComponent<Animator>();

        if (!cameraGameObject)
        {
            Debug.LogError("no camera assigned");
        }

        //boat = transform.parent.gameObject;

        //ownRigidbody = GetComponent<Rigidbody>();

        //characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);

        
        movementInput.Normalize();
        //movementInput = Quaternion.Euler(transform.parent.transform.eulerAngles) * movementInput;

        //movementInput = transform.parent.transform.InverseTransformVector(movementInput);

        //movementInput.Normalize();

        //movementInput = transform.parent.transform.TransformVector(movementInput);

        //movementInput.Normalize();

        if (movementInput.x != 0 || movementInput.y != 0)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(movementInput.x, 0, movementInput.y));
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, lookRotation, rotateSpeed * Time.deltaTime);
        }
        

        transform.localPosition += new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.deltaTime;

        

        Vector2 localPosition2d = new Vector2(transform.localPosition.x, transform.localPosition.z);
        if (!IsPointInPolygon(localPosition2d, outerBoundsPolygon))
        {
            //Debug.Log("outside");

            float closestDistance = float.MaxValue;
            Vector2 closestProjPoint = Vector2.zero;

            for(int i = 0; i < outerBoundsPolygon.Count - 1; ++i) 
            {
                Vector2 p1 = outerBoundsPolygon[i];
                Vector2 p2 = outerBoundsPolygon[i + 1];

                //calc proj point
                Vector2 projPoint = GetProjectedPointOnSegment(p1,p2,localPosition2d);

                float distance = Vector2.Distance(localPosition2d, projPoint);
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closestProjPoint = projPoint;
                }
            }

            Vector2 p3 = outerBoundsPolygon[outerBoundsPolygon.Count - 1];
            Vector2 p4 = outerBoundsPolygon[0];

            //calc proj point
            Vector2 projPoint2 = GetProjectedPointOnSegment(p3, p4, localPosition2d);

            float distance2 = Vector2.Distance(localPosition2d, projPoint2);
            if (distance2 < closestDistance)
            {
                closestDistance = distance2;
                closestProjPoint = projPoint2;
            }

            Vector3 newPos = new Vector3(closestProjPoint.x, transform.localPosition.y, closestProjPoint.y);

            transform.localPosition = newPos;
        }
        else if (IsPointInPolygon(localPosition2d, innerBoundsPolygon))
        {
            float closestDistance = float.MaxValue;
            Vector2 closestProjPoint = Vector2.zero;

            for (int i = 0; i < innerBoundsPolygon.Count - 1; ++i)
            {
                Vector2 p1 = innerBoundsPolygon[i];
                Vector2 p2 = innerBoundsPolygon[i + 1];

                //calc proj point
                Vector2 projPoint = GetProjectedPointOnSegment(p1, p2, localPosition2d);

                float distance = Vector2.Distance(localPosition2d, projPoint);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestProjPoint = projPoint;
                }
            }
            Vector2 p3 = innerBoundsPolygon[innerBoundsPolygon.Count - 1];
            Vector2 p4 = innerBoundsPolygon[0];

            //calc proj point
            Vector2 projPoint2 = GetProjectedPointOnSegment(p3, p4, localPosition2d);

            float distance2 = Vector2.Distance(localPosition2d, projPoint2);
            if (distance2 < closestDistance)
            {
                closestDistance = distance2;
                closestProjPoint = projPoint2;
            }

            Vector3 newPos = new Vector3(closestProjPoint.x, transform.localPosition.y, closestProjPoint.y);

            transform.localPosition = newPos;
        }

        rigidBody.AddForceAtPosition(Physics.gravity * downWardForce, transform.position, ForceMode.Acceleration);
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();

        //flip is temp solution, ideally it would depend on camera rotation
        //movementInput.x = -value.Get<Vector2>().y;
        //movementInput.y = value.Get<Vector2>().x;
    }

    private void OnDrawGizmos()
    {
        if (!visualize)
        {
            return;
        }

        for(int i = 0; i < outerBoundsPolygon.Count - 1; ++i)
        {
            Vector3 p1 = new Vector3(outerBoundsPolygon[i].x, transform.localPosition.y, outerBoundsPolygon[i].y);
            Vector3 p2 = new Vector3(outerBoundsPolygon[i + 1].x, transform.localPosition.y, outerBoundsPolygon[i + 1].y);
            p1 += transform.parent.transform.position;
            p2 += transform.parent.transform.position;

            Gizmos.DrawSphere(p1, 0.1f);
            Gizmos.DrawLine(p1, p2);
        }
        Vector3 p3 = new Vector3(outerBoundsPolygon[outerBoundsPolygon.Count - 1].x, transform.localPosition.y, outerBoundsPolygon[outerBoundsPolygon.Count - 1].y);
        Vector3 p4 = new Vector3(outerBoundsPolygon[0].x, transform.localPosition.y, outerBoundsPolygon[0].y);
        p3 += transform.parent.transform.position;
        p4 += transform.parent.transform.position;

        Gizmos.DrawSphere(p3, 0.1f);
        Gizmos.DrawLine(p3, p4);

        for (int i = 0; i < innerBoundsPolygon.Count - 1; ++i)
        {
            Vector3 p1 = new Vector3(innerBoundsPolygon[i].x, transform.localPosition.y, innerBoundsPolygon[i].y);
            Vector3 p2 = new Vector3(innerBoundsPolygon[i + 1].x, transform.localPosition.y, innerBoundsPolygon[i + 1].y);
            p1 += transform.parent.transform.position;
            p2 += transform.parent.transform.position;

            Gizmos.DrawSphere(p1, 0.1f);
            Gizmos.DrawLine(p1, p2);
        }
        p3 = new Vector3(innerBoundsPolygon[innerBoundsPolygon.Count - 1].x, transform.localPosition.y, innerBoundsPolygon[innerBoundsPolygon.Count - 1].y);
        p4 = new Vector3(innerBoundsPolygon[0].x, transform.localPosition.y, innerBoundsPolygon[0].y);
        p3 += transform.parent.transform.position;
        p4 += transform.parent.transform.position;

        Gizmos.DrawSphere(p3, 0.1f);
        Gizmos.DrawLine(p3, p4);
    }


    Vector2 GetProjectedPointOnSegment(Vector2 v, Vector2 w, Vector2 p)
    {
        float l2 = (w.x - v.x)*(w.x - v.x) + (w.y - v.y)*(w.y - v.y);
        //float l2 = Vector2.Distance(v, w) * Vector2.Distance(v, w);
        if (l2 == 0) //v == w
        {
            return v;
        }

        float t = Mathf.Clamp01(Vector2.Dot(p - v, w - v) / l2);
        return v + t * (w - v);
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
        const float margin = 0.0001f; //specifically for this script, could cause bugs if used elsewhere
        Vector2 p2 = new Vector2(xMax + 10.0f, p.y + margin); // Horizontal line from point to point outside polygon (p2)

        // Count the number of intersection points
        float lambda1 = 0;
        float lambda2 = 0;
        for (int i = 0; i < polygon.Count; ++i)
        {
            if (IntersectLineSegments(polygon[i], polygon[(i + 1) % polygon.Count], p, p2, ref lambda1, ref lambda2, 0.01f))
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
        if (Mathf.Abs(Vector3.Cross(ap, bp).z) > 0.01f)
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
