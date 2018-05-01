using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class taskManager : MonoBehaviour {

    public GameObject[] guiButtons;
	// Use this for initialization
	void Start () {
        guiButtons = new GameObject[5];
        guiButtons[0] = GameObject.Find("WordTasks_Reading");
        guiButtons[1] = GameObject.Find("WordTasks_Repetition");
        guiButtons[2] = GameObject.Find("SentenceTasks");
        guiButtons[3] = GameObject.Find("Brandt_naming");
        guiButtons[4] = GameObject.Find("ExtendedReading");

    }

    public void disableButtons()
    {
        guiButtons[0].SetActive(false);
        guiButtons[1].SetActive(false);
        guiButtons[2].SetActive(false);
        guiButtons[3].SetActive(false);
        guiButtons[4].SetActive(false);
    }
}
