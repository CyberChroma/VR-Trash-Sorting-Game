using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed;

    private List<Rigidbody> onBelt = new List<Rigidbody>();

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i = 0; i <= onBelt.Count - 1; i++)
        {
            onBelt[i].velocity = transform.right * speed;
        }
    }

    //When something collides with the belt:
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null) {
            onBelt.Add(collision.gameObject.GetComponent<Rigidbody>());
        }
    }

    //When object leaves the belt:
    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null) {
            onBelt.Remove(collision.gameObject.GetComponent<Rigidbody>());
        }
    }
}
