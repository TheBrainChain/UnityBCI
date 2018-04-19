//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Leap.Unity;
//using Leap;
//using System.Threading;
//using System.Diagnostics;
//public class CursorTask : MonoBehaviour 
//{
//    private LeapServiceProvider provider;

//	public GameObject ball;
//	public GameObject LeftTarget;
//	public GameObject target2;
//	public GameObject target3;
//	public GameObject target4;
//	public GameObject RightTarget;
//	public List<GameObject> targets = new List<GameObject>();

//    public float targetState;
//    public int resultState = 0;

//    private IEnumerator coroutine;

//    public BCI_Class BCI2000 = new BCI_Class();

//    public string task = "BCI";

//    public void moveBallWithLeap()
//    {
//        Frame currentFrame = provider.CurrentFrame;

//        if (currentFrame.Hands.Count > 1)
//            {
//            List<Hand> hands = currentFrame.Hands;

//            Hand hand1 = hands[0];
//            Hand hand2 = hands[1];

//            float pitch1 = hand1.Direction.Pitch;
//            float pitch2 = hand2.Direction.Pitch;

//            if ((hand1.GrabStrength < .9f && hand1.GrabStrength > .4f) || (hand2.GrabStrength < .9f && hand2.GrabStrength > .8f))
//            {
//                if (hand1.GrabStrength > .8f && hand2.GrabStrength > .8f)
//                {
//                    ball.transform.Translate(0, 10 * Time.deltaTime, 0);
//                }
//                else if (hand1.IsRight && hand1.GrabStrength > 0.8f)
//                {
//                    ball.transform.Translate(10 * Time.deltaTime, 0, 0);
//                }
//                else if (hand2.IsLeft && hand2.GrabStrength > 0.8f)
//                {
//                    ball.transform.Translate(-10 * Time.deltaTime, 0, 0);
//                }
//                else if (hand2.IsRight && hand2.GrabStrength > 0.8f)
//                {
//                    ball.transform.Translate(10 * Time.deltaTime, 0, 0);
//                }
//                else if (hand1.IsLeft && hand1.GrabStrength > 0.8f)
//                {
//                    ball.transform.Translate(-10 * Time.deltaTime, 0, 0);
//                }
//            }
//            if (pitch1 > 2.5f && pitch2 > 2.5f)
//            {
//                ball.transform.Translate(0, -5 * Time.deltaTime, 0);
//            }
//        }
//    }

//	void Start () 
//	{
//		provider = FindObjectOfType<LeapServiceProvider>();

//        targets.Add(LeftTarget);
//		targets.Add(target2);
//		targets.Add(target3);
//		targets.Add(target4);
//        targets.Add(RightTarget);

//        BCI2000.receiveThread = new Thread(() => BCI2000.receiveData(BCI2000.receivePort));
//        BCI2000.receiveThread.IsBackground = true;
//        BCI2000.receiveThread.Start();

//        ball.SetActive(false);
//        targetState = 3;
//	}


        
// //   public void setTarget(int targetNum)
////	{
// //       ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2002\\prog\\BCI2000Shell.exe");
// //       PSI.Arguments = "-c SET STATE T " + targetNum.ToString();
// //       Process.Start(PSI);
//  //      targetState = targetNum;
////	}

//    public void selectExperiment()
//    {
//        BCI2000.startExp();
//        coroutine=showBall(2f);
//        StartCoroutine(coroutine);
//    }

//    public void targetHit()
//    {
//        if ((ball.transform.position.x < LeftTarget.transform.position.x) && (LeftTarget.GetComponent<Renderer>().material.color == Color.blue))
//        {
//            LeftTarget.GetComponent<Renderer>().material.color = Color.red;
//            resultState = 1;
//        }
//        if ((ball.transform.position.x > RightTarget.transform.position.x) && (RightTarget.GetComponent<Renderer>().material.color == Color.blue))
//        {
//            RightTarget.GetComponent<Renderer>().material.color = Color.red;
//            resultState = 2;
//        }

//    }
//    public void changeTarget()
//    {
//        if (targetState == 3)
//        {
//            if (Random.value > .5f)
//            {
//                targetState = 1;
//            }
//            else
//            {
//                targetState = 2;
//            }
//        }
//    }
//    private IEnumerator nextTrial(float waitTime)
//    {
//        yield return new WaitForSeconds(waitTime);

//        LeftTarget.GetComponent<Renderer>().material.color = Color.white;
//        RightTarget.GetComponent<Renderer>().material.color = Color.white;
//        targetState = Mathf.Round(Random.value+1f);
//    }

//    private IEnumerator showBall(float waitTime)
//    {        
//        yield return new WaitForSeconds(waitTime);

//        ball.SetActive(true);
//    }

//    public void moveWithBCI()
//    {   
//        targetHit();
        
//        if (targetState == 1)
//        {
//            LeftTarget.GetComponent<Renderer>().material.color = Color.blue;

//            if (resultState == 1)
//            {
//                ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2002\\prog\\BCI2000Shell.exe");
//                PSI.Arguments = "-c SET STATE R 1";
//                Process.Start(PSI);
//                targetState = 0;
//                resultState = 0;
//                ball.SetActive(false);
//                ball.transform.position = new Vector3(0, 2.97f, 54.38f);
//            }
//        }

//        if (targetState == 2)
//        {
//           RightTarget.GetComponent<Renderer>().material.color = Color.blue;

//            if (resultState == 2)
//            {
//                ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2002\\prog\\BCI2000Shell.exe");
//                PSI.Arguments = "-c SET STATE R 2";
//                Process.Start(PSI);
//                targetState = 0;
//                resultState = 0;
//               ball.SetActive(false);
//                ball.transform.position = new Vector3(0, 2.97f, 54.38f);
//            }
//        }
//        if (targetState == 0)
//        {
//            coroutine = showBall(1f);
//            StartCoroutine(coroutine);

//            coroutine = nextTrial(2f);
//            StartCoroutine(coroutine);
//        }
//        if (targetState == 1 || targetState == 2)
//        {
//            ball.transform.position = new Vector3((BCI2000.SignalCode2), 2.97f, 54.38f);

//        }
//    }

//    public void OnDestroy()
//    {
//        UnityEngine.Debug.Log("Quitting...Closing UDP connection");
//        BCI2000.client.Close();
//        ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2002\\prog\\BCI2000Shell.exe");
//        PSI.Arguments = "-c Stop; Exit";
//        Process.Start(PSI);
//        Application.Quit ();
//    }

//	void FixedUpdate ()
//    {
//        if (task == "Leap")
//        {
//            moveBallWithLeap();
//        }
//        if (task == "BCI")
//        {
//            moveWithBCI();
//        }
//    }
//}



