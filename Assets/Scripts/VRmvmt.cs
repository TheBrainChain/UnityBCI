using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRmvmt : MonoBehaviour {


	public GameObject myplayer;
	SteamVR_Controller.Device device;
	SteamVR_TrackedObject controller;

	Vector2 touchpad;

	private float sensitivityX = 1.5f;
	private Vector3 playerPos;
	void Start () {
		controller = gameObject.GetComponent<SteamVR_TrackedObject> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (device);
		device = SteamVR_Controller.Input ((int)controller.index);
		if (device.GetTouch (SteamVR_Controller.ButtonMask.Touchpad))
		{
			touchpad = device.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
		}
		if (touchpad.y > 0.2f || touchpad.y < -0.2f)
		{
			myplayer.transform.position -= myplayer.transform.forward * Time.deltaTime * (touchpad.y * 5f);

			playerPos = myplayer.transform.position;
			playerPos.y = Terrain.activeTerrain.SampleHeight (myplayer.transform.position);
			myplayer.transform.position= playerPos;
		}
		if (touchpad.x > 0.3f || touchpad.x < -0.3f)
		{
					myplayer.transform.Rotate(0,touchpad.x * sensitivityX,0);
		}
	}
}
