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
public class butcheredUnityVRMover : MonoBehaviour
{

  	// Objects related to the Player and controllers
  	public GameObject LController,RController,Player;
	private SteamVR_Controller.Device controller {get{return SteamVR_Controller.Input((int) RController.GetComponent<SteamVR_TrackedObject>().index);}}
  	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
  	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
  	private bool triggerButtonDown,triggerButtonUp,triggerButtonPressed;


  	public void Start ()
	{
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


	public void OnDestroy()
	{
		Application.Quit ();

	}

  void Update ()
	{

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
	}
}
