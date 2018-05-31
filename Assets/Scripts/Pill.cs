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
    public AudioClip clip;
    public AudioSource source;

    public PillType pillType;


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
            intensity = 1000f;
        }
        myLight = GetComponentInChildren<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        myLight.intensity = Mathf.PingPong(100f, intensity * Time.deltaTime);
        transform.Rotate(0.5f, 0.5f, 0.5f);
	}

    void OnTriggerEnter (Collider col) {
        if(col.gameObject.CompareTag("Player")) {
            switch(pillType) {
                case PillType.Health:
                    col.gameObject.GetComponent<Health>().currentHealth += worth;
                    Debug.Log("You regained " + worth + " health from the " + pillType + " pill");
                    break;
                case PillType.Stamina:
                    col.gameObject.GetComponent<Character>().currentStamina += worth;
                    Debug.Log("You regained " + worth + " stamina from the " + pillType + " pill");
                    break;
                case PillType.Speed:
                    col.gameObject.GetComponent<Character>().defaultSpeed *= worth;
                    Debug.Log("Your speed has increased " + worth + " fold from the " + pillType + " pill");
                    break;
                case PillType.Coin:
                    GameManager.instance.coins++;
                    GameManager.instance.wallet += worth;
                    Debug.Log("You picked up a coin! You have " + worth + " extra credits in your wallet.");
                    break;
            }
            Destroy(gameObject);
        }
    }
}
