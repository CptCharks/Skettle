using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MayorInTHConversationManager : ConversationInteractable, I_ProgressConditional
{
    GameManager gameManager;

    ProgressContainer progress;
    public Conversation meetingTheMayor;
    public Conversation goToTheHotel;
    public Conversation imBusy;

    new void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void ShowOrHide(ProgressContainer data)
    {
        progress = data;
    }

    public override void StartConversation()
    {
        DetermineConversation();

        base.StartConversation();
    }

    void DetermineConversation()
    {
        if (progress.progress.currentLevel == 0)
        {
            if (!progress.progress.talkedToMayor)
            {
                toLoad = meetingTheMayor;
            }
            else
            {
                toLoad = goToTheHotel;
            }
        }
        else
        {
            toLoad = imBusy;
        }
    }

    public override void EndConversation()
    {
        progress.progress.talkedToMayor = true;

        //Or kick it to a dedicated save post or method
        gameManager.SaveGame();

        base.EndConversation();
    }
}
