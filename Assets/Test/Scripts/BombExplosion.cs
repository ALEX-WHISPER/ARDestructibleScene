using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour {

	public float moveSpeed;
	public float emitForce;
	public float rbExplosionForce;
	public float hitDistanceValue;
	public GameObject explosion;
	public float influenceRadiusValue;
	public float explosionForceValue;
	public Texture2D craterTexture;
	public Texture2D groundTexture;

	private float hitDistance;
	private float influenceRadius;
	private float explosionForce;

	private int xRes;
	private int yRes;

	private int xRes_Ground;
	private int yRes_Ground;

	private int layers;
	private Terrain terrainObject;
	private TerrainData terrainData;
	private Color[] craterData;
	private Color[] craterGroundData;

	private bool isStop = false;

	private void Start() {
		TerrainDataInit ();		//	For create crater and change the texture in Terrain
		HitSettings ();			//	For bomb's raycast hit and explosion

		GetComponent<Rigidbody> ().AddForce (Camera.main.transform.forward * emitForce);
	}

	#region If use raycast hit to detect ground hitting
//	private void HitGround() {
//		Ray ray = new Ray (transform.position, -1 * transform.up);
//		RaycastHit hitInfo;
//
//		if (Physics.Raycast(ray, out hitInfo, hitDistance) && !isStop) {
//			if (hitInfo.collider.tag == "Ground") {
//				CreateExplosion ();
//				CreateCrater ();
//				CreateCraterTexture ();
//				Invoke ("DisableExpSrc", 0.1f);
//				isStop = true;
//			}
//		}
//	}
	#endregion

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Ground") {
			
			GetComponent<Collider> ().isTrigger = false;
			CreateExplosion ();
			CreateCrater ();
			CreateCraterTexture ();
			Invoke ("DisableExpSrc", 0.1f);

			GetComponent<Rigidbody> ().AddExplosionForce (rbExplosionForce, transform.position, influenceRadius);
			isStop = true;
		}
	}

	private void TerrainDataInit() {
		terrainObject = GameObject.FindWithTag ("Ground").GetComponent<Terrain>();
		terrainData = terrainObject.terrainData;

		xRes = terrainData.heightmapWidth;
		yRes = terrainData.heightmapHeight;
		xRes_Ground = terrainData.alphamapWidth;
		yRes_Ground = terrainData.alphamapHeight;

		layers = terrainData.alphamapLayers;

		craterData = craterTexture.GetPixels ();
		craterGroundData = groundTexture.GetPixels ();
	}

	private void HitSettings() {
		hitDistance = TransformationForARWorld(hitDistanceValue);
		influenceRadius = TransformationForARWorld (influenceRadiusValue);
		explosionForce = TransformationForARWorld (explosionForceValue);
	}

	private float TransformationForARWorld(float originValue) {
		return originValue * transform.parent.localScale.magnitude;
	}

	private void CreateExplosion() {
		GameObject explosionPS = Instantiate (explosion, transform.position, Quaternion.identity);

		ParticleSystem.MainModule psMain = explosionPS.GetComponent<ParticleSystem> ().main;
		psMain.startSize = new ParticleSystem.MinMaxCurve(psMain.startSize.constant * transform.localScale.magnitude);

		ExplosionSource expSrc = GetComponent<ExplosionSource> ();
		if (expSrc != null) {
			expSrc.enabled = true;
			expSrc.InfluenceRadius = influenceRadius;
			expSrc.Force = explosionForce;
			expSrc.PosEnd = transform.position;
			expSrc.PosStart = transform.position;
		}
	}

	private void DisableExpSrc() {
		ExplosionSource expSrc = GetComponent<ExplosionSource> ();
		if (expSrc != null) {
			expSrc.enabled = false;	
		}
	}

	private void CreateCrater() {
		float xMin = terrainObject.transform.position.x;
		float xMax = xMin + terrainData.size.x;

		float zMin = terrainObject.transform.position.z;
		float zMax = zMin + terrainData.size.z;

		int x = (int)Mathf.Lerp(0, xRes, Mathf.InverseLerp(xMin, xMax, transform.position.x));
		int z = (int)Mathf.Lerp (0, yRes, Mathf.InverseLerp(zMin, zMax, transform.position.z));

		x = Mathf.Clamp (x, craterTexture.width / 2, xRes - craterTexture.width / 2);
		z = Mathf.Clamp (z, craterTexture.height / 2, yRes - craterTexture.height / 2);

		float[,] areaT = terrainData.GetHeights (x - craterTexture.width / 2, z - craterTexture.height / 2, craterTexture.width, craterTexture.height);

		for (int i = 0; i < craterTexture.height; i++) {
			for (int j = 0; j < craterTexture.width; j++) {
				areaT [i, j] = areaT [i, j] - craterData [i * craterTexture.width + j].a * 0.01f * transform.localScale.sqrMagnitude;
			}
		}
		terrainData.SetHeights (x - craterTexture.width / 2, z - craterTexture.height / 2 , areaT);
	}

	private void CreateCraterTexture() {
		float xMin = terrainObject.transform.position.x;
		float xMax = xMin + terrainData.size.x;

		float zMin = terrainObject.transform.position.z;
		float zMax = zMin + terrainData.size.z;

		int g = (int)Mathf.Lerp (0, xRes_Ground, Mathf.InverseLerp(xMin, xMax, transform.position.x));
		int b = (int)Mathf.Lerp (0, yRes_Ground, Mathf.InverseLerp(zMin, zMax, transform.position.z));

		g = Mathf.Clamp (g, groundTexture.width / 2, xRes_Ground - groundTexture.width / 2);
		b = Mathf.Clamp (b, groundTexture.height / 2, yRes_Ground - groundTexture.height / 2);

		float[,,] area = terrainData.GetAlphamaps (g - groundTexture.width / 2, b - groundTexture.height / 2, groundTexture.width, groundTexture.height);

		for (int i = 0; i < groundTexture.height; i++) {
			for (int j = 0; j < groundTexture.width; j++) {
				for (int k = 0; k < layers; k++) {
					if (k == 1) {
						area [i, j, k] += craterGroundData [i * groundTexture.width + j].a * transform.localScale.sqrMagnitude;
					} else {
						area[i,j,k] -= craterGroundData[i * groundTexture.width + j].a * transform.localScale.sqrMagnitude;
					}
				}
			}
		}
		terrainData.SetAlphamaps (g - groundTexture.width / 2, b - groundTexture.height / 2, area);
	}
}
