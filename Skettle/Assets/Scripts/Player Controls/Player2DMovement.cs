using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2DMovement : MonoBehaviour {

	private float currentSpeed = 0.0f;

	public float runSpeed = 5f;
	public float sneakSpeed = 2f;

	public float verticalSpeed = 1;
	public float horizontalSpeed = 1;

	private Vector3 v2_movement;

	[SerializeField] public bool controlsDisabled;


	// Use this for initialization
	void Start () {
		v2_movement = Vector3.zero;
		controlsDisabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!controlsDisabled)
		{
			v2_movement.z = 0f;

			float f_hor = Input.GetAxis ("Horizontal");
			float f_ver = Input.GetAxis ("Vertical");

			v2_movement = new Vector3((f_hor * horizontalSpeed),(f_ver * verticalSpeed));
			//v2_movement = v2_movement.normalized;

			// Calculate actual movement
			if (v2_movement.magnitude < 0.01 && v2_movement.magnitude > -0.01) {
				currentSpeed = 0.0f;
			} else if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
				currentSpeed = sneakSpeed;
			} else {
				currentSpeed = runSpeed;
			}

			//anim.SetFloat ("Speed", Mathf.Abs (moveSpeed));

			//update the position
			transform.position += v2_movement * currentSpeed * Time.deltaTime;
		}
	}
}
