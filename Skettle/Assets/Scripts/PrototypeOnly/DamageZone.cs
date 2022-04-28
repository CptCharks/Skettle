using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public bool instaKill = false;
    int damage = 1;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Hittable hit = collision.GetComponent<Hittable>();
        if (hit != null)
        {
            if (instaKill)
            {
                hit.onBreak.Invoke();
            }

            hit.Hit();
        }
    }

}
