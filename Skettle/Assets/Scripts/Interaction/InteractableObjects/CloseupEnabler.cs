using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseupEnabler : MonoBehaviour, I_GameplayInteractable
{
    public GameObject mapUIElements;
    public GameManager manager;

    public void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

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
        if(manager != null)
        {
            Debug.Log("We found it");
        }

        mapUIElements.SetActive(true);
        manager.SetEnabledPlayerControls(false);
    }

    public void OnButtonHeld()
    {
        
    }

    public void OnButtonUp()
    {
        mapUIElements.SetActive(false);
        manager.SetEnabledPlayerControls(true);
    }
}
