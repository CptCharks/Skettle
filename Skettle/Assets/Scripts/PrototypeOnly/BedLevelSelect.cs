using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BedLevelSelect : Interactable
{
    public GameManager gameManager;
    public GameObject levelSelectMenu;

    public List<Button> buttons;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public override void Interact(bool buttonDown, InteractionController controller)
    {
        if (!buttonDown)
            return;

        for(int i = 0; i < 4; i++)
        {
            if((gameManager.currentLevel-2) <= i)
            {
                buttons[i].interactable = false;
            }
            else
            {
                buttons[i].interactable = true;
            }

        }

        levelSelectMenu.SetActive(!levelSelectMenu.activeSelf);
        gameManager.SetEnabledPlayerControls(!levelSelectMenu.activeSelf);
    }

    public override int GetState()
    {
        throw new System.NotImplementedException();
    }

    public void LoadLevel(int level)
    {
        gameManager.LoadLevelRetry(level);
    }

}
