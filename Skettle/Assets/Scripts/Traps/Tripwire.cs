using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tripwire : MonoBehaviour
{
    [SerializeField] Sprite trippedWire_Sprite;
    SpriteRenderer renderer;

    public List<ExplosiveBarrel> explosiveTargets;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }


    public void TriggerWire()
    {
        renderer.sprite = trippedWire_Sprite;
        //Play a noise

        foreach(ExplosiveBarrel eb in explosiveTargets)
        {
            eb.Explode();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Enemy")
            TriggerWire();
    }
}
