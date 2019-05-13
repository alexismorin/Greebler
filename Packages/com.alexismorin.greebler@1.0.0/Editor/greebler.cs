using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[SelectionBase]
public class greebler : MonoBehaviour {
	// frontfacing

	[Header ("Greeble Components")]
	[SerializeField]
	greebleComponent[] greebleComponents = null;

	[Space (10)]

	[Header ("Generation Settings")]

	[Tooltip ("How dense should the generated previews and greeble be. Smaller numbers = higher detail")]
	[SerializeField]
	float voxelSize = 0.15f;
	[Space (3)]

	[Tooltip ("Noise applied to the spawners")]
	[SerializeField]
	float noiseMagnitude = 0.3f;
	[Space (3)]

	[Tooltip ("Higher values here will make greeble only appear in cracks")]
	[SerializeField]
	int footingThreshold = 3;
	[Space (3)]

	[Tooltip ("Vector direction of where greeble will settle.")]
	[SerializeField]
	Vector3 gravityDirection = Vector3.down;
	[Space (3)]

	[Tooltip ("Voxel threshold to spawn greeble. Higher values = Higher density")]
	[SerializeField]
	float dropThreshold = 0.1f;
	[Space (3)]

	[Tooltip ("Spawn mask to decide on what to spawn greeble")]
	[SerializeField]
	LayerMask spawnMask = Physics.DefaultRaycastLayers;

	// interals

	List<Vector3> greeblers;

	// Update is called once per frame
	public void Greeble () {
		for (int i = 0; i < greeblers.Count; i++) {
			greebleComponent selectedComponent = greebleComponents[Random.Range (0, greebleComponents.Length)];
			GameObject greebleInstance = PrefabUtility.InstantiatePrefab (selectedComponent.prefab) as GameObject;
			greebleInstance.transform.position = greeblers[i];
			greebleInstance.transform.parent = this.transform;

			if (selectedComponent.randomYRotation == true) {
				Vector3 randomYRotation = new Vector3 (0f, Random.Range (0f, 360f), 0f);
				greebleInstance.transform.localEulerAngles = randomYRotation;
			}

			Vector3 newScale = new Vector3 (1f, 1f, 1f) * Random.Range (selectedComponent.scale.x, selectedComponent.scale.y);
			greebleInstance.transform.localScale = newScale;
		}
	}

	// updateGreeblePreview
	public void OnValidate () {
		voxelSize = Mathf.Clamp (voxelSize, 0.1f, 10f);

		if (voxelSize > 0f) {
			clear ();
			regeneratePreview ();
		}
	}

	void regeneratePreview () {
		Vector3 bottomLeft = GetComponent<BoxCollider> ().bounds.min;
		Vector3 bottomRight = new Vector3 (GetComponent<BoxCollider> ().bounds.max.x, GetComponent<BoxCollider> ().bounds.min.y, GetComponent<BoxCollider> ().bounds.min.z);
		Vector3 endLeft = new Vector3 (GetComponent<BoxCollider> ().bounds.min.x, GetComponent<BoxCollider> ().bounds.min.y, GetComponent<BoxCollider> ().bounds.max.z);
		Vector3 topRight = GetComponent<BoxCollider> ().bounds.max;

		for (float z = bottomLeft.z; z < endLeft.z; z += voxelSize) {
			Vector3 voxelZ = new Vector3 (bottomLeft.x, bottomLeft.y, z);
			Vector3 noiseAdjustedZ = new Vector3 (voxelZ.x + Random.Range (-noiseMagnitude, noiseMagnitude), voxelZ.y + Random.Range (-noiseMagnitude, noiseMagnitude), voxelZ.z + Random.Range (-noiseMagnitude, noiseMagnitude));

			RaycastHit hitZ;
			if (Physics.Raycast (noiseAdjustedZ, transform.TransformDirection (gravityDirection), out hitZ, dropThreshold, spawnMask, QueryTriggerInteraction.Ignore)) {
				Collider[] hitColliders = Physics.OverlapSphere (hitZ.point, 0.2f);
				if (hitColliders.Length >= footingThreshold) {
					greeblers.Add (hitZ.point);
				} else {
					if (0 == Random.Range (0, footingThreshold * footingThreshold * footingThreshold)) {
						greeblers.Add (hitZ.point);
					}
				}
			}

			for (float x = bottomLeft.x; x < bottomRight.x; x += voxelSize) {
				Vector3 voxelX = new Vector3 (x, bottomLeft.y, z);
				Vector3 noiseAdjustedX = new Vector3 (voxelX.x + Random.Range (-noiseMagnitude, noiseMagnitude), voxelX.y + Random.Range (-noiseMagnitude, noiseMagnitude), voxelX.z + Random.Range (-noiseMagnitude, noiseMagnitude));

				RaycastHit hitX;
				if (Physics.Raycast (noiseAdjustedX, transform.TransformDirection (gravityDirection), out hitX, dropThreshold, spawnMask, QueryTriggerInteraction.Ignore)) {
					Collider[] hitColliders = Physics.OverlapSphere (hitX.point, 0.2f);
					if (hitColliders.Length >= footingThreshold) {
						greeblers.Add (hitX.point);
					} else {
						if (0 == Random.Range (0, footingThreshold * footingThreshold * footingThreshold)) {
							greeblers.Add (hitX.point);
						}
					}
				}

				for (float y = bottomLeft.y; y < topRight.y; y += voxelSize) {
					Vector3 voxelY = new Vector3 (x, y, z);
					Vector3 noiseAdjustedY = new Vector3 (voxelY.x + Random.Range (-noiseMagnitude, noiseMagnitude), voxelY.y + Random.Range (-noiseMagnitude, noiseMagnitude), voxelY.z + Random.Range (-noiseMagnitude, noiseMagnitude));

					RaycastHit hitY;
					if (Physics.Raycast (noiseAdjustedY, transform.TransformDirection (gravityDirection), out hitY, dropThreshold, spawnMask, QueryTriggerInteraction.Ignore)) {
						Collider[] hitColliders = Physics.OverlapSphere (hitY.point, 0.2f);

						if (hitColliders.Length >= footingThreshold) {
							greeblers.Add (hitY.point);
						} else {
							if (0 == Random.Range (0, footingThreshold * footingThreshold * footingThreshold)) {
								greeblers.Add (hitY.point);
							}
						}
					}
				}
			}
		}
	}

	// clears all previews and spawned instances
	void clear () {
		while (transform.childCount != 0) {
			DestroyImmediate (transform.GetChild (0).gameObject);
		}

		greeblers = new List<Vector3> ();
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = Color.black;

		for (int i = 0; i < greeblers.Count; i++) {
			Gizmos.DrawSphere (greeblers[i], 0.06f);
		}
	}
}