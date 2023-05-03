using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOwnerConversationManager : ConversationInteractable, I_ProgressConditional
{
    //Reference to the manager handling the store UI
    public GunStoreManager gunShopManager;

    int count = 0;
    public Conversation firstTimeNoGun;
    public Conversation secondTimeNoGun;

    public Conversation whatCanIGetYou1;
    public Conversation whatCanIGetYou2;

    public Conversation comeAgain;

    ProgressContainer progress;

    bool openingShop = true;

    public override void StartConversation()
    {
        openingShop = true;

        if (!progress.playerData.weaponOwnership[0])
        {
            if (count < 1)
            {
                toLoad = firstTimeNoGun;
            }
            else
            {
                toLoad = secondTimeNoGun;
            }
        }
        else
        {
            if (count < 1)
            {
                toLoad = whatCanIGetYou1;
            }
            else
            {
                toLoad = whatCanIGetYou2;
            }
        }

        count++;

        dialogueBeginEvent.convInteractable = this;
        dialogueBeginEvent.dialogueToBegin = toLoad;
        dialogueBeginEvent.Raise();
    }

    public override void EndConversation()
    {
        if (openingShop)
        {
            Debug.Log("Attempting to open shop");
            gunShopManager.OpenCloseMenu(true);
        }

        base.EndConversation();
    }

    public void ShowOrHide(ProgressContainer data)
    {
        progress = data;
    }

    public void LeavingStore()
    {
        openingShop = false;

        toLoad = comeAgain;

        dialogueBeginEvent.convInteractable = this;
        dialogueBeginEvent.dialogueToBegin = toLoad;
        dialogueBeginEvent.Raise();
    }
}
