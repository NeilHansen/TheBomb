using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	private Rigidbody2D rb;
	public float jumpHeight;
	public float dashThrust;

	private float finalVel;
	private float acceleration;
	private float jumpThrust; 

	private bool isOnGround = true;
	private bool canDoubleJump = false;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody2D> ();
	}

	//Check if your on the ground
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Floor")
		{
			Debug.Log ("OnGround");
			isOnGround = true;
		}
	}

	// Update is called once per frame
	void Update () {
		CheckJump ();
		CheckDash ();

	}

	//Check For Jump
	void CheckJump()
	{
		if (Input.GetKeyDown("space")) 
		{
			if (isOnGround)
			{
				ApplyJump ();
				canDoubleJump = true;
				isOnGround = false;
			} 
			else 
			{
				if (canDoubleJump) 
				{
					canDoubleJump = false;
					ApplyJump ();
				}
			}
		}
	}

	//Check for Dash
	void CheckDash()
	{
		
	}

	//Apply Velocity Calcluations 
	void ApplyJump()
	{
		finalVel = CalclulateVel (jumpHeight, rb.velocity.y, 1.0f);
		acceleration = CalclulateAccel (finalVel, rb.velocity.y, 1.0f);
		jumpThrust = CalclulateJumpVel (rb.mass, acceleration);

		rb.AddForce(transform.right * jumpThrust, ForceMode2D.Impulse);
	}

	// Calculate velocity
	float CalclulateVel(float distance, float initialVel,float time)
	{
		return(distance / time) - initialVel / 2;
	}

	// Calclulate Acceleration
	float CalclulateAccel(float finalVel, float initialVel, float time)
	{
		return(finalVel - initialVel) / time;
	}

	// Calclulate Jump Velocity 
	float CalclulateJumpVel(float mass, float acceleration)
	{
		return mass * acceleration;
	}
}
