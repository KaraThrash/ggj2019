using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawning : MonoBehaviour
{
    public List<GameObject> mazecells;
    public List<GameObject> largeObjects;
    public List<GameObject> socks;
    public int socksForMap;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnObject(Transform placeToSpawn)
    {
        float rndx = Random.Range(-2, 3);
        float rndy = Random.Range(-2, 3);
        foreach (Transform go in placeToSpawn)
        {
            rndx = Random.Range(-2, 3);
            rndy = Random.Range(-2, 3);


            if (Random.Range(0, 2) == 1)
            {

                GameObject clone = Instantiate(
                    largeObjects[Random.Range(0, largeObjects.Count)],
                    new Vector3(go.position.x + rndx, go.position.y + 0.4f, go.position.z + rndy),
                    transform.rotation
                    ) as GameObject;
                clone.transform.Rotate(0, Random.Range(-30, 30), 0);

                if (Random.Range(0, 6) == 1)
                {
                    Instantiate(
                    largeObjects[Random.Range(0, largeObjects.Count)],
                    new Vector3(go.position.x - rndx, go.position.y + 0.4f, go.position.z - rndy),
                    transform.rotation
                    );
                }
            }
        }
        while (socksForMap < 10)
        {
            Transform tempspot = placeToSpawn.GetChild(Random.Range(0, placeToSpawn.childCount));
            Instantiate(
                    socks[Random.Range(0, 2)],
                    new Vector3(tempspot.position.x , tempspot.position.y + 0.4f, tempspot.position.z ),
                    transform.rotation
                    );
            socksForMap++;
        }
    }

}
