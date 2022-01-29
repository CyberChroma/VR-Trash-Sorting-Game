using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public float speed;
    public float dis;
    public float offset;

    private float startYPos;

    // Start is called before the first frame update
    void Start()
    {
        startYPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, startYPos + Mathf.Sin(Time.time * speed + offset) * dis, transform.position.z);
    }
}
