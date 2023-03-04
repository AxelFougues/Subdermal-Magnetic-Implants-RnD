using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SensitivityEq), true)]
public class SensitivityEqualizerEditor : Editor{
    SensitivityEq sensitivityEqualizer;
    Editor editor;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    }


    private void OnEnable() {
        sensitivityEqualizer = (SensitivityEq)target;
    }


}
