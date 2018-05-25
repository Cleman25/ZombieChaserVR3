using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour {
    public CharacterController characterController;
    public Transform myTransform;
    public Vector3 currentPos;
    public NavMeshAgent nm;
    public Animator animator;
    public Health h;
    public bool isStunned;
    public bool isDead;
    public bool atNode;

    public Light light;

    [Header("States Settings:")]
    [SerializeField] [Tooltip("Current zombie state")]
    ZombieStates currentState;
    public ZombieStates previousState = ZombieStates.Idle;
    public delegate void ZombieStatesDelegate();
    public event ZombieStatesDelegate currentZombieState;

    [Header("Node Settings:")]
    public int currentNode = 0;
    public List<Transform> nodes;

    [Header("Target Settings:")]
    public bool foundTarget;
    public Transform target;
    public Vector3 playerLastPos;
    public Vector3 playerPos;
    public float distanceToTarget;

    [Header("Patrol Settings:")]
    public bool isPatrolling;
    public float patrolSpeed = 4;

    [Header("Rotation Settings:")]
    public float rotationAngle = 360;
    public float rotationSpeed = 10;
    public float rotationDamping = 6;

    [Header("Chase Settings:")]
    public bool isChasing;
    public float chaseSpeed = 7;
    public float giveUpDistance = 30;
    public float chaseDistance = 20;
    public float chaseTimer = 10;

    [Header("Search Settings:")]
    public bool isSearching;
    public float searchTimer = 4;
    public float searchDistance = 50;
    public float searchRadius = 50;
    public float searchWait = 3;
    public int searchRays = 10;
    public Vector3 axis = Vector3.up;
    public Transform ray;
    public RaycastHit hit;

    [Header("Attack Settings:")]
    public bool isAttacking;
    public float attackDistance = 0.5f;
    public float repeatAttackTime = 1;

    void Awake() {
        myTransform = transform;
        if(!characterController)
            characterController = GetComponent<CharacterController>();

        if(!h)
            h = GetComponent<Health>();

        //if (!animator)
            //animator = GetComponent<Animator>();

        if (!light)
            light = GetComponentInChildren<Light>();

        //animator.SetBool("Idling", true);
    }

    void Start () {
        //if(!target) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            playerLastPos = target.position;
            SetPlayerPos(target);
        //}
        GameObject[] pnodes = GameObject.FindGameObjectsWithTag("Node");
        if(pnodes.Length > 0) {
            for (int i = 0; i < pnodes.Length; i++) {
                nodes.Add(pnodes[i].transform);
            }
            Debug.Log("There are " + nodes.Count + " patrol node in the scene.");
        }
        if(!nm)
            nm = GetComponent<NavMeshAgent>();

        currentNode = Random.Range(0, nodes.Count);
        currentZombieState = Patrol;
        SetState(ZombieStates.Patrol);
    }
	
	// Update is called once per frame
	void Update () {
        distanceToTarget = (target.position - myTransform.position).magnitude;
        currentPos = transform.position;
        switch(currentState) {
            case ZombieStates.Idle:
                if(!nm.pathPending && nm.remainingDistance < 0.5f) {
                    SetState(ZombieStates.Patrol);
                }
                break;
            case ZombieStates.Patrol:
                Patrol();
                break;
            case ZombieStates.Search:
                Search();
                //StartSearch();
                break;
            case ZombieStates.Chase:
                Chase();
                //StartChase();
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

    public void SetPlayerPos(Transform pt) {
        playerLastPos = playerPos;
        playerPos = pt.position;
    }

    public void Attack() {
        light.color = Color.red;
        //animator.Play("Attack");
    }

    public void StopSearch() {
        isSearching = false;
        //animator.SetBool("Searching", isSearching);
        searchTimer = 0;
    }

    public void StartSearch() {
        isSearching = true;
        //animator.SetBool("Searching", isSearching);
        searchTimer = 10;
        //animator.SetTrigger("Searching");
        //animator.Play("Search");
        light.color = Color.yellow;
        //currentZombieState = Search;
        //SetState(ZombieStates.Search);
    }

    public void Search() {
        if(searchTimer > 0) {
            searchTimer -= Time.deltaTime;
            //myTransform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
            Debug.DrawRay(ray.position, ray.forward, Color.red);
            if(Physics.SphereCast(ray.position + characterController.center, searchRadius, ray.forward, out hit, searchDistance)) {
                Debug.Log("Hit: " + hit.collider.name);
                if(hit.collider.CompareTag("Player")) {
                    foundTarget = true;
                    StopSearch();
                    StartChase();
                    SetState(ZombieStates.Chase);
                }
            }
        } else {
            StopSearch();
            foundTarget = false;
            isPatrolling = true;
            light.color = Color.green;
            //currentZombieState = Patrol;
            SetState(ZombieStates.Patrol);
        }
    }

    public void StopChase() {
        isChasing = false;
        foundTarget = false;
        chaseTimer = 0;
    }

    public void StartChase() {
        isChasing = true;
        chaseTimer = 10;
        light.color = Color.blue;
        //animator.SetTrigger("Chasing");
        //currentZombieState = Chase;
    }

    public void Chase() {
        if(chaseTimer > 0) {
            chaseTimer -= Time.deltaTime;
            myTransform.LookAt(target);
            //animator.SetTrigger("Patrolling");
            //animator.Play("Patrolling");
            myTransform.position += myTransform.forward * chaseSpeed * Time.deltaTime;
            //myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
            if (distanceToTarget <= attackDistance) {
                //StopChase();
                //isAttacking = true;
                //animator.SetBool("Attacking", isAttacking);
                //currentZombieState = Attack;
                //SetState(ZombieStates.Attack);
            } else if(distanceToTarget >= giveUpDistance) {
                StopChase();
                isPatrolling = true;
                //currentZombieState = Patrol;
                light.color = Color.green;
                SetState(ZombieStates.Patrol);
            }
        } else {
            StopChase();
            isPatrolling = true;
            //currentZombieState = Patrol
            light.color = Color.green; ;
            SetState(ZombieStates.Patrol);
        }
    }

    public void Patrol() {
        if (nodes.Count <= 0)
            return;

        nm.destination = nodes[currentNode].position;
        if(distanceToTarget < attackDistance) {
            //isPatrolling = false;
            //isAttacking = false;
            //animator.SetBool("Attacking", isAttacking);
            //currentZombieState = Attack;
            //SetState(ZombieStates.Attack);
        }
        else if(distanceToTarget < chaseDistance) {
            isPatrolling = false;
            //currentZombieState = StartChase;
            StartChase();
            SetState(ZombieStates.Chase);
        } else {
            isPatrolling = false;
            //currentZombieState = StartSearch;
            StartSearch();
            SetState(ZombieStates.Search);
        }
        currentNode = (currentNode + 1) % nodes.Count;
    }

    public void Stunned() {

    }

    void OnDestroy() {
        SetState(ZombieStates.Dead);
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Node")) {
            atNode = true;
        }
    }

    public void SetState(ZombieStates state) {
        previousState = currentState;
        currentState = state;
    }

    public ZombieStates GetState() {
        return currentState;
    }

    public void OnDrawGizmos() {
        Gizmos.DrawSphere(ray.position, searchRadius);
    }
}
