﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour {

	public int playerId = 0;
	private Rigidbody2D rb;
	private Player player;
	private Vector3 moveVector;
	private bool dash;
	private bool jump;
	public float jumpHeight;
	private float dashThrust;
	public float dashCooldown;
	private float dashDelay;
	public float moveSpeed;
	private bool canDash;
	private bool movingRight;

	public float dashLength;
	public float dashSpeed;


	private float finalVel;
	private float acceleration;
	private float thrust; 

	public float pushRadius;
	public float pushForce;
	private LayerMask enemy = 1 <<8;

	private bool isOnGround = true;
	private bool canDoubleJump = false;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody2D> ();
		player = ReInput.players.GetPlayer(playerId);


	}

	//Check if your on the ground
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Floor")
		{
			Debug.Log ("OnGround");
			isOnGround = true;
			Physics2D.gravity = new Vector2( 0.0f , -9.8f);
		}
	}

	// Update is called once per frame
	void Update () {
		player = ReInput.players.GetPlayer(playerId);
		GetInput ();
		CheckJump ();
		CheckBlast ();
		//dash delay timer
		dashDelay -= Time.deltaTime;
		if (dashDelay <= 1)
		{
			rb.velocity = new Vector2 (0, rb.velocity.y);
		}

		//playing with the push away feature 

	}

	void FixedUpdate()
	{
		CheckMovement ();
		CheckDash ();
	}

	void GetInput()
	{
		moveVector.x = player.GetAxis("MoveX"); // get input by name or action id
		jump = player.GetButton("Jump");
		dash = player.GetButton ("Dash");
	}

	//Check For Jump
	void CheckJump()
	{
		if (player.GetButtonDown("Jump")) 
		{
			if (isOnGround)
			{
				rb.velocity = new Vector3 (rb.velocity.x, 0);
				ApplyJump ();
				canDoubleJump = true;
				isOnGround = false;
			} 
			else 
			{
				if (canDoubleJump) 
				{
					
					Physics2D.gravity = Physics2D.gravity*1.5f;
					rb.velocity = new Vector2(rb.velocity.x	, 0);
					ApplyJump ();
					canDoubleJump = false;
				}
			}
		}
	}
		

	//Check for Dash
	void CheckDash()
	{

		if (player.GetButtonDown ("Dash") && dashDelay <= 0) {
			canDash = true;
			Physics2D.gravity = new Vector2 (0.0f, 0.0f);
			dashDelay = dashCooldown;
		} else {
			canDash = false;
			Physics2D.gravity = new Vector2( 0.0f , -9.8f);
		}
	}

	void CheckMovement()
	{

		if (moveVector.x > 0.0f) {
			Debug.Log ("Movingright");
			movingRight = true;

			ApplyMovement ();
		} else if(moveVector.x < 0.0f) {
			Debug.Log ("MovingLeft");
			movingRight = false;

			ApplyMovement ();
		}
	}

	void CheckBlast()
	{
		if (player.GetButtonDown ("Blast"))
		{
			Debug.Log ("Here1");
			Collider2D[] colliders = Physics2D.OverlapCircleAll (this.transform.position, pushRadius, enemy);
			foreach (Collider2D collider in colliders)
			{
				Debug.Log ("Here2");
				Vector2 direction = collider.transform.position - transform.position;
				direction = direction.normalized;
				Rigidbody2D body = collider.gameObject.GetComponent<Rigidbody2D> ();
				body.AddForce (direction * pushForce, ForceMode2D.Impulse);
				Debug.Log (collider);
			}
		}
	}

	//Apply Velocity Calcluations 
	void ApplyJump()
	{
		finalVel = CalclulateVel (jumpHeight, rb.velocity.y, 1.0f);
		acceleration = CalclulateAccel (finalVel, rb.velocity.y, 1.0f);
		thrust = CalclulateJumpVel (rb.mass, acceleration);

		rb.AddForce(transform.right * thrust, ForceMode2D.Impulse);
	}

	void ApplyMovement()
	{
		
		//rb.AddForce (this.transform.forward + moveVector * moveSpeed, ForceMode2D.Force);
		if (canDash)
		{
			if (movingRight) {
				float fVel = CalclulateVel (dashLength, rb.velocity.x, dashSpeed);
				float accel = CalclulateAccel (finalVel, rb.velocity.x, dashSpeed);
				dashThrust = CalclulateJumpVel (rb.mass, accel);
				rb.AddForce (this.transform.forward + new Vector3(1,0,0) * dashThrust, ForceMode2D.Impulse);
				//this.transform.position += moveVector * (moveSpeed * dashThrust)* Time.fixedUnscaledDeltaTime;
				Debug.Log ("Dash");
			} else
			{
				float fVel = CalclulateVel (dashLength, rb.velocity.x, dashSpeed);
				float accel = CalclulateAccel (finalVel, rb.velocity.x, dashSpeed);
				dashThrust = CalclulateJumpVel (rb.mass, accel);
				rb.AddForce (this.transform.forward + new Vector3(-1,0,0) * dashThrust, ForceMode2D.Impulse);
				//this.transform.position += moveVector * (moveSpeed * dashThrust)* Time.fixedUnscaledDeltaTime;
				Debug.Log ("Dash");
			}
		}
		else 
		{
			this.transform.localPosition += moveVector * moveSpeed * Time.deltaTime;
//			Debug.Log ("Moving");
		}
	}

//	void ApplyDash()
//	{
//		//rb.AddForce (this.transform.forward + moveVector * dashThrust, ForceMode2D.Impulse);
//		this.transform.localPosition += moveVector * dashThrust * Time.deltaTime; 
//		
//	}

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
