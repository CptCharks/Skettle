using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chest : Game_Interactable
{
	public enum ChestState
	{
		Open,
		Closed
	}
	
	private ChestState state = ChestState.Closed;
	
	public int amountToDrop;
	public GameObject itemToDrop;
	
	public float force = 0.5f;

    public int GetState()
	{
		return (int)state;
	}

    public override void OnButtonDown()
    {
        if (state == ChestState.Closed)
        {
            for (int i = 0; i < amountToDrop; i++)
            {
                var money = Instantiate(itemToDrop, transform.position, transform.rotation, transform);
                money.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * force);
            }


            state = ChestState.Open;

            //onStateChanged.Invoke();
        }
    }

    public override bool IsInteractable()
    {
        return (state == ChestState.Closed);
    }
}
