using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public AmmoTypes type;
    public int amount;

    public void OnTriggerEnter2D(Collider2D other)
    {
        PlayerGunManager gunManager = other.GetComponent<PlayerGunManager>();

        if (other.tag == "Player" && gunManager != null)
        {
            other.GetComponent<PlayerGunManager>().PickupAmmo(type, amount);
            Destroy(this.gameObject, 0.2f);
        }
    }
}
