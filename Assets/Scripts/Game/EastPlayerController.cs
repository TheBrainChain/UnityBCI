using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class EastPlayerController : MonoBehaviour {

	public float speed;						//make speed a public variable so it can be changed in Unity
	//public int playerNumber = 1;			
	//public float size = 3.75f;
	public float impedanceY = 20f;
	
	private Rigidbody rb;					
	//private string motionControlName;				//to be used with playerNumber to get the correct name of the control input for motion control
	
	//BCIClass BCI = new BCIClass();
	
	void Awake () {
		
		//BCI.receiveThread = new Thread(() => BCI.receiveData(BCI.receivePort));
		//BCI.receiveThread.IsBackground = true;
		//BCI.receiveThread.Start();
		
		rb = GetComponent<Rigidbody>();
		//rb.position = new Vector3(47.0f - (size/2), 0.5f, 0.0f);
		//rb.transform.localScale += new Vector3(size, 0, size);
		
	}
	
	
	// Use this for initialization
	void Start () {
		
		//rb.position = new Vector3(47.0f - (size/2), 0.5f, 0.0f);
		//motionControlName = "Vertical" + playerNumber;
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		/* //float moveVertical = Input.GetAxis(motionControlName);
		float moveVertical = Input.GetAxis("VerticalEast");

        Vector3 movement = new Vector3(0.0f, 0.0f, moveVertical * speed);

        //rb.AddForce(movement * speed);
		rb.velocity = movement; */
		
		//rb.transform.localPosition = Vector3.Lerp (rb.transform.localPosition, new Vector3 (45f, 0.5f, BCI.SignalCode2 / impedanceY), .5f);
		
	}
	
	public void OnDestroy ()
	{
		//UnityEngine.Debug.Log ("Quitting...Closing UDP connection");
		//BCI.client.Close ();
	}
}
