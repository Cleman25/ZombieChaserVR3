using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : MonoBehaviour {
    public enum PillType {
        Health,
        Stamina,
        Speed
    }

    public float worth;

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
                    worth = 3f;
                    break;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter (Collider col) {
        if(col.gameObject.CompareTag("Player")) {

        }
    }
}
