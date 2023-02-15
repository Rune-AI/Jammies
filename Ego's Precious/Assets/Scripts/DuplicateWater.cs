using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class DuplicateWater : MonoBehaviour
{

    public GameObject waterPlane;

    public Vector3 waterPlaneStartPos;
    public float waterPlaneSize;
    public int colCount;
    public int rowCount;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < colCount; x++)
        {
            for (int z = 0; z < rowCount; z++)
            {
                Instantiate(waterPlane, new Vector3(waterPlaneStartPos.x + (x * waterPlaneSize), waterPlaneStartPos.y, waterPlaneStartPos.z + (z * waterPlaneSize)), Quaternion.identity);
            }
        }
    }
}
