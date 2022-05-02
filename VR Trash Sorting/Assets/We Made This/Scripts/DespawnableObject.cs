using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnableObject : MonoBehaviour
{
    [SerializeField]
    private float ShrinkTime = 1.5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Respawn"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = rb.velocity * 0.5f;
            StartCoroutine(shrinkAndDespawn());
        }
    }

    IEnumerator shrinkAndDespawn()
    {
        float timer = ShrinkTime;
        while (timer > 0)
        {
            // Apply new scale if difference isnt too small (min)
            transform.localScale = transform.localScale * (1 - 0.4f*Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
        
    }
}
