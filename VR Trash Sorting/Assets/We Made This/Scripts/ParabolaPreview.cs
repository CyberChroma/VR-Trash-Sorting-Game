using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaPreview : MonoBehaviour
{
    public bool lineEnabled = false;
    public float colliderRadius = 0.2f;
    public int maxSimulationTime = 5;
    public float simulationInterval = 0.1f;
    public float timerDuration = 5;

    private LineRenderer line;
    private Rigidbody rb;
    private float timer;
    private bool timerSet;
    private Vector3 lastEndpoint;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        lastEndpoint = transform.position;
    }

    void Update()
    {
        if(lineEnabled){
            List<Vector3> pointList = CalculateLine(Vector3.zero);
            // Clear any points from previous frame
            line.positionCount = pointList.Count;
            for(int i = 0; i < pointList.Count; i++)
            {
                
                //Debug.Log("Added point " + i + ": " + pointList[i].ToString());
                line.SetPosition(i, pointList[i]);
            }
            lastEndpoint = pointList[pointList.Count - 1];

            if (timerSet)
            {
                timer = Mathf.Max(0, timer - Time.deltaTime);
                if(timer == 0)
                    DisableLine();
            }
        }
    }

    public void EnableLine()
    {
        line.enabled = true;
        lineEnabled = true;
    }

    public void TempEnable()
    {
        timer = timerDuration;
        timerSet = true;
        EnableLine();
    }

    public void DisableLine()
    {
        line.enabled = false;
        lineEnabled = false;
        timerSet = false;
    }

    public Vector3 GetCalcEndpoint()
    {
        return lastEndpoint;
    }

    // Using provided offset, predict new trajectory and return the last point
    public Vector3 CalcNewEndpoint(Vector3 offset)
    {
        List<Vector3> calculatedPoints = CalculateLine(offset);

        return calculatedPoints[calculatedPoints.Count - 1];
    }

    // Simulate an arc that represents a free-falling object's movement path
    private List<Vector3> CalculateLine(Vector3 offset)
    {
        // Don't read from a global every time
        float iv = simulationInterval;
        // Get simulation steps
        int ms = (int)(maxSimulationTime/simulationInterval);
        // List of points (for the line)
        List<Vector3> points = new List<Vector3>();
        // Init pos and velocity
        Vector3 initPos = rb.position;
        Vector3 curV = rb.velocity + offset;    // Add offset to velocity

        for(int i = 0; i < ms; i++)
        {
            float timeStep = iv * i;
            // Calc x and z
            float newx = initPos.x + curV.x * timeStep;
            float newz = initPos.z + curV.z * timeStep;
            // Calc y and gravity
            float newy = initPos.y + curV.y * timeStep + Physics.gravity.y / 2 * Mathf.Pow(timeStep, 2);

            Vector3 newVector = new Vector3(newx, newy, newz);
            points.Add(newVector);

            // Stop calculating if a collision is detected, set the last point to the collision zone
            (bool, Vector3) col = HasCollided(newVector, points[Mathf.Max(i - 1, 0)]);
            if (col.Item1)
            {
                points[i] = col.Item2;
                break;
            }
        }

        //Debug.Log(points.Count);
        return points;
    }

    // Check if a collision has occured in a radius
    private (bool, Vector3) HasCollided(Vector3 centre, Vector3 oldCentre)
    {
        // Make LayerMask to ignore collidors with layer = "Hands" (9)
        int layermask = ~(1 << 9);

        // First check in a radius around the object - finds any objects that this might collide with
        Collider[] cList = Physics.OverlapSphere(centre, colliderRadius, layermask);
        /*foreach(Collider c in cList)
        {
            // Reveal the collisions
            Debug.Log(c.gameObject.name);
        }*/

        // Next check if there is a collision between step points
        Vector3 stepPath = centre - oldCentre;
        RaycastHit[] rList = Physics.RaycastAll(oldCentre, stepPath.normalized, stepPath.magnitude, layermask);     // Currently RaycastAll for debug purposes, Raycast should be more efficient
        //Debug.Log(rList.Length);
        if(rList.Length > 0)
        {
            // The Vector3 return value is a way to correct the length of the line (but its kind of lazy)
            return (true, rList[0].point);
        }

        // Since it will always collide with itself, check to see if it hit anything
        if (cList.Length > 1)
            return (true, centre);

        return (false, Vector3.zero);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, colliderRadius);
    }
}
