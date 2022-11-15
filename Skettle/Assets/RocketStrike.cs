using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketStrike : MonoBehaviour
{
    public GameObject explosion_pf;

    public void RocketStruck()
    {
        var boom = Instantiate(explosion_pf, transform.position, transform.rotation);

        Destroy(gameObject, 1.1f);
    }
}
