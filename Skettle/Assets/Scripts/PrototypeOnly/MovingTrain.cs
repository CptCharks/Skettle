using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovingTrain : GameplayComponent
{
    public Transform startLocation;
	public Transform endLocation;
	
	public float timeBetweenPasses = 4f;
	float passTimer = 0f;
	public bool chooChooing;
	[SerializeField] float speed = 1f;
	
	[SerializeField] bool repeatAutomatically;
	bool isMoving = false;
    [SerializeField] bool pingPong = false;
    public bool reversing = false;

    public UnityEvent onReachEnd = new UnityEvent();
	void Awake()
	{
		transform.SetPositionAndRotation(startLocation.position, transform.rotation);
	}

    public override void GameplayUpdate()
    {
		if(chooChooing)
		{
            if (!reversing)
            {
                isMoving = true;

                Vector3 remainingDistance = (endLocation.position - transform.position);

                transform.position += (remainingDistance.normalized * speed * Time.deltaTime);

                if (remainingDistance.magnitude <= 0.5f)
                {
                    if (!pingPong)
                    {
                        isMoving = false;
                        chooChooing = false;
                        onReachEnd.Invoke();
                    }
                    else
                    {
                        reversing = true;
                    }
                }

            }
            else
            {
                isMoving = true;

                Vector3 remainingDistance = (startLocation.position - transform.position);

                transform.position += (remainingDistance.normalized * speed * Time.deltaTime);

                if (remainingDistance.magnitude <= 0.5f)
                {
                    if (!pingPong)
                    {
                        isMoving = false;
                        chooChooing = false;
                        onReachEnd.Invoke();
                    }
                    else
                    {
                        reversing = false;
                    }
                }
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
	
    /*
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
    */
}
