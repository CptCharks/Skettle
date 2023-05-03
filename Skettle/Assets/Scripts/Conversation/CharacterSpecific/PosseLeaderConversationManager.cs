using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosseLeaderConversationManager : ConversationInteractable, I_ProgressConditional
{
    public GameManager gameManager;

    public Conversation goGetAGun;
    public Conversation readyToGoConv;
    public Conversation heyThere;
    public Conversation goingToLumbermill;
    public Conversation goingToTrain;

    [SerializeField] MissionOrderInfo missionInfo;
    ProgressContainer progress;
    bool firstTimeTalking = true;

    [SerializeField] bool loadNextLevel = false;

    new void Awake()
    {
        firstTimeTalking = true;
        gameManager = FindObjectOfType<GameManager>();
        missionInfo = (MissionOrderInfo)Resources.Load("MissionOrder");
    }

    public void ShowOrHide(ProgressContainer data)
    {
        progress = data;
    }

    public override void StartConversation()
    {
        DetermineDialogue();

        base.StartConversation();
    }

    void DetermineDialogue()
    {
        if(firstTimeTalking && progress.progress.currentLevel == 0)
        {
            switch(progress.progress.currentLevel)
            {
                case 0:
                toLoad = heyThere;
                    break;
                case 1:
                toLoad = goingToLumbermill;
                    break;
                case 2:
                toLoad = goingToTrain;
                    break;

            }
            firstTimeTalking = false;
        }
        else if (!progress.playerData.weaponOwnership[0] && progress.progress.currentLevel == 0)
        {
            //If the player doesn't have a gun, tell the player to get one
            toLoad = goGetAGun;
        }
        else
        {
            //If the player has one, give the player the option of starting the mission
            toLoad = readyToGoConv;
        }

    }

    public override void ReceivePrompt(int selection, string prompt_ID)
    {
        base.ReceivePrompt(selection, prompt_ID);

        if(prompt_ID.Equals("Posse_Yes"))
        {
            loadNextLevel = true;
        }
        else
        {
            loadNextLevel = false;
        }
    }

    public override void EndConversation()
    {
        if(loadNextLevel)
        {
            
            gameManager.LoadLevel(missionInfo.missions[progress.progress.currentLevel].missionStart);
        }


        base.EndConversation();
    }
}
