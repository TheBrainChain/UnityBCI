using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;


public class UDPReceive : MonoBehaviour
{
	public GameObject RightTarget;
	public GameObject LeftTarget;
	private GameObject RightClone = null;
	private GameObject LeftClone = null;
	public GameObject LeftPushCube;
	public GameObject RightPushCube;
	private Rigidbody rbLeft;
	private Rigidbody rbRight;

	Thread receiveThread1;

	public UdpClient client;

	private string IP = "127.0.0.1";			//bind it to the computer's IP adress that is running the VR app									// Localhost: 127.0.0.1			Phone: 10.104.231.63		//need to make static
//	public string IP2 = "128.101.202.187";
	private int CursorPosX = 0;
	private int TargetCode = 0;
	private int ResultCode = 0;
	private string CursorPos;
	private int Feedback;
	private float SignalCode;


	private int Hits = 0;
	private int Misses = 0;
	private int Aborts = 0;

	public Text hitText;
	public Text missText;
	public Text abortText;

//	int ID;
//	Thread t;

//	public void ThreadedWorker(int ID)
//	{
//		this.ID = ID;
//		t = new Thread (new ThreadStart (ReceiveData));	///Either put in the same 
//		t.Start();
//	}

	private void ReceiveData()
	{
		client = new UdpClient(55404);
		while (true)
		{
			try
			{
				IPAddress test1 = IPAddress.Parse(IP);
				IPEndPoint anyIP2 = new IPEndPoint(test1, 0);
				byte[] data2 = client.Receive(ref anyIP2);

				string text = ASCIIEncoding.ASCII.GetString(data2);

				String toFind = "CursorPosX";
				String toFind2 = "TargetCode";
				String toFind3 = "ResultCode";

				if (text.IndexOf(toFind) == 0)
				{
					int i = text.IndexOf('X');
					CursorPos = text.Substring(i + 2);
					CursorPosX = Int32.Parse(CursorPos) - 2047;
				}
				else if (text.IndexOf(toFind2) == 0)
				{
					int i = text.IndexOf('e');
					String TargetCodez = text.Substring(i + 7);
					TargetCode = Int32.Parse(TargetCodez);
				}
				else if (text.IndexOf(toFind3) == 0)
				{
					int i = text.IndexOf('e');
					String ResultCodez = text.Substring(i + 10);
					ResultCode = Int32.Parse(ResultCodez);
				}
				else if (text.IndexOf("Feedback")==0)
				{
					int i = text.IndexOf('k');
					String Signal = text.Substring(i+2);
					Feedback = Int32.Parse (Signal);
				}
				else if (text.IndexOf("Signal(0,0)")==0)
				{
					int i = text.IndexOf(')');
					String Signal = text.Substring(i+2);
					SignalCode = float.Parse (Signal, System.Globalization.CultureInfo.InvariantCulture);
				}
			}
			catch (Exception err)
			{
				print(err.ToString());
			}
		}
	}

	public void Automation ()
	{
		Process.Start ("C:\\Users\\fortu\\Documents\\BCI2000\\batch\\VRCopy.bat");
	}

	public void CloseApp()
	{
		Application.Quit ();
	}

	void Update()
	{
		if (gameObject.name=="LocalPlayer")
		{
		if (TargetCode == 1) 
		{
			Destroy (LeftClone);
			if (RightClone == null)
			{
				RightClone = (GameObject)Instantiate (RightTarget);
				transform.localScale = new Vector3 (1, 1, 1);
			}
			transform.position = new Vector3(CursorPosX/80, 5, 32);
			if (ResultCode == 1)
			{
				RightClone.GetComponent<Renderer>().material.color = Color.blue;
				Hits = Hits + 1;
			}
			if (ResultCode == 2)
			{
				Misses = Misses + 1;
			}
		}
		if (TargetCode ==2)
		{

			Destroy (RightClone);
			if (LeftClone == null)
			{
				LeftClone = (GameObject)Instantiate (LeftTarget);
				transform.localScale = new Vector3 (1, 1, 1);
			}
			transform.position = new Vector3(CursorPosX/80, 5, 32);
			if (ResultCode == 2)
			{
				LeftClone.GetComponent<Renderer>().material.color = Color.blue;
				Hits = Hits + 1;
			}
			if (ResultCode == 1)
			{
				Misses = Misses + 1;
			}
		}
		if (TargetCode == 0)
		{
			transform.localScale = new Vector3(0,0,0);
			Destroy (LeftClone);
			Destroy (RightClone);
			transform.position = new Vector3 (0, 5, 32);
		}
		hitText.text = Math.Round(Hits/60.0).ToString();
		missText.text = Math.Round(Misses/60.0).ToString();
		abortText.text = Aborts.ToString();
	}
		if (gameObject.name == "RemotePlayer")
		{
			if (TargetCode == 1) 
			{
				Destroy (LeftClone);
				if (RightClone == null)
				{
					RightClone = (GameObject)Instantiate (RightTarget);
					transform.localScale = new Vector3 (1, 1, 1);
				}
				transform.position = new Vector3(CursorPosX/80, 3, 32);
				if (ResultCode == 1)
				{
					RightClone.GetComponent<Renderer>().material.color = Color.blue;
					Hits = Hits + 1;
				}
				if (ResultCode == 2)
				{
					Misses = Misses + 1;
				}
			}
			if (TargetCode ==2)
			{

				Destroy (RightClone);
				if (LeftClone == null)
				{
					LeftClone = (GameObject)Instantiate (LeftTarget);
					transform.localScale = new Vector3 (1, 1, 1);
				}
				transform.position = new Vector3(CursorPosX/80, 3, 32);
				if (ResultCode == 2)
				{
					LeftClone.GetComponent<Renderer>().material.color = Color.blue;
					Hits = Hits + 1;
				}
				if (ResultCode == 1)
				{
					Misses = Misses + 1;
				}
			}
			if (TargetCode == 0)
			{
				transform.localScale = new Vector3(0,0,0);
				Destroy (LeftClone);
				Destroy (RightClone);
				transform.position = new Vector3 (0, 3, 32);
			}
			hitText.text = Math.Round(Hits/60.0).ToString();
			missText.text = Math.Round(Misses/60.0).ToString();
			abortText.text = Aborts.ToString();
		}
		if (gameObject.name == "PushTargets")
		{
			int Right = 1;
			int Left = 2;

			if (TargetCode == Right)
			{
				RightPushCube.GetComponent<Renderer>().material.color = Color.blue;
				LeftPushCube.transform.position = new Vector3 (.399f, 0.419f, 0);
				if (Feedback == 1)
				{
					rbRight.velocity = new Vector3 (0, 0, SignalCode);
					if (rbRight.position.z < 0)
					{
						rbRight.position = new Vector3(.109f,0.419f,0);
					}
				}
				if (ResultCode == Right)
				{
					RightPushCube.GetComponent<Renderer>().material.color = Color.white;
					Hits = Hits + 1;
				}
				if (ResultCode == Left)
				{
					Misses = Misses + 1;
				}
			}

			if (TargetCode ==Left)
			{
				LeftPushCube.GetComponent<Renderer>().material.color = Color.blue;
				RightPushCube.transform.position = new Vector3(.109f,0.419f,0);
				if (Feedback == 1)
				{
					rbLeft.velocity = new Vector3 (0, 0, -SignalCode);
					if (rbLeft.position.z < 0)
					{
						rbLeft.position = new Vector3(.399f,0.419f,0);
					}
				}
				if (ResultCode == Left)
				{
					LeftPushCube.GetComponent<Renderer>().material.color = Color.white;
					Hits = Hits + 1;
				}
				if (ResultCode == Right)
				{
					Misses = Misses + 1;
				}
			}

			if (TargetCode == 0)
			{
				LeftPushCube.transform.position = new Vector3 (.399f, 0.419f, 0);
				RightPushCube.transform.position = new Vector3 (.109f, 0.419f, 0);
				RightPushCube.GetComponent<Renderer> ().material.color = Color.black;
				LeftPushCube.GetComponent<Renderer> ().material.color = Color.black;
			}
		}
	}

	public void Start()	
	{
		init();
	}

	private void init()
	{
		receiveThread1 = new Thread(new ThreadStart(ReceiveData));
		receiveThread1.IsBackground = true;
		receiveThread1.Start();
	}
}