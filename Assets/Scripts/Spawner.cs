using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public int spawnLimit;
    public float spawnTimer;
    public int spawnCount;
    public bool startSpawn;
    public float itemsSpawned;
    public float waitTime;

    public float spawnRadius; // spawn in here
    public Vector3 originPoint;
    public Vector3 minPos;
    public Vector3 maxPos;

    public float itemRadius; // distance to avoid per pill
    public List<GameObject>items;
    public int lastSpawnedIndex = 0;
    public GameObject selected;
    public bool useCycle;

    public bool useWave;
    public int waveCount;
    public int perWave;
	// Use this for initialization
	void Start () {
        originPoint = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(startSpawn) {
            spawnTimer += Time.deltaTime;
            if(spawnTimer > waitTime) {
                if(spawnCount < spawnLimit) {
                    Spawn();
                    spawnTimer = 0;
                } else {
                    startSpawn = false;
                }
            }
        }
	}

    public void Spawn() {
        float directionFacing = Random.Range(0f, 360f);

        // need to pick a random position around originPoint but inside spawnRadius
        // must not be too close to another agent inside spawnRadius
        Vector3 point = (Random.insideUnitSphere * spawnRadius) + originPoint;
        point.y = maxPos.y;
        Instantiate(ItemCycle(), point, Quaternion.Euler(new Vector3(0f, directionFacing, 0f)));
    }

    public void Wave() {

    }

    public void SpawnX() {
        Vector3 itemPosition;
        itemPosition = new Vector3(-2.77f, 3.3f, 0);// set the position for the spawned GameObject
        Instantiate(ItemCycle(), itemPosition, Quaternion.identity);// clone/duplicate the given object
        spawnCount++;// add to the current amount of spawned GameObjects (in your case, the number of balls)
        Debug.Log("Spawned GameObject #" + spawnCount);
    }

    public GameObject ItemCycle() {
        if (useCycle) {
            for(int i = 0; i < items.Count; i++) {
                lastSpawnedIndex = i;
                if(lastSpawnedIndex == i) {
                    lastSpawnedIndex++;
                    selected = items[i];
                }
            }
        } else {
            selected = items[0];
        }
        return selected;
    }
}
