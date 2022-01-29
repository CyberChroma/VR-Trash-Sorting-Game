using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaPreview : MonoBehaviour
{
    public bool lineEnabled = false;
    public float colliderRadius = 0.2f;
    public int maxSimulationTime = 5;
    public float simulationInterval = 0.1f;

    private LineRenderer line;
    private Rigidbody rb;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(lineEnabled){
            List<Vector3> pointList = CalculateLine();
            // Clear any points from previous frame
            line.positionCount = pointList.Count;
            for(int i = 0; i < pointList.Count; i++)
            {
                
                Debug.Log("Added point " + i + ": " + pointList[i].ToString());
                line.SetPosition(i, pointList[i]);
            }
            
            
        }
    }

    public void EnableLine()
    {
        line.enabled = true;
        lineEnabled = true;
    }

    public void DisableLine()
    {
        line.enabled = false;
        lineEnabled = false;
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
            if (HasCollided(newVector))
            {
                break;
            }
        }

        Debug.Log(points.Count);
        return points;
    }

    // Check if a collision has occured in a radius
    private bool HasCollided(Vector3 centre)
    {
        //return Physics.CheckSphere(centre, colliderRadius);
        Collider[] cList = Physics.OverlapSphere(centre, colliderRadius);
        foreach(Collider c in cList)
        {
            // Reveal the collisions
            Debug.Log(c.gameObject.name);
        }
        if (cList.Length > 1)
            return true;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, colliderRadius);
    }
}
