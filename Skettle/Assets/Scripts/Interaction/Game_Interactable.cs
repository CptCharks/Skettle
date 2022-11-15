using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Game_Interactable : MonoBehaviour, I_GameplayInteractable
{
    public virtual void Dehighlight()
    {

    }

    public virtual void Highlight()
    {

    }

    public virtual bool IsInteractable()
    {
        return true;
    }

    public virtual void OnButtonDown()
    {

    }

    public virtual void OnButtonHeld()
    {

    }

    public virtual void OnButtonUp()
    {

    }
}
