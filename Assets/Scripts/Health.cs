using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public float maxHealth = 100;
    public float currentHealth;
    public float minHealth = 0;
	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if(currentHealth >= maxHealth) {
            currentHealth = maxHealth;
        }
        if (currentHealth <= minHealth) {
            currentHealth = minHealth;
        }
    }

    public void TakeDamage(float amount) {
        currentHealth -= amount;
        Debug.Log("You've sustained " + amount + " damage.");
    }
}
