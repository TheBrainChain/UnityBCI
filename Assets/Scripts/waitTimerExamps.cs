using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;

public class waitTimerExamps : MonoBehaviour {

	public int secs2Wait = 3;
	public int[] totalSec = new int[3];


	private float timer = 1f;


	void TimerInvoke()
	{
		if (totalSec [0] < secs2Wait)
		{
			totalSec [0]++;
		} else
		{
			CancelInvoke ("TimerInvoke");
		}
	}

	IEnumerator TimerEnumerator()
	{
		for(int ii =0; ii<secs2Wait; ii++)
		{
			yield return new WaitForSeconds (1);
			totalSec [1]++;
		}
	}


	void Update()																					//Add abort recognition
	{
		
		TimerUpdate ();


	}

	void TimerUpdate()
	{

		if (totalSec [2] < secs2Wait)
		{
			if (timer> 0)
			{
				timer -= Time.deltaTime;
			}
			else if (timer <= 0)
			{
				totalSec [2]++;
				timer = 1f;
			}


		}
	}

	public void Start()
	{
		InvokeRepeating ("TimerInvoke", 1, 1);
		StartCoroutine (TimerEnumerator ());
	}
}
