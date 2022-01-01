using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	public float timeActive = 1f;
	
    void Start()
	{
		Destroy(gameObject, timeActive);
	}
	
	public void OnTriggerEnter2D(Collider2D other)
	{
		var hittable = other.GetComponent<Hittable>();
		if(hittable != null)
		{
			hittable.Hit();
		}
	}
}
