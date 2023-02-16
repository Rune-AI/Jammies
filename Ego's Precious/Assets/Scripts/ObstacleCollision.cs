using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering.Universal;

public class ObstacleCollision : MonoBehaviour
{
    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name);

        HoleAndStickerManager.instance.DetectPrefab(collision.gameObject, collision.contacts[0].point, collision.contacts[0].normal);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger with " + other.name);

        Vector3 direction =  transform.position - other.transform.position;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(other.transform.position, direction, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, UnityEngine.Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, UnityEngine.Color.white);
            Debug.Log("Did not Hit");
        }


        if (!hit.transform.gameObject.CompareTag("Boat"))
        {
            Debug.Log("Not A boat");
            return;
        }

        Vector3 point = hit.point;
        Vector3 normal = -hit.normal;

        HoleAndStickerManager.instance.DetectPrefab(other.gameObject, point, normal);
    }

}
