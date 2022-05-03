using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour
{
    public bool aimAssistOn = false;
    public bool AimAssistOn
    {
        get { return aimAssistOn; }
        set { aimAssistOn = value; }
    }
    public float moveSpeed = 5.0f;
    public float cameraSensitivity = 3.0f;
    public float maxCameraAngle = 90.0f;
    public float pickupRadius = 2.0f;
    public float throwForce = 10.0f;
    public float snapTime = 0.2f;
    public float defaultFOV = 60;
    public float zoomFOV = 15;
    public GameObject uiReticle;
    public Vector3 snappedItemPosition = new Vector3(0.1f, 0.0f, 0.4f);
    public Vector3 snappedItemSinkPosition = new Vector3(0.1f, 0.0f, 0.5f);
    public Quaternion snappedItemRotation = Quaternion.Euler(-90, 180, 0);
    public Quaternion snappedItemSinkRotation = Quaternion.Euler(290, 270, 270);
    public Quaternion snappedItemPouringRotation = Quaternion.Euler(340, 270, 270);
    public bool isOverSink = false;

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
            if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRadius, ~0, QueryTriggerInteraction.Ignore))
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
                else if (hit.collider.gameObject.CompareTag("Button"))
                {
                    uiReticle.GetComponent<Image>().color = new Color32(255, 0, 255, 150);
                    if (Input.GetMouseButtonDown(0))
                    {
                        hit.collider.gameObject.GetComponent<HydraulicPressButton>().PressFPS();
                    }
                }
            }
        }
        else
        {
            if (isOverSink)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(SnapHeldItemToPositionCoroutine(snappedItemSinkPosition, snappedItemPouringRotation));
                    StartCoroutine(PourLiquidCoroutine());
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
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(SetCameraFOVCoroutine(zoomFOV));
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            StartCoroutine(SetCameraFOVCoroutine(defaultFOV));
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
        // Disabling autoaim on the object
        ParabolaPreview pp = heldItem.GetComponent<ParabolaPreview>();
        if(pp != null)
        {
            pp.DisableLine();
        }
        StartCoroutine(SnapHeldItemToPositionCoroutine(snappedItemPosition, snappedItemRotation));
    }

    void ThrowHeldItem()
    {
        heldItem.GetComponent<Rigidbody>().useGravity = true;
        heldItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        heldItem.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.Impulse);
        // Enabling the autoaim on the component
        if (aimAssistOn)
        {
            AimAssist aa = heldItem.GetComponent<AimAssist>();
            if (aa != null)
            {
                aa.EnableOnce();
            }
        }
        else
        {
            ParabolaPreview pp = heldItem.GetComponent<ParabolaPreview>();
            if(pp != null)
            {
                pp.TempEnable();
            }

        }
        isHoldingItem = false;
        heldItem = null;
    }

    void DropHeldItem()
    {
        heldItem.GetComponent<Rigidbody>().useGravity = true;
        heldItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        isHoldingItem = false;
        heldItem = null;
    }

    public IEnumerator SnapHeldItemToPositionCoroutine(Vector3 position, Quaternion rotation)
    {
        float elapsedTime = 0.0f;
        Vector3 startPos = heldItem.transform.localPosition;
        Quaternion startRotation = heldItem.transform.localRotation;
        while (elapsedTime < snapTime)
        {
            heldItem.transform.localPosition = Vector3.Lerp(startPos, position, (elapsedTime / snapTime));
            heldItem.transform.localRotation = Quaternion.Slerp(startRotation, rotation, (elapsedTime / snapTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        heldItem.transform.localPosition = position;
        heldItem.transform.localRotation = rotation;
    }

    public IEnumerator PourLiquidCoroutine()
    {
        while (Input.GetMouseButton(0))
        {
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(SnapHeldItemToPositionCoroutine(snappedItemSinkPosition, snappedItemSinkRotation));
    }

    IEnumerator SetCameraFOVCoroutine(float fov)
    {
        float elapsedTime = 0.0f;
        float startingFOV = Camera.main.fieldOfView;
        while (elapsedTime < snapTime)
        {
            Camera.main.fieldOfView = Mathf.Lerp(startingFOV, fov, (elapsedTime / snapTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
