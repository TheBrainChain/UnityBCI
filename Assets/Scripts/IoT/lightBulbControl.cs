using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightBulbControl : MonoBehaviour {

	// Use this for initialization

	public GameObject lightbulb;
	public bool lightStatus;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKey (KeyCode.A))
		{
			lightbulb.GetComponent<HueLamp> ().on = true;
		}
		if (Input.GetKey (KeyCode.B))
		{
			lightbulb.GetComponent<HueLamp> ().on = false;
		}
			
	}
}
