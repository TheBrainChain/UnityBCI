/*
 * Created by Christopher Coogan, BFINL, UMN 
 * 2016
 * 
 * 
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class BCIClass_min {

	public string IP = "127.0.0.1";            //bind it to the computer's IP adress that is running the VR app
	public int receivePort = 55404;

	public Thread receiveThread;

	public UdpClient client;

	public int CursorPosX, CursorPosY,TargetCode, ResultCode = 0;
	public int Feedback;
	public float SignalCode,SignalCode1,SignalCode2, RunningState;
	public string CursorPos, RunningStateS, text;

	public void receiveData(int port)
	{
		client = new UdpClient(port);
		while (true)
		{
			IPAddress test1 = IPAddress.Parse(IP);
			IPEndPoint anyIP2 = new IPEndPoint(test1, 0);
			byte[] data2 = client.Receive(ref anyIP2);

			text = ASCIIEncoding.ASCII.GetString(data2);
			String toFind = "CursorPosX";
			String toFindY = "CursorPosY";
			String toFind2 = "TargetCode";
			String toFind3 = "ResultCode";
			String toFind4 = "Running";

			if (text.IndexOf (toFind) == 0)
			{
				int i = text.IndexOf ('X');
				CursorPos = text.Substring (i + 2);
				CursorPosX = Int32.Parse (CursorPos) - 2047;
			} else if (text.IndexOf (toFind4) == 0)
			{
				int i = text.IndexOf ("g");
				RunningStateS = text.Substring (i + 2);
				RunningState = Int32.Parse (RunningStateS);
			}
			else if (text.IndexOf(toFindY) == 0)
			{
				int i = text.IndexOf('Y');
				CursorPos = text.Substring(i + 2);
				CursorPosY = Int32.Parse(CursorPos) - 2047;
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
			else if (text.IndexOf("Feedback") == 0)
			{
				int i = text.IndexOf('k');																								//These are going to be different because of FieldTrip
				String Signal = text.Substring(i + 2);
				Feedback = Int32.Parse(Signal);
			}
			else if (text.IndexOf("Signal(0,0)") == 0)
			{
				int i = text.IndexOf(')');
				String Signal = text.Substring(i + 2);
				SignalCode = float.Parse(Signal, System.Globalization.CultureInfo.InvariantCulture);
			}
		}
	}
}
