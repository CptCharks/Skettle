using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draft2Museum : MonoBehaviour
{
    SpriteRenderer thisSprite;

    public void Awake()
    {
        thisSprite = GetComponent<SpriteRenderer>();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutUIStuff());
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInUIStuff());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        FadeIn();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        FadeOut();
    }

    IEnumerator FadeOutUIStuff()
    {
        Color current = thisSprite.color;

        while (thisSprite.color.a < 1)
        {
            current.a += 0.06f; //= new Color(current.r, current.g, current.b, op);

            thisSprite.color = current;

            yield return new WaitForFixedUpdate();
        }

    }

    IEnumerator FadeInUIStuff()
    {
        Color current = thisSprite.color;

        while (thisSprite.color.a > 0)
        {
            current.a -= 0.06f; //= new Color(current.r, current.g, current.b, op);

            thisSprite.color = current;

            yield return new WaitForFixedUpdate();
        }

    }


}
