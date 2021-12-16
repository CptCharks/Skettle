using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableGate : MonoBehaviour
{
    [SerializeField] Animator gate_anim;

    public void OpenGate()
    {
        gate_anim.SetTrigger("Open");
        //If can't animate the collider, just enable, disable the collider here
    }

    public void CloseGate()
    {
        gate_anim.SetTrigger("Close");
        //If can't animate the collider, just enable, disable the collider here
    }
}
