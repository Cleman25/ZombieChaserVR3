using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCube : MonoBehaviour {
    public float speed = 10f;
    public BoxCollider bc;
    public float damage = 5f;

    void Start() {
        bc = GetComponent<BoxCollider>();
    }

    void Update() {
        //transform.Rotate(Vector3.up, speed * Time.deltaTime);
        transform.Rotate(0.5f, 0.5f, 0.5f);
    }

    void OnTriggerEnter (Collider col) {
        if(col.gameObject.CompareTag("Player")) {
            col.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}