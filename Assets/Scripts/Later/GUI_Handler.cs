using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
//using SFB;

public class GUI_Handler : MonoBehaviour {
    private string _path;
    public bool guiSwitch = false;
    //
    public UnityEngine.UI.InputField subjectInfo;

	public GameObject WelcomeText, userNum, Name1, Name2;
	public GameObject Inputfield, batchFileField;
	public Dropdown SignalSource, SignalProcessing, SignalApplication;

	public InputField batchFile, userName;
    public Button parameterFile;

    public GameObject GUICanvas;


	public string miTask;

	public Button beginButton;
	public Button traditionalButton, customButton, configureButton, motorImageryButton, P300Button, LRButton, UDButton, TwoDButton;

	public GameObject sceneCam, taskObj;

    void Start()
    {
		DontDestroyOnLoad (GUICanvas);

		traditionalButton = GameObject.Find ("Traditional").GetComponent<Button> ();
		customButton = GameObject.Find ("Custom").GetComponent<Button> ();
		SignalSource = GameObject.Find ("Signal Source").GetComponent<Dropdown> ();
		SignalProcessing = GameObject.Find ("Signal Processing").GetComponent<Dropdown> ();
		SignalApplication = GameObject.Find ("Signal Application").GetComponent<Dropdown> ();
		//batchFile = GameObject.Find ("Batch file").GetComponent<InputField> ();
		parameterFile = GameObject.Find ("Parameter file").GetComponent<Button> ();
		userName = GameObject.Find ("User name").GetComponent<InputField> ();
		configureButton = GameObject.Find ("Configure").GetComponent<Button> ();

		motorImageryButton = GameObject.Find ("MotorImagery").GetComponent<Button> ();
		P300Button = GameObject.Find ("P300").GetComponent<Button> ();
		LRButton = GameObject.Find ("LR_Button").GetComponent<Button> ();
		UDButton = GameObject.Find ("UD_Button").GetComponent<Button> ();
		TwoDButton = GameObject.Find ("2D_Button").GetComponent<Button> ();

		sceneCam = GameObject.Find ("[CameraRig]");
		beginButton = GameObject.Find ("Begin").GetComponent<Button> ();


		motorImageryButton.transform.localScale = new Vector3 (0, 0, 0);
		P300Button.transform.localScale = new Vector3 (0, 0, 0);

		LRButton.transform.localScale = new Vector3 (0, 0, 0);
		UDButton.transform.localScale = new Vector3 (0, 0, 0);
		TwoDButton.transform.localScale = new Vector3 (0, 0, 0);

		SignalSource.transform.localScale = new Vector3 (0, 0, 0);
		SignalProcessing.transform.localScale = new Vector3 (0, 0, 0);
		SignalApplication.transform.localScale = new Vector3 (0, 0, 0);
		//batchFile.transform.localScale = new Vector3 (0, 0, 0);
		parameterFile.transform.localScale = new Vector3 (0, 0, 0);
		userName.transform.localScale = new Vector3 (0, 0, 0);
		configureButton.transform.localScale = new Vector3 (0, 0, 0);
		beginButton.transform.localScale = new Vector3 (0, 0, 0);


		traditionalButton.GetComponent<Button> ().onClick.AddListener (delegate {
			TaskOnClick ("Traditional");
		});
		customButton.GetComponent<Button> ().onClick.AddListener (delegate {
			TaskOnClick ("Custom");
		});
		motorImageryButton.GetComponent<Button> ().onClick.AddListener (delegate {
			TaskOnClick ("showMI");
		});
		P300Button.GetComponent<Button> ().onClick.AddListener (delegate {
			TaskOnClick ("P300");
		});
		LRButton.GetComponent<Button> ().onClick.AddListener (delegate {
			TaskOnClick ("LR");
		});
		UDButton.GetComponent<Button> ().onClick.AddListener (delegate {
			TaskOnClick ("UD");
		});
		TwoDButton.GetComponent<Button> ().onClick.AddListener (delegate {
			TaskOnClick ("2D");
		});
		beginButton.GetComponent<Button> ().onClick.AddListener (delegate {
			TaskOnClick ("Begin");
		});
		configureButton.GetComponent<Button> ().onClick.AddListener (delegate {
			TaskOnClick ("Configure");
		});
	}
		
	void TaskOnClick(string task)
	{
		if (task == "Traditional")
        {
			traditionalButton.transform.localScale = new Vector3 (0, 0, 0);
			customButton.transform.localScale = new Vector3 (0, 0, 0);
			SignalSource.transform.localScale = new Vector3 (1, 1, 1);
			SignalProcessing.transform.localScale = new Vector3 (1, 1, 1);
			SignalApplication.transform.localScale = new Vector3 (1, 1, 1);
			//batchFile.transform.localScale = new Vector3 (1, 1, 1);
			parameterFile.transform.localScale = new Vector3 (1, 1, 1);
			userName.transform.localScale = new Vector3 (1.77f, 1.77f, 1.77f);
			configureButton.transform.localScale = new Vector3 (1, 1, 1);
			WelcomeText.transform.localScale = new Vector3 (0, 0, 0);

		} else if (task == "Custom")
        {	
			traditionalButton.transform.localScale = new Vector3 (0, 0, 0);
			customButton.transform.localScale = new Vector3 (0, 0, 0);
			motorImageryButton.transform.localScale = new Vector3 (1, 1, 1);
			P300Button.transform.localScale = new Vector3 (1, 1, 1);

			LRButton.transform.localScale = new Vector3 (0, 0, 0);
			UDButton.transform.localScale = new Vector3 (0, 0, 0);
			TwoDButton.transform.localScale = new Vector3 (0, 0, 0);
		} else if (task == "showMI")
        {
			LRButton.transform.localScale = new Vector3 (.5f, .5f, .5f);
			UDButton.transform.localScale = new Vector3 (.5f, .5f, .5f);
			TwoDButton.transform.localScale = new Vector3 (.5f, .5f, .5f);
			motorImageryButton.transform.localScale = new Vector3 (0, 0, 0);
			P300Button.transform.localScale = new Vector3 (0, 0, 0);
		}
        else if (task == "LR" || task == "UD" || task == "2D")
        {
			miTask = task;
			UnityEngine.SceneManagement.SceneManager.LoadScene ("MI");
			LRButton.transform.localScale = new Vector3 (0, 0, 0);
			UDButton.transform.localScale = new Vector3 (0, 0, 0);
			TwoDButton.transform.localScale = new Vector3 (0, 0, 0);
			WelcomeText.transform.localScale = new Vector3 (0, 0, 0);
			sceneCam.transform.Translate (0, 0, -10);
		}
        else if (task == "Configure")
        {
			string selectedApp = SignalApplication.options [SignalApplication.value].text;
            Debug.Log(selectedApp);
			SignalSource.transform.localScale = new Vector3 (0, 0, 0);
			SignalProcessing.transform.localScale = new Vector3 (0, 0, 0);
			SignalApplication.transform.localScale = new Vector3 (0, 0, 0);
			configureButton.transform.localScale = new Vector3 (0, 0, 0);
			//batchFile.transform.parent.localScale = new Vector3 (0, 0, 0);
			parameterFile.transform.parent.localScale = new Vector3 (0, 0, 0);
			beginButton.transform.localScale = new Vector3 (0, 0, 0);
			if (selectedApp == "CursorTask_LR") {
				miTask = "LR";
			} else if (selectedApp == "CursorTask_UD") {
				miTask = "UD";
			} else if (selectedApp == "CursorTask_2D") {
				miTask = "2D";
			}
			GetComponent<BCI_Task> ().OpenShell (true);
			GetComponent<BCI_Task> ().Start ();
		
		}
		else if (task == "Begin")
		{
			GetComponent<BCI_Task> ().configureTask();
		}
	}

void Update()
{
	//GetComponent<BCI_Task> ().Update ();

//}
//    public void buttonClick() {
//        //guiSwitch = true;
//        WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));

//    }

   
    //public void WriteResult(string[] paths)
    //{
    //    if (paths.Length == 0)
    //    {
    //        return;
    //    }

    //    _path = "";
    //    foreach (var p in paths)
    //    {
    //        _path += p + "\n";
    //    }
    }

    //public void WriteResult(string path)
    //{
    //    _path = path;
    //}

}