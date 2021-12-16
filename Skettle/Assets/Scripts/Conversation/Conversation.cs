using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TestConversation", menuName = "Story/Conversation", order = 2)]
public class Conversation : ScriptableObject
{
    [System.Serializable]
    public class Section
    {
        [TextArea]
        public string phrase;
        public string currentChatter;
        public Sprite leftCharacterImage;
        public Sprite rightCharacterImage;

        public Character leftCharacter;
        public Character.EmoteStruct leftEmote;
        //TODO: Add drop down to choose emote avaliable to that character. Make part of the scriptable object system
        public Character rightCharacter;
        public Character.EmoteStruct rightEmote;
        //TODO: Add drop down to choose emote avaliable to that character. Make part of the scriptable object system

        public bool isLeftTalking;
        public bool isRightTalking;
        //Or use this
        //public GameObject leftCharacterImage;
        //public GameObject rightCharacterImage;
        //Basically using v for anything I might need to trigger animation wise
        public UnityEvent trigger;

    }

    public List<Section> sections;
}
