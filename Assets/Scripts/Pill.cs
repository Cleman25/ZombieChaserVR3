using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : MonoBehaviour {
    public enum PillType {
        Health,
        Stamina,
        Speed
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
                    worth = 10f;
                    break;
                case PillType.Stamina:
                    worth = 10f;
                    break;
                case PillType.Speed:
                    worth = 2f;
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
            }
            Destroy(gameObject);
        }
    }
}
