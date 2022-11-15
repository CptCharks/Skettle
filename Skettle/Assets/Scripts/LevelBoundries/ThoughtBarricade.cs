using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBarricade : MonoBehaviour
{
    [SerializeField] int timesHit;
    int numberOfConversations;
    [SerializeField] Conversation[] conversations;

    protected DialogueBeginEvent dialogueBeginEvent;

    private void Start()
    {
        numberOfConversations = conversations.Length;
    }

    public void OnEnable()
    {
        dialogueBeginEvent = (DialogueBeginEvent)Resources.Load("GameEvents/DialogueBeginEvent", typeof(DialogueBeginEvent));
        dialogueBeginEvent.dialogueToBegin = conversations[0];
    }

    void CalculateWhatToDo()
    {
        //if (conversationManager == null)
            //conversationManager = FindObjectOfType<ConversationManager>();


        if (timesHit >= numberOfConversations)
        {
            timesHit = 0;
        }

        dialogueBeginEvent.dialogueToBegin = conversations[timesHit];
        dialogueBeginEvent.Raise();
        //conversationManager.StartConversation(conversations[timesHit]);

        timesHit++;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        CalculateWhatToDo();
    }
}
