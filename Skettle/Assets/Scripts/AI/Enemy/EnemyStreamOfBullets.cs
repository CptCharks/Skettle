using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStreamOfBullets : MonoBehaviour
{

    public ShootingController shootingController;

    //test variable
    public bool b_isShooting;

    public float f_fireRate;

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
        switch (state)
        {
            case TurretAIState.Idle:
                b_isShooting = false;
                break;
            case TurretAIState.Shooting:
                if (!b_isShooting)
                {
                    b_isShooting = true;
                    StartCoroutine(ShootEnumerator());
                }
                break;
    }
    }

    IEnumerator ShootEnumerator()
    {
        while (b_isShooting)
        {
            shootingController.UpdateGun(-turretDirection, true);
            yield return new WaitForSeconds(f_fireRate);
        }
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 leftSide = turretDirection;

        float distance = 5f;

        leftSide = Quaternion.AngleAxis(visionConeAngle, transform.forward) * leftSide;
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

        leftSide = Quaternion.AngleAxis(-(0.2f * visionConeAngle), transform.forward) * leftSide;
        Gizmos.DrawRay(transform.position, leftSide.normalized * distance);

        //leftSide = Quaternion.AngleAxis(-(2f * visionConeAngle), transform.forward) * leftSide;
        //Gizmos.DrawRay(transform.position, leftSide.normalized * 5f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, turretDirection.normalized * distance);
    }
}
