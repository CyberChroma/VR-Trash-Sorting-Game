using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidPour : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem partsys = null;
    [SerializeField]
    private ParticleSystem squishPartSys = null;
    public float liquidCapacity = 2f;
    private Quaternion initRotation;
    private AudioSource squishNoise;


    // Start is called before the first frame update
    void Start()
    {
        initRotation = Quaternion.Inverse(transform.rotation);
        squishNoise = GetComponent<AudioSource>();
        
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

    public void Squish()
    {
        squishPartSys.Play();
        squishNoise.Play();
        liquidCapacity = 0.1f;
    }
}
