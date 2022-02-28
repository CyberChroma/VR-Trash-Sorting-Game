using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnableThrowable : RespawnableObject
{
    public float Range = 100f;
    [Range(0, 1)]
    public float MinSize = 0.25f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Respawn"))
        {
            Respawn();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        // Shrink in shrinking zone
        if (other.CompareTag("Shrink"))
        {
            // By a factor of 95%
            Vector3 newScale = transform.localScale * 0.90f;
            if(newScale.magnitude / m_StartingScale.magnitude >= MinSize)
            {
                // Apply new scale if difference isnt too small (min)
                transform.localScale = newScale;
            }
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
