using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE: This is the old system. NOT DERRIVED FROM ENEMYLOGIC
public class EnemyLogic_Generic : GameplayComponent
{
    public float gunRange;
    public float speed;

    [SerializeField] EnemyNavMeshLogic navMesh;
    [SerializeField] ShootingController shootingController;
    [SerializeField] VisionController vision;

    [SerializeField] GameObject playerRef;
    [SerializeField] Vector3 lastKnownPos;

    Vector3 startMillPos;
    Vector3 millShift = Vector3.up;

    [SerializeField] float distanceToPlayer;
    [SerializeField] float desiredDistanceToPlayer;

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
        playerRef = GameObject.FindGameObjectWithTag("Player");
        shootingController = GetComponent<ShootingController>();
        vision = GetComponent<VisionController>();
        navMesh = GetComponent<EnemyNavMeshLogic>();

        //navMesh.UpdateSpeed(speed);

        
    }

    public void Start()
    {
        startMillPos = navMesh.SetPosition(transform.position);

        desiredDistanceToPlayer += Random.Range(-1f, 1f);
    }


    public override void GameplayUpdate()
    {
        if (!this.gameObject.activeSelf)
            return;

        distanceToPlayer = (transform.position - playerRef.transform.position).magnitude;

        switch (gameState)
        {
            case EnemyStates.Disabled:
                return;
                break;
            case EnemyStates.Idle:
                MillAboutShift();
                if (vision.b_canSeePlayer)
                {
                    gameState = EnemyStates.Attacking;
                }
                break;
            case EnemyStates.Patroling:
                if (currentPatrolPath == null)
                    return;

                if (currentTargetNode == null)
                {
                    currentTargetNode = FindClosestNodeInPath();
                }

                ProcessPatrol();

                break;
            case EnemyStates.Searching:
                if (vision.b_canSeePlayer)
                {
                    gameState = EnemyStates.Attacking;
                }


                MillAboutShift();

                break;
            case EnemyStates.Attacking:
                if (!vision.b_canSeePlayer)
                {
                    gameState = EnemyStates.Searching;
                    lastKnownPos = playerRef.transform.position;
                }


                if (distanceToPlayer > desiredDistanceToPlayer + 2)
                {
                    RunTowardsPlayer();
                }
                else
                {
                    MillAboutShift();
                }

                if (distanceToPlayer <= gunRange)
                    GunplayCycle();

                break;
        }
    }

    int shotCounter = 0;
    int numberTillAccurate = 6;
    float shotTimer = 0f;
    float timeBetweenShots = 1.5f;

    private void GunplayCycle()
    {
        bool fire = false;
        Vector3 aim = (transform.position - playerRef.transform.position).normalized;

        shotTimer += Time.deltaTime;
        if (shotTimer >= timeBetweenShots)
        {

            fire = true;
            shotCounter++;
            if (shotCounter >= numberTillAccurate)
            {
                shotCounter = 0;

                //Override aim to be more accurate
            }

            shotTimer = 0f;
        }

        shootingController.UpdateGun(aim, fire);
    }

    [SerializeField] float millWait = 5f;
    float millTimer = 0f;
    float millDist = 0f;

    private void MillAboutShift()
    {
        millTimer += Time.deltaTime;

        if (millTimer >= millWait)
        {
            navMesh.SetPosition(transform.position + millShift);

            millShift = (startMillPos - transform.position).normalized;

            millDist = Random.Range(0.5f, 3);

            millShift *= millDist;

            millTimer = 0f;
        }
    }

    [SerializeField] private PatrolContainer currentPatrolPath;
    [SerializeField] private Transform currentTargetNode;
    [SerializeField] private Transform lastReachedNode;
    [SerializeField] private float pathMargin = 0.05f;

    [SerializeField] private int direction = 1;
    [SerializeField] private int node;

    private void ProcessPatrol()
    {
        navMesh.SetPosition(currentTargetNode.position);

        if ((currentTargetNode.position - transform.position).magnitude <= pathMargin)
        {

            if (node >= currentPatrolPath.size - 1)
            {
                if (currentPatrolPath.type == PatrolContainer.PatrolType.Racetrack)
                {
                    node = -1;

                }
                else if (currentPatrolPath.type == PatrolContainer.PatrolType.BackAndForth)
                {
                    direction *= -1;
                }
            }
            else if (node <= 0 && (lastReachedNode != null) /*&& (lastReachedNode != currentTargetNode)*/)
            {
                if (currentPatrolPath.type == PatrolContainer.PatrolType.Racetrack)
                {

                }
                else if (currentPatrolPath.type == PatrolContainer.PatrolType.BackAndForth)
                {
                    direction *= -1;
                }

            }

            lastReachedNode = currentTargetNode;

            node += direction;

            currentTargetNode = currentPatrolPath.pathNodes[node];

        }
    }

    private Transform FindClosestNodeInPath()
    {
        float dist = 100f;
        Transform current = null;

        foreach (Transform tm in currentPatrolPath.pathNodes)
        {
            if ((tm.position - transform.position).magnitude < dist)
            {
                dist = (tm.position - transform.position).magnitude;
                current = tm;
                node = int.Parse(tm.gameObject.name);
            }
        }

        return current;
    }

    private PatrolContainer FindClosestPath()
    {

        return null;
    }

    public void EnableEnemy(bool activate)
    {
        if (activate)
        {
            gameState = EnemyStates.Idle;
        }
        else
        {
            gameState = EnemyStates.Disabled;
        }
    }

    [SerializeField] float timeBetweenChases = 2f;
    float chaseTimer = 0f;

    private void RunTowardsPlayer()
    {
        chaseTimer += Time.deltaTime;

        if ((chaseTimer > timeBetweenChases) || (distanceToPlayer > gunRange))
        {
            Vector3 direction = (transform.position - playerRef.transform.position).normalized;
            var pos = playerRef.transform.position + direction * desiredDistanceToPlayer;
            startMillPos = navMesh.SetPosition(pos);

            chaseTimer = 0f;
        }
    }
}
