using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainInsurance : MonoBehaviour {

	private int xRes;
	private int yRes;

	private int xRes_Ground;
	private int yRes_Ground;

	private float[,] saved;
	private float[,,] saved_Ground;
	private TerrainData terrainData;

	void Start() {
		TerrainDataInit ();
	}

	void Disable() {
		TerrainDataRecover ();
	}

	private void TerrainDataInit() {
		terrainData = GameObject.FindWithTag("Ground").GetComponent<Terrain>().terrainData;

		xRes = terrainData.heightmapWidth;
		yRes = terrainData.heightmapHeight;
		xRes_Ground = terrainData.alphamapWidth;
		yRes_Ground = terrainData.alphamapHeight;

		saved = terrainData.GetHeights (0, 0, xRes, yRes);
		saved_Ground = terrainData.GetAlphamaps (0, 0, xRes_Ground, yRes_Ground);

	}

	private void TerrainDataRecover() {
		terrainData.SetHeights (0, 0, saved);
		terrainData.SetAlphamaps (0, 0, saved_Ground);
	}
}
