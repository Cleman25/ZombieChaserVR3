using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrinPosition : MonoBehaviour {
    public Terrain terrain;
    public Vector3 terrainData;
	// Use this for initialization
	void Start () {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData.size;
        Reposition();
    }

    public void Reposition() {
        transform.position = new Vector3(-terrainData.x/2, 0, -terrainData.z/2);
    }
}
