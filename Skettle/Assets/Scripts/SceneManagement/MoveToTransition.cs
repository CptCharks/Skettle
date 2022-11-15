using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTransition : SceneStartPoint, I_WalkInto
{
    public void WalkAway()
    {

    }

    public void WalkingInto()
    {

    }

    public void WalkInto()
    {
        LoadLinkedPoint();
    }
}
