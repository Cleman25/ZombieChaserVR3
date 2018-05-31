using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimation : MonoBehaviour {
    public Animator animator;
	// Use this for initialization
	void Start () {
        if(!animator) {
            animator = GetComponent<Animator>();
        }
	}

    public void Attack() {
        animator.SetTrigger("Attacking");
        //animator.Play("Attack");
    }

    public void Patrol() {
        animator.SetTrigger("Patrolling");
        //animator.Play("Attack");
    }

    public void Idle() {
        animator.SetTrigger("Idling");
        //animator.Play("Idle");
    }

    public void Search() {
        animator.SetTrigger("Searching");
        //animator.Play("Search");
    }

    public void Chase() {
        animator.SetTrigger("Chasing");
        //animator.Play("Chase");
    }
}
