using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour {
    public Rigidbody rb;
    public CharacterController characterController;
    public Transform myTransform;
    public Vector3 currentPos;
    public NavMeshAgent nm;
    public Health h;
    public bool isStunned;
    public bool isDead;
    public bool atNode;
    public ZombieAnimation zAnim;
    
    [Header("Colour/Light Settings:")]
    public Light myLight;
    public Color patrolColor = Color.green;
    public Color stunnedColor = Color.black;
    public Color attackColor = Color.red;
    public Color chaseColor = Color.blue;
    public Color searchColor = Color.yellow;
    public Color idleColor = Color.white;

    [Header("States Settings:")]
    [SerializeField] [Tooltip("Current zombie state")]
    ZombieStates currentState;
    public ZombieStates previousState = ZombieStates.Idle;

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
    public float patrolSpeed = 7;

    [Header("Chase Settings:")]
    public bool isChasing;
    public float chaseSpeed = 7;
    public float giveUpDistance = 30;
    public float chaseDistance = 20;
    public float chaseTimer = 15;

    [Header("Search Settings:")]
    public bool isSearching;
    public float searchTimer = 10;
    public float searchDistance = 50;
    public float searchRadius = 50;
    public float searchWait = 3;
    public int searchRays = 10;
    public Vector3 axis = Vector3.up;
    public Transform ray;
    public RaycastHit hit;

    [Header("Attack Settings:")]
    public bool isAttacking;
    public float attackDistance = 2;
    public float repeatAttackTime = 1;

    void Awake() {
        myTransform = transform;
        if(!characterController)
            characterController = GetComponent<CharacterController>();

        if(!h)
            h = GetComponent<Health>();

        if (!myLight)
            myLight = GetComponentInChildren<Light>();

        if (!zAnim)
            zAnim = GetComponentInChildren<ZombieAnimation>();
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
        if(!nm)
            nm = GetComponent<NavMeshAgent>();

        currentNode = Random.Range(0, nodes.Count);
        SetState(ZombieStates.Patrol);
    }
	
	void Update () {
        Debug.DrawRay(ray.position, ray.forward, Color.red);
        distanceToTarget = (target.position - myTransform.position).magnitude;
        currentPos = transform.position;
        switch(currentState) {
            case ZombieStates.Idle:
                zAnim.Idle();
                if(!nm.pathPending && nm.remainingDistance < 0.5f) {
                    SetState(ZombieStates.Patrol);
                }
                break;
            case ZombieStates.Patrol:
                zAnim.Patrol();
                MoveRB();
                break;
            case ZombieStates.Search:
                zAnim.Search();
                Search();
                break;
            case ZombieStates.Chase:
                zAnim.Chase();
                Chase();
                break;
            case ZombieStates.Attack:
                zAnim.Attack();
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

    #region Patrol()
    public void StartPatrol() {
        isPatrolling = true;
        foundTarget = false;
        myLight.color = patrolColor;
        SetState(ZombieStates.Patrol);
    }

    public void StopPatrol() {
        isPatrolling = false;
    }

    public void Patrol() {
        if (nodes.Count <= 0)
            return;

        nm.SetDestination(nodes[currentNode].position);
        currentNode = (currentNode + 1) % nodes.Count;
        StopPatrol();
        StartSearch();
    }
    #endregion

    #region Search()
    public void StartSearch() {
        isSearching = true;
        searchTimer = 10;
        myLight.color = searchColor;
        SetState(ZombieStates.Search);
    }

    public void StopSearch() {
        isSearching = false;
        searchTimer = 0;
    }

    public void Search() {
        if(searchTimer > 0.1) {
            searchTimer -= Time.deltaTime;
            if(Physics.Raycast(ray.position, ray.forward * searchDistance, out hit, searchDistance)) {
                if(hit.collider.CompareTag("Player")) {
                    if (distanceToTarget < attackDistance) {
                        StopSearch();
                        StartAttack();
                    } else if (distanceToTarget > attackDistance && distanceToTarget < chaseDistance) {
                        StopSearch();
                        StartChase();
                    }
                }
            }
        } else if (searchTimer <= 0.1) {
            StopSearch();
            StartPatrol();
        }
    }
    #endregion

    #region Chase()
    public void StartChase() {
        foundTarget = true;
        isChasing = true;
        chaseTimer = 15;
        myLight.color = chaseColor;
        SetState(ZombieStates.Chase);
    }

    public void StopChase() {
        isChasing = false;
        chaseTimer = 0;
    }

    public void Chase() {
        if(chaseTimer > 0) {
            chaseTimer -= Time.deltaTime;
            myTransform.LookAt(target);
            nm.speed = chaseSpeed;
            nm.SetDestination(target.position);
            if (distanceToTarget <= attackDistance) {
                StopChase();
                StartAttack();
            } else if(distanceToTarget >= giveUpDistance) {
                StopChase();
                StartPatrol();
            }
        } else {
            StopChase();
            StartPatrol();
        }
    }
    #endregion

    #region Attack()
    public void StartAttack() {
        foundTarget = true;
        isAttacking = true;
        myLight.color = attackColor;
        SetState(ZombieStates.Attack);
    }

    public void StopAttack() {
        isAttacking = false;
    }

    public void Attack() {
        myTransform.LookAt(target);
        if(distanceToTarget > attackDistance && distanceToTarget < chaseDistance) {
            StopAttack();
            StartChase();
        } else if(distanceToTarget > chaseDistance) {
            StopAttack();
            StartSearch();
        }
    }
    #endregion

    #region GetSetState()
    public void SetState(ZombieStates state) {
        previousState = currentState;
        currentState = state;
    }

    public ZombieStates GetState() {
        return currentState;
    }
    #endregion

    public void Stunned() {

    }

    void OnDestroy() {
        SetState(ZombieStates.Dead);
    }

    void OnTriggerEnter (Collider other) {
        if(other.gameObject.CompareTag("Node")) {
            atNode = true;
        }
        if(other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<Health>().TakeDamage(5);
        }
    }

    void OnCollisionEnter (Collision other) {
        if(other.gameObject.CompareTag("Node")) {
            atNode = true;
        }
        if(other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<Health>().TakeDamage(5);
        }
    }

    public void MoveRB() {
        nm.updatePosition = false;
        nm.updateRotation = false;
        nm.nextPosition = nodes[currentNode].position;
        currentNode = (currentNode + 1) % nodes.Count;
        Vector3 velocity = nm.desiredVelocity;
        rb.velocity = velocity;
    }

    void OnDrawGizmos() {
        var nav = GetComponent<NavMeshAgent>();
        if (nav == null || nav.path == null)
            return;

        var line = this.GetComponent<LineRenderer>();
        if (line == null) {
            line = this.gameObject.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default")) { color = Color.yellow };
            line.startWidth = 0.5f;
            line.endWidth = 0.5f;
            line.startColor = Color.yellow;
            line.endColor = Color.yellow;
        }

        var path = nav.path;

        line.positionCount =  path.corners.Length;

        for (int i = 0; i < path.corners.Length; i++) {
            line.SetPosition(i, path.corners[i]);
        }

    }
}
