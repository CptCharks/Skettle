using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionController : MonoBehaviour
{
    public Animator anim;
	
	void Awake()
	{
		anim = GetComponent<Animator>();
	}
	
	public void Alert()
	{
		anim.SetTrigger("Alerted");
	}
	
	public void Confused()
	{
		anim.SetTrigger("Confused");
	}
}
