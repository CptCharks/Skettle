using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPistol : Gun
{
    //Will probably leave the bullet prefab in the ammo type definitions
    public GameObject bullet_prefab_test;

    public void Awake()
    {
        //Give player some ammo to start
        currentAmmo = gunMaxChamber;
        f_nextTime = f_betweenShots;
    }

    public override void Fire()
    {
        if ((f_nextTime <= 0f)&&(!b_isReloading && currentAmmo > 0))
        {
            var bulletClone = GameObject.Instantiate(bullet_prefab_test, null, true);
            bulletClone.transform.SetPositionAndRotation(barrelPoint.position, barrelPoint.rotation);
            bulletClone.GetComponent<Bullet>().SetDistance(f_distanceTillDestroy);
            bulletClone.GetComponent<Bullet>().damage = damagePerBullet;
            currentAmmo -= 1;

            f_nextTime = f_betweenShots;
        }
        else if(!b_isReloading && currentAmmo <= 0)
        {
            Reload();
        }
        else
        {
            //Error noise?
        }
    }

    public void Update()
    {
        f_nextTime = Mathf.Clamp((f_nextTime -= Time.deltaTime),0f,1000f);
    }

    public override void Reload()
    {
        if (extraAmmo > 0 || extraAmmo == -999)
        {
            b_isReloading = true;
            StartCoroutine(ReloadCoroutine());
        }
        else
        {
            //error noise?
        }
    }

    public IEnumerator ReloadCoroutine()
    {
        float reloadTime = 0f;

        while(reloadTime < f_reloadSpeed)
        {
            //maybe play a sound here
            reloadTime = f_reloadSpeed;
            yield return new WaitForSeconds(f_reloadSpeed);
        }

        if (extraAmmo > 0)
        {
            if(extraAmmo - gunMaxChamber < 0)
            {
                currentAmmo = extraAmmo;
                extraAmmo = 0;
            }
            else
            {
                currentAmmo = gunMaxChamber;
                extraAmmo -= gunMaxChamber;
            }
        }
        else if (extraAmmo == -999)
        {
            currentAmmo = gunMaxChamber;
        }
        else
        {
            extraAmmo = 0;
        }

        b_isReloading = false;
    }

}
