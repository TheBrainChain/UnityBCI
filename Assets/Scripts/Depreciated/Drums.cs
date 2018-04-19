using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;

public class Drums : MonoBehaviour
{
	private int secs2Wait = 4;
	public int Left;
	public int totalSec = 0;
	public GameObject[] CylinderDrums = new GameObject[12];

	private float timer = 1f;




	void Update()																					//Add abort recognition
	{
		TimerUpdate ();
	}

	void TimerUpdate()
	{
		
		if (totalSec < secs2Wait)
		{
			if (timer > 0)
			{
				timer -= Time.deltaTime;
			}
			else if (timer <= 0)
			{
				totalSec++;
				timer = 1f;
			}

			if (totalSec == 1 && timer == 1f)
			{
				Left = UnityEngine.Random.Range (0, 6);
				CylinderDrums [Left].GetComponent<Renderer> ().material.color = Color.green;
			} 
			else if (totalSec == 3)
			{
				CylinderDrums [Left].GetComponent<Renderer> ().material.color = Color.blue;
			} 
			else if (totalSec == 4)
			{
				CylinderDrums [Left].GetComponent<Renderer> ().material.color = Color.white;
				totalSec = 0;
			}

		}
	}
		

	public void Start()
	{		
		
	}

}
