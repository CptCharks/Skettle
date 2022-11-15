using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draft2BulletStore : MonoBehaviour
{
    public Conversation afterFirstTalk;

    public int talk = 0;

    public BasicConversationStarter hillary;

    public void NextConversation()
    {
        if (talk == 0)
        {
            hillary.ChangeConversation(afterFirstTalk);
            hillary.onConversationEnd.RemoveAllListeners();
            talk++;
        }
    }

}
