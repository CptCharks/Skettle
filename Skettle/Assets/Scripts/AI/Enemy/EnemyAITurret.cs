using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAITurret : MonoBehaviour
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

    [SerializeField] Vector3 turretDirection;

    [SerializeField] float visionConeAngle;
    [SerializeField] float refCurrentAngle;

    public enum TurretAIState
    {
        Idle,
        Shooting
    }

    public TurretAIState state;
    public TurretAIState startState;

    // Start is called before the first frame update
    void Start()
    {
        state = startState;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentVisionVector = vision.go_player.transform.position - transform.position;
        refCurrentAngle = Mathf.Abs(Vector3.Angle(currentVisionVector, turretDirection));

        switch (state)
        {
            case TurretAIState.Idle:
                if (vision.b_canSeePlayer)
                {
                    if(refCurrentAngle <= visionConeAngle)
                    {
                        state = TurretAIState.Shooting;
                    }
                }
                break;
            case TurretAIState.Shooting:


                //Shooting pattern
                if (!isShooting)
                {
                    isShooting = true;
                    StartCoroutine(ShootEnumerator());
                }

                //V having trouble with the vision script. Tends to not see the player when below the turret
                if (!vision.b_canSeePlayer || (refCurrentAngle > visionConeAngle))
                {
                    state = TurretAIState.Idle;
                    isShooting = false;

                    lastKnownLocation = new Vector3(vision.go_player.transform.position.x, vision.go_player.transform.position.y, vision.go_player.transform.position.z);
                }
                break;
        }
    }

    IEnumerator ShootEnumerator()
    {
        int count = 0;
        int shots = 0;
        bool boom = false;

        while (isShooting)
        {
            if (!vision.b_canSeePlayer)
                isShooting = false;

            float dist = (transform.position - vision.go_player.transform.position).magnitude;

            //When f_maxAccuracy is low, fmad/d = 1
            //When dist is low

            f_currentAccuracy = Mathf.Max(dist / f_maxAccurayDistance, f_maxAccuracy);

            Vector3 calculatedPosition = vision.go_player.transform.position;
            calculatedPosition.x += Random.Range(-f_currentAccuracy, f_currentAccuracy);
            calculatedPosition.y += Random.Range(-f_currentAccuracy, f_currentAccuracy);
            calculatedPosition.z = transform.position.z;

            if (boom && (shots < 4))
            {
                shootingController.UpdateGun((transform.position - calculatedPosition).normalized, boom);
                shots++;
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            shots = 0;
            boom = false;

            count += 1;

            if (count > 3)
            {
                count = 0;
                boom = true;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 leftSide = turretDirection;

        float distance = vision.f_visionRange;

        leftSide = Quaternion.AngleAxis(visionConeAngle, transform.forward) * leftSide;
        Gizmos.DrawRay(transform.position, leftSide.normalized* distance);

        leftSide = Quaternion.AngleAxis(-(0.2f*visionConeAngle), transform.forward) * leftSide;
        Gizmos.DrawRay(transform.position, leftSide.normalized * distance);

        leftSide = Quaternion.AngleAxis(-(0.2f * visionConeAngle), transform.forward) * leftSide;
        Gizmos.DrawRay(transform.position, leftSide.normalized * distance);

        leftSide = Quaternion.AngleAxis(-(0.2f * visionConeAngle), transform.forward) * leftSide;
        Gizmos.DrawRay(transform.position, leftSide.normalized * distance);

        leftSide = Quaternion.AngleAxis(-(0.2f * visionConeAngle), transform.forward) * leftSide;
        Gizmos.DrawRay(transform.position, leftSide.normalized * distance);

        leftSide = Quaternion.AngleAxis(-(0.2f * visionConeAngle), transform.forward) * leftSide;
        Gizmos.DrawRay(transform.position, leftSide.normalized * distance);

        leftSide = Quaternion.AngleAxis(-(0.2f * visionConeAngle), transform.forward) * leftSide;
        Gizmos.DrawRay(transform.position, leftSide.normalized * distance);

        leftSide = Quaternion.AngleAxis(-(0.2f * visionConeAngle), transform.forward) * leftSide;
        Gizmos.DrawRay(transform.position, leftSide.normalized * distance);

        leftSide = Quaternion.AngleAxis(-(0.2f * visionConeAngle), transform.forward) * leftSide;
        Gizmos.DrawRay(transform.position, leftSide.normalized * distance);

        leftSide = Quaternion.AngleAxis(-(0.2f * visionConeAngle), transform.forward) * leftSide;
        Gizmos.DrawRay(transform.position, leftSide.normalized * distance);

        leftSide = Quaternion.AngleAxis(-(0.2f * visionConeAngle), transform.forward) * leftSide;
        Gizmos.DrawRay(transform.position, leftSide.normalized * distance);

        //leftSide = Quaternion.AngleAxis(-(2f * visionConeAngle), transform.forward) * leftSide;
        //Gizmos.DrawRay(transform.position, leftSide.normalized * 5f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, turretDirection.normalized*distance);
    }


}
