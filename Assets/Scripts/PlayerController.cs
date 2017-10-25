using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	private Rigidbody2D rb;
	public float jumpThrust;
	public float dashThrust;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown("space")) 
		{
			rb.AddForce(transform.right * jumpThrust);
		}
	}

	void FixedUpdate()
	{
//		if (Input.GetKey("space")) 
//		{
//			rb.AddForce(transform.right * jumpThrust);
//		}
	}
}
