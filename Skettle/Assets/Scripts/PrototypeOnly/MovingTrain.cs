using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrain : MonoBehaviour
{
    public Transform startLocation;
	public Transform endLocation;
	
	public float timeBetweenPasses = 4f;
	float passTimer = 0f;
	public bool chooChooing = false;
	[SerializeField] float speed = 1f;
	
	[SerializeField] bool repeatAutomatically;
	bool isMoving = false;
	
	void Awake()
	{
		transform.SetPositionAndRotation(startLocation.position, transform.rotation);
	}
	
	void Update()
	{
		if(chooChooing)
		{
			isMoving = true;
			
			Vector3 remainingDistance = (endLocation.position - transform.position);
			
			transform.position += (remainingDistance.normalized*speed*Time.deltaTime);
			
			if(remainingDistance.magnitude <= 0.5f)
			{
				chooChooing = false;
				isMoving = false;
			}
		}
		
		if(!chooChooing && repeatAutomatically)
		{
			passTimer += Time.deltaTime;
			if(passTimer >= timeBetweenPasses)
			{
				transform.SetPositionAndRotation(startLocation.position, transform.rotation);
				chooChooing = true;
				passTimer = 0f;
			}
		}
	}
	
	public void Trigger()
	{
		chooChooing = true;
	}
	
	public void OnTriggerEnter2D(Collider2D other)
	{
		if(!isMoving)
			return;
		
		var hitController = other.gameObject.GetComponentInChildren<Hittable>();
		if(hitController == null)
		{
			return;
		}
		
	    if (other.tag == "Player")
		{
			
			hitController.Hit();
		}
		else
		{
				hitController.onBreak.Invoke();
		}
	}
}
