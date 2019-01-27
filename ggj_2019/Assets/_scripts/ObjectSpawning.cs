using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawning : MonoBehaviour
{
    public List<GameObject> mazecells;
    public List<GameObject> largeObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnObject(Vector3 placeToSpawn)
    {
        float rndx = Random.Range(-2, 3);
        float rndy = Random.Range(-2, 3);


        if (Random.Range(0, 2) == 1)
        {

            GameObject clone = Instantiate(
                largeObjects[Random.Range(0, largeObjects.Count)],
                new Vector3(placeToSpawn.x +rndx , placeToSpawn.y + 0.4f, placeToSpawn.z + rndy),
                transform.rotation
                ) as GameObject;
            clone.transform.Rotate(0, Random.Range(-30, 30), 0);

            if (Random.Range(0, 6) == 1)
            {
                Instantiate(
                largeObjects[Random.Range(0, largeObjects.Count)],
                new Vector3(placeToSpawn.x - rndx, placeToSpawn.y + 0.4f, placeToSpawn.z - rndy),
                transform.rotation
                );
            }
        }
    }

}
