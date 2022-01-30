using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestBoost))]
public class TestBoostEditor : Editor
{
    TestBoost tb;

    private void OnEnable()
    {
        tb = (TestBoost)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    void OnSceneGUI()
    {
        tb.LaunchForce = Handles.ScaleValueHandle(tb.LaunchForce, tb.transform.position, tb.LaunchDirection, tb.LaunchForce * 0.2f, Handles.ArrowHandleCap, 0.1f);
        tb.LaunchDirection = Handles.RotationHandle(tb.LaunchDirection, tb.transform.position);
        Handles.DrawLine(tb.transform.position, tb.transform.position + tb.LaunchDirection * Vector3.forward);
    }
}
