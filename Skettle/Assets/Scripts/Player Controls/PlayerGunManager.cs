using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerGunManager : GameplayComponent
{
    [System.Serializable]
    public struct GunSelection
    {
        [SerializeField] public bool avaliable;
        [SerializeField] public Gun gun;
    }

    public List<GunSelection> avaliableGuns = new List<GunSelection>();

    int totalGuns;
    [SerializeField] int currentGun_index = 0; 

    public ShootingController shootController;
    public TextMeshProUGUI ammoCounter;

    GameManager manager;

    void Awake()
    {
        manager = GetComponent<GameManager>();

        totalGuns = avaliableGuns.Count;
        shootController = GetComponent<ShootingController>();

        shootController.gun_gun.gameObject.SetActive(false);
        shootController.gun_gun = avaliableGuns[currentGun_index].gun;
        shootController.obj_gun = shootController.gun_gun.gameObject;
        shootController.gun_gun.gameObject.SetActive(true);

        shootController.SetGunEnabled(CheckForGun());

    }

    // Update is called once per frame
    public override void GameplayUpdate()
    {
        if (!shootController.shootingEnabled)
            return;

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentGun_index++;
            if (currentGun_index > totalGuns - 1)
            {
                currentGun_index = 0;
            }

            int exitValue = 0;
            while (!avaliableGuns[currentGun_index].avaliable)
            {
                if(exitValue > 5)
                {
                    shootController.SetGunEnabled(false);
                    return;
                }
                exitValue++;
                currentGun_index++;
                if (currentGun_index > totalGuns - 1)
                {
                    currentGun_index = 0;
                }
            }

            shootController.gun_gun.gameObject.SetActive(false);
            shootController.gun_gun = avaliableGuns[currentGun_index].gun;
            shootController.obj_gun = shootController.gun_gun.gameObject;
            shootController.gun_gun.gameObject.SetActive(true);
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currentGun_index--;
            if (currentGun_index < 0)
            {
                currentGun_index = totalGuns - 1;
            }

            int exitValue = 0;
            while (!avaliableGuns[currentGun_index].avaliable)
            {
                if (exitValue > 5)
                {
                    shootController.SetGunEnabled(false);
                    return;
                }
                exitValue++;

                currentGun_index--;
                if (currentGun_index < 0)
                {
                    currentGun_index = totalGuns - 1;
                }
            }


            shootController.gun_gun.gameObject.SetActive(false);
            shootController.gun_gun = avaliableGuns[currentGun_index].gun;
            shootController.obj_gun = shootController.gun_gun.gameObject;
            shootController.gun_gun.gameObject.SetActive(true);
        }
            
    }

    //TODO: Change this to a seperate class that manages ammo levels. Helps if two different guns share ammo type
    public void PickupAmmo(AmmoTypes type, int amount)
    {
        foreach(GunSelection gun in avaliableGuns)
        {
            if(gun.gun.ammoType == type)
            {
                gun.gun.GainAmmo(amount);
                return;
            }
        }
    }

    public void LoadPlayerSavedAmmo(int[] gunAmmoAmountsExtra, int[] gunAmmoAmountsInGun)
    {
        for (int i = 0; i < 5; i++)
        {
            avaliableGuns[i].gun.SetAmmo(gunAmmoAmountsExtra[i], gunAmmoAmountsInGun[i]);
        }
    }

    public void LoadPlayerSavedGuns(bool[] gunOwnership)
    {
        bool hasOneGun = false;

        for(int i = 0; i < 5; i++)
        {
            var gunAvailability = avaliableGuns[i];
            gunAvailability.avaliable = gunOwnership[i];
            avaliableGuns[i] = gunAvailability;
            if(gunOwnership[i])
            {
                hasOneGun = true;
            }
        }

        shootController.SetGunEnabled(hasOneGun);
    }

    private bool CheckForGun()
    {
        bool check = false;
        for (int i = 0; i < 5; i++)
        {
            if(avaliableGuns[i].avaliable)
            {
                check = true;
            }
        }

        return check;
    }

    public void Update()
    {
        if(ammoCounter != null)
            ammoCounter.text = shootController.gun_gun.currentAmmo.ToString("00") + "/" + ((shootController.gun_gun.extraAmmo == -999) ? "\u221E" : shootController.gun_gun.extraAmmo.ToString("00"));
    }
}
