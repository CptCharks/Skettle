using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeath : MonoBehaviour
{
    public void SpawnParticleOnOrigin(GameObject particlePrefab)
    {

        var particle = Instantiate(particlePrefab, this.transform.position, this.transform.rotation, null);

        Destroy(particle,5f);
    }

    public void DestroyParent()
    {
        Destroy(this.gameObject);
    }

}
