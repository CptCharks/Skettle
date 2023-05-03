using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyLogic : MonoBehaviour
{
    public float gunRange;
    public float currentSpeed;
    public float walkSpeed;
    public float runSpeed;

    public Action onDeath;

    [SerializeField] NavMeshAgent agent;
    public Vector3 pos;

    [SerializeField] public Animator anim;
    [SerializeField] public ShootingController shootingController;
    [SerializeField] public VisionController vision;

    [SerializeField] public GameObject playerRef;
    [SerializeField] public Vector3 lastKnownPos;

    Vector3 startMillPos;
    Vector3 millShift = Vector3.up;

    [SerializeField] public float distanceToPlayer;
    [SerializeField] public float desiredDistanceToPlayer;

    public enum EnemyStates
    {
        Disabled,
        Idle,
        Patroling,
        Searching,
        Attacking
    }

    public EnemyStates gameState = EnemyStates.Idle;

    public void Awake()
    {
        NavigationInitilization();

        //anim = GetComponent<Animator>();

        playerRef = GameObject.FindGameObjectWithTag("Player");
        shootingController = GetComponent<ShootingController>();
        vision = GetComponent<VisionController>();
    }

    #region Basic States
    public virtual void Update()
    {
        distanceToPlayer = Mathf.Abs((playerRef.transform.position - transform.position).magnitude);

        anim.SetInteger("State", (int)gameState);
        anim.SetFloat("Speed", agent.velocity.magnitude);

        switch (gameState)
        {
            case EnemyStates.Disabled:
                Disabled();
                break;
            case EnemyStates.Idle:
                Idle();
                break;
            case EnemyStates.Patroling:
                Patroling();
                break;
            case EnemyStates.Searching:
                Searching();
                break;
            case EnemyStates.Attacking:
                Attacking();
                break;
        }
    }

    public virtual void OnDeath()
    {
        onDeath.Invoke();

        //Basic death stuff until we make prefabs for dead bodies
        gameObject.SetActive(false);
    }

    public virtual void Disabled()
    {

    }

    public virtual void Idle()
    {

    }

    public virtual void Patroling()
    {

    }

    public virtual void Searching()
    {

    }

    public virtual void Attacking()
    {
        
    }

    #endregion

    #region Navigation
    //Navigation Functions
    public void NavigationInitilization()
    {
        pos = transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = currentSpeed;
    }

    public Vector3 SetPosition(Vector3 position)
    {
        Vector3 newPos;

        pos = position;
        newPos = position;

        if(agent.isOnNavMesh)
            agent.SetDestination(pos);

        return newPos;
    }

    public void UpdateSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
        agent.speed = currentSpeed;
    }

    [SerializeField] float millWait = 5f;
    float millTimer = 0f;
    float millDist = 0f;

    public void MillAboutShift()
    {
        millTimer += Time.deltaTime;

        if (millTimer >= millWait)
        {
            SetPosition(transform.position + millShift);

            millShift = (startMillPos - transform.position).normalized;

            millDist = UnityEngine.Random.Range(0.5f, 3);

            millShift *= millDist;

            millTimer = 0f;
        }
    }

    public void MillAboutShift(float shiftLowerRange = 0.5f, float shiftHigherRange = 3f)
    {
        millTimer += Time.deltaTime;

        if (millTimer >= millWait)
        {
            SetPosition(transform.position + millShift);

            millShift = (startMillPos - transform.position).normalized;

            millDist = UnityEngine.Random.Range(shiftLowerRange, shiftHigherRange);

            millShift *= millDist;

            millTimer = 0f;
        }
    }
    #endregion
}
