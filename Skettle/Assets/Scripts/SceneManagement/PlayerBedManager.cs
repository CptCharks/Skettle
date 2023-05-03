using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerBedManager : ConversationInteractable, I_ProgressConditional
{
    public Conversation noConversationLoaded;
    public Conversation genericSleep;
    public Conversation[] cantSleep;

    public SceneStartID bedSpawn;

    public PlayableAsset sleepCutscene;

    GameManager gameManager;

    ProgressContainer progress;

    int currentTalk = 0;

    int currentLevel = 0;
    int currentDay = 0;
    bool isMorning = true;

    bool goToSleep = false;

    new void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public override void StartConversation()
    {
        if(!isMorning)
        {
            //Prompt player if they want to go to bed
            toLoad = genericSleep;


            switch (currentLevel)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }
        else
        {
            //Give snarky it's day message
            if (currentTalk < 4)
            {
                toLoad = cantSleep[currentTalk];
            }
            else if (currentTalk > 10)
            {
                toLoad = cantSleep[5];
            }
            else
            {
                toLoad = cantSleep[4];
            }
            currentTalk++;
        }

        base.StartConversation();
    }

    public void ShowOrHide(ProgressContainer data)
    {
        progress = data;
        currentLevel = progress.progress.currentLevel;
        currentDay = progress.progress.currentDay;
        isMorning = progress.progress.isMorning;
    }

    public override void ReceivePrompt(int selection, string prompt_ID)
    {
        base.ReceivePrompt(selection, prompt_ID);

        if (prompt_ID.Equals("Bed_Yes"))
        {
            goToSleep = true;
        }
        else if(prompt_ID.Equals("Bed_No"))
        {
            goToSleep = false;
        }
    }

    public override void EndConversation()
    {
        if (goToSleep)
        {
            if (!isMorning)
            {
                gameManager.GetComponent<CutsceneManager>().StartCutscene(sleepCutscene, NewDay);
            }
        }

        base.EndConversation();
    }

    public void NewDay()
    {
        progress.progress.currentDay++;
        progress.progress.isMorning = true;
        gameManager.LoadLevel(bedSpawn);
    }
}
