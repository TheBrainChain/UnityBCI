using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WestScoreManager : MonoBehaviour {
	
	public static int wScore;
	
	Text text;
	
	void Awake () {
		
		text = GetComponent <Text> ();
		wScore = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		text.text = "Score: " + wScore;
		
	}
}
