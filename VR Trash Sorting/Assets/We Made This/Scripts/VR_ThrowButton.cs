using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VR_ThrowButton : MonoBehaviour
{
    // Cooldown in seconds the button will wait
    [SerializeField][Min(0f)]
    private float ButtonCooldown = 1;
    public float buttonCooldown
    {
        get { return ButtonCooldown; }
        set { ButtonCooldown = value; }
    }

    // Whether it is thrown idk
    private bool notInCooldown;
    public bool interactable
    {
        get { return notInCooldown; }
    }

    private Button butt;
    
    // UnityEvents that can be attached to button
    [SerializeField]
    private UnityEvent OnHit;


    void Start()
    {
        butt = GetComponent<Button>();
        notInCooldown = true;
    }

    // Privately press the button (no checks)
    private void pressButton()
    {
        // Go into cooldown
        StartCoroutine(goIntoCooldown());
        // Invoke all button actions
        OnHit.Invoke();
        // Visually press the button ??
        butt.Select();
    }

    // What to do when button is hit by a physics object
    private void OnCollisionEnter(Collision collision)
    {
        if(notInCooldown && collision.gameObject.layer == 8)    // Layer 8 == Grabbable (don't change this)
        {
            pressButton();
        }
    }

    // Public Press button (checks if in cooldown, returns whether button was pressed or not)
    public bool Press()
    {
        if (notInCooldown)
        {
            pressButton();
            return true;
        }

        return false;
    }

    public void TestScript(string thing)
    {
        Debug.Log(Time.realtimeSinceStartup + "s, " + gameObject.name + ": " + thing);
    }

    
    // start cooldown
    IEnumerator goIntoCooldown()
    {
        notInCooldown = false;
        yield return new WaitForSeconds(ButtonCooldown);
        // WaitForSeconds isn't cached because it is dynamic
        notInCooldown = true;
    }
}
