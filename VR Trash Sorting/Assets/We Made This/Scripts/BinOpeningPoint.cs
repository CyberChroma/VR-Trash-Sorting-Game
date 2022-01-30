using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinOpeningPoint : MonoBehaviour
{
    // Trying some wack-ass static list stuff to combine all opening points
    public static List<Vector3> openingPoints = new List<Vector3>();

    public Vector3 openingCentre = Vector3.zero;

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

    

    private void Start()
    {
        // On start, add every opening point to a big list
        BinOpeningPoint.openingPoints.Add(transform.position + openingCentre);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawSphere(transform.position + openingCentre, 0.03f);
    }

}
