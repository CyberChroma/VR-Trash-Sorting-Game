using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * So kinda big issue, but this only works as long as we have more updates than fixedupdates
 * Essentially, we want the framerate of the game to be higher than the physics update rate
 * That way, we can do calculations inbetween fixed update steps, then push when they are ready
 */

[RequireComponent(typeof(ParabolaPreview))]
public class AimAssist : MonoBehaviour
{
    // When enabled, this class will move the connected rigidbody to the closest bin opening
    [Min(0f)][SerializeField]
    private float maxCorrectiveDistance = 1.2f;
    [Range(0, 12)][SerializeField]
    private int fixedTicksBeforeCorrection = 0;
    [Range(0, 30)][SerializeField]
    private int maxEndpointIterations = 2;

    private int correctionDelay;
    private int assist;     // Determines which stage the assist is in
    //[SerializeField]
    //private float yMult;    // How much the assist affects the y component
    private Rigidbody rb;
    private ParabolaPreview pp;
    // Vector calulation stuff
    private Vector3 savedCorrection;
    private Vector3 previousEndpoint;

    public void EnableOnce()
    {
        correctionDelay = 0;
        savedCorrection = Vector3.zero;
        previousEndpoint = Vector3.zero;
        //Debug.LogError("We throin");
    }

    private void Start()
    {
        assist = -1;
        correctionDelay = -1;
        //yMult = 1;
        rb = GetComponent<Rigidbody>();
        pp = GetComponent<ParabolaPreview>();
    }

    // Instead of doing a bunch of iterations in one tick, spread out calculations over a series of frames
    // Now also enables the pp line
    private void POUpdate()             // Test: changed to POUpdate
    {
        // No calc done if -1
        while(assist >= 0)              // Test: changed to while
        {
            // Calculate newest endpoint
            Vector3 newEndpoint = pp.CalcNewEndpoint(savedCorrection);

            // Find closest bin
            Vector3 dest = BinOpeningPoint.FindClosestOpening(newEndpoint);
            // Add new correction iteraton to this iteration
            savedCorrection += (dest - newEndpoint);

            // Apply the correction if: (not first iteration AND iterated endpoints are similar) OR max iterations have been reached
            if((assist != 0 && Vector3.Distance(newEndpoint, previousEndpoint) < 0.01f) || assist >= maxEndpointIterations)
            {
                // Check if throw is in cylinder range of the chosen bin
                Vector3 tempCorr = savedCorrection;
                tempCorr.y = 0;
                if(tempCorr.magnitude <= maxCorrectiveDistance)
                {
                    // Finally, apply force
                    Debug.Log("Final correction vector: (x: " + savedCorrection.x + ", y: " + savedCorrection.y + ", z: " + savedCorrection.z + ")\nNeeded iterations: " + assist);
                    rb.AddForce(savedCorrection, ForceMode.VelocityChange);
                }
                // Whether the assist was successful or not, we disable the assist
                assist = -2;

                // And now we enable the line as well
                pp.TempEnable();

            }
            // Increment assist, update endpoint
            assist++;
            previousEndpoint = newEndpoint;
        }
    }

    private void FixedUpdate()
    {
        if(correctionDelay < 0)
        {
            // Nothing! But prevents the next two checks from being done
        }
        // Need delay to pass by obstacles, needs to be physics tick for things to move first
        else if(correctionDelay >= fixedTicksBeforeCorrection)
        {
            // Enough time has passed, start calculations
            assist = 0;
            correctionDelay = -1;
            POUpdate();                         // Test: added function call
        }
        else //if(correctionDelay >= 0)
        {
            correctionDelay++;
        }
    }


    /* moving this to update, keeping this all in case of a revert, this was before there was Enable and not just EnableOnce
    private void FixedUpdate()
    {
        // Three stages: Pre-assist activation, post assist activation, and  assist application
        if (assist < 0)
        {
            //return;     // Causing issues with repeated corrections

            // Determine if object is moving with a high horizontal velocity
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
            // Don't correct if item not falling (ex: on table)
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
                Debug.Log("x: " + correction.x + " y: " + correction.y + " z: " + correction.z);

                // Only correct if distance is within range (remove y = cylinder range check)
                correction.y = 0;
                if (correction.magnitude <= maxCorrectiveDistance)
                {
                    correction.y = corrY;
                    rb.AddForce(correction*yMult, ForceMode.VelocityChange);
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
    */
}
