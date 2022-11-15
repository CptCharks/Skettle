using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] GameObject damagingExplosion_pf;
    [SerializeField] Sprite blownBarrelSprite;
    [SerializeField] SpriteRenderer renderer;

    bool triggered = false;

    [SerializeField] float timeDelay = 0f;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void Explode()
    {
        if (triggered)
            return;

        triggered = true;

        if (timeDelay <= 0f)
        {
            Boom();
        }
        else
        {
            StartCoroutine(StartFuze());
        }
    }

    IEnumerator StartFuze()
    {
        float timer = 0f;

        while (timer < timeDelay)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Boom();
    }

    void Boom()
    {
        renderer.sprite = blownBarrelSprite;
        var explosion = Instantiate(damagingExplosion_pf, this.transform.position, this.transform.rotation);
        Destroy(explosion, 2f);
    }

}
