using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;

public class BCI_transmission : MonoBehaviour {
    public GameObject LeftArm;
    public GameObject RightArm;
    private Animator LeftArmAnim;
    private Animator RightArmAnim;

    Thread receiveThread1;

    public UdpClient client;

    public Text TargetStim;
    private string IP = "128.101.202.156";            //bind it to the computer's IP adress that is running the VR app
    private int CursorPosX = 0;
    private int TargetCode = 0;
    private int ResultCode = 0;
    private string CursorPos;
    private int Feedback;
    private float SignalCode;


    private int Hits = 0;
    private int Misses = 0;
    private int Aborts = 0;

    public Text hitText;
    public Text missText;
    public Text abortText;

 //     int ID;
 //     Thread t;
 //     public void ThreadedWorker(int ID)
 //     {
 //         this.ID = ID;
 //         t = new Thread (new ThreadStart (ReceiveData)); ///Either put in the same 
 //         t.Start();
 //     }

    public void ReceiveData()
    {
        client = new UdpClient(55404);
        while (true)
        {
            IPAddress test1 = IPAddress.Parse(IP);
            IPEndPoint anyIP2 = new IPEndPoint(test1, 0);
            byte[] data2 = client.Receive(ref anyIP2);

            string text = ASCIIEncoding.ASCII.GetString(data2);

            String toFind = "CursorPosX";
            String toFind2 = "TargetCode";
            String toFind3 = "ResultCode";

            if (text.IndexOf(toFind) == 0)
            {
                int i = text.IndexOf('X');
                CursorPos = text.Substring(i + 2);
                CursorPosX = Int32.Parse(CursorPos) - 2047;
            }
            else if (text.IndexOf(toFind2) == 0)
            {
                int i = text.IndexOf('e');
                String TargetCodez = text.Substring(i + 7);
                TargetCode = Int32.Parse(TargetCodez);
            }
            else if (text.IndexOf(toFind3) == 0)
            {
                int i = text.IndexOf('e');
                String ResultCodez = text.Substring(i + 10);
                ResultCode = Int32.Parse(ResultCodez);
            }
            else if (text.IndexOf("Feedback") == 0)
            {
                int i = text.IndexOf('k');
                String Signal = text.Substring(i + 2);
                Feedback = Int32.Parse(Signal);
            }
            else if (text.IndexOf("Signal(0,0)") == 0)
            {
                int i = text.IndexOf(')');
                String Signal = text.Substring(i + 2);
                SignalCode = float.Parse(Signal, System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }

//    public void Automation ()
  //  {
    //    Process.Start ("C:\\Users\\fortu\\Documents\\BCI2000\\batch\\VRCopy.bat");
   // }

    public void CloseApp()
    {
        Application.Quit ();
    }

    public void OnDestroy()
    {
        client.Close();
    }

    void OnApplicationQuit()
    {
        UnityEngine.Debug.Log("Quitting...");
        client.Close();
    }


    // Always keep hands in the scene.
    // Only move hand when target appears
    // Feedback when hit: Progress bars, fireworks, "Congratulations!"


    void Update()
    {
        if (TargetCode == 1)
        {
            TargetStim.text = "Right";
            if (CursorPosX > 0 && Feedback == 1)
            {
                if (RightArm.activeSelf == false)
                {
                    LeftArm.SetActive(false); // don't deactivate, just don't play the animation
                    RightArm.SetActive(true);
                }
            }
        }
        if (TargetCode == 2)
        {
            TargetStim.text = "Left";
            if (CursorPosX < 0 && Feedback == 1)
            {
                if (LeftArm.activeSelf == false)
                {
                    RightArm.SetActive(false);
                    LeftArm.SetActive(true);
                }
            }
        }
        if ((TargetCode == 1 && CursorPosX < 0) || (TargetCode == 2 && CursorPosX > 0))  // if apply above comment, can get rid of all this.
        {
            if (RightArm.activeSelf == true || LeftArm.activeSelf == true)
            {
                LeftArm.SetActive(false);
                RightArm.SetActive(false);
            }
        }
        if (TargetCode == 0)
        {
           TargetStim.text = "Rest";
           if (RightArm.activeSelf == true || LeftArm.activeSelf == true)
           {
                LeftArm.SetActive(false);
                RightArm.SetActive(false);
           }
        }
	if ((TargetCode == 1 && ResultCode == 1) || (TargetCode == 2 && ResultCode ==2))
	{
		print("Correct!");
		//apply some feedback, maybe below TargetStim.text
	}
    }

    public void Start() 
    {
        receiveThread1 = new Thread(new ThreadStart(ReceiveData));
        receiveThread1.IsBackground = true;
        receiveThread1.Start();

        LeftArmAnim = LeftArm.GetComponent<Animator>();
        RightArmAnim = RightArm.GetComponent<Animator>();
    }
}
