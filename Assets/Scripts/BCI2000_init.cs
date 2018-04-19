using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using System;

public class BCI2000_init : MonoBehaviour {
    public string[] Sources = { "SignalSource", "BioSemi2", "FilePlayback", "NeuroscanClient", "SignalGenerator" };
    public string[] Processing = { "Signal Processing", "ARSignalProcessing", "DummySignalProcessing", "SpectralSignalProcessing", "FieldTripBuffer" };
    public string[] Applications = { "Application", "CursorTask", "DummyApplication" };

    public string IP = "127.0.0.1";            //bind it to the computer's IP adress that is running the VR app
    public int receivePort = 55404;
    public int sendPort = 55405;

    public Thread receiveThread;
    public Thread sendThread;

    public UdpClient client;
    public TcpClient tcpClient;
    public NetworkStream stream;
    public string BCI2000Location = "D:\\Projects\\Hopkins\\GIT_bci2000web\\prog";

    public BCI2000_init(string ip, int port)
    {
        IP = ip;            //bind it to the computer's IP adress that is running the VR app
        receivePort = port;
    }


    public void configureBCI2000Session(string Source, string Processing, string Applictions, string subjName, string IP, int port)
    {
        ProcessStartInfo PSI = new ProcessStartInfo(BCI2000Location + "\\" + "BCI2000Shell.exe");
        PSI.Arguments = "-c Change directory C:\\Users\\fortu\\Documents\\BCI2002\\prog; Startup system;" +
            "Start executable " + BCI2000Location + "\\" + Source + ";" +
            "Start executable " + BCI2000Location + "\\" + Processing + ";" +
            "Start executable " + BCI2000Location + "\\" + Applictions + ";" +
            //"Wait for Connected; Load parameterfile " + pathToParam + ";" +
            "Set parameter SubjectName " + subjName + "_" + DateTime.Today.ToString("yy-MM-dd") + ";" +
            "Set parameter ConnectorOutputAddress " + IP + ":" + port.ToString() + ";" +
            "Set config; Show window";
        Process.Start(PSI);
    }
}
