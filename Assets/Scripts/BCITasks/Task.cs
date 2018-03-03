//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using System.Diagnostics;

//public class Task : MonoBehaviour {


//	public GameObject CollabPortal, CandlePortal;
//	//public GameObject TTTPortal, SnakePortal, PongPortal, ;
//	public GameObject CollabParticle; 
//	// public GameObject TTTParticle, SnakeParticle, PongParticle;

//	public CursorTask_mimic Cursor = new CursorTask_mimic();
//	public CandleTask Candle = new CandleTask();

//	public GameObject CursorTask1;
//	public GameObject CursorTask2;

//	void Update()
//	{
//		if (Input.GetKey(KeyCode.DownArrow))	
//		{
//			transform.position += transform.up * -1 * Time.deltaTime;
//		}
//		if (Input.GetKey(KeyCode.UpArrow))
//		{
//			transform.position += transform.up * 1 * Time.deltaTime;
//		}
//	}






//	void OnTriggerEnter(Collider other)
//	{
//		if (other.gameObject.name == "CollabPortal")
//		{
//			CursorTask1.GetComponent<CursorTask_mimic> ().enabled = true;
////			CursorTask1.SetActive (true);
//			Cursor.changeTarget ();
//			CollabPortal.SetActive (false);
//			CollabParticle.SetActive (false);
//		}
//		else if (other.gameObject.name == "PongPortal")
//		{
//			UnityEngine.Debug.Log ("Pong!");
//		}
//		else if (other.gameObject.name == "TTTPortal")
//		{
//			UnityEngine.Debug.Log ("Tic Tac Toe!");
//		}
//		else if (other.gameObject.name == "SnakePortal")
//		{
//			UnityEngine.Debug.Log ("Snake!");
//		}
//		else if (other.gameObject.name == "CandlePortal")
//		{
//			CursorTask2.SetActive (true);
//			Candle.changeTarget ();
//			CandlePortal.SetActive (false);
//		}
//	}
//}