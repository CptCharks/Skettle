using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour
{
    public UnityEvent onStateChanged;

    public abstract void Interact(bool buttonDown, InteractionController controller);

    public abstract int GetState();

    public virtual void RegisterCallback(UnityAction func)
    {
        onStateChanged.AddListener(func);
    }

    public virtual void DeregisterCallback(UnityAction func)
    {
        onStateChanged.RemoveListener(func);
    }
}
