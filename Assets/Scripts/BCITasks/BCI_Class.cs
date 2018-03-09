
using System.Diagnostics;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
//
public class BCI_Class {

    // Applications present in BCI2000
    public string[] Sources = {"SignalSource", "BioSemi2", "FilePlayback", "NeuroscanClient", "SignalGenerator"};
	public string[] Processing = { "Signal Processing", "ARSignalProcessing", "DummySignalProcessing", "SpectralSignalProcessing", "FieldTripBuffer","P3SignalProcessing" };
	public string[] Applications = { "Application", "DummyApplication","VR_LR_Candle","VR_UD_Candle","VR_2D_Candle","P300","Sandbox" };

	//Networking addresses
	public string IP;
    public int receivePort;
	public UdpClient client;

	//Thread to receive data from BCI2000
	public Thread receiveThread;

	//Writing to BCI2000Operator
	public TcpClient tcpClient;
	public NetworkStream stream;

	//BCI2000 state variables to read and write
	public float CursorPosX, CursorPosY,TargetCode, ResultCode, Feedback, ContinuousCursX, ContinuousCursY;
	public float SignalCode,SignalCode1,SignalCode2, RunningState;
	public string CursorPos, RunningStateS, text;
	public int stimCode, colCode, rowCode;

	public BCI_Class(string ip, int port)
	{
		IP = ip;            //bind it to the computer's IP adress that is running the VR app
		receivePort = port;
	}

	//Writes state variables to BCI2000. These have to be initialized in configureBCI2000Session prior to being set.
	public void setState(string TorRorF, float num)
	{
		ProcessStartInfo PSI = new ProcessStartInfo("Assets\\BCI2000\\prog\\BCI2000Shell.exe");
		if (TorRorF == "Target")
		{
			PSI.Arguments = string.Format ("-c SET STATE TargetCode {0}", num);
		} else if (TorRorF == "Result")
		{
			PSI.Arguments = string.Format ("-c SET STATE ResultCode {0}", num);
		}
		else if (TorRorF == "Feedback")
		{
			PSI.Arguments = string.Format("-c SET STATE Feedback {0}", num);
		}
		Process.Start(PSI);
	}

	//Separate thread to constantly receive state variable data from BCI2000.
	//Currently reading bytes and converting then to strings, I don't think this is optimal.
	public void receiveData(int port)
    {
		client = new UdpClient(port);
        while (true)
        {
            IPAddress test1 = IPAddress.Parse(IP);
            IPEndPoint anyIP2 = new IPEndPoint(test1, 0);
			byte[] data2 = client.Receive(ref anyIP2);

            text = ASCIIEncoding.ASCII.GetString(data2);
			//UnityEngine.Debug.Log (text);

			if (text.IndexOf ("CursorPosX") == 0)
			{
				int i = text.IndexOf ('X');
				CursorPos = text.Substring (i + 2);
				CursorPosX = float.Parse (CursorPos) - 2047;
			} else if (text.IndexOf ("Running") == 0)
			{
				int i = text.IndexOf ("g");
				RunningStateS = text.Substring (i + 2);
				RunningState = float.Parse (RunningStateS);
			}
			else if (text.IndexOf("CursorPosY") == 0)
			{
				int i = text.IndexOf('Y');
				CursorPos = text.Substring(i + 2);
				CursorPosY = float.Parse(CursorPos) - 2047;
			}
			else if (text.IndexOf("TargetCode") == 0)
            {
                int i = text.IndexOf('e');
                String TargetCodez = text.Substring(i + 7);
				TargetCode = float.Parse(TargetCodez);
            }
			else if (text.IndexOf("ResultCode") == 0)
            {
                int i = text.IndexOf('e');
                String ResultCodez = text.Substring(i + 10);
				ResultCode = float.Parse(ResultCodez);
            }
            else if (text.IndexOf("Feedback") == 0)
            {
                int i = text.IndexOf('k');
                String Signal = text.Substring(i + 2);
				Feedback = float.Parse(Signal);
            }
			else if (text.IndexOf("Signal(0,0)") == 0)
			{
				int i = text.IndexOf(')');
				String Signal = text.Substring(i + 2);
				SignalCode = float.Parse(Signal, System.Globalization.CultureInfo.InvariantCulture);
			}
			else if (text.IndexOf("Signal(1,0)") == 0)
			{
				int i = text.IndexOf(')');
				String Signal = text.Substring(i + 2);
				SignalCode2 = float.Parse(Signal, System.Globalization.CultureInfo.InvariantCulture);
			}
			if (text.IndexOf ("StimulusCode ") == 0)
			{
				stimCode = int.Parse (text.Substring (12));
				UnityEngine.Debug.Log (stimCode);

			}
			else if (text.IndexOf ("SelectedRow") == 0)
			{
				rowCode = int.Parse (text.Substring (12));
			}
			else if (text.IndexOf ("SelectedColumn") == 0)
			{
				colCode = int.Parse (text.Substring (15));
			}
        }
    }
}
