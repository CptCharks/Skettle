using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : SceneStartPoint, I_GameplayInteractable
{
    

    public void Dehighlight()
    {
        
    }

    public void Highlight()
    {
        
    }

    public bool IsInteractable()
    {
        return true;
    }

    public void OnButtonDown()
    {
        LoadLinkedPoint();
    }

    public void OnButtonHeld()
    {
        
    }

    public void OnButtonUp()
    {
        
    }
}
