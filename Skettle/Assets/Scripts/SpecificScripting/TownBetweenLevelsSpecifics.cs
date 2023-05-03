using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TownBetweenLevelsSpecifics : SceneSpecificsBase, I_ProgressConditional
{
    public GameObject posseLeaderObject;

    ProgressContainer progress;

    public Conversation INeedToSeeTheMayor;


    protected DialogueBeginEvent dialogueBeginEvent;

    public void OnEnable()
    {
        dialogueBeginEvent = (DialogueBeginEvent)Resources.Load("GameEvents/DialogueBeginEvent", typeof(DialogueBeginEvent));
        dialogueBeginEvent.dialogueToBegin = INeedToSeeTheMayor;
    }

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


    //Put enabling and disabling stuff here
    public override void SceneStart()
    {
        base.SceneStart();

        if(progress.progress.isMorning)
        {
            posseLeaderObject.SetActive(true);
        }
        else
        {

            posseLeaderObject.SetActive(false);
        }

        if (!progress.progress.talkedToMayor)
        {
            dialogueBeginEvent.dialogueToBegin = INeedToSeeTheMayor;
            dialogueBeginEvent.Raise();
        }
    }

    public void ShowOrHide(ProgressContainer data)
    {
        progress = data;

        
    }
}
