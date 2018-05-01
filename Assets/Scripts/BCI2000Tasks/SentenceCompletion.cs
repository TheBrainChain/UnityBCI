using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using System.Diagnostics;
using System;

public class SentenceCompletion : MonoBehaviour
{

    BCI2000_init initBCI2000 = new BCI2000_init("127.0.0.1", 55404);
    //Replace this with a "Get Parameter Stimulii" or something.
    private string[] wordList = new string[] { "", "They", "took", "short", "trips", "during", "the", "________.", "The", "hunter", "shot", "and", "killed", "a", "large", "________.", "He", "was", "afraid", "to", "work", "the", "night", "________.", "The", "old", "house", "was", "built", "entirely", "of", "________.", "Bill", "jumped", "in", "the", "lake", "and", "made", "a", "big", "________.", "The", "man", "was", "caught", "selling", "an", "illegal", "________.", "His", "ring", "fell", "into", "a", "hole", "in", "the", "________.", "We", "used", "to", "get", "company", "every", "________." };
    private string[] wordList2 = new string[] { "", "Jim", "hit", "his", "horse", "with", "a", "________.", "It", "was", "important", "to", "be", "on", "________.", "Surgery", "was", "needed", "to ", "repair", " his ", "failing", " ________.", "What ", "you", " find ", "depends", " on ", "where", " you", " ________.", "Matt", "was", "wild", "when", "he", "was ", "________.", "Larry ", "chose", " not", " to ", "join ", "the ", "________.", "To", " tune", " your", " car", " you ", "need", " a", " special", " ________.", "The ", "death ", "of ", "his ", "dog ", "was", " a", " great", " ________.", "Scotty ", "licked", " the ", "bottom ", "of", " the", " ________.", "A ", "future ", "energy ", "source", " is ", "the", " ________.", " He", " disliked", " having ", "to ", "commute ", "to", " the", " ________.", " Motorcycles", " can ", "create", " a ", "lot", " of", " ________.", "The ", "loaf ", "was", " eaten ", "except ", "for ", "a ", "small", " ________.", "The ", "lawyer ", "feared ", "that", " his", " client", " was", " ________. ", "They", " sat ", "together", " without ", "speaking", " a ", "single ", "________. ", "The ", "ruby", " was ", "so", " big, ", "it", " looked", " like", " a", " ________.", "Dan ", "caught", " the", " ball ", "with ", "his", " ________.", "A ", "direct ", "attack ", "failed, ", "so ", "they ", "changed ", "the ", "________.", "Rushing", " out ", "he ", "forgot", " to ", "take", " his ", "________. ", "They", " were", " startled ", "by ", "the ", "sudden", " ________.", "He was ", "miles ", "off ", "the ", "main", " ________.", "The", " surface", " of", " the", " water", " was ", "nice", " and ", "________.", "George", " must ", "keep ", "his", " pet ", "on", " a ", "________. ", "His", " boss ", "refused ", "to ", "give ", "him ", "a ", "________.", " He", " disappeared ", "last ", "year, ", "and", " has ", "not", " been", " ________.", "Her", " new ", "shoes ", "were", " the", " wrong ", "________.", "The ", "young ", "boy", " was", " granted ", "a ", "small", " ________.", "She ", "went", " to ", "the ", "salon ", "to ", "color", " her ", "________. ", "She ", "tied", " up ", "her ", "hair", " with", " a ", "yellow ", "________.", "He", " scraped", " the ", "cold ", "food", " from ", "his", " ________. ", "The", " boat", " passed ", "easily", " under", " the ", "________.", "His ", "view", " was ", "blocked", " by ", "the ", "music", " ________.", " Bob ", "would", " often ", "sleep ", "during", " his", " lunch", " ________. ", "Fred ", "sat", " in ", "his ", "chair", " on ", "the", " back", " ________. ", "She", " cleaned", " the ", "dirt ", "from", " her ", "________.", "It", " was", " clear", " that", " the", " leg ", "was ", "________. ", "You", " can't ", "take", " the ", "test", " without", " a", " ________. ", "The ", "child ", "was", " born ", "with", " a ", "rare", " ________.", " You ", "can't ", "buy ", "anything ", "for", " a", " ________.", " The ", "dough", " was", " put", " in ", "the", " hot", " ________.", "Ray ", "fell ", "down ", "and", " skinned", " his", " ________.", "There", " are ", "times ", "when ", "life ", "seems ", "________.", "When ", "you", " go ", "to ", "bed ", "turn", " off ", "the ", "________.", "You'll ", "never", " achieve ", "anything ", "if ", "you ", "don't", " ________.", " The ", "old ", "house ", "will ", "be ", "torn", " ________.", "Most ", "students ", "prefer ", "to", " work", " during ", "the ", "________. ", "Cathy ", "is ", "liked ", "by", " all ", "her", " ________.", "The ", "surgeon", " tried ", "vainly ", "to ", "save ", "his ", "________." };
    public TextMesh Stimulus;
    public string BCI2000Location = "Assets\\StreamingAssets\\BCI2000";
    [SerializeField]
    private int seqPrm = 2;
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
            "Load parameterfile " + BCI2000Location + "\\web\\paradigms\\SentenceCompletion\\sentences.prm" + "; " +
            "Set parameter CaptionSwitch 1; " +
            "Set parameter AudioSwitch 0; " +
            "Load parameterfile " + BCI2000Location + "\\parms.ecog\\screen_setup.prm" + "; " +
            "Load parameterfile " + BCI2000Location + string.Format("\\web\\paradigms\\SentenceCompletion\\sequences\\seq{0}.prm", seqPrm) + "; " +
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
        Stimulus.text = wordList2[initBCI2000.StimCode];
    }

    public void OnDestroy()
    {
        initBCI2000.quitBCI2000();
        initBCI2000.client.Close();
        Application.Quit();
    }

}
