using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RecoverTerrainGroundTexture))]
public class RecoverTerrainHeight : MonoBehaviour {
	private TerrainData terrainData;
	private float[,] saved;
	private int xRes;
	private int yRes;

	private void Start() {
		terrainData = transform.GetComponent<Terrain>().terrainData;
		xRes = terrainData.heightmapWidth;
		yRes = terrainData.heightmapHeight;
		saved = terrainData.GetHeights (0, 0, xRes, yRes);
	}

	private void OnDisable() {
		terrainData.SetHeights (0, 0, saved);
	}
}
