using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable_Interact : Interactable
{
    public GameObject prefab_pushed;
    public bool b_hasBeenPushed;

    public override void Interact(bool buttonDown, InteractionController controller)
    {
        b_hasBeenPushed = true;
    }

    public override int GetState()
    {
        return 0;
    }

    public void ResetObject()
    {
        b_hasBeenPushed = false;
    }

}
