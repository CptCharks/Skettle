using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSoundAnimSyncer : MonoBehaviour
{
    [SerializeField] UnityEvent onStep = new UnityEvent();

    public void Step()
    {
        onStep.Invoke();
    }

    public void Roll()
    {

    }

    public void Hurt()
    {

    }

    public void Drop()
    {

    }

    public void Die()
    {

    }

    public void Shoot()
    {

    }
}
