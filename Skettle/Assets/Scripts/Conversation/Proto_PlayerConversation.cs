using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proto_PlayerConversation : MonoBehaviour
{

    [SerializeField] ConversationStarter starter;

    // Update is called once per frame
    void Update()
    {
        if(starter != null)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                starter.StartConversation();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var check = collision.gameObject.GetComponent<ConversationStarter>();

        if (check != null)
        {
            starter = check;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        var check = collision.gameObject.GetComponent<ConversationStarter>();

        if (check == starter)
            starter = null;
    }
}
