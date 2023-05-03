using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicConversationStarter : ConversationInteractable
{
    public override void StartConversation()
    {
        dialogueBeginEvent.dialogueToBegin = toLoad;
        dialogueBeginEvent.Raise();
    }
}
