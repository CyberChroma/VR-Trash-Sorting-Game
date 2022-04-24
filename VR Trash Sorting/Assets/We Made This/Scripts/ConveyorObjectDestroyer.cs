using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorObjectDestroyer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null) {
            Destroy(collision.gameObject);
        }
    }
}
