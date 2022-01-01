using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConversationStarter : MonoBehaviour
{
    public ConversationManager conv_manager;
    public Conversation toLoad;
    public UnityEvent onConversationStart;
    public UnityEvent onConversationEnd;

    public void Awake()
    {
        conv_manager = FindObjectOfType<ConversationManager>();
    }

    public void StartConversation()
    {
		if(conv_manager == null)
		{
			conv_manager = FindObjectOfType<ConversationManager>();
		}
		
        if (!conv_manager.isInConversation)
        {
            //Don't know if this is the best place to put this
            onConversationStart.Invoke();

            conv_manager.StartConversation(this);
        }
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
