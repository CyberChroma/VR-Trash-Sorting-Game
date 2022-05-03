using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorObjectSpawner : MonoBehaviour
{
    private int rand;
    public GameObject[] objToSpawn;
    public float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnTime != 0) {
            StartCoroutine(SpawnObject());
        }
    }

    public void StartAgain()
    {
        StopAllCoroutines();
        if(spawnTime > 0)
        {
            StartCoroutine(SpawnObject());
        }
    }

    IEnumerator SpawnObject()
    {
        while (true) {
            yield return new WaitForSeconds(spawnTime);

            rand = Random.Range(0, objToSpawn.Length);
            Instantiate(objToSpawn[rand], transform);
        }
    }
}
