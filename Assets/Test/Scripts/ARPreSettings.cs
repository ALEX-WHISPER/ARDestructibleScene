using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.iOS;

public enum PlaneStatus {
	PLANE_FOUND_ERROR,
	PLANE_FOUND_OK,
	PLANE_SEARCHING,
	PLANE_EXPANDING,
	PLANE_CONFIRMED
}

public class ARPreSettings : MonoBehaviour {
	public GameObject m_Scene;

	public Button btn_StartSession;
	public Button btn_ConfirmPlane;

	public GameObject m_PreSettingUI;
	public GameObject m_GamingUI;

	public UnityARGeneratePlane generPlanesManager;
	public float planeSizeTrigger;

	private List<ARPlaneAnchorGameObject> curPlaneAnchorGO;
	private Transform detectedPlane;
	private Transform confirmedPlane;
	private PlaneStatus planeStatus;

	private bool startPlaneFinding = false;
	private bool isPlaneFound = false;
	private bool isPlaneSet = false;

	void Start() {
		btn_StartSession.onClick.AddListener (OnStartSession);
		btn_ConfirmPlane.onClick.AddListener (OnConfirmDetection);

		if (m_GamingUI.activeSelf) {
			m_GamingUI.SetActive (false);
		}
	}

	void Update() {
		//	If you're finding the plane
		if (startPlaneFinding && !isPlaneSet) {
			PlaneFinding ();
		} else {
			return;
		}
	}

	void OnDisable() {
		btn_StartSession.onClick.RemoveListener (OnStartSession);
		btn_ConfirmPlane.onClick.RemoveListener (OnConfirmDetection);
	}

	/// <summary>
	/// Event: Host start session: Start arkit's session with plane detection enabled
	/// </summary>
	private void OnStartSession() {
		Debug.Log ("Host hit StartSession");

		ARKitWorldTrackingSessionConfiguration config = new ARKitWorldTrackingSessionConfiguration();
		config.planeDetection = UnityARPlaneDetection.Horizontal;
		config.alignment = UnityARAlignment.UnityARAlignmentGravityAndHeading;
		config.getPointCloudData = true;
		config.enableLightEstimation = true;

		UnityARSessionNativeInterface.GetARSessionNativeInterface ().RunWithConfig (config);
		startPlaneFinding = true;
	}

	/// <summary>
	/// Event: Comfirming of plane detection
	/// </summary>
	private void OnConfirmDetection() {

		#if UNITY_EDITOR
		m_PreSettingUI.SetActive(false);
		m_GamingUI.SetActive(true);

		m_Scene.SetActive(true);
		float terrainSize_X = GameObject.FindWithTag("Ground").GetComponent<Terrain>().terrainData.size.x;
		float terrainSize_Z = GameObject.FindWithTag("Ground").GetComponent<Terrain>().terrainData.size.z;
		m_Scene.transform.position = Vector3.zero - new Vector3(terrainSize_X / 2, 0f, terrainSize_Z / 2);
		Debug.Log(string.Format("terrainSize.x: {0}, terrainSize.z: {1}", terrainSize_X, terrainSize_Z));

		isPlaneFound = isPlaneSet = true;
		#endif

		#if !UNITY_EDITOR && UNITY_IOS
		//	if the plane has not been found, can't display nothing
		if (!isPlaneFound) { 
			//			gameUIElems.p_PlaneNotFounded.SetActive (true);
			Debug.Log("PlaneDetection: Have not found the plane yet, what are you trying to confirm???");
			return; 
		}

		//	if the plane has already been confirmed, no need for reconfirming
		if (isPlaneSet) { 
			//			gameUIElems.p_PlaneAlreadyConfirmed.SetActive (true);
			Debug.Log("PlaneDetection: You have confirmed already");
			return; 
		}

		//	if the plane has been founded, and has not been confirmed, then confirm it.
		confirmedPlane = detectedPlane;	//	remember this founded plane
		planeStatus = PlaneStatus.PLANE_CONFIRMED;	//	update the status
		isPlaneSet = true;

		m_GamingUI.SetActive (true);

		m_Scene.SetActive(true);
		float terrainSize_X = GameObject.FindWithTag("Ground").GetComponent<Terrain>().terrainData.size.x;
		float terrainSize_Z = GameObject.FindWithTag("Ground").GetComponent<Terrain>().terrainData.size.z;
		m_Scene.transform.position = confirmedPlane.position - new Vector3(terrainSize_X / 2, 0f, terrainSize_Z / 2);

		isPlaneFound = isPlaneSet = true;
		m_Scene.SetActive (true);

		Debug.Log ("Host hit confirmPlane");

		//	reset session
		SessionSleep ();
		#endif
	}

	//	Find a plane being referenced
	private void PlaneFinding() {
		if (isPlaneSet) { return; }

		curPlaneAnchorGO = generPlanesManager.GetARAnchorManager.GetCurrentPlaneAnchors ();		//	Get current <ARPlaneAnchorGameObject> List

		//	No plane has been detected so far
		if (curPlaneAnchorGO.Count == 0) {
			planeStatus = PlaneStatus.PLANE_SEARCHING;
			return;
		}

		GameObject curPlane = curPlaneAnchorGO [curPlaneAnchorGO.Count - 1].gameObject;
		MeshFilter mf = curPlane.GetComponentInChildren<MeshFilter> ();
		if (curPlane == null || mf == null) {
			planeStatus = PlaneStatus.PLANE_FOUND_ERROR;
		} else {
			if (mf.transform.localScale.z >= planeSizeTrigger * 0.1f) {
				planeStatus = PlaneStatus.PLANE_FOUND_OK;
				detectedPlane = mf.gameObject.transform;
				isPlaneFound = true;
			} else {
				planeStatus = PlaneStatus.PLANE_EXPANDING;
			}
		}
	}

	private void SessionSleep() {
		ARKitWorldTrackingSessionConfiguration newConfig = new ARKitWorldTrackingSessionConfiguration();
		newConfig.planeDetection = UnityARPlaneDetection.None;
		newConfig.alignment = UnityARAlignment.UnityARAlignmentGravityAndHeading;
		newConfig.getPointCloudData = false;
		newConfig.enableLightEstimation = false;

		UnityARSessionNativeInterface.GetARSessionNativeInterface ().RunWithConfig (newConfig);
	}
}
