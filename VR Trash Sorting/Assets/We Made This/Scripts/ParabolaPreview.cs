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
            List<Vector3> pointList = CalculateLine();
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

    // Simulate an arc that represents a free-falling object's movement path
    private List<Vector3> CalculateLine()
    {
        // Don't read from a global every time
        float iv = simulationInterval;
        // Get simulation steps
        int ms = (int)(maxSimulationTime/simulationInterval);
        // List of points (for the line)
        List<Vector3> points = new List<Vector3>();
        // Init pos and velocity
        Vector3 initPos = rb.position;
        Vector3 curV = rb.velocity;

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

            // Stop calculating if a collision is detected
            if (HasCollided(newVector, points[Mathf.Max(i-1, 0)]))
            {
                break;
            }
        }

        //Debug.Log(points.Count);
        return points;
    }

    // Check if a collision has occured in a radius
    private bool HasCollided(Vector3 centre, Vector3 oldCentre)
    {
        // First check in a radius around the object
        //return Physics.CheckSphere(centre, colliderRadius);
        Collider[] cList = Physics.OverlapSphere(centre, colliderRadius);
        foreach(Collider c in cList)
        {
            // Reveal the collisions
            //Debug.Log(c.gameObject.name);
        }

        // Next check if there is a collision between step points
        Vector3 stepPath = centre - oldCentre;
        RaycastHit[] rList = Physics.RaycastAll(oldCentre, stepPath.normalized, stepPath.magnitude);
        Debug.Log(rList.Length);

        // Since it will always collide with itself, check to see if it hit anything
        if (cList.Length > 1 || rList.Length > 0)
            return true;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, colliderRadius);
    }
}
