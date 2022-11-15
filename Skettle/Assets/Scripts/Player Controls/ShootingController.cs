using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : GameplayComponent
{
    public GameObject obj_player;
    public GameObject obj_gun;
    public Gun gun_gun;
    Sprite gunSprite;

    public bool b_isPlayer;

    public bool shootingEnabled = true;

    public Vector3 mouseDirection;
    public Vector3 mousePos;

    private void Start()
    {
        mouseDirection = new Vector3();

        SetGunEnabled(shootingEnabled);

        GetGunSprite();
    }

    private void GetGunSprite()
    {
        gunSprite = obj_gun.GetComponent<Sprite>();
    }

    public override void GameplayUpdate()
    {
        if (!b_isPlayer)
            return;

        if (!shootingEnabled)
            return;


        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = obj_gun.transform.position.z;

        Vector3 mouseDir = (obj_gun.transform.position - mousePos).normalized;

        mouseDirection = mouseDir;

        UpdateGun(mouseDir,Input.GetMouseButton(0));
    }

    public void UpdateGun(Vector3 pointDirection, bool fireDown)
    {

        obj_gun.transform.right = -pointDirection;

        //Debug.Log(gun.transform.eulerAngles.z);

        if((obj_gun.transform.eulerAngles.z > 90) && (obj_gun.transform.eulerAngles.z < 270))
        {
            obj_gun.transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            obj_gun.transform.localScale = new Vector3(1, 1, 1);
        }

        if((obj_gun.transform.eulerAngles.z > 5) && (obj_gun.transform.eulerAngles.z < 175))
        {
            obj_gun.transform.localPosition = new Vector3(0, 0, 0.05f);
        }
        else
        {
            obj_gun.transform.localPosition = new Vector3(0,0,-0.05f);
        }

        //Pass fireDown Variable to selected weapon. Weapon will determine fire rate or other effects
        if (fireDown)
            //Debug.LogWarning("No gun added");
            gun_gun.Fire();

    }

    public void SetGunEnabled(bool enable)
    {
        shootingEnabled = enable;

        obj_gun.SetActive(shootingEnabled);
    }
}
