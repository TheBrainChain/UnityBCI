//using UnityEngine;
//using System.Collections;
//using System.Diagnostics;
//using System;
//using System.Text;
//using System.Net;
//using System.Net.Sockets;
//using System.Threading;
//using UnityEngine.UI;

//public class ArmTask : MonoBehaviour {

//    public GameObject LeftArm;
//    public GameObject RightArm;
//    private Animator LeftArmAnim;
//    private Animator RightArmAnim;

//    public Text TargetStim;

//    public BCI_Class BCI2000 = new BCI_Class("127.0.0.1");

//    public void Start() 
//    {
//        LeftArmAnim = LeftArm.GetComponent<Animator>();
//        RightArmAnim = RightArm.GetComponent<Animator>();
//        LeftArm.SetActive(true);
//        RightArm.SetActive(true);
//        RightArmAnim.enabled = true;
//        RightArmAnim.enabled = false;
//        LeftArmAnim.enabled = true;
//        LeftArmAnim.enabled = false;

//        BCI2000.receiveThread = new Thread(() => BCI2000.receiveData(BCI2000.receivePort));
//        BCI2000.receiveThread.IsBackground = true;
//        BCI2000.receiveThread.Start();
//    }

//    public void selectExperiment()
//    {
//        BCI2000.startExp();
//        // make start button dissapear after it is clicked once
//    }

//  /*  public void OnDestroy()
//    {
//        UnityEngine.Debug.Log("Quitting...Closing UDP connection");
//        BCI2000.client.Close();
//        ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2002\\prog\\BCI2000Shell.exe");
//        PSI.Arguments = "-c Stop; Exit";
//        Process.Start(PSI);
//        Application.Quit ();
//    }
//*/
//    void Update()
//    {
//        if (BCI2000.Feedback == 0)
//        {
//            TargetStim.text = "Rest";
//        }
//        if (BCI2000.TargetCode == 1)
//        {
//            TargetStim.text = "Right";
//            if (BCI2000.CursorPosX > 0 && BCI2000.Feedback == 1)
//            {
//                if (RightArmAnim.enabled == false)
//                {
//                    LeftArmAnim.enabled = false; // don't deactivate, just don't play the animation
//                    RightArmAnim.enabled = true;
//                }
//            }
//        }
//        else if (BCI2000.TargetCode == 2)
//        {
//            TargetStim.text = "Left";
//            if (BCI2000.CursorPosX < 0 && BCI2000.Feedback == 1)
//            {
//                if (LeftArmAnim.enabled == false)
//                {
//                    RightArmAnim.enabled = false;
//                    LeftArmAnim.enabled = true;
//                }
//            }
//        }
//        else
//        {
//            RightArmAnim.enabled = false;
//            LeftArmAnim.enabled = false;
//        }
//    }

//}
