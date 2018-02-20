using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using UnityEngine.UI;

public class P300 : MonoBehaviour {


	public string IP= "127.0.0.1";            //bind it to the computer's IP adress that is running the VR app
	public int receivePort = 3999;
	public UdpClient client;

	public Thread receiveThread;

	public GameObject[,] p300Targets;
	public string text,CursorPos;

	public Text P300_Output;
	public InputField P300_out;

	public bool skip = false;

	public BCI_Class BCI2000 = new BCI_Class("127.0.0.1",55404);
	public int stimCode;
	public int colCode;
	public int rowCode;

	public GameObject cameraRig;

	void Start () {


		#if UNITY_2017
		#endif
		#if UNITY_ANDROID
		cameraRig.transform.position = new Vector3(58.445f, -18.616f, -401.055f);
		#endif


		#region Target generation
		p300Targets = new GameObject[6, 6];
		p300Targets [0, 0] = GameObject.Find ("A");
		p300Targets [0, 1] = GameObject.Find ("B");
		p300Targets [0, 2] = GameObject.Find ("C");
		p300Targets [0, 3] = GameObject.Find ("D");
		p300Targets [0, 4] = GameObject.Find ("E");
		p300Targets [0, 5] = GameObject.Find ("F");
		p300Targets [1, 0] = GameObject.Find ("G");
		p300Targets [1, 1] = GameObject.Find ("H");
		p300Targets [1, 2] = GameObject.Find ("I");
		p300Targets [1, 3] = GameObject.Find ("J");
		p300Targets [1, 4] = GameObject.Find ("K");
		p300Targets [1, 5] = GameObject.Find ("L");
		p300Targets [2, 0] = GameObject.Find ("M");
		p300Targets [2, 1] = GameObject.Find ("N");
		p300Targets [2, 2] = GameObject.Find ("O");
		p300Targets [2, 3] = GameObject.Find ("P");
		p300Targets [2, 4] = GameObject.Find ("Q");
		p300Targets [2, 5] = GameObject.Find ("R");
		p300Targets [3, 0] = GameObject.Find ("S");
		p300Targets [3, 1] = GameObject.Find ("T");
		p300Targets [3, 2] = GameObject.Find ("U");
		p300Targets [3, 3] = GameObject.Find ("V");
		p300Targets [3, 4] = GameObject.Find ("W");
		p300Targets [3, 5] = GameObject.Find ("X");
		p300Targets [4, 0] = GameObject.Find ("Y");
		p300Targets [4, 1] = GameObject.Find ("Z");
		p300Targets [4, 2] = GameObject.Find ("1");
		p300Targets [4, 3] = GameObject.Find ("2");
		p300Targets [4, 4] = GameObject.Find ("3");
		p300Targets [4, 5] = GameObject.Find ("4");
		p300Targets [5, 0] = GameObject.Find ("5");
		p300Targets [5, 1] = GameObject.Find ("6");
		p300Targets [5, 2] = GameObject.Find ("7");
		p300Targets [5, 3] = GameObject.Find ("8");
		p300Targets [5, 4] = GameObject.Find ("9");
		p300Targets [5, 5] = GameObject.Find ("_");

		for (int i = 0; i < 6; i++)
		{
			for (int j=0;j<6;j++)
			{
				p300Targets[i,j].transform.GetChild(0).gameObject.GetComponent<TextMesh>().color = Color.gray;
			}
		}
		#endregion
		rowCode = BCI2000.rowCode;
		colCode = BCI2000.colCode;
		stimCode = BCI2000.stimCode;
//		BCI2000.receiveThread = new Thread(() => BCI2000.receiveData(receivePort));
//		BCI2000.receiveThread.IsBackground = true;
//		BCI2000.receiveThread.Start();

		StartCoroutine (flashSequence());
	}

	IEnumerator flashSequence(){
		while (true)
		{
			if (BCI2000.stimCode == 0)
			{
				p300Targets [(int)0, 0].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)0, 1].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)0, 2].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)0, 3].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)0, 4].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)0, 5].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)1, 0].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)1, 1].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)1, 2].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)1, 3].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)1, 4].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)1, 5].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)2, 0].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)2, 1].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)2, 2].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)2, 3].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)2, 4].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)2, 5].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)3, 0].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)3, 1].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)3, 2].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)3, 3].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)3, 4].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)3, 5].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)4, 0].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)4, 1].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)4, 2].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)4, 3].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)4, 4].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)4, 5].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)5, 0].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)5, 1].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)5, 2].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)5, 3].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)5, 4].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
				p300Targets [(int)5, 5].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.gray;
			} else if (BCI2000.stimCode > 0 & stimCode <= 6)
			{
				skip = false;
				print (stimCode);
				p300Targets [(int)stimCode - 1, 0].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.white;
				p300Targets [(int)stimCode - 1, 1].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.white;
				p300Targets [(int)stimCode - 1, 2].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.white;
				p300Targets [(int)stimCode - 1, 3].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.white;
				p300Targets [(int)stimCode - 1, 4].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.white;		//error
				p300Targets [(int)stimCode - 1, 5].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.white;
			} else if (stimCode >= 7 & stimCode <= 12)
			{
				skip = false;

				p300Targets [0, (int)stimCode - 7].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.white;
				p300Targets [1, (int)stimCode - 7].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.white;
				p300Targets [2, (int)stimCode - 7].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.white;
				p300Targets [3, (int)stimCode - 7].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.white;
				p300Targets [4, (int)stimCode - 7].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.white;
				p300Targets [5, (int)stimCode - 7].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.white;
			}
			if (colCode > 0)
			{
				p300Targets [colCode - 1, rowCode - 1].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().color = Color.blue;
				if (skip == false)
				{
					P300_out.text = P300_out.text + p300Targets [colCode - 1, rowCode - 1].transform.GetChild (0).gameObject.GetComponent<TextMesh> ().text;
					skip = true;
				}
			}
			yield return new WaitForSeconds (.001f);
		}
	}

	public void OnDestroy()
	{
		UnityEngine.Debug.Log("Quitting...Closing UDP connection");
		client.Close();
	}
}
