// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Networking;

// public class PlayerMoveOriginal : NetworkBehaviour {

// 	public static int numPlayers;// = 0;

// 	public float speed = 20;

// 	private Rigidbody rb;
// 	private MeshRenderer mr;
// 	private bool serv1st = true;
// 	private bool client1st = true;
// 	private float xPos = 45;

// 	public override void OnStartLocalPlayer() {
// 		//numPlayers += 1;
// 		//print ("in OnStartLocalPlayer in PlayerMove." numPlayers = " + numPlayers);

// 		rb = GetComponent<Rigidbody>();
// 		mr = GetComponent<MeshRenderer>();

// 		if(!isServer) {
// 			rb.position = new Vector3 (xPos, 0.5f, 0f);
// 			mr.material.color = Color.green;

// 			//numPlayers = numPlayers + 1;
// 			//print (PlayerMove.numPlayers + ": added client.");
// 		}
// 		else {
// 			//print ("is host");
// 			rb.position = new Vector3 (-xPos, 0.5f, 0f);
// 			mr.material.color = Color.blue;

// 			//numPlayers = 1;
// 			//print (PlayerMove.numPlayers + ": added server.");
// 		}

// 		//print ("number of connections: " + Network.connections.Length);
// 	}

// 	//maybe should try OnPlayerConnected because also have OnPlayerDisconnected so can decrement, too.

// 	public override void OnStartServer() {
// 		if (serv1st) {
// 			numPlayers = 1;
// 			serv1st = false;
// 		}
// 		//print ("in OnStartServer. numPlayers = " + numPlayers);
// 	}

// 	public override void OnStartClient() {
// 		if (client1st && !isServer) {

// 			print ("in client");
// 			numPlayers = numPlayers + 1;
// 			client1st = false;
// 		}
// 		//print ("in OnStartClient. numPlayers = " + numPlayers);
// 	}

// 	// Update is called once per frame
// 	void FixedUpdate () {

// 		if (!isLocalPlayer)
// 			return;

// 		if (!isServer){
// 			if (rb.position.x != xPos) {
// 				rb.MovePosition(new Vector3(xPos, rb.position.y, rb.position.z));
// 			}
// 		}
// 		else {
// 			if (rb.position.x != -xPos) {
// 				rb.MovePosition(new Vector3(-xPos, rb.position.y, rb.position.z));
// 			}
// 		}


// 		float moveVertical = Input.GetAxis ("Vertical");
// 		Vector3 movement = new Vector3 (0.0f, 0.0f, moveVertical * speed);
// 		rb.velocity = movement;

// 		//var x = Input.GetAxis("Horizontal")*0.1f;
// 		//var z = Input.GetAxis("Vertical")*0.1f;

// 		//transform.Translate(x, 0, z);
// 	}
// }
