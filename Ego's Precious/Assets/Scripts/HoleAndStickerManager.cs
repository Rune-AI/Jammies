using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleAndStickerManager : MonoBehaviour
{
    [NonSerialized] public static HoleAndStickerManager instance;
    
    [NonSerialized] public List<GameObject> holes;
    [NonSerialized] public List<GameObject> stickers;

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


}
