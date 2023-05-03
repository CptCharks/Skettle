using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class OnTouchEffects : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip onTouch;
    public AudioClip onShot;

    public Animator anim;

    public UnityEvent onTouched = new UnityEvent();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if(anim == null)
        {
            anim = GetComponentInChildren<Animator>(true);
        }
    }

    public void OnShot()
    {
        if(!audioSource.isPlaying)
            audioSource.PlayOneShot(onShot);

        anim.SetTrigger("Shot");
    }

    public void OnTouch()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(onTouch);

        anim?.SetTrigger("Touched");

        onTouched.Invoke();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        OnTouch();
    }

}
