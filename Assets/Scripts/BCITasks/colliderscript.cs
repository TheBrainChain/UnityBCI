using System;
using UnityEngine;

public class colliderscript : MonoBehaviour {
	public int a;

	void OnTriggerEnter(Collider theCollision)
	{
		if (theCollision.gameObject.name == "LeftTarget")
		{
			print ("left");
			a = 1;
		} 
		if (theCollision.gameObject.name == "RightTarget")
		{
			print ("right");
			a = 2;
		}
	}
}