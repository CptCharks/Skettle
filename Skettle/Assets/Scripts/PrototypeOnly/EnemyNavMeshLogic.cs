using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMeshLogic : MonoBehaviour
{
    public float speed = 2f;
    public float distanceToPlayer;
    public GameObject player;

    public Vector3 pos;

    NavMeshAgent agent;

    private void Awake()
    {
        pos = transform.position;

        player = GameObject.FindGameObjectWithTag("Player");

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            //agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);

            //Vector3 direction = (transform.position - player.transform.position).normalized;
            //pos = player.transform.position + direction * distanceToPlayer;
        }

        
    }

    public Vector3 SetPosition(Vector3 position)
    {
        Vector3 newPos;

        pos = position;
        newPos = position;

        agent.SetDestination(pos);

        return newPos;
    }

    public void UpdateSpeed(float newSpeed)
    {
        speed = newSpeed;
        agent.speed = speed;
    }

}
