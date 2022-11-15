using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TownBetweenLevelsSpecifics : SceneSpecificsBase
{
    public void LoadNextMission()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        MissionLoadInfo levelToLoad = gm.GetNextMissionLevelName();

        if (levelToLoad != null)
        {
            gm.LoadLevel(levelToLoad.missionStart);
        }
        else
        {
            //Error handling
        }
    }

    public override void SceneStart()
    {
        base.SceneStart();

    }

}
