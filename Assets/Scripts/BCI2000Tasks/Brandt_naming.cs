using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using System.Diagnostics;
using System;

public class Brandt_naming : MonoBehaviour
{

    BCI2000_init initBCI2000 = new BCI2000_init("127.0.0.1", 55404);
    public int listOfImages = 110;
    public GameObject StimulusPlane;
    private SpriteRenderer rend;
    public Sprite[] img;
    public string BCI2000Location = "Assets\\StreamingAssets\\BCI2000";
    [SerializeField]
    private int seqPrm = 2;
    private void Start()
    {
        rend = StimulusPlane.GetComponent<SpriteRenderer>();
        //rend.material = Resources.Load("Brandt_images\\Materials\\001_baby") as Material;
        img = new Sprite[110];

        var imgs = Resources.LoadAll("Brandt_images\\");
        for(int i = 0; i < 110; i++)
        {
            img[i] = imgs[i] as Sprite;
        }
    }
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
            "Load parameterfile " + BCI2000Location + "\\web\\paradigms\\ObjectNaming\\objects.prm" + "; " +
            "Set parameter CaptionSwitch 0; " +
            "Set parameter IconSwitch 1; " +
            "Set parameter AudioSwitch 0; " +
            "Load parameterfile " + BCI2000Location + "\\parms.ecog\\screen_setup.prm" + "; " +
            "Load parameterfile " + BCI2000Location + string.Format("\\web\\paradigms\\ObjectNaming\\sequences\\seq{0}.prm", seqPrm) + "; " +
            "Set parameter WSConnectorServer *:20323; " +
            "Set parameter WSSourceServer *:20100; " +
            "Set parameter VisualizeSource 0; " +
            "Set parameter VisualizeTiming 0; " +
            "Set parameter WindowLeft 7000; " +
            "Set config;" /*+ "Show window;"*/ + "Start";
        Process.Start(PSI);
    }


    public void Update()
    {
        rend.sprite = img[initBCI2000.StimCode];
    }

    public void OnDestroy()
    {
        initBCI2000.quitBCI2000();
        initBCI2000.client.Close();
        Application.Quit();
    }

}
