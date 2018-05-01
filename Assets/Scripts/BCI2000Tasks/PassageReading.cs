using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using System.Diagnostics;
using System;

public class PassageReading : MonoBehaviour
{
    BCI2000_init initBCI2000 = new BCI2000_init("127.0.0.1", 55404);
    private string[] RFrost = new string[] {
    "", "Two", "roads", "diverged", "in", "a", "yellow", "wood,",
    "And", "sorry", "I", "could", "not", "travel", "both",
    "And", "be", "one", "traveler,", "long", "I", "stood",
    "And", "looked", "down", "one", "as", "far", "as", "I", "could",
    "To", "where", "it", "bent", "in", "the", "undergrowth;",

    "Then", "took", "the", "other,", "as", "just", "as", "fair,",
    "And", "having", "perhaps", "the", "better", "claim,",
    "Because", "it", "was", "grassy", "and", "wanted", "wear;",
    "Though", "as", "for", "that", "the", "passing", "there",
    "Had", "worn", "them", "really", "about", "the", "same,",

    "And", "both that morning", "equally", "lay",
    "In", "leaves", "no", "step", "had", "trodden", "black.",
    "Oh,", "I", "kept", "the", "first", "for", "another", "day!",
    "Yet", "knowing", "how", "way", "leads", "on", "to", "way,",
    "I", "doubted", "if", "I", "should", "ever", "come", "back.",

    "I", "shall", "be", "telling", "this", "with", "a", "sigh",
    "Somewhere", "ages", "and", "ages", "hence:",
    "Two", "roads", "diverged", "in", "a", "wood,", "and", "I—",
    "I", "took", "the", "one", "less", "traveled", "by,",
    "And", "that", "has", "made", "all", "the", "difference."
        };


    public TextMesh Stimulus;
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
            "Load parameterfile " + BCI2000Location + "\\web\\paradigms\\ExtendedReading\\RFrost_.prm" + "; " +
            "Set parameter CaptionSwitch 1; " +
            "Set parameter AudioSwitch 0; " +
            "Load parameterfile " + BCI2000Location + "\\parms.ecog\\screen_setup.prm" + "; " +
            "Set parameter WSConnectorServer *:20323; " +
            "Set parameter WSSourceServer *:20100; " +
            "Set parameter VisualizeSource 0; " +
            "Set parameter VisualizeTiming 0; " +
            "Set parameter WindowLeft 7000; " +
            "Set config;" + "Show window;" + "Start" ;
        Process.Start(PSI);
    }


    public void Update()
    {
        Stimulus.text = RFrost[initBCI2000.StimCode];
    }

    public void OnDestroy()
    {
        initBCI2000.quitBCI2000();
        initBCI2000.client.Close();
        Application.Quit();
    }

}
