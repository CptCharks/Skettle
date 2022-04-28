using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
	[SerializeField] float timeForPitfall = 2;
	
	//Probably need to keep track of player direction and speed so player position can be easily reversable
	
	
	Hittable hitController;
	
	void Awake()
	{
		gameManager = FindObjectOfType<GameManager>();
	}
	
	
	public void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Enemy")
		{
			hitController = other.gameObject.GetComponentInChildren<Hittable>();
			hitController.onBreak.Invoke();
		}
		else if(other.tag == "Player")
		{
			hitController = other.gameObject.GetComponent<Hittable>();

            if(hitController != null)
                hitController.Hit();
			
			StartCoroutine(timerForPitfall());
		}
	}
	
	IEnumerator timerForPitfall()
	{
		//Can probably shake it up with an invuln function on the hit controller or make a seperate function
		gameManager.SetEnabledPlayerControls(false);
        if (hitController != null)
		    hitController.b_indestructable = true;
		
		for(int i = 0; i < timeForPitfall; i++)
		{
			yield return new WaitForSeconds(1f);
		}
		
		gameManager.SetEnabledPlayerControls(true);

        if (hitController != null)
            hitController.b_indestructable = false;
	}
}
