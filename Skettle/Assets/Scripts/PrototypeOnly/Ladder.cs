using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Game_Interactable
{
	HeightManager heightManager;
	public int targetLayer;

    public Transform dumpLocation;

    PlayerController player;

	void Awake()
	{
		heightManager = FindObjectOfType<HeightManager>();
        player = FindObjectOfType<PlayerController>();
    }

    public override void OnButtonDown()
    {
        //TODO: Add loading screen here and potentially sound effect

		heightManager.GoToLayer(targetLayer);

        if (dumpLocation == null)
        {
            player.SetPlayerPosition(transform);
        }
        else
        {
            player.SetPlayerPosition(dumpLocation);
        }

    }

    public int GetState()
	{
		return 0;
	}
}
