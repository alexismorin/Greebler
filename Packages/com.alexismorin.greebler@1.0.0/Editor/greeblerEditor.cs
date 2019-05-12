using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (greebler))]
public class greeblerEditor : Editor {
    public override void OnInspectorGUI () {
        DrawDefaultInspector ();

        greebler managerScript = (greebler) target;
        if (GUILayout.Button ("Greeble")) {
            managerScript.Greeble ();
        }
        if (GUILayout.Button ("Reseed")) {
            managerScript.OnValidate ();
        }
    }
}