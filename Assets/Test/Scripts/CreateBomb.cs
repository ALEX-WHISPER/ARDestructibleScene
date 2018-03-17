using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateBomb : MonoBehaviour {

	public Button btn_Shoot;
	public List<GameObject> bombPrefabs;

	private GameObject bombObject;

	void Start() {
		btn_Shoot.onClick.AddListener (EmitBomb);
	}

	private void Update() {
//		if (Input.GetButtonDown("Fire1")) {
//			Vector3 mouseWorldPos;
//			if (!MousePositionHelper.instance.GetMouseWorldPosition(out mouseWorldPos)) {
//				return;
//			}
//
//			if (bombPrefabs.Count > 0) {
//				Instantiate (bombPrefabs[Random.Range(0, bombPrefabs.Count)], mouseWorldPos, Quaternion.identity);
//			}
//		}

		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Space)) {
			EmitBomb();
		}
		#endif
	}

	void OnDisable() {
		btn_Shoot.onClick.RemoveListener (EmitBomb);
	}

	private void EmitBomb() {
		Instantiate (bombPrefabs[Random.Range(0, bombPrefabs.Count)], Camera.main.transform.position, Camera.main.transform.rotation);
	}
}
