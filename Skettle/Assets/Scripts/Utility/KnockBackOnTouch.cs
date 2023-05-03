using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackOnTouch : MonoBehaviour
{
    public bool knockbackActive = true;
    public float knockbackAmount = 4f;
    public float knockbackSpeed = 1f;
    public float stunAmount = 0f;

    public void OnTriggerEnter2D(Collider2D other)
    {
        var hitController = other.gameObject.GetComponentInChildren<Hittable>();
        if (hitController == null)
        {
            return;
        }

        if (other.tag == "Player")
        {
            PlayerController player = hitController.GetComponentInParent<PlayerController>();
            player.OnStun(stunAmount, true, knockbackAmount, knockbackSpeed, (player.transform.position - transform.position).normalized);
        }
        else
        {
            hitController.onBreak.Invoke();
        }
    }
}
