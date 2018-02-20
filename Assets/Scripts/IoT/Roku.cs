using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Sockets;

public class Roku : MonoBehaviour {

	// Use this for initialization
	public string sel;
	public BCI_Class BCIinst;
	public Slider bciSlider;
	float volValue = 0;
	float sliderVal;

	public void TPon(){
			var httpWebRequest = (HttpWebRequest)WebRequest.Create (string.Format ("https://wap.tplinkcloud.com?token=6b1ce74d-2dabbb6a05844aa89e345f6"));
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "POST";

			ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

			using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				string json = "{" +
					"\"method\":\"passthrough\"," +
					"\"params\":{" +
					"\"deviceId\":\"80063BE6563FBDA47600A13ED089C4C118A5CE3A\"," +
					"\"requestData\":\"{" +
					"\\\"system\\\":{" +
					"\\\"set_relay_state\\\":{" +
					"\\\"state\\\":1}}}\"}}";

				streamWriter.Write(json);
				streamWriter.Flush();
				streamWriter.Close();
			}

			var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse ();
			using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
				var result = streamReader.ReadToEnd ();
			}

		}
	public void TPoff(){
			var httpWebRequest = (HttpWebRequest)WebRequest.Create (string.Format ("https://wap.tplinkcloud.com?token=6b1ce74d-2dabbb6a05844aa89e345f6"));
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "POST";

			ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

			using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				string json = "{" +
					"\"method\":\"passthrough\"," +
					"\"params\":{" +
					"\"deviceId\":\"80063BE6563FBDA47600A13ED089C4C118A5CE3A\"," +
					"\"requestData\":\"{" +
					"\\\"system\\\":{" +
					"\\\"set_relay_state\\\":{" +
					"\\\"state\\\":0}}}\"}}";

				streamWriter.Write(json);
				streamWriter.Flush();
				streamWriter.Close();
			}

			var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse ();
			using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
				var result = streamReader.ReadToEnd ();
			}

		}
		
	public bool MyRemoteCertificateValidationCallback(System.Object sender,
			X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			bool isOk = true;
			// If there are errors in the certificate chain,
			// look at each error to determine the cause.
			if (sslPolicyErrors != SslPolicyErrors.None) {
				for (int i = 0; i < chain.ChainStatus.Length; i++) {
					if (chain.ChainStatus [i].Status == X509ChainStatusFlags.RevocationStatusUnknown) {
						continue;
					}
					chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
					chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
					chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan (0, 1, 0);
					chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
					bool chainIsValid = chain.Build ((X509Certificate2)certificate);
					if (!chainIsValid) {
						isOk = false;
						break;
					}
				}
			}
			return isOk;
	}
		
	public void openShell()
	{	
		BCIinst = new BCI_Class ("127.0.0.1", 55404);
		ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2001\\prog\\operator.exe");
		PSI.Arguments = "--StartupIdle --Telnet 127.0.0.1:" + "3"+ "997 --AllowMultipleInstances";
		Process.Start(PSI);

		BCIinst.tcpClient = new TcpClient("127.0.0.1", 3997);
		BCIinst.stream = BCIinst.tcpClient.GetStream();
		StartCoroutine(BCI2000_operator(0, "3", "SignalGenerator", "ARSignalProcessing","CursorTask"));

	}

	IEnumerator BCI2000_operator(int inst, string num, string source, string process, string app)
	{
		BCIinst.tcpClient = new TcpClient("127.0.0.1", 3997);
		BCIinst.stream = BCIinst.tcpClient.GetStream();

		string msg = "STARTUP SYSTEM * SignalSource:" + num+ "000 SignalProcessing:" + num+ "001 Application:" + num+ "002";
		string msgX = "Change directory Assets\\BCI2001\\prog";
		string msg1 = "Start executable " + source + " 127.0.0.1:" + num+ "000 --AllowMultipleInstances";
		string msg2 = "Start executable " + process + " 127.0.0.1:" + num+ "001 --AllowMultipleInstances";
		string msg3 = "Start executable " + app + " 127.0.0.1:" + num+ "002 --AllowMultipleInstances";
		string msg4 = "Wait for Connected";
		string msg5 = "Load parameterfile ..\\..\\BCI2001\\parms\\IoT.prm";
		string msg6 = "Set config";
		string msg7 = "";//"Start";

		sendMsg (inst, "\n");

		sendMsg (inst, msg);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (.1f);
		sendMsg (inst, msgX);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (.1f);
		sendMsg (inst, msg1);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (.1f);
		sendMsg (inst, msg2);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (.1f);
		sendMsg (inst, msg3);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (.1f);
		sendMsg (inst, msg4);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (.1f);
		sendMsg (inst, msg5);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (.1f);
		sendMsg (inst, msg6);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (.1f);
		sendMsg (inst, msg7);
		sendMsg (inst, "\n");
		yield return new WaitForSeconds (.1f);
		BCIinst.receiveThread = new Thread(() => BCIinst.receiveData(BCIinst.receivePort));
		BCIinst.receiveThread.IsBackground = true;
		BCIinst.receiveThread.Start();
	}

	public void OnDestroy()
	{
		Application.Quit ();
		BCIinst.tcpClient.Close ();
		BCIinst.client.Close();
	}

	public void sendMsg(int inst, string msg)
	{
		BCIinst.stream.Write(System.Text.Encoding.ASCII.GetBytes(msg),0,System.Text.Encoding.ASCII.GetBytes(msg).Length);
	}

	public void philipsHue(int sliVal){
		var httpWebRequest = (HttpWebRequest)WebRequest.Create ("http://192.168.0.15/api/aKkMwrFSuI4zeztRtAdF-KuY2LINDjkOlzMXps-O/lights/3/state");
		httpWebRequest.ContentType = "application/json";
		httpWebRequest.Method = "PUT";

		ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

		using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) {

			string json = "{" +
			             "\"on\":true" + "," +
				"\"bri\":"+sliVal+"}";
			streamWriter.Write (json);
			streamWriter.Flush ();
			streamWriter.Close ();
		}

		var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse ();
		using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
			var result = streamReader.ReadToEnd ();
			print (result);
		}
	}
	public void RokuVolume(){
		if (bciSlider.value > volValue) {
			sel = "Volumeup";
		} else if (bciSlider.value < volValue) {
			sel = "Volumedown";
		}
		volValue = bciSlider.value;
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create (string.Format ("http://192.168.0.22:8060/keypress/{0}",sel));
		request.Method = "POST";
		System.IO.Stream s = request.GetRequestStream ();
		s.Close ();
	}

	public void TPlinkControl(){
		if (bciSlider.value > .7) {
			TPon ();
		}
		if (bciSlider.value < .3) {
			TPoff ();
		}
	}

	void Update () {
		//bciSlider.value = (BCIinst.CursorPosY + 1500) / 3000;
		if (Input.GetKeyDown (KeyCode.A)) {
			TPlinkControl ();
		} else if (Input.GetKeyDown (KeyCode.B)) {
			RokuVolume ();
		} else if (Input.GetKeyDown (KeyCode.C)) {
			print (bciSlider.value);
		} else {
			philipsHue(Mathf.RoundToInt(bciSlider.value*254f));
		}
	}
}


