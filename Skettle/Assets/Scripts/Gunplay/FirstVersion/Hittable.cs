using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hittable : MonoBehaviour
{
    public Bullet.BulletAlligence ba_colliderAlligence = Bullet.BulletAlligence.Neutral;

    public bool b_instantlyBreak;
    //[SerializeField] private HealthController health;
    public bool b_indestructable;

    public float tempHit = 3f;

    public UnityEvent onHit;
    public UnityEvent onBreak;

    public void Hit()
    {
        onHit.Invoke();

        if (b_indestructable)
            return;

        if(b_instantlyBreak)
        {
            onBreak.Invoke();
        }

        

        //Remove this temp hit points section once health controller fixed
        tempHit -= 1f;
        if(tempHit <= 0f)
        {
            onBreak.Invoke();
        }
    }

}
