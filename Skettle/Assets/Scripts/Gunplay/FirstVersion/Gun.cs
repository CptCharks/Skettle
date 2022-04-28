using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public Transform barrelPoint;
    public GameObject reloadGraphics;

    public int damagePerBullet = 2;

    public float f_betweenShots;
    public float f_nextTime;

    public float f_reloadSpeed;
    public bool b_isReloading = false;
    public int extraAmmo = -999;
    public int maxExtraAmmo = 999; //temp max
    public int gunMaxChamber = 6;
    public int currentAmmo;

    public AmmoTypes ammoType = AmmoTypes.Pistol;

    //public Ammo ammoTypeAndAmmount;
    public float f_distanceTillDestroy;

    public abstract void Reload();

    public abstract void Fire();

    public virtual void GainAmmo(int amount)
    {
        if(extraAmmo != -999)
        {
            extraAmmo += amount;
            if(extraAmmo > maxExtraAmmo)
            {
                extraAmmo = maxExtraAmmo;
            }
        }
    }
}
