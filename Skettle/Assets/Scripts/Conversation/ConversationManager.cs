using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConversationManager : MonoBehaviour
{
    public bool isInConversation;
    bool waitingForInput;
    bool ui_isProcessing;

    //Simple check if the skip button was pressed
    bool skipActive;

    Conversation currentConv;

    ConversationStarter conv_Starter;
    GameManager game_Manager;
    ConversationUI conv_UI;

    public UnityEvent conversationEnded = new UnityEvent();
    public UnityEvent conversationStarted = new UnityEvent();
	
    // Start is called before the first frame update
    void Awake()
    {
        game_Manager = FindObjectOfType<GameManager>();
        conv_UI = FindObjectOfType<ConversationUI>();
    }

    public void InputSkipConversation()
    {
        //Check if currently scrolling text
        //  - If so, skip scrolling. Give a little leway to prevent going to the next textbox by accident.
        //  - Don't forget to only get InputDown instead of get Input
        //Else skip to next box or leave the conversation

        if (ui_isProcessing)
        {
            conv_UI.ForceTextSkip();
        }
        else
        {
            skipActive = true;
        }
    }


    // Probably should create a coroutine to manage the conversation when active.
    void Update()
    {
        if (!isInConversation)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            InputSkipConversation();
        }

        //Remove this for a different escape sequence
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isInConversation = false;
        }
    }

    public void ScrollReturnSignal()
    {
        ui_isProcessing = false;
    }

    public void StartCutsceneConversation(ConversationStarter starter)
    {
	if (isInConversation)
        {
            Debug.LogWarning("Already in conversation when StartConversation was called");
            return;
        }

        conv_Starter = starter;
        currentConv = conv_Starter.toLoad;

        game_Manager.SetGameplayEnabled(false);
        //Leaving this out cause seems to not get the game_manager inside the full build atm
        //game_Manager.SetEnabledPlayerControls(false);

        isInConversation = true;
        StartCoroutine(ConversationLoop());
    }

    public void EndCutsceneConversation()
    {

    }

    public void StartConversation(ConversationStarter starter)
    {
        if (isInConversation)
        {
            Debug.LogWarning("Already in conversation when StartConversation was called");
            return;
        }

        conv_Starter = starter;
        currentConv = conv_Starter.toLoad;

        game_Manager.SetGameplayEnabled(false);
        //Leaving this out cause seems to not get the game_manager inside the full build atm
        //game_Manager.SetEnabledPlayerControls(false);

	    conversationStarted.Invoke();

        isInConversation = true;
        StartCoroutine(ConversationLoop());
    }

    public void StartConversation(Conversation conversation)
    {
        if (isInConversation)
        {
            Debug.LogWarning("Already in conversation when StartConversation was called");
            return;
        }

        conv_Starter = null;
        currentConv = conversation;

        game_Manager.SetGameplayEnabled(false);
        //Leaving this out cause seems to not get the game_manager inside the full build atm
        //game_Manager.SetEnabledPlayerControls(false);

        conversationStarted.Invoke();

        isInConversation = true;
        StartCoroutine(ConversationLoop());
    }

    public void EndConversation()
    {
	    conversationEnded.Invoke();

        if (conv_Starter != null)
            conv_Starter.EndConversation();
        
        if(conv_UI != null)
            conv_UI.EndConversation();

        game_Manager.SetGameplayEnabled(true);
        //game_Manager.SetEnabledPlayerControls(true);
    }

    //New thread to prevent conversation loop from hogging main thread when pausing
    IEnumerator ConversationLoop()
    {
        int cnt = 0;
        Conversation.Section currentSection = currentConv.sections[cnt];

        //Setup visuals
        conv_UI.SetupConversation(currentSection);

        //Process text
        while (isInConversation)
        {
            //Potentially add an emergency exit to this loop from outside
            if(skipActive)
            {
                skipActive = false;
                cnt++;

                if (currentConv.sections.Count <= cnt)
                {
                    isInConversation = false;
                    break;
                }
                else
                {
                    currentSection = currentConv.sections[cnt];
                    conv_UI.LoadNextSecton(currentSection);
                    ui_isProcessing = true;
                }
            }

            yield return new WaitForEndOfFrame();
        }

        //Close out visuals and clean up

        isInConversation = false;

        EndConversation();
    }
}
