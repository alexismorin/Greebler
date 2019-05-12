using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Greeble Component", menuName = "Greeble Component")]
public class greebleComponent : ScriptableObject {

    [SerializeField]
    public GameObject prefab;
    [SerializeField]
    public bool randomYRotation = true;
    [SerializeField]
    public Vector2 scale = new Vector2 (0.8f, 1.1f);

}