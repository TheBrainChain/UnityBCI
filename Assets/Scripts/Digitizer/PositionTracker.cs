using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

//Use the HTC Vive controller to mark location points in 3D space
// Created 13 February 2017 by Christopher Coogan
using System;

public class PositionTracker : MonoBehaviour {

	public AudioSource feedbackSound;

	public TcpClient tcp_Client;

    public string IP = "127.0.0.1";
    public int port = 3947;

	IPEndPoint remoteEndPoint;
    NetworkStream stream;
    System.IAsyncResult connectedClient;

	public GameObject controllerObj, trackerObj;
	 
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private bool gripButtonDown, gripButtonUp, gripButtonPressed = false;

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	private bool triggerButtonDown, triggerButtonUp, triggerButtonPressed = false;

    private Valve.VR.EVRButtonId menu = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
	private bool menuButtonDown, menuButtonUp, menuButtonPressed = false;

	private ushort buttonStates = 0x0000;

    private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller {get{return SteamVR_Controller.Input((int) controllerObj.GetComponent<SteamVR_TrackedObject>().index);}}

	private bool ObjIsTracked;
	private int indexOfObj;
	private string nameOfObj;

	public Thread receiveMatlabData;
	private byte[] readBuffer = new byte[1];
	public int controllerID;
    

    void Start ()
    {
		controllerID = (int) controllerObj.GetComponent<SteamVR_TrackedObject>().index;

        trackedObj = GetComponent<SteamVR_TrackedObject>();
		tcp_Client = new TcpClient(AddressFamily.InterNetwork);

        if (!tcp_Client.Connected)
		{
			Debug.Log("Attempting connection...");
			try
			{
				tcp_Client = new TcpClient(AddressFamily.InterNetwork);
				IAsyncResult result = tcp_Client.BeginConnect(IPAddress.Parse(IP), port, null, null);
				bool success = result.AsyncWaitHandle.WaitOne(5000,true);

			}
			catch (SocketException e)
			{
				Debug.Log(e);
			}
            stream = tcp_Client.GetStream();
        }

		receiveMatlabData = new Thread(() => readDataFromMatlab());
		receiveMatlabData.IsBackground = true;
		receiveMatlabData.Start();



    }

    public void OnDestroy()
    {       
        buttonStates = (ushort)(buttonStates | (1 << 11));              //tells the server to close the connection
        streamWrite(System.BitConverter.GetBytes((System.UInt16)0xFF55));
        streamWrite(System.BitConverter.GetBytes(buttonStates));
        for (int i=0; i<7*2; i++) 
            streamWrite(System.BitConverter.GetBytes((System.UInt32)0x0000));
		streamWrite(System.BitConverter.GetBytes((System.UInt16)0x0000));

		try
		{
			tcp_Client.Close ();
			tcp_Client = new TcpClient (AddressFamily.InterNetwork);
			Debug.Log ("Disconnected");
		} 
		catch (System.Exception e)
		{
			Debug.Log (e);
		}
	}
	public void readDataFromMatlab()
	{
		while (true)
		{
			if (stream.CanRead)
			{
				do
				{
					print(stream.ReadByte());
					if(stream.ReadByte() > 0)
					{
						print(stream.ReadByte());
//						for(int i = 0; i <1000; i++)
//						{
							print('a');
						SteamVR_Controller.Input(controllerID).TriggerHapticPulse(3999);
			//			feedbackSound.Play();
//						}
					}

				} while(stream.DataAvailable);
			} else
			{
				print ("NO");
			}
		}
	}

    void Update ()
    {
        controllerID = (int) controllerObj.GetComponent<SteamVR_TrackedObject>().index;
        int trackerID = (int) trackerObj.GetComponent<SteamVR_TrackedObject>().index;

        if (SteamVR_Controller.Input(controllerID).outOfRange)
		{
			SteamVR_Controller.Input (controllerID).TriggerHapticPulse (3999);
		}

		if (!tcp_Client.Connected)
		{
			Debug.Log("Attempting connection...");                                                                 //We get here
			try
			{
				tcp_Client = new TcpClient(AddressFamily.InterNetwork);
				IAsyncResult result = tcp_Client.BeginConnect(IPAddress.Parse(IP), port, null, null);
			}
			catch (SocketException e)
			{
				Debug.Log(e);
			}
			stream = tcp_Client.GetStream();
		}        
        

		if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Application.Quit();
        }

        Vector2 touchpad = (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

        gripButtonDown = controller.GetPressDown(gripButton);                       //Rotate the Matlab's GUI
        gripButtonUp = controller.GetPressUp(gripButton);
        gripButtonPressed = controller.GetPress(gripButton);

        triggerButtonDown = controller.GetPressDown(triggerButton);                 //Trigger button will be used to mark location positions
        triggerButtonUp = controller.GetPressUp(triggerButton);
        triggerButtonPressed = controller.GetPress(triggerButton);

        menuButtonDown = controller.GetPressDown(menu);
        menuButtonUp = controller.GetPressUp(menu);
        menuButtonPressed = controller.GetPress(menu);

        buttonStates = 0x0000;

        if (triggerButtonDown)
        {
            SteamVR_Controller.Input(controllerID).TriggerHapticPulse(3999);
            //feedbackSound.Play();
        }
        if (triggerButtonPressed)
        {
            buttonStates = (ushort)(buttonStates | (1 << 0));
        }
        if (gripButtonPressed)
        {
            buttonStates = (ushort)(buttonStates | (1 << 1));
        }
        if (menuButtonPressed)
        {
            buttonStates = (ushort)(buttonStates | (1 << 2));
        }

        if (touchpad.y > .7f)
        {
            buttonStates = (ushort)(buttonStates | (1 << 3));
            if (controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                buttonStates = (ushort)(buttonStates | (1 << 4));
            }
        }
        else if (touchpad.y < -.7f)
        {
            buttonStates = (ushort)(buttonStates | (1 << 5));
            if (controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                buttonStates = (ushort)(buttonStates | (1 << 6));
            }
        }
        if (touchpad.x < -.7f)
        {
            buttonStates = (ushort)(buttonStates | (1 << 7));
            if (controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                buttonStates = (ushort)(buttonStates | (1 << 8));
            }
        }
        else if (touchpad.x > .7f)
        {
            buttonStates = (ushort)(buttonStates | (1 << 9));
            if (controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                buttonStates = (ushort)(buttonStates | (1 << 10));
            }
        }

			streamWrite (System.BitConverter.GetBytes ((System.UInt16)0xFF55));
			streamWrite (System.BitConverter.GetBytes (buttonStates));
			streamWrite (System.BitConverter.GetBytes (SteamVR_Controller.Input (controllerID).transform.pos.x));                   
			streamWrite (System.BitConverter.GetBytes (SteamVR_Controller.Input (controllerID).transform.pos.y)); 
			streamWrite (System.BitConverter.GetBytes (SteamVR_Controller.Input (controllerID).transform.pos.z));
			streamWrite (System.BitConverter.GetBytes (SteamVR_Controller.Input (controllerID).transform.rot.w)); 
			streamWrite (System.BitConverter.GetBytes (SteamVR_Controller.Input (controllerID).transform.rot.x)); 
			streamWrite (System.BitConverter.GetBytes (SteamVR_Controller.Input (controllerID).transform.rot.y)); 
			streamWrite (System.BitConverter.GetBytes (SteamVR_Controller.Input (controllerID).transform.rot.z)); 
			streamWrite (System.BitConverter.GetBytes (SteamVR_Controller.Input (trackerID).transform.pos.x));                   
			streamWrite (System.BitConverter.GetBytes (SteamVR_Controller.Input (trackerID).transform.pos.y)); 
			streamWrite (System.BitConverter.GetBytes (SteamVR_Controller.Input (trackerID).transform.pos.z));
			streamWrite (System.BitConverter.GetBytes (SteamVR_Controller.Input (trackerID).transform.rot.w)); 
			streamWrite (System.BitConverter.GetBytes (SteamVR_Controller.Input (trackerID).transform.rot.x)); 
			streamWrite (System.BitConverter.GetBytes (SteamVR_Controller.Input (trackerID).transform.rot.y)); 
			streamWrite (System.BitConverter.GetBytes (SteamVR_Controller.Input (trackerID).transform.rot.z)); 

			streamWrite (System.BitConverter.GetBytes ((System.UInt16)0x0000));

			//Debug.Log ("Object: " + nameOfObj + " is not being tracked: " + controller.outOfRange);

//			Debug.Log ("CX" + SteamVR_Controller.Input (controllerID).transform.pos.x);
//			Debug.Log ("TX" + SteamVR_Controller.Input (trackerID).transform.pos.x);
//			Debug.Log ("CY" + SteamVR_Controller.Input (controllerID).transform.pos.y);
//			Debug.Log ("TY" + SteamVR_Controller.Input (trackerID).transform.pos.y);
//			Debug.Log ("CZ" + SteamVR_Controller.Input (controllerID).transform.pos.z);
//			Debug.Log ("TZ" + SteamVR_Controller.Input (trackerID).transform.pos.z);



	}

	void streamWrite(byte[] toWrite)
	{
		stream.Write(toWrite, 0, toWrite.Length);
	}    
}
