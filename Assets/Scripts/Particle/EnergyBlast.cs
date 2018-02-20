//using Leap.Unity;
using Leap;
using System.Threading;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;

public class EnergyBlast : MonoBehaviour 
{
	public GameObject[] Targets;



	//public BCI_Class BCI2000 = new BCI_Class("127.0.0.1");



	public ParticleSystem partsys = new ParticleSystem();
	private ParticleSystem.ColorOverLifetimeModule col = new ParticleSystem.ColorOverLifetimeModule();

	public void Start () 
	{
		Targets = new GameObject[33];
		for (int i = 0; i < 33; i++)
		{
			Targets [i] = GameObject.Find (string.Format("Cube{0}", i+1));
		}


		col = partsys.colorOverLifetime;




		//Start a BCI2000 UDP receive thread
	//	BCI2000.receiveThread = new Thread(() => BCI2000.receiveData(BCI2000.receivePort));
	//	BCI2000.receiveThread.IsBackground = true;
	//	BCI2000.receiveThread.Start();

		//Resets VR views so it starts in the proper position
		/*
		 * if (UnityEngine.VR.VRDevice.isPresent)
		{
			Valve.VR.OpenVR.System.ResetSeatedZeroPose ();
			Valve.VR.OpenVR.Compositor.SetTrackingSpace (
				Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);
		}
		*/
	}



	/*
	 * public void OnDestroy()
	{
		UnityEngine.Debug.Log("Quitting...Closing UDP connection");
		BCI2000.client.Close();
		ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2000\\prog\\BCI2000Shell.exe");
		PSI.Arguments = "-c Stop; Exit";
		Process.Start(PSI);
		Application.Quit ();
	}
*/


	void Update ()
	{



		//Start experiment and disable controller rendering
		if (Input.GetKey(KeyCode.A) )
		{
			print ("Begin");
		//	BCI2000.startExp();

		}
			
	}
}