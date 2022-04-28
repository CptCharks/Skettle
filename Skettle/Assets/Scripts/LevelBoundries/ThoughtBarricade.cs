using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBarricade : MonoBehaviour
{
    [SerializeField] int timesHit;
    int numberOfConversations;
    [SerializeField] Conversation[] conversations;

    [SerializeField] ConversationManager conversationManager;

    private void Start()
    {
        conversationManager = FindObjectOfType<ConversationManager>();

        numberOfConversations = conversations.Length;
    }

    void CalculateWhatToDo()
    {
        //if (conversationManager == null)
            //conversationManager = FindObjectOfType<ConversationManager>();


        if (timesHit >= numberOfConversations)
        {
            timesHit = 0;
        }

        
        conversationManager.StartConversation(conversations[timesHit]);

        timesHit++;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        CalculateWhatToDo();
    }
}
