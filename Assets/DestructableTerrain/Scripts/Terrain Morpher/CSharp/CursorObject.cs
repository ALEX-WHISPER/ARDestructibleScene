using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorObject : MonoBehaviour {

	public GameObject explosionObject;

	void Update() {
		Vector3 mouseWorldPosition;
		if (!MousePositionHelper.instance.GetMouseWorldPosition (out mouseWorldPosition)) {
			return;
		} else {
			transform.position = mouseWorldPosition;
		}

		if (Input.GetButtonDown("Fire1")) {
			Instantiate (explosionObject, transform.position, Quaternion.identity);
		}
	}
}
