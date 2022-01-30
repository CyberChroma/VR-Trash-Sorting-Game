using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnableThrowable : RespawnableObject
{
    public float Range = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Respawn"))
        {
            Respawn();
        }

    }

    // Respawn if out of range
    private void FixedUpdate()
    {
        if(Vector3.Distance(m_Rigidbody.position, m_StartingPosition) > Range)
        {
            Respawn();
        }
    }
}
