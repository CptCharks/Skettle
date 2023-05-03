using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public float health;
    public float[] position;

    public bool[] weaponOwnership;
    public int[] extraAmmoCounts;
    public int[] currentAmmo;

    public int playerMoney;

    public PlayerData()
    {
        health = 4;

        playerMoney = 0;

        position = new float[3];

        extraAmmoCounts = new int[5];
        extraAmmoCounts[0] = -999;
        for (int i = 1; i < 5; i++)
        {
            extraAmmoCounts[i] = 0;
        }

        currentAmmo = new int[5];
        currentAmmo[0] = 6;
        for (int i = 1; i < 5; i++)
        {
            currentAmmo[i] = 0;
        }

        weaponOwnership = new bool[5];
        for (int i = 0; i < 5; i++)
        {
            weaponOwnership[i] = false;
        }
        //Uncomment this line to give the player a starting pistol
        //weaponOwnership[0] = true;

    }

    public PlayerData(PlayerController controller)
    {
        health = controller.healthController.tempHit;
        position = new float[3];
        position[0] = controller.gameObject.transform.position.x;
        position[1] = controller.gameObject.transform.position.y;
        position[2] = controller.gameObject.transform.position.z;

        //Will need to store what guns are avalaiable and ammo counts
        var gunManager = controller.GetComponent<PlayerGunManager>();

        SaveGunOwnership(gunManager.avaliableGuns);
        SaveAmmoCounts(gunManager.avaliableGuns);
    }

    public void SaveAmmoCounts(List<PlayerGunManager.GunSelection> guns)
    {
        extraAmmoCounts = new int[5];
        currentAmmo = new int[5];

        for (int i = 0; i < 5; i++)
        {
            extraAmmoCounts[i] = guns[i].gun.extraAmmo;
            currentAmmo[i] = guns[i].gun.currentAmmo;
        }
    }

    public void SaveGunOwnership(List<PlayerGunManager.GunSelection> guns)
    {
        weaponOwnership = new bool[5];

        for(int i = 0; i < 5; i++)
        {
            weaponOwnership[i] = guns[i].avaliable;
        }
    }
}
