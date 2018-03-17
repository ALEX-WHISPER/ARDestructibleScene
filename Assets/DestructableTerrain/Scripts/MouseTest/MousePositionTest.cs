using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionTest : MonoBehaviour {
	public GameObject clickPrefab;

	private void Update() {
		if (Input.GetButtonDown("Fire1")) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				Instantiate (clickPrefab, hit.point, Quaternion.identity);
			}
		}
	}
}