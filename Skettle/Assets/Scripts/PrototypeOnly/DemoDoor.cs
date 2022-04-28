using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoDoor : Interactable
{
    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public override void Interact(bool buttonDown, InteractionController controller)
    {
        gameManager.LoadNextLevel();
    }

    public override int GetState()
    {
        throw new System.NotImplementedException();
    }
}
