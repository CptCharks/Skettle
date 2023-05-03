using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class SceneSpecificsBase : MonoBehaviour
{

    //Every scene should check for progress conditionals since some data might be used for easter eggs or small changes
    public virtual void SceneStart()
    {

    }

    public virtual void SceneEnd()
    {

    }

}
