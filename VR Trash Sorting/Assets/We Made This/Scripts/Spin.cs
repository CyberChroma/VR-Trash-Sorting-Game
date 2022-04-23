using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public Vector3 direction;
    public bool randomInvert;

    // Start is called before the first frame update
    void Start()
    {
        if (randomInvert) {
            if (Random.Range(0, 2) == 0) {
                direction *= -1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(direction * Time.deltaTime);
    }
}
