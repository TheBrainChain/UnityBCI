using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EastScoreManager : MonoBehaviour {
	
	public static int eScore;
	
	Text text;
	
	void Awake () {
		
		text = GetComponent <Text> ();
		eScore = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		text.text = "Score: " + eScore;
		
	}
}
