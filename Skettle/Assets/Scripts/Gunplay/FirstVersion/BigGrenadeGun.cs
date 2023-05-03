using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGrenadeGun : Gun
{

    [SerializeField] public GameObject pf_grenade;
    [SerializeField] public float throwForce = 50f;

    public override void Awake()
    {
        base.Awake();
        //Give player some ammo to start
        currentAmmo = gunMaxChamber;
        f_nextTime = f_betweenShots;
    }


    public override void Fire()
    {

        if ((f_nextTime <= 0f) && (!b_isReloading && currentAmmo > 0))
        {
            var bomb = Instantiate(pf_grenade, barrelPoint.position, barrelPoint.rotation, null);

            bomb.GetComponent<Rigidbody2D>().AddForce(barrelPoint.right * throwForce);
            bomb.GetComponent<Rigidbody2D>().AddTorque(5f);

            currentAmmo -= 1;

            f_nextTime = f_betweenShots;

            PlayGunshot();
        }
        else if (!b_isReloading && currentAmmo <= 0)
        {
            Reload();
        }
        else
        {
            //Error noise?
        }
        
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

        while (reloadTime < f_reloadSpeed)
        {
            //maybe play a sound here
            reloadTime = f_reloadSpeed;
            yield return new WaitForSeconds(f_reloadSpeed);
        }

        if (extraAmmo > 0)
        {
            if (extraAmmo - gunMaxChamber < 0)
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
