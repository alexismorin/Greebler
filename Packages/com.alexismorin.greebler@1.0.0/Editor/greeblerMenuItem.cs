using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class greeblerMenuItem : MonoBehaviour {

    [MenuItem ("Tools/Spawn Greebler")]
    static void spawnGreebler () {
        if (GameObject.Find ("Greebler") != null) {
            DestroyImmediate (GameObject.Find ("Greebler"));
        }

        GameObject greeblerManager = new GameObject ("Greebler");
        greeblerManager.AddComponent<BoxCollider> ();
        greeblerManager.GetComponent<BoxCollider> ().size = new Vector3 (10f, 10f, 10f);
        greeblerManager.AddComponent<greebler> ();
    }

}