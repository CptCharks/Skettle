using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSparkEffects : MonoBehaviour
{
    public List<ParticleSystem> sparkPoints;

    public void ActivateOnce()
    {
        foreach(ParticleSystem ps in sparkPoints)
        {
            ps.Play();
        }
    }

}
