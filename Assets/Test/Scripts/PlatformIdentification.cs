using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class PlatformIdentification : MonoBehaviour {
	public static class Constants
	{
		public static bool ON = true;
		public static bool OFF = false;
	}

	public List<GameObject> arStuff = new List<GameObject>();

	void Start() {

		#if UNITY_EDITOR
		Switch(Constants.OFF);
		#endif

		#if !UNITY_EDITOR && UNITY_IOS
		Switch(Constants.ON);
		#endif
	}

	private void Switch(bool state) {
		if (arStuff.Count > 0) {
			foreach(GameObject go in arStuff) {
				go.SetActive (state);
			}
		}

		if (Camera.main.GetComponent<UnityARVideo> () != null) {
			Camera.main.GetComponent<UnityARVideo> ().enabled = state;
		}

		if (Camera.main.GetComponent<UnityARCameraNearFar> () != null) {
			Camera.main.GetComponent<UnityARCameraNearFar> ().enabled = state;
		}
	}
}
