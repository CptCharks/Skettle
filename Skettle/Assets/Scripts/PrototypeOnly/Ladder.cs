using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Interactable
{
	HeightManager heightManager;
	public int targetLayer;
	
	void Awake()
	{
		heightManager = FindObjectOfType<HeightManager>();
	}
	
    public override void Interact(bool buttonDown, InteractionController controller)
	{
		heightManager.GoToLayer(targetLayer);
	}

    public override int GetState()
	{
		return 0;
	}
}
