using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public float currentStamina;
    public float speed;
    public float defaultSpeed = 5;
    public float maxStamina = 100;
    private float staminaRegenTimer = 0.0f;

    private const float staminaDecreasePerFrame = 1.0f;
    private const float staminaIncreasePerFrame = 5.0f;
    private const float staminaTimeToRegen = 1.0f;
    public bool isRunning;
    // Use this for initialization
    void Start () {
        currentStamina = maxStamina;
        if(speed <= 0)
            speed = defaultSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        if (isRunning) {
            currentStamina = Mathf.Clamp(currentStamina - (staminaDecreasePerFrame * Time.deltaTime), 0.0f, maxStamina);
            staminaRegenTimer = 0.0f;
        } else if (currentStamina < maxStamina) {
            if (staminaRegenTimer >= staminaTimeToRegen) {
                currentStamina = Mathf.Clamp(currentStamina + (staminaIncreasePerFrame * Time.deltaTime), 0.0f, maxStamina);
            } else {
                staminaRegenTimer += Time.deltaTime;
            }
        }

        if(currentStamina < 10) {
            speed = defaultSpeed/2;
        } else {
            speed = defaultSpeed;
        }
    }
}
