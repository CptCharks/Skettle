using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draft2BuildingFader : MonoBehaviour
{
    SpriteRenderer[] thisSprite;

    public float fadeTarget = 0.2f;

    public void Awake()
    {
        thisSprite = GetComponentsInChildren<SpriteRenderer>();
        if (gameObject.activeInHierarchy)
            StartCoroutine(FadeOutUIStuff());
    }

    public void FadeOut()
    {
        if(gameObject.activeInHierarchy)
            StartCoroutine(FadeOutUIStuff());
    }

    public void FadeIn()
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(FadeInUIStuff());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            FadeIn();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")  
            FadeOut();
    }

    IEnumerator FadeOutUIStuff()
    {
        Color current = thisSprite[0].color;

        while (thisSprite[0].color.a < 1)
        {
            current.a += 0.06f; //= new Color(current.r, current.g, current.b, op);

            foreach (SpriteRenderer sr in thisSprite)
            {

                sr.color = current;
            }
            yield return new WaitForFixedUpdate();
        }

    }

    IEnumerator FadeInUIStuff()
    {
        Color current = thisSprite[0].color;

        while (thisSprite[0].color.a > fadeTarget)
        {
            current.a -= 0.06f; //= new Color(current.r, current.g, current.b, op);

            foreach (SpriteRenderer sr in thisSprite)
            {
                sr.color = current;
            }
            yield return new WaitForFixedUpdate();
        }

    }

}
