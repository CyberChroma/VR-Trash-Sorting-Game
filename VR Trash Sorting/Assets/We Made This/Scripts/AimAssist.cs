using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssist : MonoBehaviour
{
    // When enabled, this class will move the connected rigidbody to the closest bin opening
    public int assist;

    private Rigidbody rb;
    private ParabolaPreview pp;

    public void EnableOnce()
    {
        assist = 0;
    }

    private void Start()
    {
        assist = -1;
        rb = GetComponent<Rigidbody>();
        pp = GetComponent<ParabolaPreview>();
    }

    private void FixedUpdate()
    {
        if(assist == 5 && rb.velocity != Vector3.zero)
        {
            // dest - pos = vector from pos to dest
            Vector3 endpoint  = pp.GetCalcEndpoint();
            Debug.Log("Predicted endpoint: " + endpoint);
            Vector3 dest = BinOpeningPoint.FindClosestOpening(endpoint);
            Debug.Log("Location of closest bin: " + dest);
            Vector3 correction = dest - endpoint;
            Debug.Log("Calculated correction vector: " + correction);

            rb.AddForce(correction, ForceMode.VelocityChange);

            // Then disable assist
            assist = -1;
        }
        else if(assist >= 0)
        {
            assist++;
        }
    }
}
