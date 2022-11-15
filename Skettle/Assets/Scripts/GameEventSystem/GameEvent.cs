using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : ScriptableObject
{
    protected List<IGameEventListener> eventListeners = new List<IGameEventListener>();

    public virtual void Raise()
    {
        IGameEventListener[] listeners = eventListeners.ToArray();

        for(int i = listeners.Length - 1; i >= 0; i--)
        {
            IGameEventListener l = listeners[i];
            if(eventListeners.Contains(l))
            {
                l.OnGameEventRaised(this);
            }
        }
    }

    public virtual void Register(IGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    public virtual void Deregister(IGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }

}
