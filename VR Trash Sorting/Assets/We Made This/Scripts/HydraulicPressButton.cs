using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraulicPressButton : MonoBehaviour
{
    private bool pressing;
    private HydraulicPress hydraulicPress;
    private float pressTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        pressing = false;
        hydraulicPress = FindObjectOfType<HydraulicPress>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pressing) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.19f, 0.29f, 0.179f), 10 * Time.deltaTime);
        } else {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.19f, 0.29f, 0.23f), 10 * Time.deltaTime);
        }
    }

    public void PressVR()
    {
        pressing = true;
        hydraulicPress.StartSquish();
    }

    public void PressFPS()
    {
        pressing = true;
        hydraulicPress.StartSquish();
        pressTime = 0.2f;
        StartCoroutine(unpressCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9) {
            if (!pressing) {
                PressVR();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9) {
            pressing = false;
        }
    }

    IEnumerator unpressCoroutine()
    {
        while (pressTime > 0)
        {
            pressTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        pressing = false;
    }
}
