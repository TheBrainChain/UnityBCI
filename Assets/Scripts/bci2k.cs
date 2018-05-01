#if !BESTHTTP_DISABLE_WEBSOCKET

using System;
using UnityEngine;
using BestHTTP.WebSocket;
using UnityEngine.UI;

public class bci2k : MonoBehaviour
{
    public Button openWSButton;
    public Button sendMsgButton;
    public Button closeWSButton;
    public Button wordTaskButton;
    WebSocket webSocket;
    public InputField txt2send;
    public string address = "ws://127.0.0.1";

    //string msgToSend = "E 1 Get System State";
    string msgToSend = "E 1 Reset System; Startup system localhost; Start executable SignalGenerator --local; Start executable SpectralSignalProcessingMod --local; Start executable StimulusPresentationCronelab --local; Wait for Connected; Set parameter SubjectName TESTSubject; Set parameter SubjectSession 1; Set Parameter SamplingRate 1000Hz; Load Parameterfile ../parms.ecog/SpectralSigProc.prm; Set Parameter WSSpectralOutputServer *:20203; Load Parameterfile ../web/paradigms/WordTasks/words.prm; Set Parameter CaptionSwitch 1; Set Parameter AudioSwitch 0; Load Parameterfile ../parms.ecog/screen_setup.prm; Load Parameterfile ../web/paradigms/WordTasks/sequences/reading/seq1.prm; Set Parameter WSConnectorServer *:20323; Set Parameter WSSourceServer *:20100; Set Config; Wait for Resting; ";
    string showWin = "E 1 Show Window";
    string hideWin = "E 1 Hide Window";
    string resetSys = "E 1 Reset System";
    string startSys = "Startup system localhost";
    //string startSig = "Start executable SignalGenerator --local";
    //string startPrc = "Start executable SpectralSignalProcessingMod --local";
    //string startApp = "Start executable StimulusPresentationCronelab --local";
    //string WaitConn = "Wait for Connected";
    //string setPrmSNTest = "Set parameter SubjectName TESTSubject";
    //string setPrmSS1 = "Set parameter SubjectSession 1";
    //string setPrmDataFile = "Set parameter DataFile "'%24%7bSubjectName%7d/WordReading/%24%7bSubjectName%7d_WordReading_S%24%7bSubjectSession%7dR%24%7bSubjectRun%7d.%24%7bFileFormat%7d'"";
    //string setPrmSR1000 = "Set Parameter SamplingRate 1000Hz";
    //string loadPrmFileSpecSigPrc = "Load Parameterfile../parms.ecog/SpectralSigProc.prm";
    //string setPrmWOS = "Set Parameter WSSpectralOutputServer*:20203";
    //string loadPrmFileWords = "Load Parameterfile../web/paradigms/WordTasks/words.prm";
    //string setPrmCS1 = "Set Parameter CaptionSwitch 1";
    //string setPrmAS0 = "Set Parameter AudioSwitch 0";
    //string loadPrmFileScreenSetup = "Load Parameterfile../parms.ecog/screen_setup.prm";
    //string loadPrmFileRead1 = "Load Parameterfile../web/paradigms/WordTasks/sequences/reading/seq1.prm";
    //string setPrmWCS = "Set Parameter WSConnectorServer*:20323";
    //string setPrmWSS = "Set Parameter WSSourceServer*:20100";
    string setConf = "Set Config;";
    //string waitRest = "Wait for Resting";
    string exit = "Exit;";
    string stop = "Stop;";
    string start = "Start";

    private string startExecutable(string prm, string loc)
    {
        return string.Format("Start executable {0} --{1}; ", prm, loc);
    }
    private string waitFor(string prm)
    {
        return string.Format("Wait for {0}; ", prm);
    }

    private string setWatch(string prm, string ip, string port)
    {
        return string.Format("Add watch {0} at {1}:{2}; ", prm, ip, port);
    }

    private string loadParameterFile(string prm)
    {
        return string.Format("Load Parameterfile {0}; ", prm);
    }

    private string setParameter(string prm1, string prm2)
    {
        return string.Format("Set Parameter {0} {1}; ", prm1, prm2);
    }
    private string getParameter(string prm = "Stimuli(1,2) ")
    {
        return string.Format("Get Parameter {0};", prm);
    }

    private string listParameter(string prm = "Stimuli")
    {
        return string.Format("List Parameter {0}; ", prm);
    }

    private void Start()
    {
        openWSButton.GetComponent<Button>().onClick.AddListener(openWS);
        sendMsgButton.GetComponent<Button>().onClick.AddListener(sendWSmsg);
        closeWSButton.GetComponent<Button>().onClick.AddListener(closeWS);
        wordTaskButton.GetComponent<Button>().onClick.AddListener(WordTasks);
    }
    private void WordTasks()
    {
        //Set Parameter WSConnectorServer *:20323; Set Parameter WSSourceServer *:20100; Set Config; Wait for Resting;
        if (webSocket != null && webSocket.IsOpen)
        {
            webSocket.Send(
                "E 1 " +
                "Reset System; " +
                "Startup system localhost; " +
                startExecutable("SignalGenerator", "local") +
                startExecutable("SpectralSignalProcessingMod", "local") +
                startExecutable("StimulusPresentationCroneLab", "local") +
                waitFor("Connected") +
                setParameter("SubjectName", "TestSubject") +
                setParameter("SubjectSession", "1") +
                setParameter("DataFile", "%24%7bSubjectName%7d/WordReading/%24%7bSubjectName%7d_WordReading_S%24%7bSubjectSession%7dR%24%7bSubjectRun%7d.%24%7bFileFormat%7d") +
                setParameter("SamplingRate", "1000Hz") +
                loadParameterFile("../parms.ecog/SpectralSigProc.prm") +
                setParameter("WSSpectralOutputServer", "*:20203") +
                loadParameterFile("../web/paradigms/WordTasks/words.prm") +
                setParameter("CaptionSwitch", "1") +
                setParameter("AudioSwitch", "0") +
                loadParameterFile("../parms.ecog/screen_setup.prm") +
                loadParameterFile("../web/paradigms/WordTasks/sequences/reading/seq1.prm") +
                setParameter("WSConnectorServer", "*:20323") +
                setParameter("WSSourceServer", "*:20100") +
                setConf +
                waitFor("Resting")
            );
        }
    }
    void openWS()
    {
        if (webSocket == null)
        {
            // Create the WebSocket instance
            webSocket = new WebSocket(new Uri(address));

            // Subscribe to the WS events
            webSocket.OnOpen += OnOpen;
            webSocket.OnMessage += OnMessageReceived;
            webSocket.OnClosed += OnClosed;
            webSocket.OnError += OnError;

            // Start connecting to the server
            webSocket.Open();
        }
    }
    void sendWSmsg()
    {
        if (webSocket != null && webSocket.IsOpen)
        {
            webSocket.Send("E 1 " + txt2send.text);
        }
    }
    void closeWS()
    {
        webSocket.Close(1000, "Bye!");

    }
    void OnDestroy()
    {
        if (webSocket != null)
            webSocket.Close();
    }
    void OnOpen(WebSocket ws)
    {
        print("Websocket is open!");
    }
    void OnMessageReceived(WebSocket ws, string message)
    {
        print(string.Format("-Message received: {0}\n", message));
    }
    void OnClosed(WebSocket ws, UInt16 code, string message)
    {
        print(string.Format("-WebSocket closed! Code: {0} Message: {1}\n", code, message));
        webSocket = null;
    }
    void OnError(WebSocket ws, Exception ex)
    {
        string errorMsg = string.Empty;
#if !UNITY_WEBGL || UNITY_EDITOR
        if (ws.InternalRequest.Response != null)
            errorMsg = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
#endif

        print(string.Format("-An error occured: {0}\n", (ex != null ? ex.Message : "Unknown Error " + errorMsg)));
        webSocket = null;
    }
}

#endif