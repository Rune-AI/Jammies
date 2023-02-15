using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHeight : MonoBehaviour
{
    public static WaterHeight instance;

    private Material water;

    private void Awake()
    {
        water = GetComponent<Renderer>().material;


        if (water == null)
        {
            Debug.Log("Did not find water");
        }

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("WaterManager Instance Already Exists, Destroying Object!");
            //Destroy(gameObject);
        }
    }

    public float GetWaterHeight(Vector2 pos)
    {
        return CalculateWaveHeight(pos);
    }
    
    public float GetWaveHeight(Vector2 pos)
    {
        return CalculateWaveHeight(pos);
    }

    public float CalculateWaveHeight(Vector2 pos)
    {
        float waveFrequency = water.GetFloat("_HeightFrequency");
        float twoPI = 6.28f;
        float waveSpeed = water.GetFloat("_WaveSpeed");
        float waveAmplitude = water.GetFloat("_WaveAmplitude");

        float tempVar = (Time.time * waveSpeed) + (waveFrequency * twoPI * pos.y);

        float waveHeight1 = waveAmplitude * Mathf.Sin(tempVar);

        float waveHeight2 = waveHeight1 + transform.position.y;
        
        return waveHeight2;
    }
}
