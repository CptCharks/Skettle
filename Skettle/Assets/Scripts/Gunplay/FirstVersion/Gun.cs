using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public Transform barrelPoint;
    public GameObject reloadGraphics;

    //public Ammo ammoTypeAndAmmount;
    public float f_distanceTillDestroy;

    public abstract void Reload();

    public abstract void Fire();

}
