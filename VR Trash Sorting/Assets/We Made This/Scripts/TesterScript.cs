using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterScript : MonoBehaviour
{
    public Vector3 InitLaunchVector = Vector3.zero;
    public Vector3 AddVector = Vector3.zero;
    public Vector3 DesiredPoint = Vector3.zero;
    public bool Launch = false;
    public bool DoCorrect = false;

    private Rigidbody rb;
    private Vector3 start;
    private List<Vector3> collisions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        Launch = false;
        rb = GetComponent<Rigidbody>();
        start = rb.position;
        collisions = new List<Vector3>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (Vector3.Distance(start, rb.position) > 0.01) 
        {
            Debug.Log("Collision at x:" + rb.position.x + " y: " + rb.position.y + " z: " + rb.position.z);
            collisions.Add(rb.position);
            Vector3 b = DesiredPoint - rb.position;
            if (!DoCorrect)
                AddVector = b;
            else
                AddVector += b;
            Debug.Log("Correction vector would be x: " + b.x + " y: " + b.y + " z: " + b.z);
            if(collisions.Count > 1)
            {
                Debug.Log(Vector3.Distance(rb.position, collisions[collisions.Count - 2]));
            }
            rb.position = start;
            rb.velocity = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Launch)
        {
            rb.velocity = InitLaunchVector + AddVector * (DoCorrect ? 1 : 0);
            Launch = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(DesiredPoint, transform.localScale);

        Gizmos.color = Color.red;
        foreach (Vector3 v in collisions)
        {
            Gizmos.DrawSphere(v, 0.1f);
            
        }
    }
}