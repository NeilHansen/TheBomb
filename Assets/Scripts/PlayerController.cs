using System.Collections;
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
	public float dashThrust;
	public float moveSpeed;

	private float finalVel;
	private float acceleration;
	private float jumpThrust; 

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
		}
	}

	// Update is called once per frame
	void Update () {
		GetInput ();
		CheckJump ();
		CheckMovement ();
	}

	void FixedUpdate()
	{
		
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
				//rb.velocity = new Vector3 (rb.velocity.x, 0);
				ApplyJump ();
				canDoubleJump = true;
				isOnGround = false;
			} 
			else 
			{
				if (canDoubleJump) 
				{
					canDoubleJump = false;
					//rb.velocity = new Vector2(rb.velocity.x	, 0);
					ApplyJump ();
				}
			}
		}
	}
		

	//Check for Dash
	void CheckDash()
	{
		
	}

	void CheckMovement()
	{
		if(moveVector.x != 0.0f || moveVector.y != 0.0f) {
			//cc.Move(moveVector * moveSpeed * Time.deltaTime);
			rb.MovePosition(this.transform.position + moveVector * moveSpeed * Time.deltaTime);
		}     
//		if (moveVector.x != 0.0f) {
//			//cc.Move(moveVector * moveSpeed * Time.deltaTime);
//			rb.AddForce (this.transform.forward + moveVector * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
//		}
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
