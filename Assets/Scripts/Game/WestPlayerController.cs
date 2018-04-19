using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WestPlayerController : MonoBehaviour {

	public float speed;						//make speed a public variable so it can be changed in Unity
	//public int playerNumber = 1;			
	//public float size = 3.75f;
	
	private Rigidbody rb;					
	//private string motionControlName;				//to be used with playerNumber to get the correct name of the control input for motion control
	
	void Awake () {
		
		rb = GetComponent<Rigidbody>();
		//rb.position = new Vector3((-47.0f + size), 0.5f, 0.0f);
		//rb.transform.localScale += new Vector3(size, 0, size);
		
	}
	
	/*
	// Use this for initialization
	void Start () {
		
		motionControlName = "Vertical" + playerNumber;
		
	}
	*/
	
	// Update is called once per frame
	void FixedUpdate () {
		
		//float moveVertical = Input.GetAxis(motionControlName);
		float moveVertical = Input.GetAxis("VerticalWest");

        Vector3 movement = new Vector3(0.0f, 0.0f, moveVertical * speed);

        //rb.AddForce(movement * speed);
		rb.velocity = movement;
		
	}
}
