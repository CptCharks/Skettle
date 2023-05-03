using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Grenade : MonoBehaviour
{
    public enum BulletAlligence
    {
        Friendly,
        Dangerous,
        Neutral,
        Wall
    }


    public BulletAlligence alligence;
    public int damage;
    //public float speed;
    public float f_timeTillBlow = 6f;
    public float f_timeTillWarning = 5f;
    private float f_currentTime = 0f;

    public GameObject pf_explosion;

    [SerializeField] Animator anim;

    public AudioClip explosionClip;
    private AudioSource audioSource;
    private float audioVariance = 0.1f;
    private float audioBase;

    bool safetyLock = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioBase = audioSource.pitch;
    }

    public void Update()
    {
        f_currentTime += Time.deltaTime;

        if ((f_currentTime >= f_timeTillBlow) && !safetyLock)
        {
            anim.SetTrigger("Blow");

            Instantiate(pf_explosion, transform.position, transform.rotation, null);

            PlayExplosion();

            Destroy(gameObject);

            safetyLock = true;
        }
        else if(f_currentTime >= f_timeTillWarning)
        {
            anim.SetTrigger("Warn");
        }
    }

    public void PlayExplosion()
    {
        var newPitch = audioBase + Random.Range(-audioVariance,audioVariance);

        audioSource.pitch = audioBase;
        //AudioFunctions.PlayClipAtPoint(explosionClip, this.transform.position, audioSource.volume, audioSource.outputAudioMixerGroup);
    }
}
