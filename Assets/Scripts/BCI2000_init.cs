using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using System;
using System.Net;
using System.Text;

public class BCI2000_init : MonoBehaviour {
    public string[] Sources = { "SignalSource", "BioSemi2", "FilePlayback", "NeuroscanClient", "SignalGenerator" };
    public string[] Processing = { "Signal Processing", "ARSignalProcessing", "DummySignalProcessing", "SpectralSignalProcessing","SpectralSignalProcessingMod", "FieldTripBuffer" };
    public string[] Applications = { "Application", "CursorTask", "DummyApplication", "StimulusPresentationCroneLab" };

    public string IP = "127.0.0.1";            //bind it to the computer's IP adress that is running the VR app
    public int receivePort = 55404;
    public int sendPort = 55405;
    public string text;
    public Thread receiveThread;
    public Thread sendThread;

    public UdpClient client;
    public TcpClient tcpClient;
    public NetworkStream stream;
    public string BCI2000Location = "Assets\\StreamingAssets\\BCI2000";

    public int StimCode;

    public BCI2000_init(string ip, int port)
    {
        IP = ip;            //bind it to the computer's IP adress that is running the VR app
        receivePort = port;
    }

    public void onQuit()
    {
        Application.Quit();

        quitBCI2000();
    }
    
    public void quitBCI2000()
    {
        ProcessStartInfo PSI = new ProcessStartInfo("Assets\\StreamingAssets\\BCI2000" + "\\prog\\" + "BCI2000Shell.exe");
        PSI.Arguments = "-c Change directory " + "Assets\\StreamingAssets\\BCI2000" + "\\prog;" +
            "Quit";
        Process.Start(PSI);
    }

    public void receiveData(int port)
    {
        client = new UdpClient(port);
        while (true)
        {
            IPAddress test1 = IPAddress.Parse(IP);
            IPEndPoint anyIP2 = new IPEndPoint(test1, 0);
            byte[] data2 = client.Receive(ref anyIP2);

            text = ASCIIEncoding.ASCII.GetString(data2);
            if (text.IndexOf("StimulusCode") == 0)
            {
                StimCode = Int32.Parse(text.Substring(text.IndexOf("e") + 2));
            }

        }
    }

}
