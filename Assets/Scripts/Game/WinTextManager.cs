using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinTextManager : MonoBehaviour {

	public static string winText;
	
	Text text;
	
	void Awake () {
		
		text = GetComponent <Text> ();
		winText = "";
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (winText.Contains("Blue Wins!")) {
			text.color = Color.blue;
		}
		else if (winText.Contains("Green Wins!")) {
			text.color = Color.green;
		}
		
		text.text = winText;
		
	}
}
