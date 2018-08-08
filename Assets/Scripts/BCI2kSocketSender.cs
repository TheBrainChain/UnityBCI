using BestHTTP;
using BestHTTP.WebSocket;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using UnityEngine;

// We receive this kind of message from the websocket.
[Serializable]

public class BCI2kSocketSender : MonoBehaviour
{

//    public delegate void CommandAction(string commandText);
//    public static event CommandAction OnCommand;

//    /// <summary>
//    /// The WebSocket address to connect
//    /// </summary>
//    static public string bci2kIPandPort = "ws://127.0.0.1:80/";
//    static public string localPort = "12001";
//    static private byte[] bci2kUdpBuffer;
//    private int lastKeyword = 0;

//    static protected Socket bci2kWatchSocket;
//    static protected EndPoint bci2kWatchEP;

//    static private WebSocket webSocket;
//    static private bool b_WebSocketOpen = false;
//    public GameObject gameSystemObject;

//    // read the UDP port for any new data
//    void ReadUDPPort_Watch()
//    {
//        // check any 
//        if (bci2kWatchSocket != null && bci2kWatchSocket.Available > 0)
//        {
//            // Get the most recent packet.
//            while (bci2kWatchSocket.Available > 0)
//            {
//                bci2kWatchSocket.ReceiveFrom(bci2kUdpBuffer, ref bci2kWatchEP);

//                // convert from bytes to string
//                string dataString = System.Text.Encoding.UTF8.GetString(bci2kUdpBuffer).Trim();

//                // parse dataString
//                int tabIndex = dataString.IndexOf('\t');
//                int tempKeyword = int.Parse(dataString.Substring(tabIndex + 1));
//                if (tempKeyword > 0)
//                {
//                    lastKeyword = tempKeyword;
//                    Debug.Log("BCI2k UDP Keyword Registered: " + lastKeyword.ToString() + "; (str length=" + dataString.Length.ToString() + "):" + dataString);
//                }
//            }

//        }
//    }

//    // read the UDP port for any new data
//    List<int> ReadUDPPort_AppConnector(string filter, ref DateTime lastPacketTime)
//    {
//        // check any 
//        List<int> values = new List<int>();
//        lastPacketTime = DateTime.Now;
//        if (bci2kWatchSocket != null && bci2kWatchSocket.Available > 0)
//        {
//            // Get the most recent packet.
//            while (bci2kWatchSocket.Available > 0)
//            {
//                bci2kWatchSocket.ReceiveFrom(bci2kUdpBuffer, ref bci2kWatchEP);

//                // convert from bytes to string
//                string dataString = System.Text.Encoding.UTF8.GetString(bci2kUdpBuffer);
//                if (dataString.Substring(0, filter.Length).Contains(filter))
//                {
//                    values.Add(int.Parse(dataString.Substring(filter.Length, dataString.IndexOf('\n') - filter.Length)));
//                    lastPacketTime = DateTime.Now;
//                }
//            }
//        }
//        return values;
//    }

//    // Use this for initialization
//    void Start()
//    {

//        IPEndPoint socketEndPt = new IPEndPoint(IPAddress.Any, int.Parse(localPort));
//        bci2kWatchSocket = new Socket(AddressFamily.InterNetwork,
//            SocketType.Dgram, ProtocolType.Udp);
//        bci2kWatchSocket.Bind(socketEndPt);
//        bci2kUdpBuffer = new byte[2048];

//        // this will be used to keep track of where info came from
//        IPEndPoint fromEndPt = new IPEndPoint(IPAddress.Any, 0);
//        bci2kWatchEP = (EndPoint)fromEndPt;

//        // Set up WebSocket
//        // Create the WebSocket instance
//        webSocket = new WebSocket(new Uri(bci2kIPandPort));

//#if !BESTHTTP_DISABLE_PROXY && !UNITY_WEBGL
//        if (HTTPManager.Proxy != null)
//            webSocket.InternalRequest.Proxy = new HTTPProxy(HTTPManager.Proxy.Address, HTTPManager.Proxy.Credentials, false);
//#endif

//        // Subscribe to the WS events
//        webSocket.OnOpen += OnOpen;
//        webSocket.OnMessage += OnMessageReceived;
//        webSocket.OnClosed += OnClosed;
//        webSocket.OnError += OnError;


//    }

//    // this function allows the web socket to be started manually during replay mode

//    private void Update()
//    {
//        float currTime = Time.realtimeSinceStartup;

//        //ReadUDPPort_Watch();

//        // read from the AppConnector
//        DateTime packetTime = DateTime.MinValue;
//        List<int> keywordVals = ReadUDPPort_AppConnector("HMMActiveState", ref packetTime); //TODO: make the target string a public var
        

//    }

//    public static long ToUnix(DateTime datetime)
//    {
//        DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
//        return (long)((datetime - sTime).TotalSeconds * 1000);
//    }

//    // expression e.g., Keyword!=0
//    public static void AddWatch(string expression)
//    {
//        string msgString = "E " + ((int)UnityEngine.Random.Range(0f, 1000000f)).ToString() + " ADD WATCH " + expression + " AT 127.0.0.1:" + localPort;
//        if (webSocket != null)
//        {
//            webSocket.Send(msgString);
//            DateTime msgTime = DateTime.Now;
//            Debug.Log("Added Watch " + msgString + " at " + msgTime.ToString());
//        }
//        else
//        {
//            //Debug.Log("UNABLE TO Send " + msgString + " at " + msgTime.ToString());
//        }
//    }

//    public static void SendBCI2kState(string stateName, string stateVal)
//    {
//        string msgString = "E " + ((int)UnityEngine.Random.Range(0f, 1000000f)).ToString() + " SET STATE " + stateName + " " + stateVal;
//        if (webSocket != null)
//        {
//            webSocket.Send(msgString);
//            DateTime msgTime = DateTime.Now;
//            gameInterface.writeToLogFile("BCI2k State," + msgString, msgTime);
//            Debug.Log("Sent " + msgString + " at " + msgTime.ToString());
//        }
//        else
//        {
//            gameInterface.writeToLogFile("BCI2k State NOT WRITTEN," + msgString, DateTime.Now);
//            //Debug.Log("UNABLE TO Send " + msgString + " at " + msgTime.ToString());
//        }
//    }

//    public static void SendBCI2kEventPulse(string eventName, string eventVal, GameInterface gameInterface)
//    {
//        string msgString = "E " + ((int)UnityEngine.Random.Range(0f, 1000000f)).ToString() + " PULSE EVENT " + eventName + " " + eventVal;
//        if (webSocket != null)
//        {
//            webSocket.Send(msgString);
//            DateTime msgTime = DateTime.Now;
//            gameInterface.writeToLogFile("BCI2k Event," + msgString, msgTime);
//            Debug.Log("Sent " + msgString + " at " + msgTime.ToString());
//        }
//        else
//        {
//            gameInterface.writeToLogFile("BCI2k Event NOT WRITTEN," + msgString, DateTime.Now);
//            //Debug.Log("UNABLE TO Send " + msgString + " at " + msgTime.ToString());
//        }
//    }

//    public static void SendBCI2kEvent(string eventName, string eventVal, GameInterface gameInterface)
//    {
//        string msgString = "E " + ((int)UnityEngine.Random.Range(0f, 1000000f)).ToString() + " SET EVENT " + eventName + " " + eventVal;
//        if (webSocket != null)
//        {
//            webSocket.Send(msgString);
//            DateTime msgTime = DateTime.Now;
//            gameInterface.writeToLogFile("BCI2k Event," + msgString, msgTime);
//            Debug.Log("Sent " + msgString + " at " + msgTime.ToString());
//        }
//        else
//        {
//            gameInterface.writeToLogFile("BCI2k Event NOT WRITTEN," + msgString, DateTime.Now);
//            //Debug.Log("UNABLE TO Send " + msgString + " at " + msgTime.ToString());
//        }
//    }

//    void OnOpen(WebSocket ws)
//    {
//        Debug.Log("WebSocket Open");

//        // send the running state
//        SendBCI2kState("Running", "1", gameInterface_Class);

//        // add a watch for the keyword
//        //AddWatch("Keyword", DateTime.Now, gameInterface_Class);
//    }

//    void OnMessageReceived(WebSocket ws, string message)
//    {
//        Debug.LogFormat("Message received: {0}\n", message);
//    }

//    void OnClosed(WebSocket ws, UInt16 code, string message)
//    {
//        Debug.LogFormat("WebSocket closed! Code: {0} Message: {1}\n", code, message);
//        webSocket = null;
//    }

//    void OnError(WebSocket ws, Exception ex)
//    {
//        string errorMsg = string.Empty;
//#if !UNITY_WEBGL || UNITY_EDITOR
//        if (ws.InternalRequest.Response != null)
//            errorMsg = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
//#endif

//        Debug.LogFormat("WebSocket error occured: {0}\n", (ex != null ? ex.Message : "Unknown Error " + errorMsg));

//        webSocket = null;
//    }

//    private void OnApplicationQuit()
//    {
//        if (webSocket != null)
//        {
//            // send the running state
//            SendBCI2kState("Running", "0", gameInterface_Class);

//            IEnumerator closingCoroutine = CloseWebSocket(1.0f); // wait a half second before closing the socket
//            StartCoroutine(closingCoroutine);
//        }

//        if (bci2kWatchSocket != null)
//            bci2kWatchSocket.Close();
//    }

//    // the close command arrives before the stop running command.  this helps delay it
//    private IEnumerator CloseWebSocket(float waitTime)
//    {
//        yield return new WaitForSeconds(waitTime);
//        webSocket.Close();
//        Debug.Log("Closed WebSocket Successfully!");
//    }

}
