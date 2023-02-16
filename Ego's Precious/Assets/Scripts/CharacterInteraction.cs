using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 1;

    private bool isHoldingSticker = false;
    private GameObject currentInteractable;
    
    private void Update()
    {

        if (isHoldingSticker)
        {
            GameObject ClosestHole = FindNearestHole();
            if (ClosestHole == null) return;

            

            
        }
        else
        {
            GameObject closestSticker = FindNearestSticker();
            if (closestSticker == null) return;
        }
        
        
    }

    private GameObject FindNearestHole()
    {
        return HoleAndStickerManager.instance.GetClosestHole(transform.position, interactionDistance);
    }

    
    private GameObject FindNearestSticker()
    {
        return HoleAndStickerManager.instance.GetClosestSticker(transform.position, interactionDistance);
    }
}
