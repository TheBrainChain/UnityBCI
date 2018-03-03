//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using System.Diagnostics;

//public class FireTrigger : MonoBehaviour {

//	public GameObject LeftCandle;
//	public Gradient grad;
//	public GameObject me;
//	public BCI_Class BciVars = new BCI_Class("127.0.0.1");

//	void Start()
//	{
//	//	ParticleSystem.ColorOverLifetimeModule col = partsys.colorOverLifetime;
//	//	col.color 


//	}
//	void OnTriggerEnter(Collider other)
//	{
//		if (other.gameObject.name == "LeftCandle" && BciVars.TargetCode == 1)
//		{

//		//	col.color = Color.blue;
//		}
//		else if (other.gameObject.name == "LeftCandle" && BciVars.TargetCode == 2)
//		{
//			ParticleSystem partsys = GetComponent<ParticleSystem>();
//			ParticleSystem.ColorOverLifetimeModule col = partsys.colorOverLifetime;
//			col.color = Color.red;
//		}
//		else if (other.gameObject.name == "RightCandle" && BciVars.TargetCode == 1)
//		{
//			ParticleSystem partsys = GetComponent<ParticleSystem>();
//			ParticleSystem.ColorOverLifetimeModule col = partsys.colorOverLifetime;
//			col.color = Color.red;
//		}
//		else if (other.gameObject.name == "RightCandle" && BciVars.TargetCode == 2)
//		{
//			ParticleSystem partsys = GetComponent<ParticleSystem>();
//			ParticleSystem.ColorOverLifetimeModule col = partsys.colorOverLifetime;
//			col.color = Color.blue;
//		}
//}
//}