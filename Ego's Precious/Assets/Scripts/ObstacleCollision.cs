using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public GameObject HolePrefab;
    

    private GameObject HolesParent;

    private void Awake()
    {
        HolesParent = GameObject.Find("Holes");
        if (HolesParent == null)
        {
            HolesParent = new GameObject("Holes");
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name);

        Quaternion rotation = Quaternion.LookRotation(collision.contacts[0].normal);
        Vector3 position = collision.contacts[0].point;
        GameObject hole = Instantiate(HolePrefab, position, rotation, HolesParent.transform);
    }
}
