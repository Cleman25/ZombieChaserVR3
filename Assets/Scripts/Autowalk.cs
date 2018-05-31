﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autowalk : MonoBehaviour {
    private const int RIGHT_ANGLE = 90;
    public PlayerAnimation pAnim;
    // This variable determinates if the player will move or not 
    public bool isWalking = false;

    Transform mainCamera = null;

    //This is the variable for the player speed
    [Tooltip("With this speed the player will move.")]
    public float speed;

    [Tooltip("Activate this checkbox if the player shall move when the Cardboard trigger is pulled.")]
    public bool walkWhenTriggered;

    [Tooltip("Activate this checkbox if the player shall move when he looks below the threshold.")]
    public bool walkWhenLookDown;

    [Tooltip("Activate this checkbox if the player shall move when the screen is tapped.")]
    public bool walkWhenTapped = true;

    [Tooltip("This has to be an angle from 0° to 90°")]
    public double thresholdAngle;

    [Tooltip("Activate this Checkbox if you want to freeze the y-coordiante for the player. " +
        "For example in the case of you have no collider attached to your CardboardMain-GameObject" +
        "and you want to stay in a fixed level.")]
    public bool freezeYPosition;

    [Tooltip("This is the fixed y-coordinate.")]
    public float yOffset;

    void Start() {
        mainCamera = Camera.main.transform;
        speed = GetComponent<Character>().speed;
        pAnim = GetComponentInChildren<PlayerAnimation>();
    }

    void FixedUpdate() {
        if(!pAnim) {
            pAnim = GetComponentInChildren<PlayerAnimation>();
        }
        speed = GetComponent<Character>().speed;
        // Walk when the Cardboard Trigger is used 
        if (walkWhenTriggered && !walkWhenLookDown && !walkWhenTapped && !isWalking) {
            isWalking = true;
        } else if (walkWhenTriggered && !walkWhenLookDown && isWalking) {
            isWalking = false;
        }

        // Walk when player looks below the threshold angle 
        if (walkWhenLookDown && !walkWhenTriggered && !walkWhenTapped && !isWalking &&
            mainCamera.transform.eulerAngles.x >= thresholdAngle &&
            mainCamera.transform.eulerAngles.x <= RIGHT_ANGLE) {
            isWalking = true;
        } else if (walkWhenLookDown && !walkWhenTriggered && isWalking &&
            (mainCamera.transform.eulerAngles.x <= thresholdAngle ||
                mainCamera.transform.eulerAngles.x >= RIGHT_ANGLE)) {
            isWalking = false;
        }

        // Walk when the Cardboard trigger is used and the player looks down below the threshold angle
        if (walkWhenLookDown && walkWhenTriggered && !walkWhenTapped && !isWalking &&
            mainCamera.transform.eulerAngles.x >= thresholdAngle &&
            mainCamera.transform.eulerAngles.x <= RIGHT_ANGLE) {
            isWalking = true;
        } else if (walkWhenLookDown && walkWhenTriggered && isWalking &&
            mainCamera.transform.eulerAngles.x >= thresholdAngle &&
                mainCamera.transform.eulerAngles.x >= RIGHT_ANGLE) {
            isWalking = false;
        }

        if (isWalking) {
            pAnim.Move();
            GetComponent<Character>().isRunning = true;
            Vector3 direction = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z).normalized * speed * Time.fixedDeltaTime;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, -transform.rotation.eulerAngles.y, 0));
            transform.Translate(rotation * direction);
        } else {
            pAnim.Idle();
            GetComponent<Character>().isRunning = false;
        }

        if (freezeYPosition) {
            transform.position = new Vector3(transform.position.x, yOffset, transform.position.z);
        }

        if(walkWhenTapped) {
            foreach(Touch touch in Input.touches) {
                //if (touch.phase == TouchPhase.Began)  {
                    //isWalking = !isWalking;
                    //Debug.Log("Screen tapped 2nd");
                //}
                if(touch.phase == TouchPhase.Ended) {
                    isWalking = !isWalking;
                    Debug.Log("Screen held");
                }
            }
        }

        //if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && walkWhenTapped) {
        //    isWalking = !isWalking;
            //Debug.Log("Screen tapped");
        //}

        if (Input.GetMouseButtonDown(0) && walkWhenTapped) {
            isWalking = !isWalking;
            Debug.Log("Mouse button clicked");
        }
    }

}