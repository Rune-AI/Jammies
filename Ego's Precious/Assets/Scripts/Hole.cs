using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Hole : MonoBehaviour
{
    private VisualEffect waterEffect;
    private ParticleSystem waterParticles;

    void Awake()
    {
        waterEffect = GetComponent<VisualEffect>();
        waterParticles = GetComponent<ParticleSystem>();

        if (waterEffect == null)
        {
            Debug.Log("No Watereffect found");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (IsHoleUnderWater())
        {
            waterEffect.Play();
            waterParticles.Stop();
        }
        else
        {
            waterEffect.Stop();
            waterParticles.Play();
        }
    }

    
    private bool IsHoleUnderWater()
    {
        float WaterY = WaterHeight.instance.GetWaterHeight(new Vector2(transform.position.x, transform.position.z));

        return transform.position.y < WaterY;
    }
}
