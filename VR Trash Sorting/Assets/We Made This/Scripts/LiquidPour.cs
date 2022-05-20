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
        
        if(other.CompareTag("Disposal"))
        {   // Test if object is upside down (althoug we use forward vector here because objects have blender coordinates
            if (Vector3.Dot(transform.forward, Vector3.down) > 0 && liquidCapacity > 0)
            {
                // only play if not playing
                if (partsys.isStopped)
                    partsys.Play();

                //Debug.Log(liquidCapacity + " - " + Time.deltaTime);
                liquidCapacity -= Time.deltaTime;
            }
            else
            {
                partsys.Stop();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Stop when leaving trigger (deals with edge case)
        if(other.CompareTag("Disposal"))
            partsys.Stop();
    }

    public void Squish()
    {
        squishPartSys.Play();
        squishNoise.Play();
        liquidCapacity = 0.1f;
    }
}
