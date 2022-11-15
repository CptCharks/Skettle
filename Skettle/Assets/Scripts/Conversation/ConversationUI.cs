using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConversationUI : MonoBehaviour
{
    ConversationManager conv_manager;

    public TextMeshProUGUI chatterName;
    public TextMeshProUGUI chatBox;

    public GameObject chatSystem;
    public Image leftImage;
    public Image rightImage;
    public Image dialogueBox;

    public Sprite dialogueGeneric;
    public Sprite dialogueCharacter;

    Character previousLeftCharacter;
    Character.EmoteStruct previousLeftEmote;
    Character previousRightCharacter;
    Character.EmoteStruct previousRightEmote;

    public float textSpeed = 0.05f;

    Conversation.Section current_section;

    Coroutine currentDialogueCoroutine;

    public void Awake()
    {
        conv_manager = FindObjectOfType<ConversationManager>();
    }

    public void EndConversation()
    {
        
        chatSystem.SetActive(false);
        leftImage.gameObject.SetActive(false);
        rightImage.gameObject.SetActive(false);
    }

    public void SetupConversation(Conversation.Section section)
    {
        textIsScrolling = false;
        if(currentDialogueCoroutine != null)
            StopCoroutine(currentDialogueCoroutine);

        chatBox.text = "";

        chatSystem.SetActive(true);

        LoadNextSecton(section);
    }

    public void LoadNextSecton(Conversation.Section section)
    {
        if(section.currentChatter == "")
        {
            //Load generic dialogue box rather than character specific one
            dialogueBox.sprite = dialogueGeneric;
        }
        else
        {
            dialogueBox.sprite = dialogueCharacter;
        }


        /*
        if (previousLeftCharacter != section.leftCharacter)
        {
            //Change out the character

        }
        else
        {
            if(previousLeftEmote.emotion != section.leftEmote.emotion)
            {
                //Do some transition animation. Perhaps rotate?
            }
        }

        if (previousRightCharacter != section.rightCharacter)
        {
            //Change out the character

        }
        else
        {
            if (previousRightEmote.emotion != section.rightEmote.emotion)
            {
                //Do some transition animation. Perhaps rotate?
            }
        }
        */

        previousLeftCharacter = section.leftCharacter;
        previousLeftEmote = section.leftEmote;

        previousRightCharacter = section.rightCharacter;
        previousRightEmote = section.rightEmote;

        //Add animations to move character sprites away or simply flip to replace if similar character. Consider extending the character sprite system further before creating a lot of conversations (or at least populating them with images)
        if (section.leftCharacterImage != null)
        {
            leftImage.gameObject.SetActive(true);
            leftImage.sprite = section.leftCharacterImage;
        }
        else
        {
            rightImage.gameObject.SetActive(false);
            leftImage.sprite = null;
        }

        if (section.rightCharacterImage != null)
        {
            rightImage.gameObject.SetActive(true);
            rightImage.sprite = section.rightCharacterImage;
        }
        else
        {
            rightImage.gameObject.SetActive(false);
            rightImage.sprite = null;
        }


        textIsScrolling = false;
        if (currentDialogueCoroutine != null)
            StopCoroutine(currentDialogueCoroutine);

        chatBox.text = "";

        current_section = section;

        chatterName.text = section.currentChatter;

        //Sub divide this to a phrase loader
        //chatBox.text = section.phrase;
        currentDialogueCoroutine = StartCoroutine(ScrollText());


        //Do checks to see which side is talking and move/highlight them appropriately
    }

    //TODO: Come back once the character emote sprite system is in place. Use this to add and remove characters from the conversation
    /*
    void EnterRight()
    {
        float width = rightImage.rectTransform.sizeDelta.x;

        StartCoroutine(UIMover(rightImage.rectTransform, -width));
    }

    void ExitRight()
    {
        float width = rightImage.rectTransform.sizeDelta.x;

        StartCoroutine(UIMover(rightImage.rectTransform, width));
    }

    void EnterLeft()
    {
        float width = leftImage.rectTransform.sizeDelta.x;

        StartCoroutine(UIMover(leftImage.rectTransform, width));
    }

    void ExitLeft()
    {
        float width = leftImage.rectTransform.sizeDelta.x;

        StartCoroutine(UIMover(leftImage.rectTransform, -width));
    }

    IEnumerator UIMover(RectTransform rectTrans, float amount)
    {
        float start = rectTrans.position.x;

        //Change this to a smooth transition
        rectTrans.Translate(amount,0,0);

        yield return new WaitForSeconds(0.05f);
    }
    */

    public void ForceTextSkip()
    {
        if (textIsScrolling == true)
        {
            textIsScrolling = false;
            if (currentDialogueCoroutine != null)
                StopCoroutine(currentDialogueCoroutine);

            chatBox.text = current_section.phrase;

            conv_manager.ScrollReturnSignal();
        }

        //Add different sections for different parts of the UI (like characters moving, or just make it impossible to skip that

    }

    public bool textIsScrolling = false;

    IEnumerator ScrollText()
    {
        textIsScrolling = true;

        int cnt = 0;
        string temp = "";

        while (textIsScrolling)
        {
            chatBox.text = temp;
            temp += current_section.phrase[cnt];

            cnt++;

            if (cnt >= current_section.phrase.Length)
            {
                chatBox.text = current_section.phrase;

                textIsScrolling = false;
                break;
                //Maybe just call back to the manager directly or send up an event signal
            }

            //StartCoroutine
            yield return new WaitForSecondsRealtime(textSpeed);
        }

        conv_manager.ScrollReturnSignal();
    }
}
