using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerZoneConversationStarter : MonoBehaviour
{
    public bool alreadyTriggered = false;
    public Conversation conversation;
    protected DialogueBeginEvent dialogueBeginEvent;
    
    public void OnEnable()
    {
        dialogueBeginEvent = (DialogueBeginEvent)Resources.Load("GameEvents/DialogueBeginEvent", typeof(DialogueBeginEvent));
    }

    public void ResetTrigger()
    {
        alreadyTriggered = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!alreadyTriggered && collision.tag == "Player")
        {
            alreadyTriggered = true;
            dialogueBeginEvent.dialogueToBegin = conversation;
            dialogueBeginEvent.Raise();
        }
    }
}
