using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : MonoBehaviour {
    public enum PillType {
        Health,
        Stamina,
        Speed,
        Coin
    }
    public Light myLight;
    public float intensity;
    public float worth;
    public AudioClip pickUpClip;
    public AudioClip normalClip;
    public AudioSource pickUpSource;
    public AudioSource normalSource;
    public PillType pillType;
    public Hud hud;
    public Spawner[] spawners;
    public GameObject pillObject;
    public bool allowRotation;
	// Use this for initialization
	void Start () {
		if(worth <= 0) {
            switch(pillType) {
                case PillType.Health:
                    if(worth <= 0) {
                        worth = 10f;
                    }
                    break;
                case PillType.Stamina:
                    if(worth <= 0) {
                        worth = 10f;
                    }
                    break;
                case PillType.Speed:
                    if(worth <= 0) {
                        worth = 2f;
                    }
                    break;
                case PillType.Coin:
                    if(worth <= 0) {
                        worth = 100f;
                    }
                    break;
            }
        }
        if(intensity <= 0) {
            intensity = 10f;
        }
        hud = GameObject.FindObjectOfType<Hud>();
        myLight = GetComponentInChildren<Light>();
        if(normalClip) {
            SoundManager.instance.PlayMusic(normalClip, normalSource);
        }
    }
	
	// Update is called once per frame
	void Update () {
        myLight.intensity = Mathf.PingPong(100f, intensity * Time.deltaTime);
        if(pillObject && allowRotation) {
            pillObject.transform.Rotate(0.5f, 0.5f, 0.5f);
        }
	}

    void OnTriggerEnter (Collider col) {
        if(col.gameObject.CompareTag("Player")) {
            Character character = col.gameObject.GetComponent<Character>();
            Health health = col.gameObject.GetComponent<Health>();
            if(pickUpClip) {
                SoundManager.instance.PlaySound(pickUpClip, pickUpSource);
            }
            switch(pillType) {
                case PillType.Health:
                    if(health.currentHealth <= 100) {
                        health.currentHealth += worth;
                        if (health.currentHealth > 100) {
                            health.currentHealth = 100;
                        }
                        GameManager.instance.healthPowerUps++;
                        GameManager.instance.powerUpsPickedUp++;
                        Debug.Log("You regained " + worth + " health from the " + pillType + " pill");
                        //Respawn();
                        Destroy(gameObject);
                    }
                    break;
                case PillType.Stamina:
                    if(character.currentStamina <= 100) {
                        character.currentStamina += worth;
                        if (character.currentStamina > 100) {
                            character.currentStamina = 100;
                        }
                        GameManager.instance.staminaPowerUps++;
                        GameManager.instance.powerUpsPickedUp++;
                        Debug.Log("You regained " + worth + " stamina from the " + pillType + " pill");
                        //Respawn();
                        Destroy(gameObject);
                    }
                    break;
                case PillType.Speed:
                    character.defaultSpeed *= worth;
                    character.pillTimer = 10;
                    if (character.defaultSpeed > 14) {
                        character.defaultSpeed = 14;
                    }
                    GameManager.instance.speedPowerUps++;
                    GameManager.instance.powerUpsPickedUp++;
                    Debug.Log("Your speed has increased " + worth + " fold from the " + pillType + " pill");
                    //Respawn();
                    Destroy(gameObject);
                    break;
                case PillType.Coin:
                    GameManager.instance.coins++;
                    GameManager.instance.wallet += worth;
                    Debug.Log("You picked up a coin! You have " + worth + " extra credits in your wallet.");
                    //Respawn();
                    Destroy(gameObject);
                    break;
            }
        }
    }

    public void Respawn() {
        //for(int i = 0; i < spawners.Length; i++) {
        //    spawners[i].SpawnMore();
        //}
    }

    public void Rotation() {
        transform.Rotate(0.5f, 0.5f, 0.5f);
    }
}
