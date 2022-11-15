using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proto_PlayerConversation : MonoBehaviour
{

    [SerializeField] BasicConversationStarter starter;

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
        var check = collision.gameObject.GetComponent<BasicConversationStarter>();

        if (check != null)
        {
            starter = check;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        var check = collision.gameObject.GetComponent<BasicConversationStarter>();

        if (check == starter)
            starter = null;
    }
}
