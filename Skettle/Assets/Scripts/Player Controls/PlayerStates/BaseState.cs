using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerState : ScriptableObject
{
    public virtual void Process(Rigidbody2D parentCharacter, Vector2 inputDir, float speed, bool roll, bool crouch, bool hit, bool stun)
    {

    }

    public virtual void ProcessAnimations(Animator anim)
    {

    }
}
