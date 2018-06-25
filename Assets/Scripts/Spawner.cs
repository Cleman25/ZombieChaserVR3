using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public int spawnLimit = 15;
    public float spawnTimer = 10;
    public int spawnCount = 0;
    public bool startSpawn;
    public bool doneSpawning;
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
    public GameObject[] leftOver;
    public float itemsRemaining;
    public bool useCycle;

    public bool useWave;
    public int waveCount;
    public int perWave;

    public AudioClip clip;
    public AudioSource source;
    public bool playSound;
    // Use this for initialization
    void Start() {
        originPoint = transform.position;
        startSpawn = true;
        if (spawnRadius <= 0) {
            spawnRadius = 30;
        }
        spawnCount = 0;
        spawnTimer = 0;
        source = GetComponentInChildren<AudioSource>();
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
                    doneSpawning = true;
                }
            }
        }
	}

    public void CountRemaining() {
        for(int i = 0; i < items.Count; i++) {
            leftOver[i] = GameObject.FindGameObjectWithTag(items[i].tag);
        }
        itemsRemaining = leftOver.Length;
    }

    public void SpawnMore() {
        CountRemaining();
        if(itemsRemaining < spawnLimit && doneSpawning) {
            doneSpawning = false;
            startSpawn = true;
        }
        Debug.Log("Respawned an item.");
    }

    public IEnumerator Spawn() {
        Vector3 point = (Random.insideUnitSphere * spawnRadius) + originPoint;
        point.y = maxPos.y;
        Instantiate(ItemCycle(), point, Quaternion.identity);
        spawnCount++;
        if(playSound) {
            SoundManager.instance.PlaySound(clip, source);
            //SoundManager
        }
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
            selected = items[lastSpawnedIndex];
            lastSpawnedIndex = (lastSpawnedIndex + 1) % items.Count;
        } else {
            selected = items[0];
        }
        return selected;
    }
}
