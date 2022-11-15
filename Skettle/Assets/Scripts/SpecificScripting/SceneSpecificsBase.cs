using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class SceneSpecificsBase : MonoBehaviour
{
    ProgressionData gameData;

    //Every scene should check for progress conditionals since some data might be used for easter eggs or small changes
    public virtual void SceneStart()
    {
        gameData = FindObjectOfType<ProgressContainer>().progress;

        var conditionals = FindObjectsOfType<MonoBehaviour>().OfType<I_ProgressConditional>();

        foreach (I_ProgressConditional ipc in conditionals)
        {
            ipc.ShowOrHide(gameData);
        }
    }

    public virtual void SceneEnd()
    {

    }

}
