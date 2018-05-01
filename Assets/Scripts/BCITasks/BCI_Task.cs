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
public class BCI_Task : MonoBehaviour
{


 	//Cursor and Targets for Cursor Task
	public GameObject ball,LeftTarget,RightTarget,TopTarget,BottomTarget;
	public List<GameObject> Targets;

  	// Objects related to the Player and controllers
  	public GameObject LController,RController,Player;
	//private SteamVR_Controller.Device controller {get{return SteamVR_Controller.Input((int) RController.GetComponent<SteamVR_TrackedObject>().index);}}
 // 	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
 // 	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
  	private bool triggerButtonDown,triggerButtonUp,triggerButtonPressed;


	private string selectedSource, selectedProcessing, selectedApp;
	public Dropdown SignalSource, SignalProcessing, SignalApplication;

    public string BCI2000Location = "D:\\Projects\\Hopkins\\GIT_bci2000web\\prog";

    public bool skip = false;

 	// Target, session, and score counter
  	public int overallNum = 0;
  	public int counter = 0;
  	private int hits, misses, timeouts, pvc = 0;
  	public GameObject hitBox, timeoutBox, pvcBox;

  	// Create new instance of BCI_Class
	public BCI_Class[] BCIinst;
	public string task;


	public void OpenShell(bool start)
	{
        BCIinst[1] = new BCI_Class("127.0.0.1", 55404);
        ProcessStartInfo PSI = new ProcessStartInfo(BCI2000Location+"\\operator.exe");
		PSI.Arguments = "--StartupIdle --Telnet 127.0.0.1:" + "3"+ "997 --AllowMultipleInstances";
		Process.Start(PSI);

		BCIinst[1].tcpClient = new TcpClient("127.0.0.1", 3997);
		BCIinst[1].stream = BCIinst[1].tcpClient.GetStream();
		if (start == true) {
			configureTask ();
		}
	}

	IEnumerator BCI2000_operator(int inst, string num, string source, string process, string app)
	{
		BCIinst[1].tcpClient = new TcpClient("127.0.0.1", 3997);
		BCIinst[1].stream = BCIinst[1].tcpClient.GetStream();
        UnityEngine.Debug.Log(app);
		string msg = "STARTUP SYSTEM * SignalSource:" + num+ "000 SignalProcessing:" + num+ "001 Application:" + num+ "002";
		string msgX = "Change directory Assets\\BCI2000\\prog";
		string msg1 = "Start executable " + source + " 127.0.0.1:" + num+ "000 --AllowMultipleInstances";
		string msg2 = "Start executable " + process + " 127.0.0.1:" + num+ "001 --AllowMultipleInstances";
		string msg3 = "Start executable " + app + " 127.0.0.1:" + num+ "002 --AllowMultipleInstances";
		string msg4 = "Wait for Connected";
		string msg5 = "Load parameterfile ..\\parms\\VR_"+app.Substring(11,2)+"_Candle.prm";
		string msg6 = "Set config";
		string msg7 = "Start";

		sendMsg (inst, "\n");

		sendMsg (inst, msg);
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
		BCIinst[1].receiveThread = new Thread(() => BCIinst[1].receiveData(BCIinst[1].receivePort));
		BCIinst[1].receiveThread.IsBackground = true;
		BCIinst[1].receiveThread.Start();
	}
		

	public void sendMsg(int inst, string msg)
	{
		BCIinst[1].stream.Write(System.Text.Encoding.ASCII.GetBytes(msg),0,System.Text.Encoding.ASCII.GetBytes(msg).Length);
	}

	public void configureTask()
	{
		selectedSource =SignalSource.options [SignalSource.value].text;
		selectedProcessing = SignalProcessing.options[SignalProcessing.value].text;
		selectedApp = SignalApplication.options[SignalApplication.value].text;
		StartCoroutine(BCI2000_operator(0, "3", selectedSource, selectedProcessing,selectedApp));
	}

	public void Awake()
	{
		BCIinst = new BCI_Class[2];

		SignalSource = GameObject.Find ("Signal Source").GetComponent<Dropdown> ();
		SignalProcessing = GameObject.Find ("Signal Processing").GetComponent<Dropdown> ();
		SignalApplication = GameObject.Find ("Signal Application").GetComponent<Dropdown> ();
	}

  	public void Start ()
	{
		task = GameObject.Find ("Canvas").GetComponent<GUI_Handler>().miTask;

		if (task == "LR")
		{
			LeftTarget = (GameObject)Instantiate (Resources.Load ("Target"), new Vector3 (-5, 0, 0), new Quaternion (0, 0, 0, 0));
			RightTarget = (GameObject)Instantiate (Resources.Load ("Target"), new Vector3 (5, 0, 0), new Quaternion (0, 0, 0, 0));
			ball = (GameObject)Instantiate (Resources.Load ("Cursor"), new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0));

			Targets.Add (RightTarget);
			Targets.Add (LeftTarget);
		}
		else if(task == "UD")
		{
			TopTarget = (GameObject)Instantiate (Resources.Load ("Target"), new Vector3 (0, -5, 0), Quaternion.Euler (0, 0, 90));
			BottomTarget = (GameObject)Instantiate (Resources.Load ("Target"), new Vector3 (0, 5, 0), Quaternion.Euler (0, 0, 90));
			ball = (GameObject)Instantiate (Resources.Load ("Cursor"), new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0));
			Targets.Add (BottomTarget);
			Targets.Add (TopTarget);
		}
		else if(task == "2D")
		{
			LeftTarget = (GameObject)Instantiate (Resources.Load ("Target"), new Vector3 (-5, 0, 0), new Quaternion (0, 0, 0, 0));
			RightTarget = (GameObject)Instantiate (Resources.Load ("Target"), new Vector3 (5, 0, 0), new Quaternion (0, 0, 0, 0));
			TopTarget = (GameObject)Instantiate (Resources.Load ("Target"), new Vector3 (0, 5, 0), Quaternion.Euler (0, 0, 90));
			BottomTarget = (GameObject)Instantiate (Resources.Load ("Target"), new Vector3 (0, -5, 0), Quaternion.Euler (0, 0, 90));
			ball = (GameObject)Instantiate (Resources.Load ("Cursor"), new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0));
			Targets.Add (RightTarget);
			Targets.Add (LeftTarget);
			Targets.Add (TopTarget);
			Targets.Add (BottomTarget);
			//1 right
			//2left
			//3 top
			//4 bottom
		}



		//Resets VR views so it starts in the proper position
		//if (UnityEngine.XR.XRDevice.isPresent)
		//{
		//	triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
		//	gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
		//	Valve.VR.OpenVR.System.ResetSeatedZeroPose ();
		//	Valve.VR.OpenVR.Compositor.SetTrackingSpace (
		//		Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);
		//	//Disable blue Vive box around player
		//	Player.GetComponent<MeshRenderer> ().enabled = false;

		//}
	}

  public void moveWithBCI()
	{
		if (BCIinst[1].Feedback == 1 && skip == false)
		{
			counter = counter + 1;
			skip = true;
		}
		if (BCIinst[1].ResultCode == 0 && BCIinst[1].TargetCode != 0)
		{
			Targets [(int)BCIinst[1].TargetCode - 1].GetComponent<Renderer> ().material.color = new Color (0, 255, 8, 0);
		}
		if (BCIinst[1].TargetCode == 0)
		{
			skip = false;
			if (task == "LR") {
				LeftTarget.GetComponent<Renderer> ().material.color = Color.white;
				RightTarget.GetComponent<Renderer> ().material.color = Color.white;
			} else if (task == "UD") {
				TopTarget.GetComponent<Renderer> ().material.color = Color.white;
				BottomTarget.GetComponent<Renderer> ().material.color = Color.white;
			} else if (task == "2D") {
				LeftTarget.GetComponent<Renderer> ().material.color = Color.white;
				TopTarget.GetComponent<Renderer> ().material.color = Color.white;
				BottomTarget.GetComponent<Renderer> ().material.color = Color.white;
				RightTarget.GetComponent<Renderer> ().material.color = Color.white;
					
			}
		}

		if ((BCIinst[1].ResultCode == BCIinst[1].TargetCode) && BCIinst[1].TargetCode != 0)
		{
			Targets [(int)BCIinst[1].ResultCode - 1].GetComponent<Renderer> ().material.color = Color.blue;
			ball.transform.position = Targets [(int)BCIinst[1].ResultCode - 1].transform.position;
		} else if (BCIinst[1].ResultCode != BCIinst[1].TargetCode && BCIinst[1].ResultCode != 0)
		{
			Targets [(int)BCIinst[1].ResultCode - 1].GetComponent<Renderer> ().material.color = Color.red;
			ball.transform.position = Targets [(int)BCIinst[1].ResultCode - 1].transform.position;
		}
		if (BCIinst[1].Feedback == 1)
		{
			ball.transform.localPosition = Vector3.Lerp (ball.transform.localPosition, new Vector3 ((BCIinst[1].CursorPosX / 400f), (BCIinst[1].CursorPosY / 400f), 0),.5f);
		}

		if (BCIinst[1].RunningState == 0)
		{
//			LeftTarget.GetComponent<Renderer> ().material.color = Color.blue;
//			RightTarget.GetComponent<Renderer> ().material.color = Color.blue;
//			TopTarget.GetComponent<Renderer> ().material.color = Color.blue;
//			BottomTarget.GetComponent<Renderer> ().material.color = Color.blue;
	}
	}
	public void OnDestroy()
	{
		Application.Quit ();
		BCIinst[1].tcpClient.Close ();
		BCIinst[1].client.Close();

	}

  public void Update ()
	{
		moveWithBCI ();
		//Basics for continuous paradigm
		/*
    ball.transform.localPosition = Vector3.Lerp (ball.transform.localPosition, new Vector3 ((-BCI2000.Cursor_PosX_u), (BCI2000.Cursor_PosY_u)), 5f);
*/



		//Scoreboard
		//	pvcBox.GetComponent<Text> ().text = pvc.ToString();
		//	timeoutBox.GetComponent<Text> ().text = timeouts.ToString();
		//	hitBox.GetComponent<Text> ().text = hits.ToString();



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
