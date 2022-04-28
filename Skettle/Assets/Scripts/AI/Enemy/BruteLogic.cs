using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteLogic : MonoBehaviour
{

    [SerializeField] VisionController vision;
    [SerializeField] EnemyNavMeshLogic navMesh;


    public enum BruteStates
    {
        Searching,
        Reving,
        Charging,
        Recovering
    }

    [SerializeField] BruteStates currentState = BruteStates.Searching;

    public float walkSpeed;
    public float revSpeed;
    public float chargeSpeed;
    public float chargeDistance = 4;

    Vector3 chargeTarget = new Vector3();

    public float timeTillCharge = 1f;
    float tillChargeTimer = 0;

    public float timeTillRev = 0.5f;
    float tillRevTimer = 0;

    public float recoveryTime = 1.5f;
    float recoveryTimer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        navMesh = GetComponent<EnemyNavMeshLogic>();
        vision = GetComponent<VisionController>();
    }

    private void Start()
    {
        navMesh.UpdateSpeed(walkSpeed);
        FindClosestNodeInPath();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case BruteStates.Searching:
                if (vision.b_canSeePlayer)
                {
                    if (tillRevTimer > timeTillRev)
                    {
                        //Start timer for reving
                        currentState = BruteStates.Reving;
                        tillRevTimer = 0;
                        tillChargeTimer = 0;
                        break;
                    }

                    tillRevTimer += Time.deltaTime;

                    //Set this to zero for the next stage
                    
                }

                navMesh.UpdateSpeed(walkSpeed);

                ProcessPatrol();


                break;
            case BruteStates.Reving:
                navMesh.UpdateSpeed(revSpeed);
                //Start timer for charging

                if (tillChargeTimer > timeTillCharge)
                {
                    //Start timer for reving
                    chargeTarget = ((vision.go_player.transform.position - transform.position).normalized * chargeDistance) + transform.position;
                    currentState = BruteStates.Charging;
                    tillChargeTimer = 0;
                    break;
                }

                tillChargeTimer += Time.deltaTime;


                break;
            case BruteStates.Charging:

                navMesh.SetPosition(chargeTarget);
                navMesh.UpdateSpeed(chargeSpeed);
                //Check distance to make sure you don't run too far or just wait for collision trigger
                if ((transform.position - chargeTarget).magnitude <= 0.1f)
                {
                    currentState = BruteStates.Recovering;
                }

                break;
            case BruteStates.Recovering:

                //Few seconds to wait then either rev up again or resume patrol 

                break;
        }

        if(!vision.b_canSeePlayer)
        {
            currentState = BruteStates.Searching;
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        currentState = BruteStates.Recovering;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Hittable hittableObject = collision.GetComponent<Hittable>();

        if (hittableObject != null)
        {
            hittableObject.Hit();
        }
    }
}
