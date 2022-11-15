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


    void Awake()
    {
        totalGuns = avaliableGuns.Count;
        shootController = GetComponent<ShootingController>();


        shootController.gun_gun.gameObject.SetActive(false);
        shootController.gun_gun = avaliableGuns[currentGun_index].gun;
        shootController.obj_gun = shootController.gun_gun.gameObject;
        shootController.gun_gun.gameObject.SetActive(true);
    }

    // Update is called once per frame
    public override void GameplayUpdate()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentGun_index++;
            if(currentGun_index > totalGuns - 1)
            {
                currentGun_index = 0;
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

    public void Update()
    {
        if(ammoCounter != null)
            ammoCounter.text = shootController.gun_gun.currentAmmo.ToString("00") + "/" + ((shootController.gun_gun.extraAmmo == -999) ? "\u221E" : shootController.gun_gun.extraAmmo.ToString("00"));
    }
}
