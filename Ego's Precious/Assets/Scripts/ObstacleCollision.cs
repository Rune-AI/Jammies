using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public GameObject HolePrefab;
    public GameObject StickerPrefab;


    private GameObject HolesParent;
    private GameObject StickersParent;

    private void Awake()
    {
        HolesParent = GameObject.Find("Holes");
        if (HolesParent == null)
        {
            HolesParent = new GameObject("Holes");
            HolesParent.transform.parent = this.transform;
        }


        StickersParent = GameObject.Find("Stickers");
        if (StickersParent == null)
        {
            StickersParent = new GameObject("Stickers");
            StickersParent.transform.parent = this.transform;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name);

        switch (collision.gameObject.tag)
        {
            case "Obstacle":
                addPrefab(HolePrefab, collision.contacts[0].point, collision.contacts[0].normal, HolesParent);
                break;
            case "Sticker":
                addPrefab(StickerPrefab, collision.contacts[0].point, collision.contacts[0].normal, StickersParent);
                Destroy(collision.gameObject);
                break;
            default:
                break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger with " + other.name);

        Vector3 direction =  transform.position - other.transform.position;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(other.transform.position, direction, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }


        if (!hit.transform.gameObject.CompareTag("Boat"))
        {
            Debug.Log("Not A boat");
            return;
        }

        Vector3 point = hit.point;
        Vector3 normal = -hit.normal;
        
        switch (other.gameObject.tag)
        {
            case "Obstacle":
                addPrefab(HolePrefab, point, normal, HolesParent);
                break;
            case "Sticker":
                addPrefab(StickerPrefab, point, normal, StickersParent);
                Destroy(other.gameObject);
                break;
            default:
                break;
        }
    }

    private void addPrefab(GameObject prefab, Vector3 point, Vector3 normal, GameObject parent)
    {
        Quaternion rotation = Quaternion.LookRotation(normal);
        Vector3 position = point;
        GameObject hole = Instantiate(prefab, position, rotation, parent.transform);
    }
}
