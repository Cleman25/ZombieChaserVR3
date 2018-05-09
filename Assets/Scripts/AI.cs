using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {
    [SerializeField][Tooltip("Current zombie state")] ZombieStates currentState;
    public ZombieStates previousState;

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void SetState(ZombieStates state) {
        previousState = currentState;
        currentState = state;
    }

    public ZombieStates GetState() {
        return currentState;
    }
}
