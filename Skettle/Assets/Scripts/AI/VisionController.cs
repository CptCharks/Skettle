using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionController : MonoBehaviour
{
    [SerializeField] public GameObject go_player;
    [SerializeField] public float f_visionRange;
    [SerializeField] public bool b_canSeePlayer;

    [SerializeField] LayerMask layermask_LM;
    [SerializeField] LayerMask layermask_WH;

    //[SerializeReference] float f_testVisionRayRange = 7f;

    //TODO: Flesh out vision controller as needed

    private void Awake()
    {
        go_player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position - go_player.transform.position).magnitude <= f_visionRange)
        {
            //Works well enough. Make sure we sort layers properly. Should only have one layer full of wall and player collisions
            /*Requirements:
             - Goes through waist high objects but the player can hide behind a wall
                   -Method: Two raycasts, one normal that ignores bullet phyiscs layer and waisthigh wall layer, another for just the waist high wall. Seeing someone on either layer causes a detection. Move the player to the waisthigh layer when ducking
             - Detects where waist high and normal walls are for movement
                   -Need to improve navigation method and walking patterns/routes
             - Doesn't go through the wall but can see someone standing next to a wall
                   -Method: Make sure enemies and player have proper colliders for detection and movement. Potentially also provide different wall colliders for bullets (Bullets) and movement (WaistHigh/default) and vision (Default)
             */
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (go_player.transform.position - transform.position).normalized,f_visionRange, layermask_LM);
            RaycastHit2D waistHighCheck = Physics2D.Raycast(transform.position, (go_player.transform.position - transform.position).normalized, f_visionRange, layermask_WH);

            if (hit.transform != null)
            {
                if (hit.transform.tag == "Player")
                {
                    b_canSeePlayer = true;
                }
                else
                {
                    b_canSeePlayer = false;
                }
            }

            if (waistHighCheck.transform != null && b_canSeePlayer == false)
            {
                if (waistHighCheck.transform.tag == "Player")
                {
                    b_canSeePlayer = true;
                }
                else
                {
                    b_canSeePlayer = false;
                }
            }
        }
        else
        {
            b_canSeePlayer = false;
        }
    }

    private void OnDrawGizmos()
    {
	if(go_player != null)
	{
        Gizmos.DrawRay(transform.position, (go_player.transform.position - transform.position).normalized* f_visionRange);
	}
    }

}
