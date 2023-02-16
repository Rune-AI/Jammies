using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sticker : MonoBehaviour
{
    public GameObject quad;
    public GameObject decalProjector;

    public void SetDecalMode(bool isDecal)
    {
        decalProjector.SetActive(isDecal);
        quad.SetActive(!isDecal);
    }
    
    public void SetQuadMode(bool isQuad)
    {
        quad.SetActive(isQuad);
        decalProjector.SetActive(!isQuad);
    }

    public void SetStickyMode(bool isSticky)
    {
        if (isSticky)
        {
            gameObject.tag = "Sticker";
        }
        else
        {
            gameObject.tag = "Untagged";
        }
    }
    



}
