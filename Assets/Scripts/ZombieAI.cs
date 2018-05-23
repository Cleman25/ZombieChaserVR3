using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class ZombieAI : MonoBehaviour {
    public Transform target;
    public Transform myTransform;
    [SerializeField][Tooltip("Current zombie state")] ZombieStates currentState;
    public ZombieStates previousState = ZombieStates.Idle;
    public Vector3 playerLastPos;
    public Vector3 playerPos;
    public Vector3 currentPos;
    public Vector3 axis = Vector3.up;
    public float distanceToTarget;

    public Transform ray;
    public int currentNode = 0;
    public List<Transform> nodes;

    public bool isPatrolling;
    public bool isSearching;
    public bool isChasing;
    public bool isAttacking;
    public bool isStunned;
    public bool isDead;
    public bool foundTarget;
    public bool atNode;

    public float patrolSpeed = 4;
    public float chaseSpeed = 5;
    public float rotationAngle = 360;
    public float rotationSpeed = 5;
    public float attackDistance = 0.5f;
    public float giveUpDistance = 5;
    public float chaseDistance = 5;
    public float repeatAttackTime = 1;
    public float rotationDamping = 6;
    public float chaseTimer = 10;
    public float searchTimer = 4;
    public float searchDistance = 20;
    public float searchTimeRedux = 0.01f;
    public float searchWait = 3;
    public int searchRays = 10;

    public NavMeshAgent nm;

    public RaycastHit hit;
    public Health h;
    void Awake() {
        myTransform = transform;
        h = GetComponent<Health>();
    }

    void Start () {
        if(!target) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            playerLastPos = target.position;
            SetPlayerPos(target);
        }
        GameObject[] pnodes = GameObject.FindGameObjectsWithTag("Node");
        if(pnodes.Length > 0) {
            for (int i = 0; i < pnodes.Length; i++) {
                nodes.Add(pnodes[i].transform);
            }
            Debug.Log("There are " + nodes.Count + " patrol node in the scene.");
        }
        nm = GetComponent<NavMeshAgent>();
        SetState(ZombieStates.Patrol);
	}
	
	void Update () {
        distanceToTarget = (target.position - myTransform.position).magnitude;
        if(distanceToTarget < chaseDistance) {
        //    if(!isChasing) {
        //        chaseTimer = 10;
        //    }
            SetState(ZombieStates.Chase);
        } else {
            SetState(ZombieStates.Search);
        }
        switch(currentState) {
            case ZombieStates.Idle:
                if(!nm.pathPending && nm.remainingDistance < 0.5f) {
                    SetState(ZombieStates.Patrol);
                }
                break;
            case ZombieStates.Patrol:
                GoToDestination();
                break;
            case ZombieStates.Search:
                searchTimer = 4;
                Search();
                break;
            case ZombieStates.Chase:
                chaseTimer = 10;
                Chase();
                break;
            case ZombieStates.Attack:
                Attack();
                break;
            case ZombieStates.Stunned:
                break;
            case ZombieStates.Dead:
                break;
        }
	}

    public void SetState(ZombieStates state) {
        previousState = currentState;
        currentState = state;
    }

    public ZombieStates GetState() {
        return currentState;
    }

    public void GoToDestination() {
        if (nodes.Count > 0) {
            //if(!foundTarget) {
            nm.destination = nodes[currentNode].position;
            //if(atNode) {
            SetState(ZombieStates.Search);
            currentNode = (currentNode + 1) % nodes.Count;
            //} else {
            //}
            //}
            //if (currentNode >= nodes.Count) {
            //     currentNode = 0;
            //} else {
            //    currentNode++;
            //}
        }
    }

    public void Chase() {
        chaseSpeed = 5;
        if (chaseTimer > 0) {
            chaseTimer -= Time.deltaTime;
            isChasing = true;
            myTransform.LookAt(target);
            myTransform.position += myTransform.forward * chaseSpeed * Time.deltaTime;
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime * rotationDamping);
            if(distanceToTarget < attackDistance) {
                chaseTimer = 10;
                //SetState(ZombieStates.Attack);
            } else {
                chaseTimer = 10;
                SetState(ZombieStates.Patrol);
            }
        }
    }

    public void StopChase() {
        isChasing = false;
        chaseSpeed = 5;
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Node")) {
            atNode = true;
        }
    }

    public void Search() {
        myTransform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        Debug.DrawRay(myTransform.position, myTransform.forward, Color.red);
        if (Physics.Raycast(ray.position, ray.forward, out hit, searchDistance)) {
            Debug.Log(hit.collider.name);
            if(hit.collider.CompareTag("Player")) {
                foundTarget = true;
                SetState(ZombieStates.Chase);
            } else {
                SetState(ZombieStates.Patrol);
            }
        }
    }

    public void SetPlayerPos(Transform pt) {
        playerLastPos = playerPos;
        playerPos = pt.position;
    }

    public void Attack() {

    }

    void OnDestroy() {
        SetState(ZombieStates.Dead);
    }
}
