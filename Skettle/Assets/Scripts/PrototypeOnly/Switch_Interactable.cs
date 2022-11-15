using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//One way switch. Potentially add variable to make it a two way
public class Switch_Interactable : Interactable
{
    enum SwitchState
    {
        On,
        Off
    }
    //Either use the events or use the current state to control animations
    [SerializeField] private SwitchState currentState;

    public UnityEvent onSwitchOn;
    public UnityEvent onSwitchOff;

    bool switched;

    public void Awake()
    {
        switch(currentState)
        {
            case SwitchState.Off:
                break;
            case SwitchState.On:
                break;
            default:
                break;
        }
    }

    public override int GetState()
    {
        return (int)currentState;
    }

    public override void Interact(bool buttonDown, InteractionController controller)
    {
        if(!switched && buttonDown)
        {
            switch (currentState)
            {
                case SwitchState.Off:
                    currentState = SwitchState.On;
                    onSwitchOn.Invoke();
                    break;
                case SwitchState.On:
                    currentState = SwitchState.Off;
                    onSwitchOff.Invoke();
                    break;
                default:
                    break;
            }

            onStateChanged.Invoke();
        }

        switched = buttonDown;
    }

    
}
