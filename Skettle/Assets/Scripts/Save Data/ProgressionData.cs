using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressionData
{
    public SceneStartID currentScene;
    public int lastCompletedLevel;
    public bool foundTheEgg;
    public bool parentsDead;
    public bool noWeapons;

    public ProgressionData()
    {
        currentScene = null;
        lastCompletedLevel = 0;
        foundTheEgg = false;
        parentsDead = false;
        noWeapons = false;
    }

    //TODO: Can potentialyl rework this to just create a generic scriptable object with the required strings
    public void ResetSceneID()
    {
        currentScene = (SceneStartID)Resources.Load("SceneEntryPointIDs/HomeFarm/HomeFarm_Default", typeof(SceneStartID));
    }

    public ProgressionData(ProgressContainer progressContainer)
    {
        var reference = progressContainer.progress;
        currentScene = reference.currentScene;
        lastCompletedLevel = reference.lastCompletedLevel;
        foundTheEgg = reference.foundTheEgg;
        parentsDead = reference.parentsDead;
        noWeapons = reference.noWeapons;
    }

}
