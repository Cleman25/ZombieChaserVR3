using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public int spawnLimit = 15;
    public float spawnTimer = 10;
    public int spawnCount = 0;
    public bool startSpawn;
    public float itemsSpawned;
    public float waitTime = 1;

    public float spawnRadius = 30; // spawn in here
    public Vector3 originPoint;
    public Vector3 minPos = new Vector3(-30, 1, -30);
    public Vector3 maxPos = new Vector3(30, 1, 30);

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
        startSpawn = true;
        spawnRadius = 30;
	}
	
	// Update is called once per frame
	void Update () {
		if(startSpawn) {
            spawnTimer += Time.deltaTime;
            if(spawnTimer > waitTime) {
                if(spawnCount < spawnLimit) {
                    StartCoroutine(Spawn());
                    spawnTimer = 0;
                } else {
                    startSpawn = false;
                }
            }
        }
	}

    public IEnumerator Spawn() {
        //float directionFacing = Random.Range(0f, 360f);

        // need to pick a random position around originPoint but inside spawnRadius
        // must not be too close to another agent inside spawnRadius
        Vector3 point = (Random.insideUnitSphere * spawnRadius) + originPoint;
        point.y = maxPos.y;
        //Vector3 box = transform.localScale;
        //Vector3 pos = new Vector3(Random.Range(minPos.x, maxPos.x), maxPos.y, Random.Range(minPos.z, maxPos.z));
        //Vector3 pos = new Vector3(Random.value * box.x, maxPos.y, Random.value * box.x);
        //pos = transform.TransformPoint((pos) * .5f);
        //Instantiate(ItemCycle(), pos, Quaternion.Euler(new Vector3(0f, directionFacing, 0f)));
        Instantiate(ItemCycle(), point, Quaternion.identity);
        spawnCount++;
        yield return null;
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
            //for(int i = 0; i < items.Count; i++) {
            //    if(lastSpawnedIndex == i) {
            //        selected = items[i];
            //    }
            //    lastSpawnedIndex++;
            //    if (lastSpawnedIndex > items.Count) {
            //        lastSpawnedIndex = 0;
            //    }
            //    break;
            //}
            selected = items[lastSpawnedIndex];
            //lastSpawnedIndex++;
            lastSpawnedIndex = (lastSpawnedIndex + 1) % items.Count;
        } else {
            selected = items[0];
        }
        return selected;
    }
}
