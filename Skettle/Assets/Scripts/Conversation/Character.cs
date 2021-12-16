using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestCharacter", menuName = "Story/Character", order = 1)]
[System.Serializable]
public class Character : ScriptableObject
{
    [System.Serializable]
    public struct EmoteStruct
    {
        public string emotion;
        public Sprite sprite;
    }

    public List<EmoteStruct> emotes;
}
