using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform boat;
    [SerializeField] private Transform camera;

    [SerializeField] private float maxDistance;
    [Range(0.01f, 1)]
    [SerializeField] private float currentDistancePercentage;
    [SerializeField] private List<float> CameraDistancePercentages;
    [SerializeField] private List<float> CameraZPoints;

    private Vector3 direction;

    private void Start()
    {
        direction = camera.position - boat.position;
        direction = direction.normalized;
    }


    private void FixedUpdate()
    {
        for (int i = 1; i < CameraZPoints.Count; i++)
        {
            if (CameraZPoints[i] < boat.position.z)
            {
                continue;
            }
            else
            {
                currentDistancePercentage = (boat.position.z - CameraZPoints[i - 1]) / (CameraZPoints[i] - CameraZPoints[i - 1]);
                //currentDistancePercentage = Mathf.Lerp(CameraZPoints[i - 1], CameraZPoints[i], t);
                break;
            }
            
        }

        //Vector3 direction = camera.position - boat.position;
        //direction = direction.normalized;
        Vector3 pos = boat.position + maxDistance * direction * currentDistancePercentage;

        camera.position = pos;

        camera.rotation = Quaternion.Lerp(camera.rotation, Quaternion.LookRotation(-direction), 0.01f);
        //camera.rotation = Quaternion.LookRotation(-direction);

        //camera.position = new Vector3(camera.position.x, camera.position.y, boat.position.z);
    }

}
