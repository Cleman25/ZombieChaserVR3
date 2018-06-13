using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Blackhole : MonoBehaviour {
    public GameObject blackHole;
    public Rigidbody rb;
    public Vector3 direction;
    public float pullRadius;
    public float gravitationalPull = 100;
    public float minRadius;
    public float distanceMultiplier;
    public LayerMask pullLayers;
    public Collider[] colliders;
    public float gravitationalConstant = 6.671e-11f;
    public GameObject objToRotate;
    // Use this for initialization
    void Start () {
        blackHole = this.gameObject;
        rb = GetComponent<Rigidbody>();
        if(distanceMultiplier <= 0) {
            distanceMultiplier = 3;
        }
        if(pullRadius <= 0) {
            pullRadius = 20;
        }
        if(minRadius <= 0) {
            minRadius = 15;
        }

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(objToRotate) {
            objToRotate.transform.Rotate(0.5f, 0.5f, 0.5f);
        }
        colliders = Physics.OverlapSphere(blackHole.transform.position, pullRadius, pullLayers);
        foreach(Collider collider in colliders) {
            Rigidbody rbo = collider.GetComponent<Rigidbody>();
            if (!rbo)
                continue;

            direction = blackHole.transform.position - collider.transform.position;
            //gravitationalPull = gravitationalConstant * ((rb.mass * rbo.mass) * direction.sqrMagnitude);
            //gravitationalPull /= rbo.mass;
            if (direction.magnitude < minRadius)
                continue;

            Debug.Log("Collider: " + collider.name);

            float distance = direction.sqrMagnitude * distanceMultiplier + 1;
            rbo.AddForce(direction.normalized * (gravitationalPull / distance) * rbo.mass);
            //rbo.AddForce(direction.normalized * (gravitationalPull / distance) * rbo.mass * Time.fixedDeltaTime, 0);
            //rbo.AddForce(direction.normalized * gravitationalPull * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.name != "Terrain") {
        }
    }
}
