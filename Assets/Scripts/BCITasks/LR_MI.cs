using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Net;
using System.Net.Sockets;
public class LR_MI : MonoBehaviour
{


	//Cursor and Targets for Cursor Task
	public GameObject ball,LeftTarget,RightTarget;
	public List<GameObject> Targets;

	// Objects related to the Player and controllers
	public GameObject LController,RController,Player;
	private SteamVR_Controller.Device controller {get{return SteamVR_Controller.Input((int) RController.GetComponent<SteamVR_TrackedObject>().index);}}
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private bool triggerButtonDown,triggerButtonUp,triggerButtonPressed;


	private string selectedSource, selectedProcessing, selectedApp;
	public Dropdown SignalSource, SignalProcessing, SignalApplication;



	public bool skip = false;

	// Target, session, and score counter
	public int overallNum = 0;
	public int counter = 0;

	// Create new instance of BCI_Class
	public BCI_Class BCIinst;
	public string task;

	public IEnumerator a;
	public void openShell()
	{	
		BCIinst = new BCI_Class ("127.0.0.1", 55404);
		ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2000\\prog\\operator.exe");
		PSI.Arguments = "--StartupIdle --Telnet 127.0.0.1:" + "3"+ "997 --AllowMultipleInstances";
		Process.Start(PSI);
		BCIinst.tcpClient = new TcpClient("127.0.0.1", 3997);
		BCIinst.stream = BCIinst.tcpClient.GetStream();
		a = BCI2000_operator (0, "3", "SignalGenerator", "ARSignalProcessing", "CursorTask");
		StartCoroutine(a);
	}

	IEnumerator BCI2000_operator(int inst, string num, string source, string process, string app)
	{

		string msg = "STARTUP SYSTEM * SignalSource:" + num+ "000 SignalProcessing:" + num+ "001 Application:" + num+ "002";
		string msgY = "Show window";
		string msgX = "Change directory Assets\\BCI2000\\prog";
		string msg1 = "Start executable " + source + " 127.0.0.1:" + num+ "000 --AllowMultipleInstances";
		string msg2 = "Start executable " + process + " 127.0.0.1:" + num+ "001 --AllowMultipleInstances";
		string msg3 = "Start executable " + app + " 127.0.0.1:" + num+ "002 --AllowMultipleInstances";
		string msg4 = "Wait for Connected";
		string msg5 = "Load parameterfile ..\\parms\\examples\\CursorTask_SignalGenerator.prm";
		string msg6 = "Set config";
		string msg7 = "Start";

		sendMsg (inst, "\n");

		sendMsg (inst, msg);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (1f);
		sendMsg (inst, msgY);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (1f);
		sendMsg (inst, msgX);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (1f);
		sendMsg (inst, msg1);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (1f);
		sendMsg (inst, msg2);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (1f);
		sendMsg (inst, msg3);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (1f);
		sendMsg (inst, msg4);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (1f);
		sendMsg (inst, msg5);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (1f);
		sendMsg (inst, msg6);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (1f);
		sendMsg (inst, msg7);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (1f);
		BCIinst.receiveThread = new Thread(() => BCIinst.receiveData(BCIinst.receivePort));
		BCIinst.receiveThread.IsBackground = true;
		BCIinst.receiveThread.Start();

	}


	public void sendMsg(int inst, string msg)
	{
		BCIinst.stream.Write(System.Text.Encoding.ASCII.GetBytes(msg),0,System.Text.Encoding.ASCII.GetBytes(msg).Length);
	}



	public void Start ()
	{
		task = "LR";

		if (task == "LR")
		{
			LeftTarget = (GameObject)Instantiate (Resources.Load ("Target"), new Vector3 (-5, 0, 0), new Quaternion (0, 0, 0, 0));
			RightTarget = (GameObject)Instantiate (Resources.Load ("Target"), new Vector3 (5, 0, 0), new Quaternion (0, 0, 0, 0));
			ball = (GameObject)Instantiate (Resources.Load ("Cursor"), new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0));

			Targets.Add (RightTarget);
			Targets.Add (LeftTarget);
		}

		//Resets VR views so it starts in the proper position
		if (UnityEngine.XR.XRDevice.isPresent)
		{
			triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
			gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
			Valve.VR.OpenVR.System.ResetSeatedZeroPose ();
			Valve.VR.OpenVR.Compositor.SetTrackingSpace (
				Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);
			//Disable blue Vive box around player
			Player.GetComponent<MeshRenderer> ().enabled = false;

		}
	}

	public void moveWithBCI()
	{
		print (BCIinst.CursorPosX);
		print ("Feedback" + BCIinst.Feedback);
		if (BCIinst.Feedback == 1 && skip == false)
		{
			counter = counter + 1;
			skip = true;
		}
		if (BCIinst.ResultCode == 0 && BCIinst.TargetCode != 0)
		{
			Targets [(int)BCIinst.TargetCode - 1].GetComponent<Renderer> ().material.color = new Color (0, 255, 8, 0);
		}
		if (BCIinst.TargetCode == 0)
		{
			skip = false;
			//	LeftTarget.GetComponent<Renderer> ().material.color = Color.white;
			//	RightTarget.GetComponent<Renderer> ().material.color = Color.white;
			//	TopTarget.GetComponent<Renderer> ().material.color = Color.white;
			//	BottomTarget.GetComponent<Renderer> ().material.color = Color.white;
		}

		if ((BCIinst.ResultCode == BCIinst.TargetCode) && BCIinst.TargetCode != 0)
		{
			Targets [(int)BCIinst.ResultCode - 1].GetComponent<Renderer> ().material.color = Color.blue;
			ball.transform.position = Targets [(int)BCIinst.ResultCode - 1].transform.position;
		} else if (BCIinst.ResultCode != BCIinst.TargetCode && BCIinst.ResultCode != 0)
		{
			Targets [(int)BCIinst.ResultCode - 1].GetComponent<Renderer> ().material.color = Color.red;
			ball.transform.position = Targets [(int)BCIinst.ResultCode - 1].transform.position;
		}
		if (BCIinst.Feedback == 1)
		{
			ball.transform.localPosition = Vector3.Lerp (ball.transform.localPosition, new Vector3 (-.013f, (BCIinst.CursorPosY / 1600f) + 1.37f, (-BCIinst.CursorPosX / 1300f)), .5f);
		}

		if (BCIinst.RunningState == 0)
		{
			//			LeftTarget.GetComponent<Renderer> ().material.color = Color.blue;
			//			RightTarget.GetComponent<Renderer> ().material.color = Color.blue;
			//			TopTarget.GetComponent<Renderer> ().material.color = Color.blue;
			//			BottomTarget.GetComponent<Renderer> ().material.color = Color.blue;
		}
	}

	public void OnDestroy()
	{
		StopCoroutine(a);

		Application.Quit ();
		BCIinst.tcpClient.Close ();
		BCIinst.stream.Close ();
		BCIinst.client.Close();

	}

	void Update ()
	{

		moveWithBCI ();

		//Buttons on Vive Controllers
		//   Vector2 touchpad = (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

		//    triggerButtonDown = controller.GetPressDown(triggerButton);                 //Trigger button will be used to mark location positions
		//  triggerButtonUp = controller.GetPressUp(triggerButton);
		//triggerButtonPressed = controller.GetPress(triggerButton);

		//if (touchpad.y > .20f || touchpad.y < -.20f)
		//{
		//Player.transform.position += Player.transform.up*Time.deltaTime*touchpad.y*3.5f;
		//  print (touchpad.y);
		//  }

		//Start experiment and disable controller rendering
		if (Input.GetKey (KeyCode.A) || triggerButtonDown)
		{
			//Start BCI2000 and begin reading data
			//			BCI2000.startExp ();
			//Disable controller renderer
			LController.SetActive (false);
			RController.SetActive (false);
		}

		//	moveWithBCI ();
		/*
 * if (counter == 25)
    {
      overallNum = overallNum + 1;
      counter = 0;
      if (overallNum == 2 && BCI2000.Feedback == 0)
      {
        task = null;
        OnDestroy ();
        LRButton.transform.localScale = new Vector3 (.5f, 0.5f, 0.5f);
        UDButton.transform.localScale = new Vector3 (.5f, 0.5f, 0.5f);
        TwoDButton.transform.localScale = new Vector3 (.5f, 0.5f, 0.5f);
        overallNum = 0;
      }
    }
		  */
	}
}
