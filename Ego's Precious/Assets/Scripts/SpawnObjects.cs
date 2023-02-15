using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnObjects : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToSpawn;

    [SerializeField] private int ObjectCount = 10;
    [SerializeField] private float SpawnRadiusX = 10f;
    [SerializeField] private float SpawnRadiusZ = 10f;



    

    private void Awake()
    {
        GameObject objectsParent = GameObject.Find("SpawnedObjects");
        if (objectsParent == null)
        {
            objectsParent = new GameObject("SpawnedObjects");
            objectsParent.transform.parent = this.transform;
        }



        for (int i = 0; i < ObjectCount; i++)
        {
            int randomInex = Random.Range(0, objectsToSpawn.Count);
            GameObject object = Instantiate(objectsToSpawn[randomInex]);
            object.Parent = objectsParent.transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
