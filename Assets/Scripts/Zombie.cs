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
    public Health health;
    public bool isStunned = false;
    public bool isDead = false;
    public bool atNode = false;
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
    public bool foundTarget = false;
    public Transform target;
    public Vector3 playerLastPos;
    public Vector3 playerPos;
    public float distanceToTarget;

    [Header("Patrol Settings:")]
    public bool isPatrolling = false;
    public float patrolSpeed = 7;

    [Header("Chase Settings:")]
    public bool isChasing = false;
    public float chaseSpeed = 7;
    public float giveUpDistance = 30;
    public float chaseDistance = 20;
    public float chaseTimer = 15;

    [Header("Search Settings:")]
    public bool isSearching = false;
    public float searchTimer = 10;
    public float searchDistance = 50;
    public float searchRadius = 50;
    public float searchWait = 3;
    public int searchRays = 10;
    public Vector3 axis = Vector3.up;
    public Transform ray;
    public RaycastHit hit;

    [Header("Attack Settings:")]
    public bool isAttacking = false;
    public float attackDistance = 2;
    public float repeatAttackTime = 1;

    void Awake() {
        myTransform = transform;

        if (!nm) {
            nm = GetComponent<NavMeshAgent>();
        }

        if(!characterController) {
            characterController = GetComponent<CharacterController>();
        }

        if(!health) {
            health = GetComponent<Health>();
        }

        if (!myLight) {
            myLight = GetComponentInChildren<Light>();
        }

        if (!zAnim) {
            zAnim = GetComponentInChildren<ZombieAnimation>();
        }
        SetState(ZombieStates.Patrol);
    }

    void Start () {
        if(!target) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            playerLastPos = target.position;
            SetPlayerPos(target);
        }
        FindNodes();
        
        if(nodes.Count > 0) {
            currentNode = Random.Range(0, nodes.Count);
        }
    }
	
	void Update () {
        FindNodes();
        Debug.DrawRay(ray.position, ray.forward, Color.red);
        distanceToTarget = (target.position - myTransform.position).magnitude;
        if (nodes.Count > 0) {
            currentPos = transform.position;
            switch (currentState) {
                case ZombieStates.Idle:
                    zAnim.Idle();
                    if (!nm.pathPending && nm.remainingDistance < 0.5f) {
                        SetState(ZombieStates.Patrol);
                    }
                    break;
                case ZombieStates.Patrol:
                    zAnim.Patrol();
                    Patrol();
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
        
    }

    public void FindNodes() {
        nodes = new List<Transform>();
        GameObject[] pnodes = GameObject.FindGameObjectsWithTag("Node");
        if(pnodes.Length > 0) {
            for (int i = 0; i < pnodes.Length; i++) {
                nodes.Add(pnodes[i].transform);
            }
        }
    }

    public void SetPlayerPos(Transform pt) {
        playerLastPos = playerPos;
        playerPos = pt.position;
    }

    #region Patrol
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

        StopPatrol();
        nm.SetDestination(nodes[currentNode].position);
        StartSearch();
        currentNode = (currentNode + 1) % nodes.Count;
    }
    #endregion

    #region Search
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

    #region Chase
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
        } else if(chaseTimer <= 0) {
            StopChase();
            StartPatrol();
        }
    }
    #endregion

    #region Attack
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

    #region GetSetState
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

    //void OnTriggerEnter (Collider other) {
    //    if(other.gameObject.CompareTag("Node")) {
    //        atNode = true;
    //    }
    //    if(other.gameObject.CompareTag("Player")) {
    //        other.gameObject.GetComponent<Health>().TakeDamage(5);
    //    }
    //}

    void OnCollisionEnter (Collision other) {
        if(other.gameObject.CompareTag("Node")) {
            atNode = true;
        }
        if(other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<Health>().TakeDamage(5);
        }
    }

    void OnDrawGizmos() {
        var nav = GetComponent<NavMeshAgent>();
        if (nav == null || nav.path == null)
            return;

        var line = this.GetComponent<LineRenderer>();
        if (line == null)
        {
            line = this.gameObject.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default")) { color = Color.yellow };
            line.startWidth = 0.5f;
            line.endWidth = 0.5f;
            line.startColor = Color.yellow;
            line.endColor = Color.yellow;
        }

        var path = nav.path;

        line.positionCount = path.corners.Length;

        for (int i = 0; i < path.corners.Length; i++)
        {
            line.SetPosition(i, path.corners[i]);
        }

    }
}
