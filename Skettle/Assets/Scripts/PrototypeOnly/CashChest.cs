using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CashChest : Interactable 
{
	public enum ChestState
	{
		Open,
		Closed
	}
	
	private ChestState state = ChestState.Closed;
	
	public int amountOfMoney;
	public GameObject moneyPrefab;
	
	public float force = 0.5f;

    public override void Interact(bool buttonDown, InteractionController controller)
	{
		if(state == ChestState.Closed)
		{
			for(int i = 0; i < amountOfMoney; i++)
			{
				var money = Instantiate(moneyPrefab, transform.position, transform.rotation, transform);
				money.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * force);
			}
			
			
			state = ChestState.Open;
			
			onStateChanged.Invoke();
		}
	}

    public override int GetState()
	{
		return (int)state;
	}

	
}
