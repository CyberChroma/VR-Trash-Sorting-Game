using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public float timeBetweenSpawn;
    private float spawnTime;

    private int rand;
    public GameObject[] objects;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnTime)
        {
            Spawn();
            spawnTime = Time.time + timeBetweenSpawn;
        }
    }

    void Spawn()
    {
        rand = Random.Range(0, objects.Length);

        Instantiate(objects[rand], transform.position + new Vector3(0, 0, 0), transform.rotation);
    }
}
