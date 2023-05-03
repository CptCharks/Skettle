using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proto_PlayerConversation : MonoBehaviour
{
    //TODO: Make it shoot a 2D sphere cast in the last movement direction of the player to check for a conversation (or interactable object. This is still a hold over from the original prototoype)


    //Change this to ConversationInteractable
    [SerializeField] ConversationInteractable target;

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                target.StartConversation();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var check = collision.gameObject.GetComponent<ConversationInteractable>();

        if (check != null)
        {
            target = check;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        var check = collision.gameObject.GetComponent<ConversationInteractable>();

        if (check == target)
            target = null;
    }
}
