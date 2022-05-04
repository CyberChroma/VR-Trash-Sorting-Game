using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMenu : MonoBehaviour
{
    [SerializeField]
    private BinOpeningPoint hihi;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void KillSelf()
    {
        gameObject.SetActive(false);
    }

    public void LoveSelf()
    {
        hihi.enabled = true;
    }
}
