using UnityEngine;

using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;




public class telnet : MonoBehaviour {

	public void startBCI ()
	{
		ProcessStartInfo PSI = new ProcessStartInfo ("Assets\\BCI2000\\prog\\BCI2000Shell.exe");
		PSI.Arguments = "-c Change directory Assets\\BCI2000\\prog;" + "--AllowMultipleInstances;" +
		"Startup system * SignalSource:5000 SignalProcessing:5001 Application:5002;" +
		"Show window;" +
		"Start executable " + "SignalGenerator 127.0.0.1:5000 --AllowMultipleInstances" + ";" +
		"Start executable " + "ARSignalProcessing 127.0.0.1:5001 --AllowMultipleInstances" + ";" +
		"Start executable " + "CursorTask 127.0.0.1:5002 --AllowMultipleInstances" + ";" +
		"Wait for Connected";
		Process.Start (PSI);
	}

	public void startBCI2 ()
	{
		ProcessStartInfo PSI2 = new ProcessStartInfo ("Assets\\BCI2000\\prog\\BCI2000Shell.exe");
		PSI2.Arguments = "-c Change directory Assets\\BCI2000\\prog;" +
		"Startup system * SignalSource:4000 SignalProcessing:4001 Application:4002;" +
		"Show window;" +
		"Start executable " + "SignalGenerator 127.0.0.1:4000 --AllowMultipleInstances" + ";" +
		"Start executable " + "ARSignalProcessing 127.0.0.1:4001 --AllowMultipleInstances" + ";" +
		"Start executable " + "CursorTask 127.0.0.1:4002 --AllowMultipleInstances" + ";" +
		"Wait for Connected";
		Process.Start (PSI2);
	}
}