using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrideTestConditional : MonoBehaviour, I_ProgressConditional
{
    public Sprite eggSprite;
    SpriteRenderer render;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    public void ShowOrHide(ProgressContainer data)
    {
        Debug.Log("Trying to load egg sprite");

        if (data.progress.foundTheEgg)
        {
            render.sprite = eggSprite;
        }
    }
}
