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
    // Debug Stuff
    private Vector3 binPoint;
    private List<Vector3> binPoints;
    private List<Vector3> calculatedPoints;
    private List<Vector3> endPoints;

    public void EnableOnce()
    {
        correctionDelay = 0;
        savedCorrection = Vector3.zero;
        previousEndpoint = Vector3.zero;
        Debug.LogError("We throin");
    }

    private void Start()
    {
        assist = -1;
        correctionDelay = -1;
        //yMult = 1;
        rb = GetComponent<Rigidbody>();
        pp = GetComponent<ParabolaPreview>();
        binPoints = new List<Vector3>();
        calculatedPoints = new List<Vector3>();
        endPoints = new List<Vector3>();
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
            endPoints.Add(newEndpoint);

            // Find closest bin
            Vector3 dest;
            if (assist == 0)
            {
                dest = BinOpeningPoint.FindClosestOpening(newEndpoint);
                binPoint = dest;

                //Range check V2.1
                Vector3 tempCorr = newEndpoint;
                tempCorr.y = dest.y;
                float calcDist = Vector3.Distance(tempCorr, dest);
                
                if (calcDist > maxCorrectiveDistance)
                {
                    Debug.Log("Calculated distance = " + calcDist + ", while max accepted distance is = " + maxCorrectiveDistance);
                    pp.TempEnable();
                    assist = -1;
                    return;
                }
            }
            else
            {
                dest = binPoint;
            }

            binPoints.Add(dest);
            // Add new correction iteraton to this iteration
            savedCorrection += (dest - newEndpoint);
            calculatedPoints.Add(savedCorrection);

            // Apply the correction if: (not first iteration AND iterated endpoints are similar) OR max iterations have been reached
            if((assist != 0 && Vector3.Distance(newEndpoint, previousEndpoint) < 0.01f) || assist >= maxEndpointIterations)
            {
                // Check if throw is in cylinder range of the chosen bin
                /*Range Check V1
                Vector3 tempCorr = savedCorrection;
                tempCorr.y = 0;

                Debug.Log("Calculated distance = " + tempCorr.magnitude + ", while max accepted distance is = " + maxCorrectiveDistance);
                if(tempCorr.magnitude <= maxCorrectiveDistance)*/

                /*Range check V2
                Vector3 tempCorr = endPoints[0];
                tempCorr.y = dest.y;
                float calcDist = Vector3.Distance(tempCorr, dest);
                Debug.Log("Calculated distance = " + calcDist + ", while max accepted distance is = " + maxCorrectiveDistance);
                if (calcDist <= maxCorrectiveDistance)
                {*/
                // Finally, apply force
                Debug.Log("Final correction vector: (x: " + savedCorrection.x + ", y: " + savedCorrection.y + ", z: " + savedCorrection.z + ")\nNeeded iterations: " + assist);
                rb.AddForce(savedCorrection, ForceMode.VelocityChange);
                //}
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

            binPoints.Clear();
            calculatedPoints.Clear();
            endPoints.Clear();
            POUpdate();                         // Test: added function call
        }
        else //if(correctionDelay >= 0)
        {
            correctionDelay++;
        }
    }

    private void OnDrawGizmos()
    {
        if(calculatedPoints != null && calculatedPoints.Count > 0)
        {
            Gizmos.color = Color.blue;
            foreach (Vector3 v in binPoints)
            {
                Gizmos.DrawSphere(v, 0.04f);
            }
            Gizmos.color = Color.red;
            float i = 0;
            foreach(Vector3 v in calculatedPoints)
            {
                Gizmos.DrawSphere(v, 0.02f + i);
                i += 0.003f;
            }
            Gizmos.color = Color.green;
            i = 0;
            foreach (Vector3 v in endPoints)
            {
                Gizmos.DrawSphere(v, 0.02f + i);
                i += 0.003f;
            }
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
