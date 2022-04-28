using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
	public UnityEvent onEntered = new UnityEvent();
	public UnityEvent onExited = new UnityEvent();
	
    [SerializeField] public bool repeatable;
	bool firstShotEnter = true;
	bool firstShotExit = true;
	
	public void OnTriggerEnter2D(Collider2D other)
	{
        if (other.tag != "Player")
            return;

        if (repeatable || (!repeatable && firstShotEnter))
		{
				firstShotEnter = false;
				onEntered.Invoke();
		}
	}
	
	public void OnTriggerExit2D(Collider2D other)
	{
        if (other.tag != "Player")
            return;

		if(repeatable || (!repeatable && firstShotExit))
		{
				firstShotExit = false;
				onEntered.Invoke();
		}
	}
}
