using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkliftRandomDrive : MonoBehaviour
{
    public float randomStartMin;
    public float randomStartMax;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(WaitToStartDrive());
    }

    IEnumerator WaitToStartDrive()
    {
        while (true) {
            yield return new WaitForSeconds(Random.Range(randomStartMin, randomStartMax));
            anim.SetTrigger("Drive");
        }
    }
}
