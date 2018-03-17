using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTest : MonoBehaviour {

	public GameObject bombPrefab;

	public void Emit() {
		GameObject bombObject = Instantiate (bombPrefab, transform.position, Quaternion.identity);
	}
}