using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidPour : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem partsys = null;
    public float liquidCapacity = 2f;
    private Quaternion initRotation;


    // Start is called before the first frame update
    void Start()
    {
        initRotation = Quaternion.Inverse(transform.rotation);
        
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Disposal") && Vector3.Dot(transform.forward, Vector3.down) > 0 && liquidCapacity > 0)
        {
            if(partsys.isStopped)
                partsys.Play();

            liquidCapacity -= Time.deltaTime;
        }
        else
        {
            partsys.Stop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        partsys.Stop();
    }
}
