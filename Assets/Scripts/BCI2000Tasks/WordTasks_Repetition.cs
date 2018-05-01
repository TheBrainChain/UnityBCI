using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using System.Diagnostics;
using System;

public class WordTasks_Repetition : MonoBehaviour
{

    BCI2000_init initBCI2000 = new BCI2000_init("127.0.0.1", 55404);
    public string BCI2000Location = "Assets\\StreamingAssets\\BCI2000";
    [SerializeField]
    private int seqPrm = 1;
    public void runBCI2000()
    {
        configureBCI2000Session("SignalGenerator", "SpectralSignalProcessingMod", "StimulusPresentationCroneLab", "CGC", "127.0.0.1", 55404);
        initBCI2000.receiveThread = new Thread(() => initBCI2000.receiveData(initBCI2000.receivePort));
        initBCI2000.receiveThread.IsBackground = true;
        initBCI2000.receiveThread.Start();
    }
    public void configureBCI2000Session(string Source, string Processing, string Applictions, string subjName, string IP, int port)
    {
        ProcessStartInfo PSI = new ProcessStartInfo(BCI2000Location + "\\prog\\" + "BCI2000Shell.exe");
        PSI.Arguments = "-c Change directory " + BCI2000Location + "\\prog; Startup system;" +
            "Start executable " + BCI2000Location + "\\prog\\" + Source + ";" +
            "Start executable " + BCI2000Location + "\\prog\\" + Processing + ";" +
            "Start executable " + BCI2000Location + "\\prog\\" + Applictions + ";" +
            "Wait for Connected; Load parameterfile " + BCI2000Location + "\\parms.ecog\\SpectralSigProc.prm" + ";" +
            "Set parameter SubjectName " + subjName + "_" + DateTime.Today.ToString("yy-MM-dd") + ";" +
            "Set parameter ConnectorOutputAddress " + IP + ":" + port.ToString() + ";" +
            "Set parameter WSSpectralOutputServer *:20203; " +
            "Load parameterfile " + BCI2000Location + "\\web\\paradigms\\WordTasks\\words.prm" + "; " +
            "Set parameter CaptionSwitch 0; " +
            "Set parameter AudioSwitch 1; " +
            "Load parameterfile " + BCI2000Location + "\\parms.ecog\\screen_setup.prm" + "; " +
            "Load parameterfile " + BCI2000Location + string.Format("\\web\\paradigms\\WordTasks\\sequences\\repetition\\seq{0}.prm", seqPrm) + "; " +
            "Set parameter WSConnectorServer *:20323; " +
            "Set parameter WSSourceServer *:20100; " +
            "Set parameter VisualizeSource 0; " +
            "Set parameter VisualizeTiming 0; " +
            "Set parameter WindowLeft 7000; " +
            "Set config;" + "Start";// + "Show window";
        Process.Start(PSI);
    }
    public void Update()
    {
    }

    public void OnDestroy()
    {
        initBCI2000.quitBCI2000();
        initBCI2000.client.Close();
        Application.Quit();
    }

}
