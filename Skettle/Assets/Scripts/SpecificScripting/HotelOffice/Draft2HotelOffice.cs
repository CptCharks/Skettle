using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draft2HotelOffice : MonoBehaviour
{
    public Conversation afterFirstTalk;
    public Conversation afterSecondTalk;

    public int talk = 0;

    public BasicConversationStarter hotelHead;

    public void NextConversation()
    {
        if (talk == 0)
        {
            hotelHead.ChangeConversation(afterFirstTalk);
            hotelHead.onConversationEnd.RemoveAllListeners();
            talk++;
        }
        else if (talk == 1)
        {
            hotelHead.ChangeConversation(afterSecondTalk);
            hotelHead.onConversationEnd.RemoveAllListeners();
            talk++;
        }
    }

}
