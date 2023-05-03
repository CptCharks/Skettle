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

    float startingHealth;

    public UnityEvent onHit;
    public UnityEvent onBreak;
    public UnityEvent onDamaged;
    public UnityEvent onHealed;

    private void Start()
    {
        startingHealth = tempHit;
    }

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
        onDamaged.Invoke();

        if (tempHit <= 0f)
        {
            onBreak.Invoke();
        }
    }

    public void Hit(int damage)
    {
        onHit.Invoke();

        if (b_indestructable)
            return;

        if (b_instantlyBreak)
        {
            onBreak.Invoke();
        }



        //Remove this temp hit points section once health controller fixed
        tempHit -= damage;
        onDamaged.Invoke();

        if (tempHit <= 0f)
        {
            onBreak.Invoke();
        }
    }

    public void TriggerInvul(float time)
    {
        StartCoroutine(InvulTimer(time));
    }

    public IEnumerator InvulTimer(float time)
    {
        b_indestructable = true;

        yield return new WaitForSeconds(time);

        b_indestructable = false;
    }


    public void Heal()
    {
        tempHit++;

        onHealed.Invoke();
    }

    public void Heal(int amount)
    {
        tempHit += amount;

        onHealed.Invoke();
    }

    public void ResetHealth()
    {
        tempHit = startingHealth;
    }

    public void ChangeStartingHealth(float newHealth)
    {
        startingHealth = newHealth;
    }
}
