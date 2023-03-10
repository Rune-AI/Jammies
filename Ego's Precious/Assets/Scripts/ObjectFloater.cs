using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectFloater : MonoBehaviour
{
    void FixedUpdate()
    {
        //Debug.Log(transform.position);
        float waveHeight = WaterHeight.instance.GetWaveHeight(new Vector2(transform.position.x, transform.position.z));
        transform.position = new Vector3(transform.position.x, waveHeight + 0.03f, transform.position.z);
    }
}
