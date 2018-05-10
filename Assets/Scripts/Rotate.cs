using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
    public GameObject camera;
	// Use this for initialization
	void Start () {
        camera = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 rot = transform.eulerAngles;
        rot.y = camera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(rot);

	}
}
