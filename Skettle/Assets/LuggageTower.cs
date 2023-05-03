using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuggageTower : MonoBehaviour
{
    public Transform[] hideSpotsLR;
    public Transform pushSpot;
    public Transform[] luggageFallSpots;

    Hittable hitmanager;
    Collider2D towerCollider;
    Animator anim;

    public GameObject luggageAttack_pf;

    float timeTillLuggageGain = 3f;
    float secondsBetweenLuggage = 1f;
    int maxLuggage = 3;
    int luggage = 0;

    public bool canPush = true;

    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hitmanager = GetComponent<Hittable>();
        towerCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    public void PushTower()
    {
        canPush = false;
        DisableCollisions();

        //Start timer till luggage acrual from the ceiling
        StartCoroutine(LuggageAttack());
    }

    IEnumerator LuggageAttack()
    {
        //Turn the tower of luggage into a bunch of falling luggage

        //Animate the tower falling appart (upward)
        anim.SetTrigger("Push");

        yield return new WaitForSeconds(0.5f);

        Vector3 currentTargetPos = luggageFallSpots[0].position;
        float maxTurnAngle = 10f;
        Vector3 currentAngle = currentTargetPos - transform.position;
        Vector2 variableDistanceMarch = new Vector2(0.5f, 2f);

        foreach (Transform t in luggageFallSpots)
        {
            var attack = Instantiate(luggageAttack_pf, currentTargetPos, t.rotation, null);

            currentTargetPos += Vector3.RotateTowards(currentAngle, player.transform.position - t.position, maxTurnAngle, 1f).normalized * Random.Range(variableDistanceMarch.x,variableDistanceMarch.y);

        }

        /*foreach (Transform t in luggageFallSpots)
        {
            var attack = Instantiate(luggageAttack_pf, t.position, t.rotation, null);
        }*/
    }

    void CanPush()
    {
        canPush = true;
    }

    void DisableCollisions()
    {
        towerCollider.enabled = false;
    }

    void EnableCollisions()
    {
        towerCollider.enabled = true;
    }


}
