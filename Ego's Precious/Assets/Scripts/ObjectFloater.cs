using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFloater : MonoBehaviour
{
    void FixedUpdate()
    {
        Debug.Log(transform.position);
        float waveHeight = WaterHeight.instance.GetWaveHeight(new Vector2(transform.position.x, transform.position.z));
        transform.position = new Vector3(transform.position.x, waveHeight, transform.position.z);
    }
}
