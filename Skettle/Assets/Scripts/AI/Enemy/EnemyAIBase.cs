using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIBase : GameplayComponent
{
    public ShootingController shootingController;

    public VisionController vision;

    //test variable
    public bool isShooting;

    public float f_speed;

    public float f_maxAccuracy;
    public float f_maxAccurayDistance;
    public float f_currentAccuracy;


    [SerializeField] Vector3 lastKnownLocation;
    [SerializeField] Vector3 v3_move;

    //Probably should do a better state system 
    public enum AIState
    {
        Idle,
        Searching,
        Shooting
    }

    public AIState state;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void GameplayUpdate()
    {
        switch(state)
        {
            case AIState.Idle:
                //Perhaps basic mill about
                if(vision.b_canSeePlayer)
                {
                    state = AIState.Shooting;
                }
                break;
            case AIState.Searching:
                //Move into range of player
                v3_move = (lastKnownLocation - transform.position).normalized;


                transform.position += v3_move * f_speed * Time.deltaTime;

                if (vision.b_canSeePlayer)
                {
                   state = AIState.Shooting;
                }

                    break;
            case AIState.Shooting:
                //Navigate toward the player
                v3_move = (vision.go_player.transform.position - transform.position).normalized;

                transform.position += v3_move * f_speed * Time.deltaTime;

                //Shooting pattern
                if (!isShooting)
                {
                    isShooting = true;
                    StartCoroutine(ShootEnumerator());
                }

                if (!vision.b_canSeePlayer)
                {
                    state = AIState.Searching;
                    isShooting = false;
                    StopCoroutine(ShootEnumerator());

                    lastKnownLocation = new Vector3(vision.go_player.transform.position.x, vision.go_player.transform.position.y, vision.go_player.transform.position.z);
                }

                //Seems to be overriding the targeting when the enemy fires. Probably should make the timing in the Enumerator function use delta time to calculate the next shot time
                //shootingController.UpdateGun(transform.position - calculatedPosition, false);

                break;
        }
    }

    IEnumerator ShootEnumerator()
    {
        int count = 0;
        bool boom = false;

        while(isShooting)
        {
            if (!vision.b_canSeePlayer)
                isShooting = false;

            float dist = (transform.position - vision.go_player.transform.position).magnitude;

            //When f_maxAccuracy is low, fmad/d = 1
            //When dist is low

            f_currentAccuracy =  Mathf.Max(dist / f_maxAccurayDistance , f_maxAccuracy);

            Vector3 calculatedPosition = vision.go_player.transform.position;
            calculatedPosition.x += Random.Range(-f_currentAccuracy, f_currentAccuracy);
            calculatedPosition.y += Random.Range(-f_currentAccuracy, f_currentAccuracy);
            calculatedPosition.z = transform.position.z;


            shootingController.UpdateGun((transform.position - calculatedPosition).normalized, boom);

            boom = false;

            count += 1;
            if(count > 3)
            {
                boom = true;
                count = 0;
            }
            yield return new WaitForSeconds(1f);


        }
    }

}
