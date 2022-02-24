using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssist : MonoBehaviour
{
    // When enabled, this class will move the connected rigidbody to the closest bin opening
    [Min(0f)]
    public float maxCorrectiveDistance = 1.2f;
    [Range(0, 12)]
    public int fixedTicksBeforeCorrection = 5;

    [SerializeField]
    private int assist;
    private Rigidbody rb;
    private ParabolaPreview pp;

    public void EnableOnce()
    {
        assist = 0;
        Debug.LogError("We throin");
    }

    private void Start()
    {
        assist = -1;
        rb = GetComponent<Rigidbody>();
        pp = GetComponent<ParabolaPreview>();
    }

    private void FixedUpdate()
    {
        if (assist < 0)
        {
            return;     // Causing issues with repeated corrections

            Vector3 testVelocity = rb.velocity;
            testVelocity.y = 0;
            float mag = testVelocity.magnitude;
            if (mag > 1)
            {
                //Debug.Log(testVelocity + " " + mag);
                EnableOnce();
            }
        }
        // Once timer is up
        else if (assist >= fixedTicksBeforeCorrection)
        {
            // Don't correct if item not rising/falling (ex: on table)
            if (rb.velocity.y != 0)
            {
                // dest - pos = vector from pos to dest
                Vector3 endpoint = pp.GetCalcEndpoint();
                //Debug.Log("Predicted endpoint: " + endpoint);
                Vector3 dest = BinOpeningPoint.FindClosestOpening(endpoint);
                //Debug.Log("Location of closest bin: " + dest);

                // By adding the difference between the basket point and the predicted collision point,
                // you can add that difference to change the predicted endpoint to the basket point
                Vector3 correction = dest - endpoint;
                float corrY = correction.y;
                Debug.Log("Calculated correction vector: " + correction);

                // Only correct if distance is within range (remove y = cylinder range check)
                correction.y = 0;
                if (correction.magnitude <= maxCorrectiveDistance)
                {
                    correction.y = corrY;
                    rb.AddForce(correction, ForceMode.VelocityChange);
                }
            }

            // Then disable assist
            assist = -1;
        }
        else
        {
            // Increment buffer
            assist++;
        }
        
    }
}
