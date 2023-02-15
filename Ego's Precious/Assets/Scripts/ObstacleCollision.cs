using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering.Universal;

public class ObstacleCollision : MonoBehaviour
{
    public GameObject HolePrefab;
    public GameObject StickerPrefab;


    private GameObject HolesParent;
    private GameObject StickersParent;

    [SerializeField] private List<Material> holeMaterials;
    [SerializeField] private List<Material> stickerMaterials;

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

        DetectPrefab(collision.gameObject, collision.contacts[0].point, collision.contacts[0].normal);
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

        DetectPrefab(other.gameObject, point, normal);
    }

    private void DetectPrefab(GameObject other, Vector3 point, Vector3 normal)
    {
        switch (other.tag)
        {
            case "Obstacle":
                GameObject hole = addPrefab(HolePrefab, point, normal, HolesParent);
                hole.GetComponent<DecalProjector>().material = GetRandomHoleMaterial();
                HoleAndStickerManager.instance.AddHole(hole);
                other.tag = "Untagged";
                break;
            case "Sticker":
                GameObject sticker = addPrefab(StickerPrefab, point, normal, StickersParent);
                sticker.GetComponent<DecalProjector>().material = GetRandomStickerMaterial();
                HoleAndStickerManager.instance.AddSticker(sticker);
                Destroy(other);
                break;
            default:
                break;
        }
        
    }

    private GameObject addPrefab(GameObject prefab, Vector3 point, Vector3 normal, GameObject parent)
    {
        Quaternion rotation = Quaternion.LookRotation(normal);
        Vector3 position = point;
        GameObject item = Instantiate(prefab, position, rotation, parent.transform);

        return item;
    }


    private Material GetRandomHoleMaterial()
    {
        int randomIndex = Random.Range(0, holeMaterials.Count);
        return holeMaterials[randomIndex];
    }
    private Material GetRandomStickerMaterial()
    {
        int randomIndex = Random.Range(0, stickerMaterials.Count);
        return stickerMaterials[randomIndex];
    }
}
