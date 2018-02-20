/*
 * Created by Christopher Coogan, BFINL, UMN 
 * 2016
 * 
 * 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;
using System.Threading;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorTask_mimic : MonoBehaviour 
{
	//Leap Motion provider
	private LeapServiceProvider provider;

	//Cursor and Targets for Cursor Task
	public GameObject ball,LeftTarget,RightTarget,TopTarget,BottomTarget;
	public List<GameObject> Targets;

	// Objects related to the Player and controllers
	public GameObject LController,RController,Player,controllerObj;
	private SteamVR_Controller.Device controller {get{return SteamVR_Controller.Input((int) controllerObj.GetComponent<SteamVR_TrackedObject>().index);}}
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private bool triggerButtonDown,triggerButtonUp,triggerButtonPressed = false;


	//Buttons to choose specific BCI MI task
	public GameObject LRButton, UDButton, TwoDButton;

	public string task;

	public int overallNum = 0;

	// new instance of BCI_Class
	public BCI_Class BCI2000 = new BCI_Class("127.0.0.1");

	private GameObject ParamFile;

	public int counter = 0;

	public bool skip = false;


	//private int hits, misses, timeouts, pvc = 0;
//	public GameObject hitBox, timeoutBox, pvcBox;

	public void Start () 
	{
		//define gameobjects from scene
		ParamFile = GameObject.Find ("Signal Application");
		LRButton = GameObject.Find ("LR_Button");
		UDButton = GameObject.Find ("UD_Button");
		TwoDButton = GameObject.Find ("2D_Button");

		//Hide the task selection buttons when Tasks scene begins
//		LRButton.transform.localScale = new Vector3 (0, 0, 0);
//		UDButton.transform.localScale = new Vector3 (0, 0, 0);
//		TwoDButton.transform.localScale = new Vector3 (0, 0, 0);

		//Add Targets to a list
		Targets.Add (RightTarget);
		Targets.Add (LeftTarget);
		Targets.Add (TopTarget);
		Targets.Add (BottomTarget);

		//Start a BCI2000 UDP receive thread
		BCI2000.receiveThread = new Thread(() => BCI2000.receiveData(BCI2000.receivePort));
		BCI2000.receiveThread.IsBackground = true;
		BCI2000.receiveThread.Start();

		//Resets VR views so it starts in the proper position
		Valve.VR.OpenVR.System.ResetSeatedZeroPose();
		Valve.VR.OpenVR.Compositor.SetTrackingSpace(
		Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);

		//Disable blue Vive box around player (I don't think this is working)
		Player.GetComponent<MeshRenderer> ().enabled = false;
	}
	public void moveBallWithLeap()
	{
		Frame currentFrame = provider.CurrentFrame;

		if (currentFrame.Hands.Count > 1)
		{
			List<Hand> hands = currentFrame.Hands;

			Hand hand1 = hands[0];
			Hand hand2 = hands[1];

			float pitch1 = hand1.Direction.Pitch;
			float pitch2 = hand2.Direction.Pitch;

			if ((hand1.GrabStrength < .9f && hand1.GrabStrength > .4f) || (hand2.GrabStrength < .9f && hand2.GrabStrength > .8f))
			{
				if (hand1.GrabStrength > .8f && hand2.GrabStrength > .8f)
				{
					ball.transform.Translate(0, 10 * Time.deltaTime, 0);
				}
				else if (hand1.IsRight && hand1.GrabStrength > 0.8f)
				{
					ball.transform.Translate(10 * Time.deltaTime, 0, 0);
				}
				else if (hand2.IsLeft && hand2.GrabStrength > 0.8f)
				{
					ball.transform.Translate(-10 * Time.deltaTime, 0, 0);
				}
				else if (hand2.IsRight && hand2.GrabStrength > 0.8f)
				{
					ball.transform.Translate(10 * Time.deltaTime, 0, 0);
				}
				else if (hand1.IsLeft && hand1.GrabStrength > 0.8f)
				{
					ball.transform.Translate(-10 * Time.deltaTime, 0, 0);
				}
			}
			if (pitch1 > 2.5f && pitch2 > 2.5f)
			{
				ball.transform.Translate(0, -5 * Time.deltaTime, 0);
			}
		}
	}

	//When the player enter's the portal, turn the targets/cursor on
	public void changeTarget()
	{
		ball.SetActive (true);
		RightTarget.SetActive (true);
		LeftTarget.SetActive (true);
		TopTarget.SetActive (true);
		BottomTarget.SetActive (true);
	}

	public void moveWithBCI()
	{
		if (BCI2000.Feedback == 1 && skip == false)
		{
			counter = counter + 1;
			skip = true;
		}
      if (BCI2000.ResultCode == 0 && BCI2000.TargetCode !=0)
		{   
			Targets [(int)BCI2000.TargetCode - 1].GetComponent<Renderer> ().material.color = new Color(0,255,8,0);
		}
		if (BCI2000.TargetCode == 0)
		{
			LeftTarget.GetComponent<Renderer> ().material.color = Color.white;
			RightTarget.GetComponent<Renderer> ().material.color = Color.white;
			TopTarget.GetComponent<Renderer> ().material.color = Color.white;
			BottomTarget.GetComponent<Renderer> ().material.color = Color.white;
			ball.transform.localPosition = new Vector3 (0,34.1f, 50f);
			skip = false;
		}

		if ((BCI2000.ResultCode == BCI2000.TargetCode) && BCI2000.TargetCode != 0)
		{
			Targets [(int)BCI2000.ResultCode - 1].GetComponent<Renderer> ().material.color = Color.blue;
			ball.transform.position = Targets [(int)BCI2000.ResultCode - 1].transform.position;
		//	hits = hits + 1;

		} else if (BCI2000.ResultCode != BCI2000.TargetCode && BCI2000.ResultCode != 0)
		{
			Targets [(int)BCI2000.ResultCode - 1].GetComponent<Renderer> ().material.color = Color.red;
			ball.transform.position = Targets [(int)BCI2000.ResultCode - 1].transform.position;
		//	misses = misses + 1;
		}
		if (BCI2000.Feedback == 1)
		{
			ball.transform.localPosition = Vector3.Lerp (ball.transform.localPosition, new Vector3 ((-BCI2000.CursorPosX/75), (BCI2000.CursorPosY/150) + 34.1f, 50f), .5f);
		}

		if (BCI2000.RunningState == 0)
		{
			LeftTarget.GetComponent<Renderer> ().material.color = Color.blue;
			RightTarget.GetComponent<Renderer> ().material.color = Color.blue;
			TopTarget.GetComponent<Renderer> ().material.color = Color.blue;
			BottomTarget.GetComponent<Renderer> ().material.color = Color.blue;
		}
	//	timeouts = 25 - hits - misses;
	//	pvc = hits / (25 - timeouts);

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

	public void UD_Task()
	{
		BCI2000.configureBCI2000Session ("NeuroscanClient","ARSignalProcessing", "CursorTask", null, BCI2000.IP, BCI2000.receivePort,"VR_UD");
		changeTarget ();
		BCI2000.receiveThread = new Thread(() => BCI2000.receiveData(BCI2000.receivePort));
		BCI2000.receiveThread.IsBackground = true;
		BCI2000.receiveThread.Start();
	}
	public void TwoD_Task()
	{
		BCI2000.configureBCI2000Session ("NeuroscanClient","ARSignalProcessing", "CursorTask", null, BCI2000.IP, BCI2000.receivePort,"VR_2D");
		changeTarget ();
		BCI2000.receiveThread = new Thread(() => BCI2000.receiveData(BCI2000.receivePort));
		BCI2000.receiveThread.IsBackground = true;
		BCI2000.receiveThread.Start();
	}
	public void LR_Task()
	{
		BCI2000.configureBCI2000Session ("NeuroscanClient","ARSignalProcessing", "CursorTask", null, BCI2000.IP, BCI2000.receivePort,"VR_LR");
		changeTarget ();
		BCI2000.receiveThread = new Thread(() => BCI2000.receiveData(BCI2000.receivePort));
		BCI2000.receiveThread.IsBackground = true;
		BCI2000.receiveThread.Start();
	}

	public void moveToPos()
	{
		Player.transform.position = new Vector3 (146.1f, 44.1f, 33.6f);
	}

	void Update ()
	{

		//Basics for continuous paradigm
		/*
		ball.transform.localPosition = Vector3.Lerp (ball.transform.localPosition, new Vector3 ((-BCI2000.Cursor_PosX_u), (BCI2000.Cursor_PosY_u)), 5f);
*/



		//Scoreboard
	//	pvcBox.GetComponent<Text> ().text = pvc.ToString();
	//	timeoutBox.GetComponent<Text> ().text = timeouts.ToString();
	//	hitBox.GetComponent<Text> ().text = hits.ToString();



		//Buttons on Vive Controllers
		Vector2 touchpad = (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

		triggerButtonDown = controller.GetPressDown(triggerButton);                 //Trigger button will be used to mark location positions
		triggerButtonUp = controller.GetPressUp(triggerButton);
		triggerButtonPressed = controller.GetPress(triggerButton);

		if (touchpad.y > .20f || touchpad.y < -.20f)
		{
			Player.transform.position += Player.transform.up*Time.deltaTime*touchpad.y*3.5f;
			print (touchpad.y);
		}

		//Start experiment and disable controller rendering
		if (triggerButtonDown)
		{
			BCI2000.startExp();
			LController.SetActive (false);
			RController.SetActive (false);
		}

		moveWithBCI ();

			if (ParamFile.GetComponent<Dropdown> ().value == 2)
		{
			task = "VR_LR";
		}
		else if (ParamFile.GetComponent<Dropdown> ().value == 3)
		{
			task = "VR_UD";
		}
		else if (ParamFile.GetComponent<Dropdown> ().value == 4)
		{
			task = "VR_2D";
		}

		if (counter == 25)
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
	}
}