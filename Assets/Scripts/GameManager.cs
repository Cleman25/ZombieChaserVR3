using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public float coins;
    public float wallet;
    static GameManager _instance = null;
	// Use this for initialization
	void Awake () {
        if(instance) {
            DestroyImmediate(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
        coins = 0;
        wallet = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static GameManager instance {
        get { return _instance; }
        set { _instance = value; }
    }
}
