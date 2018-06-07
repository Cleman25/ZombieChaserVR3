using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
    public GameObject mainCamera;
	// Use this for initialization
	void Start () {
        mainCamera = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 rot = transform.eulerAngles;
        rot.y = mainCamera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(rot);

	}
}
