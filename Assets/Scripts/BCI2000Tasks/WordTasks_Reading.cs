using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using System.Diagnostics;
using System;

public class WordTasks_Reading : MonoBehaviour {

    BCI2000_init initBCI2000 = new BCI2000_init("127.0.0.1", 55404);
    private string[] wordList = new string[] { "","BELT", "BLADE", "BROAD", "BULB", "BULL", "BURNT", "CAGE", "CASK", "CAUSE", "CHOIR", "CHORE", "CLERK", "CLICK", "CLIMB", "CLOCK", "COKE", "COMB", "COURT", "CURVE", "CUSP", "DEAF", "DICE", "DOUBT", "DOUGH", "DRAIN", "DRINK", "DUCT", "EFFECT", "EXTENT", "FAITH", "FEAR", "FILL", "FILM", "FIRE", "FLOOR", "FONT", "FOREST", "FRESH", "FROM", "GIFT", "GOLF", "GUARD", "GULF", "HEALTH", "HEART", "HEIGHT", "ISLAND", "JUICE", "KNIFE", "LAND", "LIMB", "MANNER", "METHOD", "MIST", "MOLE", "MONK", "MOUTH", "MYTH", "NAIL", "NOUN", "NUDE", "OFFICE", "PACK", "PEACE", "PLAIT", "PLANE", "PLANT", "POET", "PRIDE", "PRINCE", "RATE", "SCALP", "SCARF", "SERF", "SHACK", "SIEVE", "SINK", "SKATE", "SLATE", "SLIME", "SOAP", "SOUL", "SPARK", "SPIKE", "SPOOK", "STING", "STYLE", "SUEDE", "SULK", "SVELTE", "SWAMP", "SWEAT", "SWORD", "TEETH", "TENTH", "THEIR", "THESE", "TONGUE", "TOOTH", "TOUR", "TRAIN", "TRAP", "TRUCK", "TRUTH", "TSAR", "TYPE", "VASE", "VEIL", "VENT", "WAIST", "WATCH", "WEDGE", "WINDOW", "WITH", "WORSE", "WORTH" };
    public TextMesh Stimulus;
    public string BCI2000Location = "Assets\\StreamingAssets\\BCI2000";
    [SerializeField]
    private int seqPrm = 1;
    public void runBCI2000()
    {
        configureBCI2000Session("SignalGenerator", "SpectralSignalProcessingMod", "StimulusPresentationCroneLab", "CGC", "127.0.0.1",55404);
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
            "Set parameter CaptionSwitch 1; " +
            "Set parameter AudioSwitch 0; " +
            "Load parameterfile " + BCI2000Location + "\\parms.ecog\\screen_setup.prm" + "; " +
            "Load parameterfile " + BCI2000Location + string.Format("\\web\\paradigms\\WordTasks\\sequences\\reading\\seq{0}.prm",seqPrm) + "; " +
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
        Stimulus.text = wordList[initBCI2000.StimCode];
    }

    public void OnDestroy()
    {
        initBCI2000.quitBCI2000();
        initBCI2000.client.Close();
        Application.Quit();
    }

}
