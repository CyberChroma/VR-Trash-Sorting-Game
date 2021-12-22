using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float cameraSensitivity = 3.0f;
    public float maxCameraAngle = 90.0f;

    private float xRotation;
    private float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Getting movement inputs and applying them
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = (Vector3.forward * verticalInput + Vector3.right * horizontalInput);
        transform.Translate(direction * Time.deltaTime * moveSpeed);

        // Getting camera inputs and applying them
        xRotation += Input.GetAxis("Mouse X") * cameraSensitivity;
        yRotation -= Input.GetAxis("Mouse Y") * cameraSensitivity;
        xRotation = Mathf.Repeat(xRotation, 360);
        yRotation = Mathf.Clamp(yRotation, -maxCameraAngle, maxCameraAngle);

        transform.rotation = Quaternion.Euler(yRotation, xRotation, 0);
        
    }
}
