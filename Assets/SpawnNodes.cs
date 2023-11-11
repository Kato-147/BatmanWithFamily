using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNodes : MonoBehaviour
{
    // Start is called before the first frame update
    int numToSpawn = 28;
    public float spawnOffset = 0.3f;
    public float currentSpawnOffset;

    void Start()
    {
        gameObject.name = "Node";
        return;
        if (gameObject.name =="Node")
            currentSpawnOffset = spawnOffset;
            for (int i = 0; i < numToSpawn; i++)
                {
                    //clone a new node
                    GameObject clone = Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y+ currentSpawnOffset, 0), Quaternion.identity);
            currentSpawnOffset += spawnOffset;
                }
            {

        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
