using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DialogueBeginEvent",menuName = "GameEvents/DialogueBeginEvent", order = 0)]
public class DialogueBeginEvent : GameEvent
{
    public Conversation dialogueToBegin;
    public ConversationInteractable convInteractable;
}
