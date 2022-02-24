using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoost : MonoBehaviour
{
    public float LaunchAfterSeconds = 5;
    public Quaternion LaunchDirection = Quaternion.identity;
    public float LaunchForce = 1f;

    private Rigidbody rb;
    private AimAssist aa;
    private float countdown;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        aa = GetComponent<AimAssist>();
        countdown = LaunchAfterSeconds;
    }

    // Launch object every x seconds in specified direction
    void Update()
    {
        if(countdown <= 0)
        {
            // Launch!
            rb.AddForce(LaunchDirection * Vector3.forward * LaunchForce, ForceMode.VelocityChange);

            // Activate Aim Assist (if it exists)
            if (aa != null) aa.EnableOnce();
            

            countdown = LaunchAfterSeconds;
        }
        countdown -= Time.deltaTime;
    }

}
