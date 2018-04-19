using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuckController2 : MonoBehaviour {

	public float puckSpeed;
	public int playTo;

	private Rigidbody puckRb;
	private float xDirection;
	private float randomX;
	private float randomZ;
	private bool gameStart;						//has the game started?
	private bool gameOver;						//has the game finished?


	void Start () {

		puckRb = GetComponent<Rigidbody>();
		
		gameStart = false;
		gameOver = false;

		InitiateMovement("none");

	}

	// Update is called once per frame
	void FixedUpdate () {

		//print (PlayerMove.numPlayers + " PuckController2");


		if (!gameStart) {
			//print ("in !gameStart if statement");
			InitiateMovement("none");
			return;
		}
		
		//if statement to check to ensure that the magnitude of the puck's velocity is not slower than the specified puckSpeed and updates accordingly.
		if (puckRb.velocity.magnitude < puckSpeed) {
			puckRb.velocity = puckRb.velocity * 2;
		}

		//if statement to make sure puck doesn't get stuck moving only in z direction.
		if (Mathf.Abs(puckRb.velocity.x) < puckSpeed/2) {
			if (xDirection < 0) {
				puckRb.velocity = new Vector3(puckRb.velocity.x - puckSpeed/4, 0, puckRb.velocity.z);
			}
			else if (xDirection > 0) {
				puckRb.velocity = new Vector3(puckRb.velocity.x + puckSpeed/4, 0, puckRb.velocity.z);
			}
		}

	}

	//initiates movement based on the last goal scored, as long as the game isn't over.
	void InitiateMovement(string lastGoal) {

		puckRb.velocity = new Vector3(0, 0, 0);
		
		if (!gameOver) {
            print("GameOver");
            //print ("in !gameOver if statement. number of connections: " + Network.connections.Length);
            //print ("in !gameOver if statement. numPlayers: " + PlayerMove.numPlayers);
            //no goals (beginning of game) so set puck to center and move either towards east or west once there are 2 players.
            //if (lastGoal == "none" && PlayerMove.numPlayers == 2) {//Network.connections.Length == 2) {
            //if (lastGoal == "none" && GameManager.numPlayers == 2) {
            //	UnityEngine.Debug.Log("1st check");
            //	puckRb.position = new Vector3(0, 0, 0);
            //	gameStart = true;
            //	randomX = Random.value;
            //	if (randomX < 0.5) {
            //	UnityEngine.Debug.Log("2st check");
            //		puckRb.AddForce(puckSpeed, 0, 0, ForceMode.Impulse);
            //	}
            //	else {
            //	UnityEngine.Debug.Log("3st check");
            //		puckRb.AddForce(-1 * puckSpeed, 0, 0, ForceMode.Impulse);
            //	}
            //	UnityEngine.Debug.Log("4st check");
        }
        //last goal scored on east side so set puck for east player on either north or south side of board, wait, and move towards east wall
        else if (lastGoal == "East") {
				randomZ = Random.value;
				if (randomZ < 0.5) {
					puckRb.position = new Vector3(27.7f, 0.0f, -13.7f);
				}
				else {
					puckRb.position = new Vector3(27.7f, 0.0f, 13.7f);
				}
				//WaitTime(5);
				puckRb.AddForce(puckSpeed/2, 0, 0, ForceMode.Impulse);
			}
			//last goal scored on west side so set puck for west player on either north or south side of board, wait, and move towards west wall
			else if (lastGoal == "West") {
				randomZ = Random.value;
				if (randomZ < 0.5) {
					puckRb.position = new Vector3(-27.7f, 0.0f, -13.7f);
				}
				else {
					puckRb.position = new Vector3(-27.7f, 0.0f, 13.7f);
				}
				//WaitTime(5);
				puckRb.AddForce(-1 * puckSpeed/2, 0, 0, ForceMode.Impulse);
			}
			else {
				return;
				//should I throw error if not passed "none", "East", or "West"???
			}

	}

	/* void WaitTime(int wait) {
		for(int t = 0; t < 100000 * wait; t++) {
			//time delay loop
		}
	} */

	void OnCollisionEnter(Collision other) {

		//bounce off Player objects
		if (other.gameObject.CompareTag("Player")) {
			//puckRb.AddForce(other.relativeVelocity, ForceMode.Impulse);
			puckRb.velocity = other.relativeVelocity;
		}

	}

	void OnCollisionExit (Collision collisionInfo) {

		//keep track of puck's x direction after last collision if the velocity in the x direction isn't 0, otherwise keep previous state.
		if (collisionInfo.relativeVelocity.x != 0) {
			xDirection = puckRb.velocity.x;
		}

	}

	void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.CompareTag("EastGoal")) {
			WestScoreManager.wScore += 1;


			if (WestScoreManager.wScore == playTo) {
				WinTextManager.winText = "Blue Wins!\nBlue: " + WestScoreManager.wScore + "\nGreen: " + EastScoreManager.eScore;
				gameOver = true;
			}

			InitiateMovement("East");
		}
		else if (collision.gameObject.CompareTag("WestGoal")) {
			EastScoreManager.eScore += 1;

			if (EastScoreManager.eScore == playTo) {
				WinTextManager.winText = "Green Wins!\nBlue: " + WestScoreManager.wScore + "\nGreen: " + EastScoreManager.eScore;
				gameOver = true;
			}

			InitiateMovement("West");
		}
	}
}
