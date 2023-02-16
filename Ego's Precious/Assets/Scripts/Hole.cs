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
        //waterParticles = GetComponent<ParticleSystem>();

        if (waterEffect == null)
        {
            Debug.Log("No Watereffect found");
        }

        //if (waterParticles == null)
        //{
        //    Debug.Log("No WaterParticles found");
        //}
    }
    
    // Update is called once per frame
    void Start()
    {
        if (IsHoleUnderWater())
        {
            Debug.Log("Water Activated");
            waterEffect.Play();
            //waterParticles.Play();
        }
        else
        {
            waterEffect.Stop();
            //waterParticles.Stop();
        }
    }

    private void OnEnable()
    {
        if (IsHoleUnderWater())
        {
            Debug.Log("Water Activated");
            waterEffect.Play();
            //waterParticles.Play();
        }
        else
        {
            waterEffect.Stop();
            //waterParticles.Stop();
        }
    }


    private bool IsHoleUnderWater()
    {
        
        float WaterY = WaterHeight.instance.GetWaterHeight(new Vector2(transform.position.x, transform.position.z));

        return transform.position.y < WaterY + 1.5f;
    }
}
