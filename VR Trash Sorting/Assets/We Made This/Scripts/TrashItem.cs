using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrashType
{
    Compost,
    Landfill,
    Container,
    Paper
}

public class TrashItem : MonoBehaviour
{
    public TrashType trashType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (transform.parent != null && transform.parent.name == "First Person Controller")
        {
            if (other.gameObject.CompareTag("Disposal"))
            {
                FirstPersonController fpsController = transform.parent.GetComponent<FirstPersonController>();
                fpsController.isOverSink = true;
                StartCoroutine(fpsController.SnapHeldItemToPositionCoroutine(fpsController.snappedItemSinkPosition, fpsController.snappedItemSinkRotation));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (transform.parent != null && transform.parent.name == "First Person Controller")
        {
            if (other.gameObject.CompareTag("Disposal"))
            {
                FirstPersonController fpsController = transform.parent.GetComponent<FirstPersonController>();
                fpsController.isOverSink = false;
                StartCoroutine(fpsController.SnapHeldItemToPositionCoroutine(fpsController.snappedItemPosition, fpsController.snappedItemRotation));
            }
        }
    }
}
