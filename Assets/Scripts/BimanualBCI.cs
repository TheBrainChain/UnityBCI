using System.Collections;
using System;
using UnityEngine;
using UnityEngine.XR;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
public class BimanualBCI : MonoBehaviour {


    public string IP = "127.0.0.1";            //bind it to the computer's IP adress that is running the VR app
    public int receivePort = 55404;
    public Thread receiveThread;

    public UdpClient client;
    public TcpClient tcpClient;
    public NetworkStream stream;
    public string text;
    public string text1;
    private int stimCode;
    public void receiveData(int port)
    {
        client = new UdpClient(port);
        while (true)
        {
            IPEndPoint anyIP2 = new IPEndPoint(IPAddress.Parse(IP), 0);
            text1 = ASCIIEncoding.ASCII.GetString(client.Receive(ref anyIP2));

            stimCode = int.Parse(text1.Substring(3, text1.Length - 3));

        }
    }


    // Update is called once per frame
    void Update () {
		
	}
}



//    public TextMesh Stimulus;
//    [SerializeField]
//    private int seqPrm = 1;
//    bci2k BCI2K;
//    private IEnumerator coroutine;
//    public string receivedMsg;

//    private void Start()
//    {
//        BCI2K = GameObject.Find("Main Camera").GetComponent<bci2k>();
//        coroutine = UpdateStimCode(.05f);
//        StartCoroutine(coroutine);
//    }
//    public void configureBCI2000Session()
//    {
//        BCI2K.websockets[0].Send(
//            "E 1 " +
//            "Reset System; " +
//            "Startup system localhost; " +
//            BCI2K.startExecutable("SignalGenerator", "local") +
//            BCI2K.startExecutable("SpectralSignalProcessingMod", "local") +
//            BCI2K.startExecutable("StimulusPresentationCroneLab", "local") +
//            BCI2K.waitFor("Connected") +
//            BCI2K.loadParameterFile("../parms.ecog/SpectralSigProc.prm") +
//            BCI2K.loadParameterFile("../web/paradigms/WordTasks/words.prm") +
//            BCI2K.loadParameterFile("../web/paradigms/WordTasks/sequences/reading/seq1.prm") +
//            BCI2K.setParameter("SubjectName", "TestSubject") +
//            BCI2K.setParameter("SubjectSession", "1") +
//            BCI2K.setParameter("DataFile", "%24%7bSubjectName%7d/WordReading/%24%7bSubjectName%7d_WordReading_S%24%7bSubjectSession%7dR%24%7bSubjectRun%7d.%24%7bFileFormat%7d") +
//            BCI2K.setParameter("SamplingRate", "1000Hz") +
//            BCI2K.setParameter("CaptionSwitch", "1") +
//            BCI2K.setParameter("AudioSwitch", "0") +
//            BCI2K.setParameter("WSSpectralOutputServer", "*:20203") +
//            BCI2K.setParameter("WSConnectorServer", "*:20323") +
//            BCI2K.setParameter("WSSourceServer", "*:20100") +
//            BCI2K.setParameter("VisualizeSource", "0") +
//            BCI2K.setParameter("VisualizeTiming", "0") +
//            BCI2K.setParameter("WindowLeft", "-700") +
//            BCI2K.setConf +
//            BCI2K.waitFor("Resting") //+
//                                     //            BCI2K.start
//        );
//        receiveThread = new Thread(() => receiveData(receivePort));
//        receiveThread.IsBackground = true;
//        receiveThread.Start();

//        BCI2K.websockets[0].Send("E 1 " + BCI2K.setWatch("StimulusCode", "127.0.0.1", "55404"));
//    }

//    private void Update()
//    {



//    }
//    public IEnumerator UpdateStimCode(float waittime)
//    {
//        while (true)
//        {
//            //BCI2K.webSocket.Send("E 1 Get System State");

//            //BCI2K.webSocket.Send("E 1 Get StimulusCode");

//            if (stimCode == 0)
//            {
//                Stimulus.text = "";
//                BCI2K.stim = "";
//            }
//            if (stimCode > 0)
//            {
//                BCI2K.websockets[0].Send("E 2 " + string.Format("Get Parameter Stimuli(1,{0})", stimCode));
//                Stimulus.text = BCI2K.stim;
//            }

//            yield return new WaitForSeconds(waittime);

//        }
//    }
//    private void OnApplicationQuit()
//    {
//        BCI2K.websockets[0].Send("E 1 Exit");

//    }
//    public void OnDestroy()
//    {

//        print("OnDestroy");
//        client.Close();
//        Application.Quit();

//    }

//}
