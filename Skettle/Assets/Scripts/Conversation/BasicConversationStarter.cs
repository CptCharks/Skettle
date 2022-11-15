using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicConversationStarter : MonoBehaviour
{
    public ConversationManager conv_manager;
    public Conversation toLoad;
    public UnityEvent onConversationStart;
    public UnityEvent onConversationEnd;

    protected DialogueBeginEvent dialogueBeginEvent;

    public void Awake()
    {
        conv_manager = FindObjectOfType<ConversationManager>();
    }

    public void OnEnable()
    {
        dialogueBeginEvent = (DialogueBeginEvent)Resources.Load("GameEvents/DialogueBeginEvent", typeof(DialogueBeginEvent));
    }

    public void StartConversation()
    {
        /*if(conv_manager == null)
		{
			conv_manager = FindObjectOfType<ConversationManager>();
		}
		
        if (!conv_manager.isInConversation)
        {
            //Don't know if this is the best place to put this
            onConversationStart.Invoke();

            conv_manager.StartConversation(this);
        }*/
        dialogueBeginEvent.dialogueToBegin = toLoad;
        dialogueBeginEvent.Raise();

    }


    //Put any this side only processing for conversation end
    public void EndConversation()
    {
        onConversationEnd.Invoke();
    }

    public void ChangeConversation(Conversation newConversation)
    {
        toLoad = newConversation;
    }
}
