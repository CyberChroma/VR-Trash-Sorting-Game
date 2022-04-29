using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneTrashSpawner : MonoBehaviour
{
    public GameObject objToSpawn;
    public bool shouldSpawn;

    private bool canSpawn; 

    // Start is called before the first frame update
    void Start()
    {
        shouldSpawn = false;
        canSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn && shouldSpawn) {
            Instantiate(objToSpawn, transform.position, transform.rotation);
            shouldSpawn = false;
            StartCoroutine(WaitToSpawnAgain());
        }
    }

    IEnumerator WaitToSpawnAgain()
    {
        canSpawn = false;
        yield return new WaitForSeconds(5);
        canSpawn = true;
        shouldSpawn = false;
    }
}
