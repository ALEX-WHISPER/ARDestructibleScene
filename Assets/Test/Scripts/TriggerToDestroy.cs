using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToDestroy : MonoBehaviour {

	void OnTriggerEnetr(Collider other) {
		if (other != null) {
			Destroy (other.gameObject);
		}
	}
}
