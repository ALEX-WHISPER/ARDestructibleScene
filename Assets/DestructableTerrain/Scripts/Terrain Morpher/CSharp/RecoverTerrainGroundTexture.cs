using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverTerrainGroundTexture : MonoBehaviour {
	private int xRes;
	private int yRes;
	private float[,,] saved;
	private TerrainData tData;

	private void Start() {
		tData = transform.GetComponent<Terrain>().terrainData;
		xRes = tData.alphamapWidth;
		yRes = tData.alphamapHeight;
		saved = tData.GetAlphamaps (0, 0, xRes, yRes);
	}

	private void OnDisable() {
		tData.SetAlphamaps (0, 0, saved);
	}
}
