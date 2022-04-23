using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowShrink : MonoBehaviour
{
    public float speed;
    public Vector3 scaleDirection;
    public float offset;
    public bool timeUnscaled;

    private Vector3 startScale;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeUnscaled) {
            Vector3 newScale = Mathf.Sin(Time.unscaledTime * speed + offset) * scaleDirection;
            newScale = new Vector3(newScale.x + 1, newScale.y + 1, newScale.z + 1);
            transform.localScale = Vector3.Scale(startScale, newScale);
        }
        else {
            Vector3 newScale = Mathf.Sin(Time.time * speed + offset) * scaleDirection;
            newScale = new Vector3(newScale.x + 1, newScale.y + 1, newScale.z + 1);
            transform.localScale = Vector3.Scale(startScale, newScale);
        }
    }
}
