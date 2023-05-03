using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic_Grenade : EnemyLogic
{
    float throwPower = 50f;
    BigGrenadeGun grenadeGun;

    Vector3 startSpot;

    private void Awake()
    {
        base.Awake();
        currentSpeed = walkSpeed;
        startSpot = transform.position;
        grenadeGun = GetComponentInChildren<BigGrenadeGun>();
    }

    public void ToIdle()
    {
        gameState = EnemyStates.Idle;
    }

    public override void Idle()
    {
        base.Idle();

        MillAboutShift();


        if (vision.b_canSeePlayer)
        {
            ToAttack();
        }
    }

    [SerializeField] float searchTimer = 3f;
    [SerializeField] float searchT = 0f;

    bool nextToLastKnown;
    bool goingBack;

    public void ToSearch()
    {
        lastKnownPos = playerRef.transform.position;
        searchT = 0f;
        nextToLastKnown = false;
        goingBack = false;

        gameState = EnemyStates.Searching;
    }

    public override void Searching()
    {
        base.Searching();

        /* Behavior */

        if (!nextToLastKnown && !goingBack)
        {
            UpdateSpeed(walkSpeed);

            SetPosition(lastKnownPos);
            

            if(Mathf.Abs((lastKnownPos - transform.position).magnitude) < 0.5f)
            {
                nextToLastKnown = true;
            }

        }
        else if(nextToLastKnown && !goingBack)
        {
            MillAboutShift();
            searchT += Time.deltaTime;
            if (searchT > searchTimer)
            {
                goingBack = true;
            }
        }
        else
        {
            UpdateSpeed(walkSpeed);

            SetPosition(startSpot);

            if(Mathf.Abs((startSpot - transform.position).magnitude) < 0.02f)
            {
                ToIdle();
            }
        }

        /* State Checks and Transitions */

        if (vision.b_canSeePlayer)
        {
            ToAttack();
        }
    }

    public void ToAttack()
    {
        gameState = EnemyStates.Attacking;
    }

    public override void Attacking()
    {
        base.Attacking();

        MillAboutShift();

        /* Behavior */
        GunplayCycle();


        /* State Checks and Transitions */
        if(!vision.b_canSeePlayer)
        {
            ToSearch();
        }

        if(distanceToPlayer < desiredDistanceToPlayer)
        {
            float difference = (desiredDistanceToPlayer - distanceToPlayer) +0.5f;

            SetPosition(transform.position + (playerRef.transform.position - transform.position).normalized*difference);
        }

        if(gunRange < distanceToPlayer)
        {



        }


        //See player? Attack!
    }


    int shotCounter = 0;
    int numberTillAccurate = 6;
    float shotTimer = 0f;
    float timeBetweenShots = 1.5f;

    private void GunplayCycle()
    {
        bool fire = false;
        Vector3 distanceToPlayer = (transform.position - playerRef.transform.position);

        throwPower = Mathf.Clamp(distanceToPlayer.magnitude*20f, 10f, 80f);

        grenadeGun.throwForce = throwPower;

        Vector3 aim = distanceToPlayer.normalized;

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
}
