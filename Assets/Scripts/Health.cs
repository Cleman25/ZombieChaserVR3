using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public float maxHealth = 100;
    public float currentHealth;
    public float minHealth = 0;
    public Hud hud;
	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
        hud = GameObject.FindObjectOfType<Hud>();
	}
	
	// Update is called once per frame
	void Update () {
		if(currentHealth >= maxHealth) {
            currentHealth = maxHealth;
        } else if (currentHealth < minHealth) {
            currentHealth = minHealth;
        }
    }

    public void TakeDamage(float amount) {
        currentHealth -= amount;
        hud.TakeDamage();
        Debug.Log("You've sustained " + amount + " damage.");
    }
}
