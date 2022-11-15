using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController_V2 : GameplayComponent
{
    [SerializeField] GameObject eKey;

    bool worldInteracting = false;
    bool gameInteracting = false;

    I_WorldInteractable worldInteractable;

    I_GameplayInteractable gameInteractable;

    public override void GameplayUpdate()
    {
        if(gameInteractable != null)
        {
            eKey.SetActive(true);
        }
        else
        {
            eKey.SetActive(false);
        }


        if ((gameInteractable != null) && gameInteractable.IsInteractable() && !worldInteracting && !gameInteracting && Input.GetKeyDown(KeyCode.E))
        {
            gameInteracting = true;
            gameInteractable.OnButtonDown();
        }

        if((gameInteractable != null) && gameInteracting)
        {
            gameInteractable.OnButtonHeld();
        }

        if ((gameInteractable != null) && gameInteracting && Input.GetKeyUp(KeyCode.E))
        {
            gameInteracting = false;
            gameInteractable.OnButtonUp();
        }

        if ((worldInteractable != null) && worldInteractable.IsInteractable() && !worldInteracting && !gameInteracting  && Input.GetKeyDown(KeyCode.F))
        {
            worldInteracting = true;
            worldInteractable.OnButtonDown();
        }

        if ((worldInteractable != null) && worldInteracting)
        {
            worldInteractable.OnButtonHeld();
        }

        if ((worldInteractable != null) && worldInteracting && Input.GetKeyUp(KeyCode.F))
        {
            worldInteracting = false;
            worldInteractable.OnButtonUp();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<I_WalkInto>(out I_WalkInto comp))
        {
            comp.WalkInto();
        }

        if (worldInteractable != null)
        {
            worldInteractable.Dehighlight();
            if(worldInteracting)
            {
                worldInteracting = false;
                worldInteractable.OnButtonUp();
            }
        }

        worldInteractable = collision.GetComponent<I_WorldInteractable>();

        if(worldInteractable != null)
        {
            worldInteractable.Highlight();
        }

        if (gameInteractable != null)
        {
            gameInteractable.Dehighlight();
            if(gameInteracting)
            {
                gameInteracting = false;
                gameInteractable.OnButtonUp();
            }
        }

        gameInteractable = collision.GetComponent<I_GameplayInteractable>();

        if (gameInteractable != null)
        {
            gameInteractable.Highlight();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (worldInteractable != null && collision.GetComponent<I_WorldInteractable>() == worldInteractable)
        {
            worldInteractable.Dehighlight();
            worldInteractable = null;
        }

        if (gameInteractable != null && collision.GetComponent<I_GameplayInteractable>() == gameInteractable)
        {
            gameInteractable.Dehighlight();
            gameInteractable = null;
        }
    }
}
