using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SMIS), true)]
public class SMISEditor : Editor{
    SMIS smis;
    Editor editor;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Start Test")) {
            smis.smisSettings.testInitiated = true;
            EditorApplication.isPlaying = true;
        }

        if (Application.isPlaying && smis.smisSettings.testInitiated) {
            smis.startTestSequence();
            smis.smisSettings.testInitiated = false;
        }
    }

    private void OnEnable() {
        smis = (SMIS)target;
    }
}
