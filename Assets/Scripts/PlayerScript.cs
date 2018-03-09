using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class PlayerScript : MonoBehaviour {
	public float Xspeed = 30f;
	public float Yspeed = 30f;
	public float Ballspeed = 10f;

	private Rigidbody rb;
	private Rigidbody rpUD;
	private Rigidbody rpLR;

	public GameObject Ball;
	public GameObject LRPaddle;
	public GameObject UDPaddle;

	int Hit1 = 0;
	int Hit2 = 0;
	int Hit3 = 0;
	int Hit4 = 0;

	int Miss1 = 0;
	int Miss2 = 0;
	int Miss3 = 0;
	int Miss4 = 0;

	BCIClass BCI = new BCIClass();

	void Start () {
		BCI.receiveThread = new Thread(() => BCI.receiveData(BCI.receivePort));
		BCI.receiveThread.IsBackground = true;
		BCI.receiveThread.Start();
		Application.runInBackground = true;

		//BCI.startBCI2000 ();
		rb = Ball.GetComponent <Rigidbody> ();
		rpUD = UDPaddle.GetComponent <Rigidbody> ();
		rpLR = LRPaddle.GetComponent <Rigidbody> ();
		rb.AddForce (
			(new Vector3(UnityEngine.Random.Range(-20.0f, 20.0f), 0f, UnityEngine.Random.Range(-20.0f, 20.0f)).normalized) * Ballspeed, 
			ForceMode.Impulse); // Random impulse force
	}

	// Update is called once per frame
	void FixedUpdate () {
		rpUD.velocity =  (new Vector3(0f, 0f, BCI.SignalCode2 * Yspeed)); 
		rpLR.velocity =  (new Vector3(BCI.SignalCode * Xspeed, 0f, 0f));
		print (BCI.CursorPosX);
		//UnityEngine.Debug.Log ("X vel is " + BCI.SignalCode); 
		//UnityEngine.Debug.Log ("Y vel is " + BCI.SignalCode2); 
	}

	public void OnDestroy () {
		UnityEngine.Debug.Log ("Quitting...Closing UDP connection");
		BCI.client.Close ();
	}

	void OnCollisionEnter (Collision col) {
		// Hit Log when ball hits paddles
		if (col.gameObject == UDPaddle && UDPaddle.transform.position.z >= 3.5000000f) {// Hit left top quadrant
			Hit3++;
			UnityEngine.Debug.Log ("Hit Left Top is " + Hit3);
			//BCI.setState ("Target",3);
			//BCI.setState ("Result",3);
		}
		if (col.gameObject == UDPaddle && UDPaddle.transform.position.z < 3.5000000f) {// hit left bottom quadrant
			Hit4++;
			UnityEngine.Debug.Log ("Hit Left Bottom is " + Hit4); 
			//BCI.setState ("Target",4);
			//BCI.setState ("Result",4);
		}
		if (col.gameObject == LRPaddle && LRPaddle.transform.position.x < -3.2500000f) { // Hit top left quadrant
			Hit1++;
			UnityEngine.Debug.Log ("Hit Top Left is " + Hit1);
			//BCI.setState ("Target",1);
			//BCI.setState ("Result",1);
		}
		if (col.gameObject == LRPaddle && LRPaddle.transform.position.x >= -3.2500000f) { // Hit top right quadrant
			Hit2++;
			UnityEngine.Debug.Log ("Hit Top Right is " + Hit2);
			//BCI.setState ("Target",2);
			//BCI.setState ("Result",2);
		}

		// Miss Log when ball hits borders
		if (col.gameObject.tag == "TopRight") 	{
			Miss2++;
			UnityEngine.Debug.Log ("Miss Top Right is " + Miss2);
			//BCI.setState ("Target",2);
			//BCI.setState ("Result",0);
		}
		if (col.gameObject.tag == "TopLeft") 	{
			Miss1++;
			UnityEngine.Debug.Log ("Miss Top Left is " + Miss1); 
			//BCI.setState ("Target",1);
			//BCI.setState ("Result",0);
		}
		if (col.gameObject.tag == "LeftTop") 	{
			Miss3++;
			UnityEngine.Debug.Log ("Miss Left Top is " + Miss3); 
			//BCI.setState ("Target",3);
			//BCI.setState ("Result",0);
		}
		if (col.gameObject.tag == "LeftBottom") {
			Miss4++;
			UnityEngine.Debug.Log ("Miss Left Bottom is " + Miss4);
			//BCI.setState ("Target", 4);
			//BCI.setState ("Result", 0);
		}
	}
}

