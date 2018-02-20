using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class APISelector : MonoBehaviour {

    private GameObject Hue, TP, Roku;
    private GameObject APIText;

	// Use this for initialization
	void Start () {
        APIText = this.gameObject.transform.GetChild(0).gameObject;
        Hue = this.gameObject.transform.GetChild(1).gameObject;
        TP = this.gameObject.transform.GetChild(2).gameObject;
        Roku = this.gameObject.transform.GetChild(3).gameObject;
    }
	
	// Update is called once per frame

    
}
