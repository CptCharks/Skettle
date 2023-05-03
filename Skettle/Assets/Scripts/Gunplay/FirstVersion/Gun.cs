using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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

    public AudioSource audioSource;
    public AudioClip gunSoundEffect;
    public Animator muzzleFlash;

    protected float audioVariance = 0.1f;
    private float audioBasePitch;

    public virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioBasePitch = audioSource.pitch;
        }
    }

    public abstract void Reload();

    public abstract void Fire();

    public virtual void GainAmmo(int amount)
    {
        if(extraAmmo != -999)
        {
            extraAmmo += amount;
            if(extraAmmo > maxExtraAmmo)
            {
                int difference = extraAmmo - maxExtraAmmo;
                extraAmmo = maxExtraAmmo;
                currentAmmo += difference;
                if(currentAmmo > gunMaxChamber)
                {
                    currentAmmo = gunMaxChamber;
                }
            }
        }
    }

    public virtual void SetAmmo(int backup, int current)
    {
        extraAmmo = backup;
        currentAmmo = current;
    }

    public virtual void PlayGunshot()
    {
        var newPitch = audioBasePitch + Random.Range(-audioVariance, audioVariance);

        if(muzzleFlash != null)
            muzzleFlash?.SetTrigger("Fire");

        if (gunSoundEffect != null && audioSource != null)
        {
            audioSource.pitch = newPitch;
            AudioFunctions.PlayClipAtPoint(gunSoundEffect, transform.position, audioSource.pitch, audioSource.volume, audioSource.outputAudioMixerGroup);
        }
    }
}
