using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawblade : MonoBehaviour
{
    public PatrolContainer currentPatrolPath;
	[SerializeField] private Transform currentTargetNode;
    [SerializeField] private Transform lastReachedNode;
    [SerializeField] private float pathMargin = 0.05f;

    [SerializeField] private int direction = 1;
    [SerializeField] private int node;
	public float speed = 1.5f;
	
	void Start()
	{
		currentTargetNode = currentPatrolPath.pathNodes[0];
	}
	
	void Update()
	{
		transform.position += (currentTargetNode.position - transform.position).normalized*speed*Time.deltaTime;
		
		ProcessPatrol();
	}
	
	void ProcessPatrol()
	{
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
	
	public void OnTriggerEnter2D(Collider2D other)
	{
		Hittable hit = other.GetComponent<Hittable>();
		if(hit != null)
		{
			hit.Hit();
		}
	}
	
	
}
