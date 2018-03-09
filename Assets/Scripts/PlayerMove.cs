using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Diagnostics;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class PlayerMove : NetworkBehaviour {
	
	public static int numPlayers;

	public float speed = 20;
	
	private Rigidbody rb;
	private MeshRenderer mr;
	private bool serv1st = true;
	private bool client1st = true;
	private float xPos = 45;

	BCIClass BCI1 = new BCIClass ();
	//BCIClass BCI2 = new BCIClass();

	public void Awake()
	{
		BCI1.receivePort = 55404;
		BCI1.IP = "127.0.0.1";

		//BCI2.receivePort = 55405;
		//BCI2.IP = "192.168.0.5";
	}


	public override void OnStartLocalPlayer() {
		BCI1.receiveThread = new Thread (() => BCI1.receiveData (BCI1.receivePort));
		BCI1.receiveThread.IsBackground = true;
		BCI1.receiveThread.Start ();
		Application.runInBackground = true;

		//numPlayers += 1;
		//print ("in OnStartLocalPlayer in PlayerMove." numPlayers = " + numPlayers);

		rb = GetComponent<Rigidbody>();
		mr = GetComponent<MeshRenderer>();

        if(!isServer) {
			rb.position = new Vector3 (xPos, 0.5f, 0f);
			mr.material.color = Color.green;

			//numPlayers = numPlayers + 1;
			//print (PlayerMove.numPlayers + ": added client.");
		}
		else {
			//print ("is host");
			rb.position = new Vector3 (-xPos, 0.5f, 0f);
			mr.material.color = Color.blue;

			//numPlayers = 1;
			//print (PlayerMove.numPlayers + ": added server.");
		}

		//print ("number of connections: " + Network.connections.Length);
    }

	//maybe should try OnPlayerConnected because also have OnPlayerDisconnected so can decrement, too.

	public override void OnStartServer() {
		if (serv1st) {
			numPlayers = 1;
			serv1st = false;
		}
		//print ("in OnStartServer. numPlayers = " + numPlayers);
	}

	public override void OnStartClient() {
		if (client1st && !isServer) {

			//print ("in client");
			numPlayers = numPlayers + 1;
			client1st = false;
		}
		//print ("in OnStartClient. numPlayers = " + numPlayers);
	}
	
	// Update is called once per frame
	void Update () {

		print (BCI1.CursorPosX);

		if (!isLocalPlayer)
			return;

		if (!isServer){
			if (rb.position.x != xPos) {
				rb.MovePosition(new Vector3(xPos, rb.position.y, rb.position.z));
			}
		}
		else {
			if (rb.position.x != -xPos) {
				rb.MovePosition(new Vector3(-xPos, rb.position.y, rb.position.z));
			}
		}

		Vector3 movement = new Vector3 (0f, 0f, BCI1.CursorPosY/300f);
		rb.velocity = movement;

		//float moveVertical = Input.GetAxis ("Vertical");
		//Vector3 movement = new Vector3 (0.0f, 0.0f, moveVertical * speed);
		//rb.velocity = movement;
	}

	public void OnDestroy () {
		UnityEngine.Debug.Log ("Quitting...Closing UDP connection");
		BCI1.client.Close ();
	}
}
