using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoost : MonoBehaviour
{
    public float LaunchAfterSeconds = 5;
    public Quaternion LaunchDirection = Quaternion.identity;
    public float LaunchForce = 1f;

    private Rigidbody rb;
    private float countdown;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        countdown = LaunchAfterSeconds;
    }

    // Launch object every x seconds in specified direction
    void Update()
    {
        if(countdown <= 0)
        {
            rb.AddForce(LaunchDirection * Vector3.forward * LaunchForce, ForceMode.VelocityChange);
            

            countdown = LaunchAfterSeconds;
        }
        countdown -= Time.deltaTime;
    }

}
