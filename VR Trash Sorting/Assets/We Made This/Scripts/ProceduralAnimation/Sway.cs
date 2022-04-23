using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    public float downSpeed;
    public float upSpeed;
    public float drag;

    public int forwardBonesToAffect;
    public Transform rotationOffset;

    public float gravityInfluence;

    public float windChangeSpeed;
    public bool windRandomStart;
    public Vector3 windDirMin;
    public Vector3 windDirMax;

    private Vector3 baseVelocity;
    private Vector3 lastBasePosition;
    private Vector3[] boneVelocities;
    private Vector3[] boneStartUpDirections;
    private Quaternion[] boneStartRotations;
    private float[] boneNumPercentage;
    private Transform[] bones;
    private Vector3 curWind;
    private float randomWindStart;

    // Start is called before the first frame update
    void Start()
    {
        if (rotationOffset == null) {
            rotationOffset = transform;
        }
        baseVelocity = Vector3.zero;
        lastBasePosition = transform.position;
        if (forwardBonesToAffect == 0) {
            forwardBonesToAffect = GetComponentsInChildren<Transform>().Length-1;
        }
        bones = new Transform[forwardBonesToAffect];
        boneVelocities = new Vector3[forwardBonesToAffect];
        boneStartUpDirections = new Vector3[forwardBonesToAffect];
        boneStartRotations = new Quaternion[forwardBonesToAffect];
        boneNumPercentage = new float[forwardBonesToAffect];
        bones[0] = transform;
        boneVelocities[0] = Vector3.zero;
        boneStartUpDirections[0] = rotationOffset.InverseTransformDirection(transform.up);
        boneStartRotations[0] = bones[0].localRotation;
        boneNumPercentage[0] = 1; //0.1f/(bones.Length+1) + 0.9f;
        for (int i = 1; i < bones.Length; i++) {
            bones[i] = bones[i-1].GetChild(0);
            boneVelocities[i] = Vector3.zero;
            boneStartUpDirections[i] = rotationOffset.InverseTransformDirection(bones[i].up);
            boneStartRotations[i] = bones[i].localRotation;
            boneNumPercentage[i] = (i*0.1f)+1;//(i+1)*0.1f/(bones.Length+1) + 0.9f;
        }
        if (windRandomStart) {
            randomWindStart = Random.Range(0f, 2*Mathf.PI);
            curWind = Vector3.Lerp(windDirMin, windDirMax, Mathf.Sin(Time.time * windChangeSpeed + randomWindStart) / 2 + 0.5f);
        }
        else {
            curWind = windDirMin;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move away from center
        baseVelocity = (((transform.position - lastBasePosition) / Time.deltaTime) + baseVelocity) / 2;
        if (baseVelocity.magnitude > 0.01f) {
            for (int i = 0; i < bones.Length; i++) {
                Vector3 axisToVelocity = Vector3.Cross(baseVelocity, bones[i].up) * downSpeed * boneNumPercentage[i];
                bones[i].rotation = Quaternion.AngleAxis(axisToVelocity.magnitude, axisToVelocity) * bones[i].rotation;
            }
        }

        // Turn from wind
        curWind = Vector3.Lerp(windDirMin, windDirMax, Mathf.Sin(Time.time * windChangeSpeed + randomWindStart) / 2 + 0.5f);
        if (curWind.magnitude != 0) {
            for (int i = 0; i < bones.Length; i++) {
                Vector3 axisToWind = Vector3.Cross(bones[i].up, curWind) * curWind.magnitude * boneNumPercentage[i];
                bones[i].rotation = Quaternion.AngleAxis(axisToWind.magnitude, axisToWind) * bones[i].rotation;
            }
        }

        // Turn from gravity
        if (gravityInfluence != 0) {
            for (int i = 0; i < bones.Length; i++) {
                Vector3 axisToGravity = Vector3.Cross(bones[i].up, Physics.gravity) * gravityInfluence * 0.05f * boneNumPercentage[i];
                bones[i].rotation = Quaternion.AngleAxis(axisToGravity.magnitude, axisToGravity) * bones[i].rotation;
            }
        }

        // Try to move towards center
        for (int i = 0; i < bones.Length; i++) {
            Vector3 acceleration = Vector3.zero;
            if (Vector3.Distance(bones[i].up, rotationOffset.TransformDirection(boneStartUpDirections[i])) != 0) {
                Vector3 turnToCenterDirection = Vector3.Cross(bones[i].up, rotationOffset.TransformDirection(boneStartUpDirections[i]));
                acceleration = turnToCenterDirection * upSpeed;
            }
            boneVelocities[i] += acceleration * Time.deltaTime;
            boneVelocities[i] *= Mathf.Max(1 - Time.deltaTime * drag * boneNumPercentage[i], 0);
            bones[i].rotation = Quaternion.AngleAxis(boneVelocities[i].magnitude, boneVelocities[i]) * bones[i].rotation;
            if (boneVelocities[i].magnitude < 0.01f && Vector3.Distance(bones[i].up, rotationOffset.TransformDirection(boneStartUpDirections[i])) < 0.01f) {
                bones[i].localRotation = Quaternion.Slerp(bones[i].localRotation, boneStartRotations[i], 10 * Time.deltaTime);
            }
        }
        lastBasePosition = transform.position;
    }
}
