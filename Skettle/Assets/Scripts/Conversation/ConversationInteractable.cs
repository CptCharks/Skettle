using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ConversationInteractable : MonoBehaviour
{
    public ConversationManager conv_manager;
    public Conversation toLoad;
    public UnityEvent onConversationStart;
    public UnityEvent onConversationEnd;

    protected DialogueBeginEvent dialogueBeginEvent;


    public virtual void Awake()
    {
        conv_manager = FindObjectOfType<ConversationManager>();
    }

    public virtual void OnEnable()
    {
        dialogueBeginEvent = (DialogueBeginEvent)Resources.Load("GameEvents/DialogueBeginEvent", typeof(DialogueBeginEvent));
    }

    public virtual void StartConversation()
    {
        dialogueBeginEvent.convInteractable = this;
        dialogueBeginEvent.dialogueToBegin = toLoad;
        dialogueBeginEvent.Raise();
    }

    //Put any this side only processing for conversation end
    public virtual void EndConversation()
    {
        Debug.Log("Ending conversation");
        onConversationEnd.Invoke();
    }

    public virtual void ReceivePrompt(int selection, string prompt_ID)
    {
        Debug.Log("Recieving prompt callback: " + selection + " : " + prompt_ID);

    }

    public virtual void ChangeConversation(Conversation newConversation)
    {
        toLoad = newConversation;
    }
}
