using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraulicPress : MonoBehaviour
{
    [SerializeField]
    private AudioClip extend;
    [SerializeField]
    private AudioClip retract;
    [SerializeField]
    private AudioClip[] squishNoises;
    private AudioSource audioS;
    private Animator anim;
    private List<Transform> objectsToSquish = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInParent<Animator>();
        audioS = GetComponent<AudioSource>();
    }
    
    public void StartSquish()
    {
        StartCoroutine(Squish());
    }

    IEnumerator Squish()
    {
        anim.SetTrigger("Squish");
        audioS.clip = extend;
        audioS.Play();
        yield return new WaitForSeconds(0.2f);
        audioS.Stop();
        foreach(Transform obj in objectsToSquish) {
            obj.position = new Vector3(obj.position.x, transform.position.y, transform.position.z);
            obj.localScale = new Vector3(obj.localScale.x * 0.25f, obj.localScale.y, obj.localScale.z);
            audioS.clip = squishNoises[Random.Range(0, squishNoises.Length)];
            audioS.Play();
        }
        audioS.clip = retract;
        audioS.PlayDelayed(0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!objectsToSquish.Contains(other.transform) && other.GetComponent<Rigidbody>() && !other.name.Contains("Button") && !other.name.Contains("Menu")) {
            objectsToSquish.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsToSquish.Contains(other.transform) && other.GetComponent<Rigidbody>() && !other.name.Contains("Button") && !other.name.Contains("Menu")) {
            objectsToSquish.Remove(other.transform);
        }
    }
}
