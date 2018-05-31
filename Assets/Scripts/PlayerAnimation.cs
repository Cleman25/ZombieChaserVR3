using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    public Animator animator;
	// Use this for initialization
	void Start () {
        if(!animator) {
            animator = GetComponent<Animator>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Move() {
        animator.SetTrigger("Move");
        //animator.Play("Move");
    }

    public void Idle() {
        animator.SetTrigger("Idle");
        //animator.Play("Idle");
    }
}
