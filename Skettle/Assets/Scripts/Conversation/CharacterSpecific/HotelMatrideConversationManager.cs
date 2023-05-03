using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotelMatrideConversationManager : ConversationInteractable, I_ProgressConditional
{
    ProgressContainer progress;

    public Conversation firstTalk;
    public Conversation secondTalk;
    public Conversation thirdTalk;

    public Conversation showYouToYourRoom;

    public Conversation bitBusyRightNow;

    public int talk = 0;

    GameManager gameManager;

    new void Awake()
    {
        talk = 0;
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
        if(progress.progress.currentLevel == 0 && !progress.progress.isMorning)
        {
            if (!progress.progress.talkedToMayor)
            {
                if (talk == 0)
                {
                    toLoad = firstTalk;
                }
                else if (talk == 1)
                {
                    toLoad = secondTalk;

                }
                else
                {
                    toLoad = thirdTalk;
                }
            }
            else
            {
                toLoad = showYouToYourRoom;
                progress.progress.talkedToMatride = true;
                gameManager.SaveGame();
            }
        }
        else if(progress.progress.currentLevel == 0 && progress.progress.isMorning)
        {
            //TODO: Potentially make this different
            toLoad = bitBusyRightNow;
        }
        else
        {
            //Put unhandled I'm busy or chitchat stuff here
            toLoad = bitBusyRightNow;
        }

        talk++;

    }
}
