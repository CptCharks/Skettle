using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovementState : BasePlayerState
{
    public float f_baseSpeed;

    public float f_crouchSpeed;

    float f_currentSpeed;

    public override void Process(Rigidbody2D parentCharacter, Vector2 inputDir, float speed, bool roll, bool crouch, bool hit, bool stun)
    {
        if (hit)
            //return;

        if (roll)
        {
            //timeSinceLastRoll = 0;
            //return;
        }


        if (roll )//&& (timeSinceLastRoll > timeBetweenRolls))
        {
            /*
            b_isRolling = true;
            StartCoroutine(RollProcess());

            playerAnimator.SetTrigger("Roll");

            //Start roll animation
            //Start "physics" calculations
            */
        }

        //timeSinceLastRoll += Time.deltaTime;

        if (inputDir.magnitude < 0.01 && inputDir.magnitude > -0.01)
        {
            f_currentSpeed = 0.0f;
        }
        else
        {
            if (crouch)
            {
                f_currentSpeed = f_crouchSpeed;
            }
            else
            {
                f_currentSpeed = f_baseSpeed;
            }
        }

        //Probably need to change to a rigidbody system
        parentCharacter.velocity += inputDir * f_currentSpeed * Time.deltaTime;
    }

    public override void ProcessAnimations(Animator anim)
    {


    }
}
