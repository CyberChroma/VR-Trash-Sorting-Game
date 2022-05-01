using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float cameraSensitivity = 3.0f;
    public float maxCameraAngle = 90.0f;
    public float pickupRadius = 2.0f;
    public float throwForce = 10.0f;
    public float snapTime = 0.2f;
    public Vector3 snappedItemPosition = new Vector3(0.1f, 0.0f, 0.4f);
    public GameObject uiReticle;
    public GameObject itemSpawner;

    private float xRotation;
    private float yRotation;
    private bool isHoldingItem = false;
    private GameObject heldItem;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Getting movement inputs and applying them
        // DEBUG ONLY -- Remove for final build
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = (Vector3.forward * verticalInput + Vector3.right * horizontalInput);
        transform.Translate(direction * Time.deltaTime * moveSpeed);

        uiReticle.GetComponent<Image>().color = new Color32(255, 255, 255, 150);
        if (!isHoldingItem)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRadius))
            {
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
                if (hit.collider.gameObject.CompareTag("Trash"))
                {
                    uiReticle.GetComponent<Image>().color = new Color32(255, 0, 255, 150);
                    if (Input.GetMouseButtonDown(0))
                    {
                        PickupItem(hit);
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                ThrowHeldItem();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                DropHeldItem();
            }
        }

        // Getting camera inputs and applying them
        xRotation += Input.GetAxis("Mouse X") * cameraSensitivity;
        yRotation -= Input.GetAxis("Mouse Y") * cameraSensitivity;
        xRotation = Mathf.Repeat(xRotation, 360);
        yRotation = Mathf.Clamp(yRotation, -maxCameraAngle, maxCameraAngle);

        transform.rotation = Quaternion.Euler(yRotation, xRotation, 0);
        
    }

    void PickupItem(RaycastHit hit)
    {
        isHoldingItem = true;
        heldItem = hit.collider.gameObject;
        hit.rigidbody.useGravity = false;
        hit.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        hit.collider.transform.parent = transform;
        StartCoroutine(SnapHeldItemToPositionCoroutine());
    }

    void ThrowHeldItem()
    {
        heldItem.transform.parent = itemSpawner.transform;
        heldItem.GetComponent<Rigidbody>().useGravity = true;
        heldItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        heldItem.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.Impulse);
        isHoldingItem = false;
        heldItem = null;
    }

    void DropHeldItem()
    {
        heldItem.transform.parent = itemSpawner.transform;
        heldItem.GetComponent<Rigidbody>().useGravity = true;
        heldItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        isHoldingItem = false;
        heldItem = null;
    }

    IEnumerator SnapHeldItemToPositionCoroutine()
    {
        float elapsedTime = 0.0f;
        Vector3 startPos = heldItem.transform.localPosition;
        while (elapsedTime < snapTime)
        {
            heldItem.transform.localPosition = Vector3.Lerp(startPos, snappedItemPosition, (elapsedTime / snapTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        heldItem.transform.localPosition = snappedItemPosition;
    }
}
