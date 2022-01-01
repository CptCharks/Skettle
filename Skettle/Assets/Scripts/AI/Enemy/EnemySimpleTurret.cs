using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleTurret : MonoBehaviour
{
	
	[SerializeField] public Vector3 currentDirection;
	
	[SerializeField] VisionController visionController;
	[SerializeField] ShootingController shootController;
	
	[SerializeField] bool hasBeenAlerted = false;
	
	void Awake()
	{
		visionController = GetComponent<VisionController>();
		shootController = GetComponent<ShootingController>();
	}
	
	
    // Update is called once per frame
    void Update()
    {
        if(!hasBeenAlerted)
		{
			if(visionController.b_canSeePlayer)
			{
				hasBeenAlerted = true;
			}
			
			return;
		}
		
		
		ProcessShots();
		
    }
	
	[SerializeField] float timeBetweenBursts = 2f;
	[SerializeField] float timeBetweenShots = 0.2f;
	[SerializeField] float burstTimer = 0f;
	[SerializeField] float shotTimer = 0f;
	[SerializeField] int bulletsPerBurst = 5;
	[SerializeField] int bulletCounter = 0;
	
	[SerializeField] float accuracy;
	
	void ProcessShots()
	{
		if(burstTimer >= timeBetweenBursts)
		{
			if(shotTimer >= timeBetweenShots)
			{
				//Vector3 currentVisionVector = vision.go_player.transform.position - transform.position;
				//refCurrentAngle = Mathf.Abs(Vector3.Angle(currentVisionVector, turretDirection));
				
				
				Vector3 calculatedPosition = (transform.position + currentDirection * 3); //vision.go_player.transform.position;
				calculatedPosition.x += Random.Range(-accuracy, accuracy);
				calculatedPosition.y += Random.Range(-accuracy, accuracy);
				calculatedPosition.z = transform.position.z;
				
				Vector3 currentShotDir = (transform.position - calculatedPosition).normalized;
				
				shootController.UpdateGun(currentShotDir, true);
				bulletCounter++;
				shotTimer = 0f;
				
				if(bulletCounter >= bulletsPerBurst)
				{
					burstTimer = 0f;
					bulletCounter = 0;
				}
			}
			
			shotTimer += Time.deltaTime;
		
		}
		burstTimer += Time.deltaTime;
		
		//shootController.UpdateGun(currentDirection, false);
	}

}
