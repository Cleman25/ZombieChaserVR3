using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public Vector3 spawnPoint;
    public bool reSpawn;
    public float spawnDelay;
    public int spawnedCount;
    public List<Transform> spawns;
    public Transform selectedSpawn;
    public bool useCycle; // uses random if this is false
    public int spawnIndex;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<Transform> ListSpawns() {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        for(int i = 0; i < spawnPoints.Length; i++) {
            spawns.Add(spawnPoints[i].transform);
        }
        return spawns;
    }

    public void Spawn() {
        List<Transform> allSpawns = ListSpawns();
        if(useCycle) {
            selectedSpawn = allSpawns[spawnIndex];
            spawnIndex = (spawnIndex + 1) % allSpawns.Count;
        } else {
            spawnIndex = Random.Range(0, allSpawns.Count - 1);
            selectedSpawn = allSpawns[spawnIndex];
        }
        spawnPoint = selectedSpawn.position;
        GameObject player = Instantiate(GameManager.instance.playerPrefab, spawnPoint, selectedSpawn.rotation) as GameObject;
    }
}
