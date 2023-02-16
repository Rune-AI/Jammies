using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.ProBuilder;
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

    private SoundEffectPlayer soundEffectPlayer;

    private void Awake()
    {
        soundEffectPlayer = FindObjectOfType<SoundEffectPlayer>();

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
            bool active = true;
            foreach (var sticker in stickers)
            {
                if (checkIfStickerOverHole(sticker, hole))
                {
                    active = false;
                }                    
            }
            hole.SetActive(active);
            if (stickers.Count == 0)
            {
                hole.SetActive(true);
            }
        }
    }

    public bool checkIfStickerOverHole(GameObject sticker, GameObject hole)
    {
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
            if (hole.activeInHierarchy == false)
            {
                continue;
            }
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
                int rand = UnityEngine.Random.Range(0, 2);
                switch (rand)
                {
                    case 0:
                        soundEffectPlayer.PlayTear();
                        break;
                    case 1:
                        soundEffectPlayer.PlayTearFast();
                        break;
                    case 2:
                        soundEffectPlayer.PlayTearCardboard();
                        break;
                }
                
                GameObject hole = addPrefab(HolePrefab, point, normal, HolesParent);
                hole.GetComponent<DecalProjector>().material = GetRandomHoleMaterial();
                AddHole(hole);
                other.tag = "Untagged";
                break;
            case "Sticker":
                Sticker stickerComponent = other.GetComponent<Sticker>();
                if (stickerComponent == null)
                {
                    other = other.transform.parent.gameObject;
                }
                other.GetComponent<Sticker>().SetStickyMode(false);
                other.GetComponent<Sticker>().SetDecalMode(true);
                other.transform.position = point;
                other.transform.rotation = Quaternion.LookRotation(normal);
                other.transform.parent = StickersParent.transform;
                AddSticker(other);
                break;
            default:
                break;
        }

    }

    private GameObject addPrefab(GameObject prefab, Vector3 point, Vector3 normal, GameObject parent)
    {
        Quaternion rotation = Quaternion.LookRotation(normal);
        Vector3 position = point - normal * 0.01f;
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

    public void PasteStickerOnHole(GameObject sticker, GameObject closestHole)
    {
        soundEffectPlayer.PlayBandaid();

        sticker.GetComponent<Sticker>().SetStickyMode(false);
        sticker.GetComponent<Sticker>().SetDecalMode(true);
        sticker.transform.position = closestHole.transform.position;
        sticker.transform.rotation = closestHole.transform.rotation;
        sticker.transform.parent = StickersParent.transform;
        AddSticker(sticker);
        //GameObject sticker2D = addPrefab(StickerPrefab, closestHole.transform.position, closestHole.transform.forward, StickersParent);
        //sticker2D.GetComponent<DecalProjector>().material = GetRandomStickerMaterial();
        //AddSticker(sticker2D);
        //Destroy(sticker);

        //closestHole.SetActive(false);
    }

    public void PasteStickerOnGround(GameObject sticker)
    {
        sticker.GetComponent<Sticker>().SetStickyMode(false);
        sticker.GetComponent<Sticker>().SetDecalMode(true);
        //sticker.transform.position = point;
        //sticker.transform.rotation = Quaternion.LookRotation(normal);
        sticker.transform.parent = StickersParent.transform;
        AddSticker(sticker);



        //Vector3 point = transform.position;
        //Vector3 normal = Vector3.up;
        //GameObject sticker2D = addPrefab(StickerPrefab, point, normal, StickersParent);
        //sticker2D.GetComponent<DecalProjector>().material = GetRandomStickerMaterial();
        //AddSticker(sticker2D);
        //Destroy(sticker);
    }
}
