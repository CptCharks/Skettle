using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackHandler : MonoBehaviour
{
    public UnityEvent onKnockbacked = new UnityEvent();

    public Bullet.BulletAlligence current_alligence = Bullet.BulletAlligence.Neutral;

    public float currentStunTime;

    private void Start()
    {
        if (current_alligence != Bullet.BulletAlligence.Neutral)
            return;

        if (TryGetComponent<Hittable>(out Hittable hit))//.ba_colliderAlligence;
        {
            current_alligence = hit.ba_colliderAlligence;
        }
        else if(GetComponentInChildren<Hittable>())
        {
            current_alligence = GetComponentInChildren<Hittable>().ba_colliderAlligence;
        }

    }

    public void Knockback(Bullet.BulletAlligence alligence, Transform origin, float force)
    {
        if(current_alligence != alligence || current_alligence == Bullet.BulletAlligence.Neutral || alligence == Bullet.BulletAlligence.Neutral)
        {
            if(transform.parent.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb2d))
            {
                rb2d.AddForce((origin.position - transform.parent.position).normalized*force);
            }
            
            onKnockbacked.Invoke();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<KnockbackZone>(out KnockbackZone kz))
        {
            currentStunTime = kz.timeStunned;
            Knockback(kz.alligence,kz.transform,kz.force);
        }
    }
}
