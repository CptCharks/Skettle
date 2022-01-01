using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideBomb : MonoBehaviour
{
	public VisionController visionController;
	public EnemyNavMeshLogic navMeshController;
	public bool targetAcquired;
	public bool visionActivated;
	
	public GameObject explosion_pf;
	
	public float timeTillExplode = 2f;
	float timerTillBoom = 0f;
	
	Vector3 explodeZone;
	bool setToGo = false;
	
	GameObject playerRef;
	
	void Awake()
	{
		navMeshController = GetComponent<EnemyNavMeshLogic>();
        visionController = GetComponent<VisionController>();
	}
	
    // Start is called before the first frame update
    void Start()
    {
		playerRef = visionController.go_player;
    }

    // Update is called once per frame
    void Update()
    {
		if(visionController.b_canSeePlayer && visionActivated)
		{
			targetAcquired = true;
		}
		
        if(targetAcquired)
		{
			timerTillBoom += Time.deltaTime;
			
			
			if((timerTillBoom >= timeTillExplode) && !setToGo)
			{
				explodeZone = ((playerRef.transform.position - transform.position)*1.1f) + playerRef.transform.position;
				navMeshController.SetPosition(explodeZone);
				setToGo = true;
			}
			else if(timerTillBoom < timeTillExplode)
			{
				navMeshController.SetPosition(playerRef.transform.position);
			}
			
			if(setToGo)
			{
				if((explodeZone - transform.position).magnitude < 0.2f)
				{
					Explode();
					Destroy(gameObject);
				}
			}
			
		}
    }
	
	public void Explode()
	{
		Instantiate(explosion_pf, transform.position, transform.rotation, transform.parent);
	}
	
}
