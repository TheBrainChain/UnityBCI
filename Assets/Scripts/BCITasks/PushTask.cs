using System;
//using Leap.Unity;
//using Leap;
using System.Threading;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PushTask : MonoBehaviour 
{
	/*

	private LeapServiceProvider provider;
	public int opposite_BCITargetCode;
	public GameObject ball;
	public GameObject LeftBumperP1;	
	public GameObject RightBumperP1;
	public GameObject LeftBumperP2;	
	public GameObject RightBumperP2;

	public BCI_Class BCI2000 = new BCI_Class();

	public void Start () 
	{
		BCI2000.receiveThread = new Thread(() => BCI2000.receiveData(BCI2000.receivePort));
		BCI2000.receiveThread.IsBackground = true;
		BCI2000.receiveThread.Start();

		ball.SetActive(false);
	}



	private IEnumerator nextTrial(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		targetState = Mathf.Round(Random.value+1f);
		if (targetState == 1)
		{
			opposite_targetstate = 2;
		} else
		{
			opposite_targetstate = 1;
		}
		skip = false;
	}

	private IEnumerator showBall(float waitTime)
	{        
		yield return new WaitForSeconds(waitTime);
		LeftTarget.GetComponent<Renderer>().material.color = Color.white;
		RightTarget.GetComponent<Renderer>().material.color = Color.white;
		ball.SetActive(true);
	}

	public void moveWithBCI()
	{
		if (targetState == 1)
		{
			opposite_BCITargetCode = 2;
		}
		if (targetState == 2)
		{
			opposite_BCITargetCode = 1;
		}
		if (targetState == 1 || targetState == 2)
		{   

			ball.transform.localPosition = Vector3.Lerp(ball.transform.localPosition, new Vector3((BCI2000.SignalCode*30)-34.04f, 0f, 59.2f), .5f);

			if (skip == false) 
			{
				BCI2000.setState ("Feedback", 1);

				Targets[(int)targetState-1].GetComponent<Renderer> ().material.color = Color.yellow;
				skip = true;
				BCI2000.setState ("Target", targetState);
			}
			if (ball.GetComponent<colliderscript> ().a == targetState)
			{
				Targets[(int)targetState-1].GetComponent<Renderer> ().material.color = Color.blue;
				BCI2000.setState ("Result", targetState);

				targetState = 0;
				resultState = 0;
				BCI2000.setState ("Feedback", 0);
				skip = false;
				ball.SetActive (false);
				ball.transform.position = new Vector3 (-34.03f, 0, 59.2f);
				ball.GetComponent<colliderscript> ().a = 0;
			} 
			else if (ball.GetComponent<colliderscript> ().a == opposite_targetstate)
			{
				Targets[(int)opposite_BCITargetCode-1].GetComponent<Renderer>().material.color = Color.red;
				BCI2000.setState ("Result", opposite_targetstate);

				targetState = 0;
				resultState = 0;
				BCI2000.setState ("Feedback", 0);
				skip = false;
				ball.SetActive (false);
				ball.transform.position = new Vector3 (-34.03f, 0, 59.2f);
				ball.GetComponent<colliderscript> ().a = 0;
			}
		}
		if (targetState == 0)
		{
			if (skip == false)
			{
				BCI2000.setState ("Target", targetState);

				skip = true;
				coroutine = showBall (1.5f);
				StartCoroutine (coroutine);

				coroutine = nextTrial (2.5f);
				StartCoroutine (coroutine);
			}
		}
	}

	public void OnDestroy()
	{
		UnityEngine.Debug.Log("Quitting...Closing UDP connection");
		BCI2000.client.Close();
		ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2000\\prog\\BCI2000Shell.exe");
		PSI.Arguments = "-c Stop; Exit";
		Process.Start(PSI);
		Application.Quit ();
	}

	void Update ()
	{
		if (task == "BCI")
		{
			moveWithBCI();
		}
	}
	*/
}