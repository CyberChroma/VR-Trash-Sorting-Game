using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTesting : MonoBehaviour
{
    Vector3 acceleration = Vector3.zero;
    Vector3 boneVelocity = Vector3.zero;
    Vector3 boneRotation = Vector3.zero;
    float drag = 0;
    public float upSpeed;
    public Vector3 dirToRotAround;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q)) {
            boneVelocity.x += 0.01f;
        }
        if (Input.GetKey(KeyCode.A)) {
            boneVelocity.x -= 0.01f;
        }

        if (Input.GetKey(KeyCode.W)) {
            boneVelocity.y += 0.01f;
        }
        if (Input.GetKey(KeyCode.S)) {
            boneVelocity.y -= 0.01f;
        }

        if (Input.GetKey(KeyCode.E)) {
            boneVelocity.z += 0.01f;
        }
        if (Input.GetKey(KeyCode.D)) {
            boneVelocity.z -= 0.01f;
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            boneVelocity = Vector3.zero;
        }

        //if (dirToRotAround.magnitude != 0) {
            //Vector3 turnToCenterDirection = Vector3.Cross(transform.up, Vector3.up);
            //turnToCenterDirection = dirToRotAround;
            Debug.DrawRay(transform.GetChild(0).position, dirToRotAround, Color.green, 0.01f);
            acceleration = dirToRotAround * upSpeed;
        //}
        boneVelocity = acceleration * Time.deltaTime;
        boneVelocity *= 1 - Time.deltaTime * drag;
        boneRotation += boneVelocity;
        //transform.rotation = Quaternion.Euler(boneRotation);
        transform.rotation = Quaternion.LookRotation(Vector3.up);
    }
}
