using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinOpeningPoint : MonoBehaviour
{
    // Trying some wack-ass static list stuff to combine all opening points
    public static HashSet<Vector3> openingPoints = new HashSet<Vector3>();

    public Vector3 openingCentre = Vector3.zero;
    [Range(0, 10)]
    public float radius = 0f;
    private Vector3 generatedTransformPoint;

    /// <summary>
    /// Given a position, returns the closest bin opening to given position.
    /// </summary>
    /// <param name="pos">Vector3 position to compare distance to.</param>
    /// <returns>Vector3 position of closest bin opening, otherwise returns Zero Vector.</returns>
    public static Vector3 FindClosestOpening(Vector3 pos)
    {
        // Create bogus vector initially
        Vector3 closest = Vector3.zero;
        float dist = float.MaxValue;
        // Compare the distance of each vector in the list
        foreach(Vector3 op in openingPoints)
        {
            float f = Vector3.Distance(pos, op);
            if(f < dist)
            {
                closest = op;
                dist = f;
            }
        }

        return closest;
    }

    

    /*private void Start()
    {
        // On start, add every opening point to a big list
        BinOpeningPoint.openingPoints.Add(transform.position + openingCentre);
    }*/

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawSphere(transform.position + openingCentre, 0.03f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + openingCentre, radius);
    }

    private void OnEnable()
    {
        // On enable, subscribe opening point to a big list
        generatedTransformPoint = transform.position + openingCentre;
        BinOpeningPoint.openingPoints.Add(generatedTransformPoint);
        Debug.Log("Adding self, size: " + BinOpeningPoint.openingPoints.Count);
    }

    private void OnDisable()
    {
        // On disable, unsubscribe opening point from list
        BinOpeningPoint.openingPoints.Remove(generatedTransformPoint);
        Debug.Log("Removing self, size: " + BinOpeningPoint.openingPoints.Count);
    }
}
