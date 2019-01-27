using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnsecondary : MonoBehaviour
{
    public List<GameObject> smallObjects;
    public List<Transform> spawnspot;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform go in spawnspot)
        {
            if (Random.Range(0, 3) == 1)
            {

                GameObject clone = Instantiate(
                    smallObjects[Random.Range(0, smallObjects.Count)],
                    go.transform.position,
                    transform.rotation
                    ) as GameObject;
                clone.transform.Rotate(0, Random.Range(-60, 60), 0);
            } }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
