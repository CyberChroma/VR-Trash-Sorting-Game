using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorObjectSpawner : MonoBehaviour
{
    public GameObject objToSpawn;
    public float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObject());
    }

    IEnumerator SpawnObject()
    {
        while (true) {
            yield return new WaitForSeconds(spawnTime);
            Instantiate(objToSpawn, this.transform);
        }
    }
}
