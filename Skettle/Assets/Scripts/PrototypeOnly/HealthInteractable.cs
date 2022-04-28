using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthInteractable : Interactable
{
    public int healthToRegain;
    Hittable playerHealth;
    GameManager gameManager;

    [SerializeField] bool reusable;
    bool used;

    private void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Hittable>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public override void Interact(bool buttonDown, InteractionController controller)
    {
        if (!used)
        {
            playerHealth.tempHit += healthToRegain;
            if(playerHealth.tempHit > gameManager.defaultMaxHealth)
            {
                playerHealth.tempHit = gameManager.defaultMaxHealth;
            }

            if (!reusable)
            {
                used = true;
                gameObject.SetActive(false);
                this.enabled = false;
            }
        }
    }

    public override int GetState()
    {
        throw new System.NotImplementedException();
    }
}
