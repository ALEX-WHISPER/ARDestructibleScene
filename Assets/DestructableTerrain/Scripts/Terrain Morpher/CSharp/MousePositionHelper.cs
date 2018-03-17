using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionHelper : MonoBehaviour {

	public static MousePositionHelper instance;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (instance.gameObject);
		}
	}

	public bool GetMouseWorldPosition(out Vector3 mouseWorldPosition) {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			mouseWorldPosition = hit.point; 
			return true;
		} else {
			mouseWorldPosition = Vector3.zero;
			return false;
		}
	}
}
