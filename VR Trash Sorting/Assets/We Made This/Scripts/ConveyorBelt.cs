using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed = 1;
    public float scrollSpeed = 1;

    private Material conveyorMat;
    private List<Rigidbody> onBelt = new List<Rigidbody>();

    private void Start()
    {
        conveyorMat = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        conveyorMat.mainTextureOffset = new Vector2(0, conveyorMat.mainTextureOffset.y - scrollSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i = 0; i <= onBelt.Count - 1; i++)
        {
            if (onBelt[i] != null) {
                onBelt[i].velocity = transform.right * speed;
            } else {
                onBelt.Remove(onBelt[i]);
            }
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
