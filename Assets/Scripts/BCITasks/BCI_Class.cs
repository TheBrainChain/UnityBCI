using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class BCI_Class {
    
    public string[] Sources = {"SignalSource", "BioSemi2", "FilePlayback", "NeuroscanClient", "SignalGenerator"};
    public string[] Processing = { "Signal Processing", "ARSignalProcessing", "DummySignalProcessing", "SpectralSignalProcessing", "FieldTripBuffer" };
    public string[] Applications = { "Application", "CursorTask", "DummyApplication" };

    public string IP = "127.0.0.1";            //bind it to the computer's IP adress that is running the VR app
    public int receivePort = 55404;
    public int sendPort = 55405;

    public string text;
    public Thread receiveThread;
    public Thread sendThread;

    public UdpClient client;
    public TcpClient tcpClient;
    public NetworkStream stream;
    public int CursorPosX = 0;
    public int TargetCode = 0;
    public int ResultCode = 0;

    public string CursorPos;
    public int Feedback;
    public float SignalCode;
    public float SignalCode1;
    public float SignalCode2;

    public int stimCode;
    public int rowCode;
    public int colCode;
    public string BCI2000Location = "D:\\Projects\\Hopkins\\GIT_bci2000web\\prog";
    public float CursorPosY= 0f;
    public float RunningState;
    public string RunningStateS;
    public BCI_Class(string ip, int port)
    {
        IP = ip;            //bind it to the computer's IP adress that is running the VR app
        receivePort = port;
    }
    public void setState(string TorRorF, float num)
    {
        ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2000\\prog\\BCI2000Shell.exe");
        if (TorRorF == "Target")
        {
            PSI.Arguments = string.Format("-c SET STATE TargetCode {0}", num);
        }
        else if (TorRorF == "Result")
        {
            PSI.Arguments = string.Format("-c SET STATE ResultCode {0}", num);
        }
        else if (TorRorF == "Feedback")
        {
            PSI.Arguments = string.Format("-c SET STATE Feedback {0}", num);
        }
        Process.Start(PSI);
    }

    public void configureBCI2000Session(string Source, string Processing, string Applictions, string pathToParam, string subjName, string IP, int port)
    {
        UnityEngine.Debug.Log(Applications);
        ProcessStartInfo PSI = new ProcessStartInfo(BCI2000Location + "\\" + "BCI2000Shell.exe");
        PSI.Arguments = "-c Change directory C:\\Users\\fortu\\Documents\\BCI2002\\prog; Startup system;" +
            "Start executable " + BCI2000Location + "\\" + Source + ";" +
            "Start executable " + BCI2000Location + "\\" + Processing + ";" +
            "Start executable " + BCI2000Location + "\\" + Applictions + ";" +
            "Wait for Connected; Load parameterfile " + pathToParam + ";" +
            "Set parameter SubjectName " + subjName + "_" + DateTime.Today.ToString("yy-MM-dd") + ";" +
            "Set parameter ConnectorOutputAddress " + IP + ":" + port.ToString() + ";" +
            "ADD STATE T 2 1" + ";" +
            "ADD STATE R 2 1" + ";" +
            "Set config; Show window";
        Process.Start(PSI);
    }

    public void startExp()
    {
        ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2002\\prog\\BCI2000Shell.exe");
        PSI.Arguments = "-c Start";
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
            else if (text.IndexOf("Signal(0,1)") == 0)
            {
                int i = text.IndexOf(')');
                String Signal1 = text.Substring(i + 2);
                SignalCode1 = float.Parse(Signal1, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (text.IndexOf("Signal(1,0)") == 0)
            {
                int i = text.IndexOf(')');
                String Signal2 = text.Substring(i + 2);
                SignalCode2 = float.Parse(Signal2, System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }

}
