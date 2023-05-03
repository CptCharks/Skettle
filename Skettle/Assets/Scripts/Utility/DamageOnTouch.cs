using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{

    public bool damageActive = true;
    public int damageAmount = 1;
    public bool instaKill = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        var hitController = other.gameObject.GetComponentInChildren<Hittable>();
        if (hitController == null)
        {
            return;
        }

        if (!instaKill)
        {
            hitController.Hit(damageAmount);
        }
        else
        {
            hitController.onBreak.Invoke();
        }
    }

    //I am worried this will cause enemies that touch damage locations to be hit twice. Granted, that's fine if it's only the enemies

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var hitController = collision.gameObject.GetComponentInChildren<Hittable>();
        if (hitController == null)
        {
            return;
        }

        if (collision.gameObject.tag != "Player")
        {
            if (!instaKill)
            {
                hitController.Hit(damageAmount);
            }
            else
            {
                hitController.onBreak.Invoke();
            }
        }
    }
}
