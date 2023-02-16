using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HoleAndStickerManager : MonoBehaviour
{
    [NonSerialized] public static HoleAndStickerManager instance;
    
    [NonSerialized] public List<GameObject> holes;
    [NonSerialized] public List<GameObject> stickers;

    [SerializeField] private GameObject HolePrefab;
    [SerializeField] private GameObject StickerPrefab;

    private GameObject HolesParent;
    private GameObject StickersParent;

    [SerializeField] private List<Material> holeMaterials;
    [SerializeField] private List<Material> stickerMaterials;


    private void Awake()
    {
        holes = new List<GameObject>();
        stickers = new List<GameObject>();
        
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("HoleAndStickerManager Instance Already Exists, Destroying Object!");
            Destroy(gameObject);
        }


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

    private void Update()
    {
        checkAll();
    }






    public void checkAll()
    {
        foreach (var hole in holes)
        {
            foreach (var sticker in stickers)
            {
                if (checkIfStickerOverHole(sticker, hole))
                {
                    hole.SetActive(false);
                }
            }
        }
    }

    public bool checkIfStickerOverHole(GameObject sticker, GameObject hole)
    {
        if (hole.activeSelf == false)
        {
            return false;
        }


        if (sticker.transform.position.x > hole.transform.position.x - 0.5f &&
            sticker.transform.position.x < hole.transform.position.x + 0.5f &&
            sticker.transform.position.y > hole.transform.position.y - 0.5f &&
            sticker.transform.position.y < hole.transform.position.y + 0.5f &&
            sticker.transform.position.z > hole.transform.position.z - 0.5f &&
            sticker.transform.position.z < hole.transform.position.z + 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetActiveHoleCount()
    {
        {
            int activeHoleCount = 0;
            foreach (var hole in holes)
            {
                if (hole.activeSelf)
                {
                    activeHoleCount++;
                }
            }

            return activeHoleCount;
        }
    }


    public void AddHole(GameObject hole)
    {
        holes.Add(hole);
    }

    public void AddSticker(GameObject sticker)
    {
        stickers.Add(sticker);
    }
    
    public void RemoveHole(GameObject hole)
    {
        holes.Remove(hole);
    }
    
    public void RemoveSticker(GameObject sticker)
    {
        stickers.Remove(sticker);
    }

    public GameObject GetClosestSticker(Vector3 pos, float minDistance)
    {
            float closestDistance = minDistance;
            GameObject closestSticker = null;
            foreach (var sticker in stickers)
            {
                float distance = Vector3.Distance(sticker.transform.position, pos);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSticker = sticker;
                }
            }

            return closestSticker;
    }

    public GameObject GetClosestHole(Vector3 pos, float minDistance)
    {
        float closestDistance = minDistance;
        GameObject closestHole = null;
        foreach (var hole in holes)
        {
            float distance = Vector3.Distance(hole.transform.position, pos);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHole = hole;
            }
        }

        return closestHole;
    }
    

    public void DetectPrefab(GameObject other, Vector3 point, Vector3 normal)
    {
        switch (other.tag)
        {
            case "Obstacle":
                GameObject hole = addPrefab(HolePrefab, point, normal, HolesParent);
                hole.GetComponent<DecalProjector>().material = GetRandomHoleMaterial();
                AddHole(hole);
                other.tag = "Untagged";
                break;
            case "Sticker":
                GameObject sticker = addPrefab(StickerPrefab, point, normal, StickersParent);
                sticker.GetComponent<DecalProjector>().material = GetRandomStickerMaterial();
                AddSticker(sticker);
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
        int randomIndex = UnityEngine.Random.Range(0, holeMaterials.Count);
        return holeMaterials[randomIndex];
    }
    private Material GetRandomStickerMaterial()
    {
        int randomIndex = UnityEngine.Random.Range(0, stickerMaterials.Count);
        return stickerMaterials[randomIndex];
    }
}
