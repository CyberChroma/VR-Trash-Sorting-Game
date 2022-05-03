using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraulicPress : MonoBehaviour
{
    public bool tempButtonPressed;

    private Animator anim;
    private List<Transform> objectsToSquish = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tempButtonPressed) {
            StartCoroutine(Squish());
            tempButtonPressed = false;
        }
    }
    
    public void StartSquish()
    {
        StartCoroutine(Squish());
    }

    IEnumerator Squish()
    {
        anim.SetTrigger("Squish");
        yield return new WaitForSeconds(0.2f);
        foreach(Transform obj in objectsToSquish) {
            obj.position = new Vector3(obj.position.x, transform.position.y, transform.position.z);
            obj.localScale = new Vector3(0.25f, 1, 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!objectsToSquish.Contains(other.transform) && other.GetComponent<Rigidbody>() && !other.name.Contains("Press")) {
            objectsToSquish.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsToSquish.Contains(other.transform) && other.GetComponent<Rigidbody>() && !other.name.Contains("Press")) {
            objectsToSquish.Remove(other.transform);
        }
    }
}
