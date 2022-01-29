using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnableThrowable : RespawnableObject
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Respawn"))
        {
            Respawn();
        }

    }
}
