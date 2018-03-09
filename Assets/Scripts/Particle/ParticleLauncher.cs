using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Diagnostics;
//using Leap.Unity;
//using Leap;

public class ParticleLauncher : MonoBehaviour {

	public ParticleSystem particleLauncher;
	public ParticleSystem splatterParticles;

	List<ParticleCollisionEvent> collisionEvents;
	public Gradient particleColorGradient;
	public ParticleDecalPool splatDecalPool;
	private int tar;
	public BCI_Class BCI2000 = new BCI_Class("127.0.0.1", 55404);

	public GameObject Target1, Target2;
	private GameObject[] Target;


	//private LeapServiceProvider provider;

	void Start () 
	{
		collisionEvents = new List<ParticleCollisionEvent> ();	

		BCI2000.receiveThread = new Thread(() => BCI2000.receiveData(BCI2000.receivePort));
		BCI2000.receiveThread.IsBackground = true;
		BCI2000.receiveThread.Start();
		Target1.SetActive (false);
		Target2.SetActive (false);

		Target = new GameObject[3];
		Target [1] = Target1;
		Target [2] = Target2;
	}


	//void fireWithLeap()
	//{
	//	Frame currentFrame = provider.CurrentFrame;

	//	if (currentFrame.Hands.Count > 1)
	//	{
	//		List<Hand> hands = currentFrame.Hands;

	//		Hand hand1 = hands [0];
	//		Hand hand2 = hands [1];

	//		float pitch1 = hand1.Direction.Pitch;
	//		float pitch2 = hand2.Direction.Pitch;

	//	//	if ((hand1.GrabStrength < .9f && hand1.GrabStrength > .4f) || (hand2.GrabStrength < .9f && hand2.GrabStrength > .8f))
	//		//{
	//			if (hand1.IsRight && hand1.GrabStrength > 0.8f)
	//			{
	//				particleLauncher.transform.position = GameObject.Find ("LoPoly_Hand_Mesh_Right").transform.position;

	//			} 
	//			else if (hand2.IsLeft && hand2.GrabStrength > 0.8f)
	//			{
	//				particleLauncher.transform.position = GameObject.Find ("LoPoly_Hand_Mesh_Left").transform.position;

	//			} 
	//			else if (hand2.IsRight && hand2.GrabStrength > 0.8f)
	//			{
	//				particleLauncher.transform.position = GameObject.Find ("LoPoly_Hand_Mesh_Right").transform.position;

	//			} 
	//			else if (hand1.IsLeft && hand1.GrabStrength > 0.8f)
	//			{
	//				particleLauncher.transform.position = GameObject.Find ("LoPoly_Hand_Mesh_Left").transform.position;

	//			}
	//	//	}
	//	}
	//}



	public void startBCI()
	{
		ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2000\\prog\\BCI2000Shell.exe");
		PSI.Arguments = "-c Change directory Assets\\BCI2000\\prog; Startup system;" +
			"Show window;" +
			"Start executable " + "SignalGenerator" + ";" +
			"Start executable " + "ARSignalProcessing" + ";" +
			"Start executable " + "DummyApplication" + ";" +
			"Wait for Connected" +";" +
			"Set parameter SubjectName " + "test" + ";" + //"_" + DateTime.Today.ToString("yy-MM-dd") + "_" + ";" +
			"Set parameter ConnectorOutputAddress " + "127.0.0.1" + ":" + "55404" + ";" +
			"ADD STATE TargetCode 8 0 0 0" + ";" +  
			"ADD STATE ResultCode 8 0 0 0"+ ";" + 
			"ADD STATE Feedback 8 0 0 0" + ";" + 
			"Load parameterfile E:\\GitHub\\BCI_VR\\Assets\\BCI2000\\ParameterFiles\\TwoTargetEnergyBlast.prm" + ";" +
			"Set config";
		Process.Start(PSI);
	}

	void OnParticleCollision(GameObject other)
	{
		print ("You hit: " + other.name);

		if (other.name == "LeftTarget")
		{
			ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2000\\prog\\BCI2000Shell.exe");
			PSI.Arguments = "-c SET STATE ResultCode 1";
			Process.Start(PSI);
			if (tar == 1)
			{
				Target [1].SetActive (true);

			}
		}
		else if (other.name == "RightTarget")
		{
			ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2000\\prog\\BCI2000Shell.exe");
			PSI.Arguments = "-c SET STATE ResultCode 2";
			Process.Start(PSI);
		}


		ParticlePhysicsExtensions.GetCollisionEvents (particleLauncher,other,collisionEvents);

		for (int i = 0; i < collisionEvents.Count; i++)
		{
			splatDecalPool.ParticleHit (collisionEvents [i], particleColorGradient);
			EmitAtLocation (collisionEvents[i]);
		}
	}

	void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent)
	{
		splatterParticles.transform.position = particleCollisionEvent.intersection;
		splatterParticles.transform.rotation = Quaternion.LookRotation (particleCollisionEvent.normal);
		ParticleSystem.MainModule psMain = splatterParticles.main;
		psMain.startColor = particleColorGradient.Evaluate(Random.Range(0f,1f));
		splatterParticles.Emit (1);
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

	int setTargetState()
	{
		tar = (int)System.Math.Round(Random.Range (1f, 2f));
		return tar;
	}

	// Update is called once per frame
	void Update () {



		if (Input.GetKeyDown (KeyCode.Space))
		{		
			ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2000\\prog\\BCI2000Shell.exe");
			PSI.Arguments = string.Format("-c SET STATE TargetCode {0}", setTargetState());
			PSI.Arguments = "-c SET STATE Feedback 1";
			Process.Start(PSI);
			Target [tar].SetActive (true);
		}


		if (BCI2000.SignalCode > 5f)
		{
			particleLauncher.Emit (1);
			particleLauncher.transform.position = GameObject.Find ("LoPoly_Hand_Mesh_Right").transform.position;
		}
		else if (BCI2000.SignalCode < -5f)
		{
			particleLauncher.transform.position = GameObject.Find ("LoPoly_Hand_Mesh_Left").transform.position;

			particleLauncher.Emit (1);
		}


		if(Input.GetButton("Fire1"))
		{
			particleLauncher.transform.position = GameObject.Find ("LoPoly_Hand_Mesh_Left").transform.position;
			particleLauncher.Emit (1);
		}
	}
}
