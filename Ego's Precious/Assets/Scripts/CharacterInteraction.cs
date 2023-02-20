using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 100;

    private bool isHoldingSticker = false;
    private GameObject currentInteractable;

    [SerializeField] private GameObject buttonPromptPrefab;
    private GameObject buttonPrompt;

    [SerializeField] private GameObject stickerHoldingPrefab;

    private void Awake()
    {
        buttonPrompt = Instantiate(buttonPromptPrefab, transform);

        if (buttonPrompt == null)
        {
            Debug.LogError("Button Prompt Prefab is null");
        }

        buttonPrompt.SetActive(false);
    }

    public void OnInteract()
    {
        Debug.Log("Interacting");
        GameObject closestHole = FindNearestHole();
        GameObject closestSticker = FindNearestSticker();
        if (isHoldingSticker)
        {
            if (closestHole != null)
            {
                //if hole is nearby, paste on hole
                HoleAndStickerManager.instance.PasteStickerOnHole(currentInteractable, closestHole);
            }
            else
            {
                //else, drop sticker
                Debug.Log("Dropping Sticker");
                HoleAndStickerManager.instance.PasteStickerOnGround(currentInteractable);
            }
            isHoldingSticker = false;
            currentInteractable = null;
        }
        else if (closestSticker != null)
        {
            //if Sticker is near, pick up sticker
            HoleAndStickerManager.instance.RemoveSticker(closestSticker);
            //Add sticker to hands
            currentInteractable = closestSticker;
            closestSticker.GetComponent<Sticker>().SetStickyMode(false);
            closestSticker.GetComponent<Sticker>().SetQuadMode(false);
            closestSticker.transform.parent = transform;

            isHoldingSticker = true;

        }
    }

    private void Update()
    {
        buttonPrompt.SetActive(false);
        if (isHoldingSticker)
        {
            GameObject closestHole = FindNearestHole();
            if (closestHole == null) return;
            
            SetButtonPrompt(closestHole);

        }
        else
        {
            GameObject closestSticker = FindNearestSticker();
            if (closestSticker == null) return;

            SetButtonPrompt(closestSticker);
        }
    }

    private void SetButtonPrompt(GameObject gameObject)
    {
        Vector3 direction = Camera.main.transform.position - gameObject.transform.position;


        buttonPrompt.transform.position = gameObject.transform.position + new Vector3(0, 1, 0) + direction.normalized;
        buttonPrompt.transform.LookAt(Camera.main.transform.position);
        if (!buttonPrompt.activeInHierarchy)
        {
            buttonPrompt.SetActive(true);
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
