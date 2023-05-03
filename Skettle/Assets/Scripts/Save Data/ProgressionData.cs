using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressionData
{
    public SerialSceneData currentScene;
    public int currentLevel;
    public int currentDay;
    public bool foundTheEgg;
    public bool parentsDead;
    public bool talkedToMayor;
    public bool talkedToMatride;
    public bool isMorning;
    public int moneyGivenToMuseum;

    public ProgressionData()
    {
        currentScene = new SerialSceneData();
        currentLevel = 0;
        currentDay = 0;
        moneyGivenToMuseum = 0;
        foundTheEgg = false;
        parentsDead = false;
        isMorning = false;
        talkedToMayor = false;
        talkedToMatride = false;
    }


    public void ResetSceneID()
    {
        var startSceneID = (SceneStartID)Resources.Load("SceneEntryPointIDs/HomeFarm/HomeFarm_Default", typeof(SceneStartID));
        currentScene.sceneName = startSceneID.sceneName;
        currentScene.startID = startSceneID.startID;
    }

    public ProgressionData(ProgressContainer progressContainer)
    {
        var reference = progressContainer.progress;
        currentScene = reference.currentScene;
        currentLevel = reference.currentLevel;
        foundTheEgg = reference.foundTheEgg;
        parentsDead = reference.parentsDead;
        currentDay = reference.currentDay;
        moneyGivenToMuseum = reference.moneyGivenToMuseum;
    }

}
