using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using System;
using System.IO;
public class targetChase : MonoBehaviour {

    bci2k BCI2K;
    int count = 0;
    public GameObject leftController;
    public GameObject rightController;
    private string hand;
    public int testingHand;
    public string SignalSource;
    private System.IO.StreamWriter logFile;

    public void writeToLogFile(string message, System.DateTime msgTime)
    {
        logFile.WriteLine(msgTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + message);
    }

    void Start()
    {
        logFile = new System.IO.StreamWriter("Logs/3DVR_FreeReach" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
        logFile.AutoFlush = true;
        writeToLogFile("START," + ",", System.DateTime.Now);

        BCI2K = GameObject.Find("BCI2000Manager").GetComponent<bci2k>();
        if (BCI2K.configFile != null && File.Exists(BCI2K.configFile))
        {
            var tempStruct = JsonUtility.FromJson<externalConfig>(BCI2K.ReadFile(BCI2K.configFile));
            hand = tempStruct.hand;
            SignalSource = tempStruct.SignalSource;

        }
        BCI2K.websockets[0].Send(
            "E 1 " +
            "Reset System; " +
            BCI2K.addEvent("ControllerPosX", 16, 0) +
            BCI2K.addEvent("ControllerPosY", 16, 0) +
            BCI2K.addEvent("ControllerPosZ", 16, 0) +
            BCI2K.addEvent("ControllerRotX", 16, 0) +
            BCI2K.addEvent("ControllerRotY", 16, 0) +
            BCI2K.addEvent("ControllerRotZ", 16, 0) +
            "Startup system localhost; " +
            BCI2K.startExecutable(SignalSource, "local") +
            BCI2K.startExecutable("SpectralSignalProcessingMod", "local") +
            BCI2K.startExecutable("StimulusPresentationCroneLab", "local") +
            BCI2K.waitFor("Connected") +
            BCI2K.addState("SelectedTarget", 16, 0) +
            BCI2K.loadParameterFile("../parms.ecog/SpectralSigProc.prm") +
            BCI2K.loadParameterFile("../parms.ecog/go.prm") +
            BCI2K.setParameter("SubjectName", "TestSubject") +
            BCI2K.setParameter("SubjectSession", "1") +
            BCI2K.setParameter("DataFile", "%24%7bSubjectName%7d/Motor_Chase/%24%7bSubjectName%7d_MotorChase_S%24%7bSubjectSession%7dR%24%7bSubjectRun%7d.%24%7bFileFormat%7d") +
            BCI2K.setParameter("SamplingRate", "1000Hz") +
            BCI2K.setParameter("WSSpectralOutputServer", "*:20203") +
            BCI2K.setParameter("WSConnectorServer", "*:20323") +
            BCI2K.setParameter("WSSourceServer", "*:20100") +
            BCI2K.setParameter("VisualizeSource", "1") +
            BCI2K.setParameter("VisualizeTiming", "1") +
            BCI2K.setConf +
            BCI2K.waitFor("Resting") +
            BCI2K.start
            );
        StartCoroutine(counterTask(hand));
        if (hand == "left")
        {
            leftController.GetComponent<DetectAndSend>().enabled = true;
        }
        else if(hand == "right")
        {
            rightController.GetComponent<DetectAndSend>().enabled = true;
        }
    }

    IEnumerator counterTask(string targetHand)
    {
        if (hand == "left")
        {
            testingHand = 4;
        }
        if (hand == "right")
        {
            testingHand = 5;
        }

        while (true)
        {
            BCI2K.websockets[0].Send(
            "E 1 " +
            BCI2K.setEvent("ControllerPosX", (InputTracking.GetLocalPosition((XRNode)testingHand).x + 1) * 100) +
            BCI2K.setEvent("ControllerPosY", (InputTracking.GetLocalPosition((XRNode)testingHand).y + 1) * 100) +
            BCI2K.setEvent("ControllerPosZ", (InputTracking.GetLocalPosition((XRNode)testingHand).z + 1) * 100)
            );

            yield return new WaitForSeconds(.04f);
        }
    }

    private void OnApplicationQuit()
    {
        BCI2K.websockets[0].Send("E 1 Exit");
    }

    private void Update()
    {
        writeToLogFile("ControllerPos: " + (InputTracking.GetLocalPosition(XRNode.LeftHand) + new Vector3(1, 1, 1 )) * 100, System.DateTime.Now);
        writeToLogFile("ControllerRot: " + (InputTracking.GetLocalRotation(XRNode.LeftHand)), System.DateTime.Now);
        writeToLogFile("HeadPosition: " + (InputTracking.GetLocalPosition(XRNode.Head)), System.DateTime.Now);
        writeToLogFile("HeadRotation: " + (InputTracking.GetLocalRotation(XRNode.Head)), System.DateTime.Now);
    }
}
